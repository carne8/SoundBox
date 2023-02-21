module App.API.Sounds

open Fable.Dart.Future
open Fable.Dart.Convert

type RemoteSound =
    { name: string
      soundUri: string
      imageUri: string }

let private databaseId: string = "f2d524d5eeea463ebcccfa127f3a90ec"

let private retrieveSoundPages () : Result<(string * string * string * string * string * string) list, string> Future =
    databaseId
    |> Notion.Database.retrieveContent
    |> FutureResult.map (fun response -> response?results)
    |> FutureResult.map List.ofArray
    |> FutureResult.map (List.map (fun notionSoundPage ->
        notionSoundPage?id,
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
        let lastEditedTime = System.DateTime.Parse(lastEditedTime).ToString("yyyy-MM-dd-HH-mm-ss")

        return nameResult, indexResult, soundResult, imageResult, lastEditedTime
    }

let private createSoundFromPageProps (nameResult, indexResult, soundResult, imageResult, lastEditedTime: string) =
    let name = $"{indexResult?number}-{(nameResult?results |> Array.item 0)?title?plain_text}-{lastEditedTime}"
    let soundUri = (soundResult?files |> Array.item 0)?file?url
    let imageUri = (imageResult?files |> Array.item 0)?file?url

    { name = name
      soundUri = soundUri
      imageUri = imageUri }

let getSounds () =
    futureResult {
        let! sounds = retrieveSoundPages() |> FutureResult.mapError List.singleton

        return!
            sounds
            |> List.map retrieveSoundPageProps
            |> List.map (FutureResult.map createSoundFromPageProps)
            |> List.sequenceFutureResultA
    }