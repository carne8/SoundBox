open System.IO

let rec getAllFsFiles path : string array =
    let subDirFiles =
        path
        |> Directory.GetDirectories
        |> Array.collect getAllFsFiles

    let currentDirFiles = path |> Directory.GetFiles

    Array.append subDirFiles currentDirFiles
    |> Array.filter (Path.GetFileName >> (fun fileName -> fileName.EndsWith ".fs"))

let getCompiledFile (fsFilePath: string) = fsFilePath.Replace("\\src", "\\lib").Replace(".fs", ".dart")


let srcPath = __SOURCE_DIRECTORY__ + "\\src"

srcPath
|> getAllFsFiles
|> Array.filter (File.ReadAllLines >> Array.contains "// import \"dart:core\";")
|> Array.map getCompiledFile
|> Array.map (fun path -> path, File.ReadAllLines path)
|> Array.map (fun (path, content) -> path, Array.append [| "import \"dart:core\";" |] content)
|> Array.map (fun (path, content) -> File.WriteAllLines(path, content); path)
|> Array.map (fun path -> Path.GetRelativePath(__SOURCE_DIRECTORY__, path))
|> Array.iter (printf "Fixed %s")