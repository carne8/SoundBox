namespace App

[<RequireQualifiedAccess>]
type Msg =
    | TextChanged of string
    | ToggleSwitch of bool
    | ToggleExtended of bool

type Model =
    { Text: string
      Switch: bool
      Extended: bool
      Scale: float }