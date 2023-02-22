module Fable.Dart.Http

open Fable.Core
open Fable.Core.Dart
open Fable.Dart.Future
open System.Runtime.InteropServices

[<ImportMember "package:http/http.dart">]
type Response =
    abstract member body : string

[<ImportMember "package:http/http.dart">]
type BaseClient() =
    member _.close() : unit = nativeOnly
    [<NamedParams(fromIndex=1)>] member _.delete (url: Uri, [<Optional>] headers: Map<string, string>, [<Optional>] body: obj, [<Optional>] encoding: IEncoding) : Response Future = nativeOnly
    [<NamedParams(fromIndex=1)>] member _.get (url: Uri, [<Optional>] headers: Map<string, string>) : Response Future = nativeOnly
    [<NamedParams(fromIndex=1)>] member _.head (url: Uri, [<Optional>] headers: Map<string, string>) : Response Future = nativeOnly
    [<NamedParams(fromIndex=1)>] member _.patch (url: Uri, [<Optional>] headers: Map<string, string>, [<Optional>] body: obj, [<Optional>] encoding: IEncoding) : Response Future = nativeOnly
    [<NamedParams(fromIndex=1)>] member _.post (url: Uri, [<Optional>] headers: Map<string, string>, [<Optional>] body: obj, [<Optional>] encoding: IEncoding) : Response Future = nativeOnly
    [<NamedParams(fromIndex=1)>] member _.put (url: Uri, [<Optional>] headers: Map<string, string>, [<Optional>] body: obj, [<Optional>] encoding: IEncoding) : Response Future = nativeOnly
    [<NamedParams(fromIndex=1)>] member _.read (url: Uri, [<Optional>] headers: Map<string, string>) : Response Future = nativeOnly
    [<NamedParams(fromIndex=1)>] member _.readBytes (url: Uri, [<Optional>] headers: Map<string, string>) : uint8 list Future = nativeOnly
    // member _.send (request: BaseRequest) : Response Future = nativeOnly

[<ImportMember "package:http/http.dart">]
type Client() = inherit BaseClient()

[<ImportAll "package:http/http.dart">]
type Http() =
    static member Client() : Client = nativeOnly