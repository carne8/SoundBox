module Flutter.Dio

open Fable.Core
open Fable.Dart
open Fable.Dart.Future

type [<ImportMember "package:dio/dio.dart">] Response<'T> = interface end

[<ImportMember "package:dio/dio.dart">]
type Dio() =
    member _.download (url: string) (filePath: string) : Future<Response<dynamic>> = nativeOnly