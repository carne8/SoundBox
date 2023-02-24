module Fable.Flutter.JustAudio

open Fable.Core
open Fable.Core.Dart
open Fable.Dart.Future

let [<Literal>] package = "package:just_audio/just_audio.dart"

[<ImportMember(package)>]
type AudioPlayer() =
    [<NamedParams(fromIndex=1)>] member _.setFilePath (filePath: string, [<OptionalArgument>] preload: bool) : Future<DartNullable<System.TimeSpan>> = nativeOnly
    member _.play () : FutureVoid = nativeOnly
    member _.pause () : FutureVoid = nativeOnly
    member _.stop () : FutureVoid = nativeOnly
    member _.playing : bool = nativeOnly