module Fable.Dart.Environment

open Fable.Core
open Fable.Core.Dart

[<ImportMember "dart:core">]
type String =
    [<IsConst; NamedParams(fromIndex=1)>] static member fromEnvironment (key: string, [<OptionalArgument>] defaultValue: string) : string = nativeOnly