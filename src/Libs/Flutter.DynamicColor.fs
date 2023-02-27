module Flutter.Libs.DynamicColor

open Fable.Core
open Fable.Core.Dart
open Flutter.Widgets
open Flutter.Material

/// https://pub.dev/packages/dynamic_color
[<ImportMember("package:dynamic_color/dynamic_color.dart")>]
type DynamicColorBuilder [<IsConst; NamedParams>] (builder: DartNullable<ColorScheme> -> DartNullable<ColorScheme> -> Widget) =
    inherit Widget()