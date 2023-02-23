namespace App

open Elmish
open Fable.Core.Dart
open Fable.Dart.Future

module Cmds =
    let loadSounds () =
        Cmd.ofEffect (fun dispatch ->
            FileManager.retrieveSounds()
            |> Future.map Msg.SoundsLoaded
            |> Future.map dispatch
            |> ignore
        )

    let downloadSounds () =
        Cmd.ofEffect (fun dispatch ->
            API.Sounds.getSounds()
            |> FutureResult.map (List.map FileManager.downloadSound)
            |> FutureResult.map (List.map (Future.map (fun x -> print $"Downloaded {x}")))
            |> FutureResult.mapError print
            |> ignore
        )

module State =
    let init () =
        { Sounds = Loading None }, Cmds.loadSounds()

    let update msg model =
        match msg with
        | Msg.SoundsLoaded sounds ->
            print sounds
            { model with Sounds = Loaded sounds }, Cmds.downloadSounds()