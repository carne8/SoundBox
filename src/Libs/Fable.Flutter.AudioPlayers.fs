namespace Fable.Flutter

open Fable.Core
open Fable.Core.Dart
open Fable.Dart.Future

module JustAudio =
    let [<Literal>] package = "package:just_audio/just_audio.dart"

    [<ImportMember(package)>]
    type AudioPlayer() =
        [<NamedParams(fromIndex=1)>] member _.setUrl (url: string, [<OptionalArgument>] preload: bool) : Future<DartNullable<System.TimeSpan>> = nativeOnly
        [<NamedParams(fromIndex=1)>] member _.setFilePath (filePath: string, [<OptionalArgument>] preload: bool) : Future<DartNullable<System.TimeSpan>> = nativeOnly
        member _.play () : FutureVoid = nativeOnly
        member _.pause () : FutureVoid = nativeOnly
        member _.stop () : FutureVoid = nativeOnly
        member _.seek (position: System.TimeSpan) : FutureVoid = nativeOnly
        member _.playing : bool = nativeOnly

module AudioPlayers =
    let [<Literal>] package = "package:audioplayers/audioplayers.dart"

    [<ImportMember(package); RequireQualifiedAccess>]
    type PlayerMode =
        static member mediaPlayer : PlayerMode = nativeOnly
        static member lowLatency : PlayerMode = nativeOnly

    [<ImportMember(package); RequireQualifiedAccess>]
    type ReleaseMode =
        static member release : ReleaseMode = nativeOnly
        static member loop : ReleaseMode = nativeOnly
        static member stop : ReleaseMode = nativeOnly

    [<ImportMember(package); RequireQualifiedAccess>]
    type PlayerState =
        static member stopped : PlayerState = nativeOnly
        static member playing : PlayerState = nativeOnly
        static member paused : PlayerState = nativeOnly
        static member completed : PlayerState = nativeOnly

    [<RequireQualifiedAccess>]
    module PlayerState =
        let (|Stopped|Playing|Paused|Completed|) x =
            if x = PlayerState.stopped then Stopped
            elif x = PlayerState.playing then Playing
            elif x = PlayerState.paused then Paused
            else Completed

    [<ImportMember(package)>]
    type Source() = class end

    type [<ImportMember(package)>] AssetSource(path: string) = inherit Source()
    type [<ImportMember(package)>] BytesSource(bytes: uint8 []) = inherit Source()
    type [<ImportMember(package)>] DeviceFileSource(path: string) = inherit Source()
    type [<ImportMember(package)>] UrlSource(url: string) = inherit Source()


    [<ImportMember(package)>]
    type AudioPlayer() =
        member _.playerId: string = nativeOnly
        member val state: PlayerState = nativeOnly with get, set

        /// Closes all StreamControllers.
        member _.dispose() : FutureVoid = nativeOnly
        member _.getCurrentPosition() : Future<DartNullable<System.TimeSpan>> = nativeOnly
        /// Get audio duration after setting url. Use it in conjunction with setUrl.
        member _.getDuration() : Future<DartNullable<System.TimeSpan>> = nativeOnly
        /// Pauses the audio that is currently playing.
        member _.pause() : FutureVoid = nativeOnly
        /// Releases the resources associated with this media player.
        member _.release() : FutureVoid = nativeOnly
        [<NamedParams(fromIndex=1)>] member _.play(source: Source, [<OptionalArgument>] volume: float, [<OptionalArgument>] balance: float, (*[<OptionalArgument>] ctx: AudioContext,*) [<OptionalArgument>] position: System.TimeSpan, [<OptionalArgument>] mode: PlayerMode) : FutureVoid = nativeOnly
        /// Resumes the audio that has been paused or stopped.
        member _.resume() : FutureVoid = nativeOnly
        /// Moves the cursor to the desired position.
        member _.seek(position: System.TimeSpan) : FutureVoid = nativeOnly
        // member _.setAudioContext(ctx: AudioContext) : FutureVoid = nativeOnly
        /// Sets the stereo balance.
        member _.setBalance(balance: float) : FutureVoid = nativeOnly
        /// Sets the playback rate - call this after first calling play() or resume().
        member _.setPlaybackRate(playbackRate: float) : FutureVoid = nativeOnly
        /// Sets the release mode.
        member _.setReleaseMode(releaseMode: ReleaseMode) : FutureVoid = nativeOnly
        member _.setPlayerMode(mode: PlayerMode) : FutureVoid = nativeOnly
        /// Sets the audio source for this player.
        member _.setSource(source: Source) : FutureVoid = nativeOnly
        /// Sets the URL to an asset in your Flutter application. The global instance of AudioCache will be used by default.
        member _.setSourceAsset(path: string) : FutureVoid = nativeOnly
        member _.setSourceBytes(bytes: uint8 []) : FutureVoid = nativeOnly
        /// Sets the URL to a file in the users device.
        member _.setSourceDeviceFile(path: string) : FutureVoid = nativeOnly
        /// Sets the URL to a remote link.
        member _.setSourceUrl(url: string) : FutureVoid = nativeOnly
        /// Sets the volume (amplitude).
        member _.setVolume(volume: float) : FutureVoid = nativeOnly
        /// Stops the audio that is currently playing.
        member _.stop() : FutureVoid = nativeOnly