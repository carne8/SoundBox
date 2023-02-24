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
            |> FutureResult.map Msg.RemoteSoundsLoaded
            |> FutureResult.map dispatch
            |> FutureResult.mapError print
            |> ignore
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
          Update = UpdateState.Loading },
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
                | Loaded remoteSounds ->
                    (sounds, remoteSounds)
                    ||> SoundUpdater.compareSounds
                    |> Msg.UpdateLoaded
                    |> Cmd.ofMsg
                | Loading _ -> Cmd.none

            { model with
                LocalSounds = Loaded sounds
                Sounds = Loaded (sounds |> Array.map Sound.fromLocalSound) },
            cmd

        | Msg.RemoteSoundsLoaded sounds ->
            let cmd =
                match model.LocalSounds with
                | Loaded localSounds ->
                    (localSounds, sounds)
                    ||> SoundUpdater.compareSounds
                    |> Msg.UpdateLoaded
                    |> Cmd.ofMsg
                | Loading _ -> Cmd.none

            { model with RemoteSounds = Loaded sounds }, cmd

        // Update
        | Msg.UpdateLoaded updateOpt ->
            match model.LocalSounds, updateOpt with
            | Loaded sounds, Some update when sounds.Length > 0 -> { model with Update = UpdateState.LoadedSome update }, Cmd.none
            | Loaded _, Some update -> { model with Update = UpdateState.ApplyingUpdate }, Cmds.applyUpdate update
            | _ -> { model with Update = UpdateState.LoadedNone }, Cmd.none

        | Msg.ApplyUpdate ->
            match model.Update with
            | UpdateState.LoadedSome update ->
                { model with Update = UpdateState.ApplyingUpdate },
                Cmds.applyUpdate update
            | _ -> model, Cmd.none

        | Msg.UpdateApplied -> { model with Update = UpdateState.LoadedNone }, Cmds.loadLocalSounds()