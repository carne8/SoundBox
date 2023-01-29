namespace App

open Elmish

module State =
    let init () =
        { Text = "This is a text"
          Switch = true
          Extended = false
          Scale = 1 }, Cmd.none

    let update msg model =
        match msg with
        | Msg.TextChanged newText -> { model with Text = newText }, Cmd.none
        | Msg.ToggleSwitch newValue -> { model with Switch = newValue; Scale = model.Scale * 2. }, Cmd.none
        | Msg.ToggleExtended newValue -> { model with Extended = newValue }, Cmd.none