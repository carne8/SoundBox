module App.Secrets

open Fable.Flutter.Dotenv

let private env =
    let env = DotEnv(false)
    env.load()
    env

let getSecret name = env.getOrElse name (fun _ -> $"{name} secret not found")