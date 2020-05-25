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


Target.create "All" ignore

let runDotNet cmd workingDir =
    let result =
        Fake.DotNet.DotNet.exec (Fake.DotNet.DotNet.Options.withWorkingDirectory workingDir) cmd ""
    if result.ExitCode <> 0 then failwithf "'dotnet %s' failed in %s" cmd workingDir

let sitePath = Path.getFullName "./src" 

Target.create "Watch" (fun _ ->
    runDotNet "fornax watch" sitePath
    )

Target.runOrDefaultWithArguments "All"
