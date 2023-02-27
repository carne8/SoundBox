module rec App.Components.Sound

open Fable.Core
open Fable.Core.Dart
open Fable.Dart.IO
open Flutter.Widgets
open Flutter.Material
open Flutter.Painting
open Flutter.Animation

type SoundState(image, onTap, onLongPress) =
    inherit State<Sound>()

    member val scale = 1. with get, set
    member this.down() = this.setState(fun _ -> this.scale <- 0.95)
    member this.up() = this.setState(fun _ -> this.scale <- 1.)

    override this.build context =
        let radius = (23. / 384.) * MediaQuery.of'(context).size.width

        GestureDetector(
            onTapDown = (fun _ -> this.down()),
            onTapUp = (fun _ -> this.up()),
            onLongPressDown = (fun _ -> this.down()),
            onLongPressUp = (fun _ -> this.up()),
            onTap = onTap,
            onLongPress = onLongPress,
            child = AnimatedScale(
                scale = this.scale,
                duration = System.TimeSpan(0, 0, 0, 0, 50),
                curve = Curves.easeInOut,
                child = Container(
                    child = Image(image = image, fit = BoxFit.contain),
                    padding = EdgeInsets.all 10.,
                    decoration = BoxDecoration(
                        color = Theme.of'(context).colorScheme.primaryContainer,
                        borderRadius = BorderRadius.all(Dart.Radius.circular radius)
                    )
                )
            )
        ) :> Widget

type Sound(image, onTap, onLongPress) =
    inherit StatefulWidget()
    override _.createState() = SoundState(image, onTap, onLongPress)


let sound context imagePath onTap onLongPress =
    let imageProvider = imagePath |> File |> FileImage
    Sound(imageProvider, onTap, onLongPress) |> Some |> DartNullable.ofOption