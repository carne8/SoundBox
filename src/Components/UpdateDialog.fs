module App.Components.Dialog.Update

open Fable.Core
open Flutter.Widgets
open Flutter.Material
open Flutter.Rendering

let show context totalOperations onValidated () =
    showDialog(
        context = context,
        builder = (fun _ctx -> AlertDialog(
            title = Text("Perform update ?", textAlign = Dart.TextAlign.center),
            content =
                (match totalOperations with
                 | x when x = 1 -> Text($"This update will perform {x} operation", textAlign = Dart.TextAlign.center)
                 | x -> Text($"This update will perform {x} operations", textAlign = Dart.TextAlign.center)),
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
                        onValidated()
                    )
                )
            |]
        ))
    ) |> ignore
