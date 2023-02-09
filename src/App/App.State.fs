namespace App

open Elmish
open Fable.Core.Dart
// open Fable.Core.DartInterop
open Fable.Dart
open Fable.Dart.Http

// import "dart:core";

module API =
    type Sound =
        { name: string
          soundUri: string
          imageUri: string }

    let private http = Http.Client()

    let inline private tryGet headers url = http.get(url, headers = headers)
    let inline private tryPost headers url = http.post(url, headers = headers)

    let notionVersion: string = "2022-06-28" // emitJsExpr () "process.env.NOTION_VERSION"
    let notionToken: string = "secret_I1HrhGwPT3Ylu2yOIP3owWMBqBkkLJnp9DVkHI2eGH7" // emitJsExpr () "process.env.NOTION_TOKEN"
    let databaseId: string = "f2d524d5eeea463ebcccfa127f3a90ec" // emitJsExpr () "process.env.NOTION_DATABASE_ID"

    let headers =
        [ MapEntry("Authorization", ("Bearer " + notionToken))
          MapEntry("Notion-Version", notionVersion) ]
        |> Map.fromEntries

    let getDatabaseContent () (*JS.Promise<Result<(string * string * string * string * string * string) [], FetchError>>*) =
        let path = $"v1/databases/{databaseId}/query"

        Uri.https("api.notion.com", path)
        |> tryPost headers
        // |> Promise.mapResult (fun response ->
        //     response?results
        //     |> Array.map (fun notionSoundPage ->
        //         notionSoundPage?id,
        //         notionSoundPage?properties?Name?id,
        //         notionSoundPage?properties?Order?id,
        //         notionSoundPage?properties?``Sound file``?id,
        //         notionSoundPage?properties?``Image file``?id,
        //         notionSoundPage?last_edited_time
        //     )
        // )

    // let getProperty pageId propertyId =
    //     (pageId, propertyId)
    //     ||> sprintf "https://api.notion.com/v1/pages/%s/properties/%s"
    //     |> tryGet<obj> headers

    // let getSounds () =
    //     promiseResult {
    //         let! sounds =
    //             getDatabaseContent()
    //             |> PromiseResult.mapError (fun x -> [x])

    //         return!
    //             sounds
    //             |> Array.map (fun (pageId, namePropId, indexPropId, soundPropId, imagePropId, lastEditedTime) ->
    //                 promiseResult {
    //                     let! nameResult = getProperty pageId namePropId
    //                     let! indexResult = getProperty pageId indexPropId
    //                     let! soundResult = getProperty pageId soundPropId
    //                     let! imageResult = getProperty pageId imagePropId
    //                     let lastEditedTime = DateTime.Parse(lastEditedTime).ToString("yyyy-MM-dd-HH-mm-ss")

    //                     return
    //                         { name = sprintf "%i-%s-%s" indexResult?number (nameResult?results |> Array.item 0)?title?plain_text lastEditedTime
    //                         soundUri = (soundResult?files |> Array.item 0)?file?url
    //                         imageUri = (imageResult?files |> Array.item 0)?file?url }
    //                 }
    //             )
    //             |> Array.toList
    //             |> List.sequencePromiseResultA
    //     }

    // let findSoundFromName sounds soundName =
    //     sounds |> List.findBack (fun sound -> sound.name = soundName)

module State =
    let init () =
        API.getDatabaseContent() |> Future.Future.map (fun x -> print x.body) |> ignore
        { Text = "This is a text"
          Switch = true
          Extended = false
          Scale = 1 }, Cmd.none

    let update msg model =
        match msg with
        | Msg.TextChanged newText -> { model with Text = newText }, Cmd.none
        | Msg.ToggleSwitch newValue -> { model with Switch = newValue; Scale = model.Scale * 2. }, Cmd.none
        | Msg.ToggleExtended newValue -> { model with Extended = newValue }, Cmd.none