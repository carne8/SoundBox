module Flutter.PackageInfoPlus

open Fable.Core
open Fable.Core.Dart
open Fable.Dart.Future

let [<Literal>] private package = "package:package_info_plus/package_info_plus.dart"

[<ImportMember(package)>]
type PackageInfo() =
    /// Retrieves package information from the platform. The result is cached.
    static member fromPlatform() : Future<PackageInfo> = nativeOnly

    /// The app name. CFBundleDisplayName on iOS, application/label on Android.
    member _.appName : string = nativeOnly
    /// The build number. CFBundleVersion on iOS, versionCode on Android.
    member _.buildNumber : string = nativeOnly
    /// The build signature. Empty string on iOS, signing key signature (hex) on Android.
    member _.buildSignature : string = nativeOnly
    /// Overwrite hashCode for value equality
    member _.hashCode : int = nativeOnly
    /// The installer store. Indicates through which store this application was installed.
    member _.installerStore : DartNullable<string> = nativeOnly
    /// The package name. bundleIdentifier on iOS, getPackageName on Android.
    member _.packageName : string = nativeOnly
    /// The package version. CFBundleShortVersionString on iOS, versionName on Android.
    member _.version : string = nativeOnly
