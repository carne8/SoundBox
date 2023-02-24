namespace App

open Fable.Flutter.JustAudio

open API.Sounds
open FileManager
open SoundUpdater
open Fable.Dart.Future

type Loadable<'T> =
    | Loading of 'T option
    | Loaded of 'T

type Sound =
    { AudioPlayer: AudioPlayer
      ImagePath: string }

    static member fromLocalSound (localSound: LocalSound) =
        future {
            let player = AudioPlayer()
            let! _ = player.setFilePath(localSound.SoundPath, preload = true)

            return
                { AudioPlayer = player
                  ImagePath = localSound.ImagePath }
        }


[<RequireQualifiedAccess>]
type UpdateState =
    | Loading
    | LoadedSome of Update
    | LoadedNone
    | ApplyingUpdate


[<RequireQualifiedAccess>]
type Msg =
    | LocalSoundsLoaded of LocalSound array
    | RemoteSoundsLoaded of RemoteSound array
    | SoundsLoaded of Sound array
    | ErrorOcurred of string

    | UpdateLoaded of Update option
    | ApplyUpdate
    | UpdateApplied

type Model =
    { LocalSounds: LocalSound array Loadable
      RemoteSounds: RemoteSound array Loadable
      Sounds: Sound array Loadable
      Update: UpdateState
      Error: string option }