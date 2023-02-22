/// https://pub.dev/packages/dotenv
module Fable.Flutter.Dotenv

open Fable.Core
open Fable.Core.Dart

[<ImportMember "package:dotenv/dotenv.dart">]
type DotEnv [<NamedParams>] (includePlatformEnvironment: bool) =
    static member (?) (a: DotEnv, b: obj) : DartNullable<string> = emitExpr (a, b) "$0[$1]"
    member _.load () : unit = nativeOnly
    member _.load (filenames: seq<string>) : unit = nativeOnly
    member _.getOrElse (key: string) (orElse: unit -> string) : string = nativeOnly
    member _.isDefined (key: string) : bool = nativeOnly
    member _.isEveryDefined (keys: seq<string>) : bool = nativeOnly
