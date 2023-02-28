module rec App.Components.Dialog.Settings

open App
open Flutter.Widgets
open Flutter.Material
open Flutter.Rendering
open Flutter.Painting
open Flutter.PackageInfoPlus
open Fable.Dart.Future

type SettingsState(model: Model, dispatch: Elmish.Dispatch<Msg>) =
    inherit State<Settings>()

    let mutable _spamMode = model.SpamMode
    member _.spamMode
        with get() = _spamMode
        and set(value) =
            value |> Msg.SettingsChanged |> dispatch
            _spamMode <- value

    override this.build context =
        SimpleDialog(
            title = Text("Settings"),
            children = [|
                // Spam mode
                SimpleDialogOption(
                    onPressed = (fun _ -> this.setState(fun _ -> this.spamMode <- not this.spamMode)),
                    child = Row(
                        mainAxisAlignment = MainAxisAlignment.spaceBetween,
                        children = [|
                            Text("Spam mode")
                            Switch(value = this.spamMode, onChanged = (fun newValue -> this.setState(fun _ -> this.spamMode <- newValue)))
                        |]
                    )
                ) :> Widget

                // About / License
                SimpleDialogOption(
                    onPressed = (fun _ ->
                        PackageInfo.fromPlatform()
                        |> Future.map (fun packageInfo ->
                            showLicensePage(
                                context = context,
                                applicationName = "SoundBox",
                                applicationVersion = packageInfo.version,
                                applicationIcon = Padding(
                                    padding = EdgeInsets.fromLTRB(10., 20., 10., 10.),
                                    child = SizedBox(
                                        width = 70.,
                                        height = 70.,
                                        child = ClipRRect(
                                            borderRadius = BorderRadius.circular(23.),
                                            child = Image.asset("assets/icons/icon.png")
                                        )
                                    )
                                )
                            )
                        )
                        |> ignore
                    ),
                    child = Text("About / Licenses")
                ) :> Widget

                // OK button
                Padding(
                    padding = EdgeInsets.only(right = 15.),
                    child = Row(
                        mainAxisAlignment = MainAxisAlignment.``end``,
                        children = [|
                            TextButton(
                                child = Text("OK"),
                                onPressed = (fun _ -> Navigator.pop(context))
                            )
                        |]
                    )
                )
            |]
        )

type Settings(model: Model, dispatch: Elmish.Dispatch<Msg>) =
    inherit StatefulWidget()
    override _.createState() = SettingsState(model = model, dispatch = dispatch)


let show context model dispatch =
    showDialog(
        context = context,
        builder = (fun _ctx -> Settings(model, dispatch) |> unbox<Widget>)
    ) |> ignore