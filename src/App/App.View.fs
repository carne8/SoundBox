module App.View

open App
open Fable.Core
open Fable.Core.Dart
open Fable.Dart.IO
open Flutter.Widgets
open Flutter.Material
open Flutter.Painting
open Flutter.Rendering

// import "dart:core";

type DartNullable<'T> with
    static member inline singleton x = x |> Some |> DartNullable.ofOption

let soundComponent context (sound: Sound) =
    let primaryContainer = Theme.of'(context).colorScheme.primaryContainer
    let radius = (23. / 384.) * MediaQuery.of'(context).size.width

    let imageProvider = sound.ImagePath |> File |> FileImage
    let image = Image(image = imageProvider, fit = BoxFit.contain)

    GestureDetector(
        onTap = (fun _ ->
            match sound.AudioPlayer.playing with
            | true -> sound.AudioPlayer.stop() |> ignore
            | false -> sound.AudioPlayer.play() |> ignore
        ),
        child = Container(
            child = image,
            padding = EdgeInsets.all 10.,
            decoration = BoxDecoration(
                color = primaryContainer,
                borderRadius = BorderRadius.all(Dart.Radius.circular radius)
            )
        )
    )
    :> Widget
    |> DartNullable.singleton

let view (model: Model) (dispatch: Msg -> unit) (context: BuildContext) : Widget =
    let size = MediaQuery.of'(context).size
    let buttonMargin = 0.038 * size.width

    Scaffold(
        body =
            match model.Error, model.Update, model.Sounds with
            | Some error, _, _ ->
                Center(child = Column(
                    mainAxisAlignment = MainAxisAlignment.center,
                    crossAxisAlignment = CrossAxisAlignment.center,
                    children = [|
                        Text("An error ocurred", style = Theme.of'(context).textTheme.titleLarge)
                        Text(error, style = Theme.of'(context).textTheme.titleMedium, textAlign = Dart.TextAlign.center)
                    |]
                )) :> Widget
            | _, _, Loading _ -> Center(child = Text("Loading sounds"))
            | _, UpdateState.ApplyingUpdate, Loaded _ ->
                Center(child = Column(
                    mainAxisAlignment = MainAxisAlignment.center,
                    crossAxisAlignment = CrossAxisAlignment.center,
                    children = [|
                        CircularProgressIndicator()
                        Padding(
                            padding = EdgeInsets.only(top = 35.),
                            child = Text("Installing sounds ...", style = Theme.of'(context).textTheme.titleLarge)
                        )
                    |]
                ))
            | _, _, Loaded sounds when sounds.Length = 0 ->
                Center(child = Column(
                    mainAxisAlignment = MainAxisAlignment.center,
                    crossAxisAlignment = CrossAxisAlignment.center,
                    children = [|
                        CircularProgressIndicator()
                        Padding(
                            padding = EdgeInsets.only(top = 35.),
                            child = Column(children = [|
                                Text("No sounds installed", style = Theme.of'(context).textTheme.titleLarge)
                                Text("Searching for sounds online...", style = Theme.of'(context).textTheme.titleMedium)
                            |])
                        )
                    |]
                ))
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
                                match model.Update with
                                | UpdateState.LoadedSome update ->
                                    let totalOperations = update.SoundsToDelete.Length + update.SoundsToDownload.Length

                                    IconButton(
                                        icon = Icon(Icons.cloud_sync),
                                        onPressed = (fun _ ->
                                            showDialog(
                                                context = context,
                                                builder = (fun _ctx -> AlertDialog(
                                                    title = Text("Perform update ?", textAlign = Dart.TextAlign.center),
                                                    content = Text($"This update will perform {totalOperations} operations", textAlign = Dart.TextAlign.center),
                                                    actionsAlignment = MainAxisAlignment.spaceBetween,
                                                    actions = [|
                                                        OutlinedButton(
                                                            style = ButtonStyle(),
                                                            child = Text "Cancel",
                                                            onPressed = Navigator.``of``(context).pop
                                                        )
                                                        FilledButton(
                                                            child = (Text "Update"),
                                                            onPressed = (fun _ ->
                                                                Navigator.``of``(context).pop()
                                                                Msg.ApplyUpdate |> dispatch
                                                            )
                                                        )
                                                    |]
                                                ))
                                            ) |> ignore
                                        )
                                    )
                                | _ -> ()
                            |]
                        )
                        SliverPadding(
                            padding = (EdgeInsets.only(top = 60., left = buttonMargin, right = buttonMargin)),
                            sliver =
                                SliverGrid(
                                    gridDelegate = SliverGridDelegateWithFixedCrossAxisCount(3, mainAxisSpacing = buttonMargin, crossAxisSpacing = buttonMargin, childAspectRatio = 29. / 21.),
                                    ``delegate`` = SliverChildBuilderDelegate(
                                        childCount = sounds.Length,
                                        builder = (fun ctx idx -> sounds |> Array.item idx |> soundComponent ctx)
                                    )
                                )
                        )
                        SliverToBoxAdapter(child = SizedBox(height = buttonMargin))
                    |]
                )
    )