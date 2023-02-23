module App.View

open App
open Fable.Core.Dart
open Fable.Dart.IO
open Flutter.Widgets
open Flutter.Material
open Flutter.Painting
open Flutter.Rendering

let soundComponent context (sound: FileManager.LocalSound) =
    let primaryContainer = Theme.of'(context).colorScheme.primaryContainer
    let imageProvider =
        sound.ImagePath
        |> File
        |> FileImage
        |> unbox

    let image = Image(image = imageProvider, fit = BoxFit.contain)

    Container(
        child = image,
        padding = EdgeInsets.all 10.,
        decoration = BoxDecoration(
            color = primaryContainer,
            borderRadius = BorderRadius.all(Dart.Radius.circular 23.)
        )
    )
    :> Widget
    |> Some
    |> DartNullable.ofOption

let view (model: Model) (dispatch: Msg -> unit) (context: BuildContext) : Widget =
    Scaffold(
        body =
            match model.Sounds with
            | Loaded sounds ->
                CustomScrollView(
                    slivers = [|
                        SliverAppBar(
                            pinned = true,
                            expandedHeight = 185.0,
                            flexibleSpace = FlexibleSpaceBar(
                                title = Text("Sounds", style = TextStyle(fontSize = 35, color = Theme.of'(context).colorScheme.onBackground)),
                                centerTitle = true,
                                titlePadding = EdgeInsets.only(bottom = 6.)
                            )
                        )
                        SliverPadding(
                            padding = (EdgeInsets.only(top = 60., left = 16., right = 16.)),
                            sliver =
                                SliverGrid(
                                    gridDelegate = SliverGridDelegateWithFixedCrossAxisCount(3, mainAxisSpacing = 16., crossAxisSpacing = 16., childAspectRatio = 29. / 21.),
                                    ``delegate`` = SliverChildBuilderDelegate(
                                        childCount = sounds.Length,
                                        builder = (fun ctx idx -> sounds |> Array.item idx |> soundComponent ctx)
                                    )
                                )
                        )
                        SliverPadding(
                            padding = (EdgeInsets.only(top = 60., left = 16., right = 16.)),
                            sliver =
                                SliverGrid(
                                    gridDelegate = SliverGridDelegateWithFixedCrossAxisCount(3, mainAxisSpacing = 16., crossAxisSpacing = 16., childAspectRatio = 29. / 21.),
                                    ``delegate`` = SliverChildBuilderDelegate(
                                        childCount = sounds.Length,
                                        builder = (fun ctx idx -> sounds |> Array.item idx |> soundComponent ctx)
                                    )
                                )
                        )
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