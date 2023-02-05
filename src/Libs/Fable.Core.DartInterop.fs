module Fable.Core.DartInterop

let importSideEffects (_path: string) = nativeOnly
let inline (!!) x = unbox x