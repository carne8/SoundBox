namespace App

type Loadable<'T> =
    | Loading of 'T option
    | Loaded of 'T

[<RequireQualifiedAccess>]
type Msg =
    | SoundsLoaded of FileManager.LocalSound array

type Model =
    { Sounds: FileManager.LocalSound array Loadable }