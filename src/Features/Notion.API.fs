module App.API.Notion

open Fable.Core.Dart
open Fable.Dart
open Fable.Dart.Future
open Fable.Dart.Convert

open Fable.Flutter.Dotenv

#nowarn "59"
// import "dart:core";

let private http = Http.Client()
let private notionVersion: string = App.Secrets.getSecret "NOTION_VERSION"
let private notionToken: string = App.Secrets.getSecret "NOTION_SECRET"

let private headers =
    [ MapEntry("Authorization", "Bearer " + notionToken)
      MapEntry("Notion-Version", notionVersion) ]
    |> Map.fromEntries

module Database =
    let retrieveContent databaseId =
        let path = $"v1/databases/{databaseId}/query"
        let url = Uri.https("api.notion.com", path)

        http.post(url, headers)
        |> FutureResult.catchError
        |> FutureResult.mapError string
        |> FutureResult.map (fun res -> res.body |> json.decode)
        |> FutureResult.map (fun decoded -> decoded?status |> DartNullable.toOption |> Option.map int, decoded)
        |> FutureResult.bind (fun (status, decoded) ->
            match status with
            | None | Some 200 -> Future.singleton (Ok decoded)
            | _ -> Future.singleton (Error (decoded?message |> DartNullable.defaultValue "Cannot found error message"))
        )

module Page =
    let getProperty (pageId: string) (propertyId: string) =
        let url = Uri("https", host = "api.notion.com", path = $"/v1/pages/{pageId}/properties/{propertyId}")

        http.get(url, headers)
        |> FutureResult.catchError
        |> FutureResult.mapError string
        |> FutureResult.map (fun res -> res.body |> json.decode)
        |> FutureResult.map (fun decoded -> decoded?status |> DartNullable.toOption |> Option.map int, decoded)
        |> FutureResult.bind (fun (status, decoded) ->
            match status with
            | None | Some 200 -> Future.singleton (Ok decoded)
            | _ -> Future.singleton (Error (decoded?message |> DartNullable.defaultValue "Cannot found error message"))
        )