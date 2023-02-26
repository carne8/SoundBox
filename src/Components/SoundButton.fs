module App.Components.Sound

open Fable.Core
open Fable.Core.Dart
open Fable.Dart.IO
open Flutter.Widgets
open Flutter.Material
open Flutter.Painting

let sound context imagePath onTap onLongPress =
    let primaryContainer = Theme.of'(context).colorScheme.primaryContainer
    let radius = (23. / 384.) * MediaQuery.of'(context).size.width

    let imageProvider = imagePath |> File |> FileImage
    let image = Image(image = imageProvider, fit = BoxFit.contain)

    GestureDetector(
        onTap = onTap,
        onLongPress = onLongPress,
        child = Container(
            child = image,
            padding = EdgeInsets.all 10.,
            decoration = BoxDecoration(
                color = primaryContainer,
                borderRadius = BorderRadius.all(Dart.Radius.circular radius)
            )
        )
    ) :> Widget |> Some |> DartNullable.ofOption