namespace App

open Elmish
open Fable.Core.Dart
open Fable.Dart.Future
open Fable.Flutter.AudioPlayers

module Future =
    let wait (futureList: Future<'T> array) : Future<'T array> = emitExpr (import "Future" "dart:async", futureList) "$0.wait($1)"

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

    let updateSounds (update: SoundUpdater.Update) =
        Cmd.ofEffect (fun dispatch ->
            // update.SoundsToDownload
            // |> Array.map FileManager.downloadSound
            // |> Future.wait
            // |> ignore
            ()
        )

module State =
    let init () =
        { LocalSounds = Loading None
          RemoteSounds = Loading None
          Sounds = Loading None
          Update = None },
        Cmd.batch [
            Cmds.loadLocalSounds()
            Cmds.fetchRemoteSounds()
        ]

    let update msg model =
        print msg
        match msg with
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

        | Msg.UpdateLoaded update ->
            match update.SoundsToDownload.Length, update.SoundsToDelete.Length with
            | 0, 0 -> { model with Update = None }, Cmd.none
            | _ -> { model with Update = Some update }, Cmd.none