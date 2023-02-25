module Main

open Flutter.Foundation
open Flutter.Widgets
open Flutter.Material

open Dart
open Fable.Core.Dart
open Elmish.Flutter
open Flutter.Libs.DynamicColor

type App(?key: Key) =
    inherit StatelessWidget(?key = key)
    override _.build(context) =
        let defaultLightColorScheme = ColorScheme.fromSeed(seedColor = Colors.lightBlue)
        let defaultDarkColorScheme = ColorScheme.fromSeed(seedColor = Colors.lightBlue, brightness = Brightness.dark)

        DynamicColorBuilder(builder = (fun lightDynamic darkDynamic ->
            MaterialApp(
                title = "SoundBox",
                theme = ThemeData(colorScheme = (lightDynamic |> DartNullable.defaultValue defaultLightColorScheme), useMaterial3 = true, fontFamily = "ProductSans"),
                darkTheme = ThemeData(colorScheme = (darkDynamic |> DartNullable.defaultValue defaultDarkColorScheme), useMaterial3 = true, fontFamily = "ProductSans"),
                home = ElmishWidget.From(App.State.init, App.State.update, App.View.view)
            )
        ))

let main () = App() |> runApp