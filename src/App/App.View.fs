module App.View

open App
open Fable.Core
open Flutter.Widgets
open Flutter.Material
open Flutter.Painting
open Flutter.Rendering
open Flutter.AudioPlayers


let center children =
    Center(child = Column(
        mainAxisAlignment = MainAxisAlignment.center,
        crossAxisAlignment = CrossAxisAlignment.center,
        children = children
    ))

let errorOcurred context error =
    center [|
        Text("An error ocurred", style = Theme.of'(context).textTheme.titleLarge)
        Text(error, style = Theme.of'(context).textTheme.titleMedium, textAlign = Dart.TextAlign.center)
    |]
    :> Widget

let fetchingApi context =
    center [|
        CircularProgressIndicator()
        Padding(
            padding = EdgeInsets.only(top = 35.),
            child = Column(children = [|
                Text("No sounds installed", style = Theme.of'(context).textTheme.titleLarge)
                Text("Searching for sounds online...", style = Theme.of'(context).textTheme.titleMedium)
            |])
        )
    |]

let applyingUpdate context =
    center [|
        CircularProgressIndicator()
        Padding(
            padding = EdgeInsets.only(top = 35.),
            child = Text("Installing sounds...", style = Theme.of'(context).textTheme.titleLarge)
        )
    |]

let loadingSounds context =
    center [|
        CircularProgressIndicator()
        Padding(
            padding = EdgeInsets.only(top = 35.),
            child = Text("Loading sounds...", style = Theme.of'(context).textTheme.titleLarge)
        )
    |]


let forceStopSound sound = sound.AudioPlayer.stop() |> ignore

let toggleSound model sound =
    match model.SpamMode, sound.AudioPlayer.state with
    | false, PlayerState.Playing -> forceStopSound sound
    | true, _ ->
        forceStopSound sound
        sound.SoundPath |> DeviceFileSource |> sound.AudioPlayer.play |> ignore
    | _ -> sound.SoundPath |> DeviceFileSource |> sound.AudioPlayer.play |> ignore


let view (model: Model) (dispatch: Msg -> unit) (context: BuildContext) : Widget =
    let size = MediaQuery.of'(context).size
    let buttonMargin = 0.038 * size.width

    Scaffold(body =
        match model.Error, model.Update, model.Sounds with
        | Some error, _, _ -> errorOcurred context error
        | _, UpdateState.Loading , Loaded sounds when sounds.Length = 0 -> fetchingApi context
        | _, UpdateState.ApplyingUpdate, _ -> applyingUpdate context
        | _, _, Loading _ -> loadingSounds context
        | _, _, Loaded sounds ->
            CustomScrollView(
                slivers = [|
                    SliverAppBar(
                        pinned = true,
                        expandedHeight = (185. / 853.) * size.height,
                        flexibleSpace = FlexibleSpaceBar(
                            title = Text("Sounds", style = TextStyle(fontSize = 35, color = Theme.of'(context).colorScheme.onBackground)),
                            centerTitle = true,
                            titlePadding = EdgeInsets.only(bottom = 6.)
                        ),
                        actions = [|
                            // Update button
                            match model.Update with
                            | UpdateState.LoadedSome update ->
                                IconButton(
                                    icon = Icon(Icons.cloud_sync),
                                    onPressed =
                                        Components.Dialog.Update.show
                                            context
                                            (update.SoundsToDelete.Length + update.SoundsToDownload.Length)
                                            (fun _ -> Msg.ApplyUpdate |> dispatch)
                                )
                            | _ -> ()

                            IconButton(
                                icon = Icon(Icons.settings),
                                onPressed = (fun _ -> Components.Dialog.Settings.show context model dispatch)
                            )
                        |]
                    )
                    SliverPadding(
                        padding = (EdgeInsets.only(top = 60., left = buttonMargin, right = buttonMargin)),
                        sliver =
                            SliverGrid(
                                gridDelegate = SliverGridDelegateWithFixedCrossAxisCount(
                                    crossAxisCount = 3,
                                    mainAxisSpacing = buttonMargin,
                                    crossAxisSpacing = buttonMargin,
                                    childAspectRatio = 29. / 21.
                                ),
                                ``delegate`` = SliverChildBuilderDelegate(
                                    childCount = sounds.Length,
                                    builder = (fun ctx idx ->
                                        let sound = sounds |> Array.item idx
                                        Components.Sound.sound
                                            sound.ImagePath
                                            (fun _ -> toggleSound model sound)
                                            (fun _ -> forceStopSound sound)
                                    )
                                )
                            )
                    )
                    SliverToBoxAdapter(child = SizedBox(height = buttonMargin))
                |]
            )
    )