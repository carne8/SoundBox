module rec App.Components.Dialog.Settings

open App
open Fable.Core
open Flutter.Widgets
open Flutter.Material
open Flutter.Painting
open Flutter.Rendering

type SettingsState(model: Model, dispatch: Elmish.Dispatch<Msg>) =
    inherit State<Settings>()

    let mutable _spamMode = model.SpamMode
    member _.spamMode
        with get() = _spamMode
        and set(value) =
            value |> Msg.SettingsChanged |> dispatch
            _spamMode <- value

    override this.build context =
        AlertDialog(
            title = Text("Settings"),
            content = Row(
                children = [|
                    Text("Spam mode")
                    Switch(value = this.spamMode, onChanged = (fun newValue -> this.setState(fun _ -> this.spamMode <- newValue)))
                |],
                mainAxisAlignment = MainAxisAlignment.spaceBetween
            ),
            actions = [| TextButton(child = Text "OK", onPressed = (fun _ -> Navigator.``of``(context).pop())) |]
        ) :> Widget

type Settings(model: Model, dispatch: Elmish.Dispatch<Msg>) =
    inherit StatefulWidget()
    override _.createState() = SettingsState(model = model, dispatch = dispatch)


let show context model dispatch =
    showDialog(
        context = context,
        builder = (fun _ctx -> Settings(model, dispatch) |> unbox<Widget>)
    ) |> ignore