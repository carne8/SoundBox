module App.FileManager

open Fable.Dart
open Fable.Dart.IO
open Fable.Dart.Future
open App.API.Sounds

open Flutter.PathProvider
open Flutter.Dio

type LocalSound =
    { Name: string
      Path: string
      ImagePath: string
      SoundPath: string }


let private soundsDirectory =
    PathProvider.getApplicationSupportDirectory()
    |> Future.map (fun dir -> $"{dir.path}/sounds")

let private dio = Dio()

let downloadSound (remoteSound: RemoteSound) : string Future =
    future {
        let! baseDir = soundsDirectory
        let! soundDir = Directory($"{baseDir}/{remoteSound.Name}").create true

        let! _ = dio.download remoteSound.ImageUri $"{soundDir.path}/image.png"
        let! _ = dio.download remoteSound.SoundUri $"{soundDir.path}/sound.mp3"

        return soundDir.path
    }

let retrieveSounds () =
    future {
        let! soundsDirPath = soundsDirectory
        let! soundsDir = Directory(soundsDirPath).create true

        return
            soundsDir.listSync()
            |> Array.map (fun x -> x.path)
            |> Array.map (fun path -> path.Replace("\\", "/"))
            |> Array.map (fun path -> path.Split('/') |> Array.last, path)
            |> Array.map (fun (name, path) -> path, name, path + "/image.png", path + "/sound.mp3")
            |> Array.map (fun (path, name, image, sound) -> { Name = name; Path = path; ImagePath = image; SoundPath = sound })
            |> Array.sortBy (
                (fun sound -> sound.Name.Split '-')
                >> Array.head
                >> int
            )
    }