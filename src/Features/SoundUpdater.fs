module App.SoundUpdater

open API.Sounds
open FileManager

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

    { SoundsToDownload = soundsToInstall
      SoundsToDelete = soundsToDelete }