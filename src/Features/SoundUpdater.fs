module App.SoundUpdater

open API.Sounds
open FileManager
open Fable.Dart.IO
open Fable.Dart
open Fable.Dart.Future


type Update =
    { SoundsToDownload: RemoteSound array
      SoundsToDelete: LocalSound array }

let compareSounds (localSounds: LocalSound array) (remoteSounds: RemoteSound array) =
    let soundsToDelete =
        localSounds
        |> Array.filter (fun localSound ->
            remoteSounds
            |> List.ofArray
            |> List.map (fun remoteSound -> remoteSound.Name)
            |> List.contains localSound.Name
            |> not
        )

    let soundsToInstall =
        remoteSounds
        |> Array.filter (fun remoteSound ->
            localSounds
            |> List.ofArray
            |> List.map (fun localSound -> localSound.Name)
            |> List.contains remoteSound.Name
            |> not
        )

    match soundsToInstall.Length, soundsToDelete.Length with
    | 0, 0 -> None
    | _ ->
        { SoundsToDownload = soundsToInstall
          SoundsToDelete = soundsToDelete }
        |> Some

let applyUpdate (update: Update) =
    future {
        let! _ =
            update.SoundsToDelete
            |> Array.map (fun soundFolder -> (Directory soundFolder.Path).delete true)
            |> Future.wait
            |> Future.map ignore
            |> Future.toVoid

        let! _ =
            update.SoundsToDownload
            |> Array.map FileManager.downloadSound
            |> Future.wait
            |> Future.map ignore
            |> Future.toVoid

        ()
    }