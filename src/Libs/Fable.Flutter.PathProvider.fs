module Fable.Flutter.PathProvider

open Fable.Core
open Fable.Core.Dart
open Fable.Dart.Future
open Fable.Dart.IO

[<ImportAll "package:path_provider/path_provider.dart">]
type PathProvider =
    /// Path to a directory where the application may place data that is user-generated, or that cannot otherwise be recreated by your application.
    static member getApplicationDocumentsDirectory() : Future<Directory> = nativeOnly
    /// Path to a directory where the application may place application support files.
    static member getApplicationSupportDirectory() : Future<Directory> = nativeOnly
    /// Path to the directory where downloaded files can be stored.
    static member getDownloadsDirectory() : Future<DartNullable<Directory>> = nativeOnly
    /// Paths to directories where application specific cache data can be stored externally.
    static member getExternalCacheDirectories() : Future<DartNullable<seq<Directory>>> = nativeOnly
    /// Path to a directory where the application may access top level storage.
    static member getExternalStorageDirectory() : Future<DartNullable<Directory>> = nativeOnly
    /// Path to the directory where application can store files that are persistent, backed up, and not visible to the user, such as sqlite.db.
    static member getLibraryDirectory() : Future<Directory> = nativeOnly
    /// Path to the temporary directory on the device that is not backed up and is suitable for storing caches of downloaded files.
    static member getTemporaryDirectory() : Future<Directory> = nativeOnly