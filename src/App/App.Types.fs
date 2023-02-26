namespace App

open Fable.Flutter.AudioPlayers

open API.Sounds
open FileManager
open SoundUpdater
open Fable.Dart.Future

type Loadable<'T> =
    | Loading of 'T option
    | Loaded of 'T

type Sound =
    { AudioPlayer: AudioPlayer
      SoundPath: string
      ImagePath: string }

    static member fromLocalSound (localSound: LocalSound) =
        future {
            let player = AudioPlayer()
            let! _ = player.setReleaseMode ReleaseMode.stop

            return
                { AudioPlayer = player
                  SoundPath = localSound.SoundPath
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

    | UpdateAvailable of Update
    | ApplyUpdate
    | UpdateApplied

    | SettingsChanged of bool

type Model =
    { LocalSounds: LocalSound array Loadable
      RemoteSounds: RemoteSound array Loadable
      Sounds: Sound array Loadable
      Update: UpdateState
      Error: string option
      SpamMode: bool }