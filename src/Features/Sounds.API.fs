module App.API.Sounds

open Fable.Dart
open Fable.Dart.Future
open Fable.Dart.Environment

// import "dart:core";

type RemoteSound =
    { Name: string
      SoundUri: string
      ImageUri: string }

let private databaseId: string = String.fromEnvironment "NOTION_DATABASE_ID"

let private retrieveSoundPages () : Result<(string * string * string * string * string * string) array, string> Future =
    databaseId
    |> Notion.Database.retrieveContent
    |> FutureResult.map (fun response -> response?results)
    |> FutureResult.map Seq.toArray
    |> FutureResult.map (Array.map (fun notionSoundPage ->
        notionSoundPage?id |> string,
        notionSoundPage?properties?Name?id,
        notionSoundPage?properties?Order?id,
        notionSoundPage?properties?``Sound file``?id,
        notionSoundPage?properties?``Image file``?id,
        notionSoundPage?last_edited_time
    ))

let private retrieveSoundPageProps (pageId, namePropId, indexPropId, soundPropId, imagePropId, lastEditedTime) =
    futureResult {
        let! nameResult = Notion.Page.getProperty pageId namePropId
        let! indexResult = Notion.Page.getProperty pageId indexPropId
        let! soundResult = Notion.Page.getProperty pageId soundPropId
        let! imageResult = Notion.Page.getProperty pageId imagePropId
        let lastEditedTime = System.DateTime.Parse(lastEditedTime).ToString("yyyy-MM-dd-HH-mm")

        return nameResult, indexResult, soundResult, imageResult, lastEditedTime
    }

let private createSoundFromPageProps (nameResult: Map<string, dynamic>, indexResult: Map<string, dynamic>, soundResult: Map<string, dynamic>, imageResult: Map<string, dynamic>, lastEditedTime: string) =
    let name = $"{indexResult?number}-{(nameResult?results |> Array.item 0)?title?plain_text}-{lastEditedTime}"
    let soundUri = (soundResult?files |> Array.head)?file?url
    let imageUri = (imageResult?files |> Array.head)?file?url

    { Name = name
      SoundUri = soundUri
      ImageUri = imageUri }

let getSounds () =
    futureResult {
        let! sounds = retrieveSoundPages() |> FutureResult.mapError ((+) "Retrieve sounds page failed: " >> Array.singleton)

        return!
            sounds
            |> Array.map retrieveSoundPageProps
            |> Array.map (FutureResult.map createSoundFromPageProps)
            |> Array.toList
            |> List.sequenceFutureResultA
            |> FutureResult.map Array.ofList
            |> FutureResult.mapError Array.ofList
    }