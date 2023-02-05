module Fable.Dart.Http

open Fable.Core
open Fable.Dart
open Fable.Dart.Future
open System.Runtime.InteropServices

[<ImportMember "package:http/http.dart">]
type Response() =
    member _.body : string = nativeOnly

[<ImportMember "package:http/http.dart">]
type BaseClient() =
    member _.close() : unit = nativeOnly
    member _.delete (url: Uri, [<Optional>] headers: Map<string, string>, [<Optional>] body: obj, [<Optional>] encoding: IEncoding) : Response Future = nativeOnly
    member _.get (url: Uri, [<Optional>] headers: Map<string, string>) : Response Future = nativeOnly
    member _.head (url: Uri, [<Optional>] headers: Map<string, string>) : Response Future = nativeOnly
    member _.patch (url: Uri, [<Optional>] headers: Map<string, string>, [<Optional>] body: obj, [<Optional>] encoding: IEncoding) : Response Future = nativeOnly
    member _.post (url: Uri, [<Optional>] headers: Map<string, string>, [<Optional>] body: obj, [<Optional>] encoding: IEncoding) : Response Future = nativeOnly
    member _.put (url: Uri, [<Optional>] headers: Map<string, string>, [<Optional>] body: obj, [<Optional>] encoding: IEncoding) : Response Future = nativeOnly
    member _.read (url: Uri, [<Optional>] headers: Map<string, string>) : Response Future = nativeOnly
    member _.readBytes (url: Uri, [<Optional>] headers: Map<string, string>) : uint8 list Future = nativeOnly
    // member _.send (request: BaseRequest) : Response Future = nativeOnly

[<ImportMember "package:http/http.dart">]
type Client() = inherit BaseClient()

[<ImportAll "package:http/http.dart">]
type Http() =
    static member Client() : BaseClient = nativeOnly