#r "paket:
nuget Fake.DotNet.Cli
nuget Fake.IO.FileSystem
nuget Fake.Core.Target //"
#load ".fake/build.fsx/intellisense.fsx"

open Fake.Core
open Fake.DotNet
open Fake.IO
open Fake.IO.FileSystemOperators
open Fake.IO.Globbing.Operators
open Fake.Core.TargetOperators

Target.initEnvironment ()

let runDotNet cmd workingDir =
    let result =
        Fake.DotNet.DotNet.exec (Fake.DotNet.DotNet.Options.withWorkingDirectory workingDir) cmd ""
    if result.ExitCode <> 0 then failwithf "'dotnet %s' failed in %s" cmd workingDir

let sitePath = Path.getFullName "./src"

Target.create "All" ignore

Target.create "GenerateLoadScript" (fun _ ->
    runDotNet "paket restore" "./"
    runDotNet "paket generate-load-scripts -t fsx" "./"
)

//Target.create "Clean" (fun _ ->
//    runDotNet "fornax clean" sitePath 
//)

Target.create "Watch" (fun _ ->
    runDotNet "fornax watch" sitePath
    )

//"Clean"
"GenerateLoadScript"
    ==> "Watch"

Target.runOrDefaultWithArguments "All"
