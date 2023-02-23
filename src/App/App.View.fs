module App.View

open App
open Fable.Dart.IO
open Fable.Core
open Fable.Core.Dart
open Flutter.Widgets
open Flutter.Material
open Flutter.Painting
open Flutter.Rendering
open Fable.Flutter.AudioPlayers

let soundComponent context (sound: Sound) =
    let primaryContainer = Theme.of'(context).colorScheme.primaryContainer
    let radius = (23. / 384.) * MediaQuery.of'(context).size.width

    let imageProvider = sound.ImagePath |> File |> FileImage
    let image = Image(image = imageProvider, fit = BoxFit.contain)

    GestureDetector(
        onTap = (fun _ ->
            match sound.AudioPlayer.state with
            | PlayerState.Playing -> sound.AudioPlayer.stop() |> ignore
            | _ -> sound.AudioPlayer.play(DeviceFileSource sound.SoundPath) |> ignore
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
    |> Some
    |> DartNullable.ofOption

let view (model: Model) (dispatch: Msg -> unit) (context: BuildContext) : Widget =
    let size = MediaQuery.of'(context).size
    let buttonMargin = 0.038 * size.width

    Scaffold(
        body =
            match model.Sounds with
            | Loaded sounds ->
                CustomScrollView(
                    slivers = [|
                        SliverAppBar(
                            pinned = true,
                            expandedHeight = (185. / 853.) * size.height,
                            flexibleSpace = FlexibleSpaceBar(
                                title = Text("Sounds", style = TextStyle(fontSize = 35, color = Theme.of'(context).colorScheme.onBackground)),
                                centerTitle = true,
                                titlePadding = EdgeInsets.only(bottom = 6.)
                            )
                        )
                        // match model.Update with
                        // | Some update ->
                        //     SliverToBoxAdapter(
                        //         child = ElevatedButton(
                        //             onPressed = (fun _ -> ()),
                        //             child = (Text("Update") :> Widget |> Some |> DartNullable.ofOption)
                        //         )
                        //     )
                        // | None -> ()
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
                        SliverPadding(padding = EdgeInsets.only(bottom = buttonMargin))
                    |]
                ) :> Widget
            | Loading _ -> Text("Hello world !")

        // floatingActionButton =
        //     FloatingActionButton.extended(
        //         icon = Icon Icons.plus_one,
        //         label = Text "Button",
        //         onPressed = (fun _ -> not model.Extended |> Msg.ToggleExtended |> dispatch),
        //         isExtended = model.Extended
        //     )
    )