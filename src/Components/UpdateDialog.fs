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
                        onValidated()
                    )
                )
            |]
        ))
    ) |> ignore
