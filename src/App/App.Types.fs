namespace App

open Fable.Flutter.AudioPlayers

open API.Sounds
open FileManager
open SoundUpdater

type Loadable<'T> =
    | Loading of 'T option
    | Loaded of 'T

type Sound =
    { AudioPlayer: AudioPlayer
      SoundPath: string
      ImagePath: string }

    static member private defaultAudioPlayer =
        let player = AudioPlayer()
        player.setPlayerMode PlayerMode.lowLatency |> ignore
        player.setReleaseMode ReleaseMode.stop |> ignore
        player

    static member fromLocalSound (localSound: LocalSound) =
        { AudioPlayer = Sound.defaultAudioPlayer
          SoundPath = localSound.SoundPath
          ImagePath = localSound.ImagePath }

[<RequireQualifiedAccess>]
type Msg =
    | LocalSoundsLoaded of LocalSound array
    | RemoteSoundsLoaded of RemoteSound array
    | UpdateLoaded of Update

type Model =
    { LocalSounds: LocalSound array Loadable
      RemoteSounds: RemoteSound array Loadable
      Sounds: Sound array Loadable
      Update: Update option }