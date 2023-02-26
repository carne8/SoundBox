namespace App

open Elmish
open Fable.Core.Dart
open Fable.Dart.Future

module Cmds =
    let loadLocalSounds () =
        Cmd.ofEffect (fun dispatch ->
            FileManager.retrieveSounds()
            |> Future.map Msg.LocalSoundsLoaded
            |> Future.map dispatch
            |> ignore
        )

    let fetchRemoteSounds () =
        Cmd.ofEffect (fun dispatch ->
            API.Sounds.getSounds()
            |> FutureResult.map (Msg.RemoteSoundsLoaded >> dispatch)
            |> FutureResult.mapError (Array.head >> Msg.ErrorOcurred >> dispatch)
            |> ignore
        )

    let loadSounds sounds =
        Cmd.ofEffect (fun dispatch ->
            sounds
            |> Array.map Sound.fromLocalSound
            |> Future.wait
            |> Future.map Msg.SoundsLoaded
            |> Future.map dispatch
            |> ignore
        )


    let compareSounds localSounds remoteSounds =
        Cmd.ofEffect (fun dispatch ->
                (localSounds, remoteSounds)
                ||> SoundUpdater.compareSounds
                |> function
                    | Some update -> update |> Msg.UpdateAvailable |> dispatch
                    | None -> ()
        )

    let applyUpdate (update: SoundUpdater.Update) =
        Cmd.ofEffect (fun dispatch ->
            update
            |> SoundUpdater.applyUpdate
            |> Future.toVoid
            |> Future.map (fun _ -> Msg.UpdateApplied |> dispatch)
            |> ignore
        )

module State =
    let init () =
        { LocalSounds = Loading None
          RemoteSounds = Loading None
          Sounds = Loading None
          Update = UpdateState.Loading
          Error = None
          SpamMode = false },
        Cmd.batch [
            Cmds.loadLocalSounds()
            Cmds.fetchRemoteSounds()
        ]

    let update msg model =
        print msg
        match msg with
        // Sound loading
        | Msg.LocalSoundsLoaded sounds ->
            let cmd =
                match model.RemoteSounds with
                | Loaded remoteSounds -> Cmds.compareSounds sounds remoteSounds
                | Loading _ -> Cmd.none

            { model with LocalSounds = Loaded sounds },
            Cmd.batch [ cmd; Cmds.loadSounds sounds ]

        | Msg.RemoteSoundsLoaded sounds ->
            let cmd =
                match model.LocalSounds with
                | Loaded localSounds -> Cmds.compareSounds localSounds sounds
                | Loading _ -> Cmd.none

            { model with RemoteSounds = Loaded sounds }, cmd

        | Msg.SoundsLoaded sounds -> { model with Sounds = Loaded sounds }, Cmd.none
        | Msg.ErrorOcurred error -> { model with Error = Some error }, Cmd.none

        // Update
        | Msg.UpdateAvailable update ->
            match model.LocalSounds with
            | Loaded sounds ->
                match sounds.Length with
                | 0 -> { model with Update = UpdateState.ApplyingUpdate }, Cmds.applyUpdate update
                | _ -> { model with Update = UpdateState.LoadedSome update }, Cmd.none
            | _ -> { model with Update = UpdateState.LoadedSome update }, Cmd.none

        | Msg.ApplyUpdate ->
            match model.Update with
            | UpdateState.LoadedSome update ->
                { model with Update = UpdateState.ApplyingUpdate },
                Cmds.applyUpdate update
            | _ -> model, Cmd.none

        | Msg.UpdateApplied -> { model with Update = UpdateState.LoadedNone }, Cmds.loadLocalSounds()

        | Msg.SettingsChanged newSettings -> { model with SpamMode = newSettings }, Cmd.none