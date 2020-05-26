#r "paket:
nuget Fake.DotNet.Cli
nuget Fake.IO.FileSystem
nuget Fake.Api.Github
nuget Fake.Tools.Git
nuget Fake.Core.Target //"
#load ".fake/build.fsx/intellisense.fsx"

open Fake.Core
open Fake.DotNet
open Fake.IO
open Fake.IO.FileSystemOperators
open Fake.IO.Globbing.Operators
open Fake.Core.TargetOperators

Target.initEnvironment ()

let gitOwner = "nfdi4plants"
let gitHome = sprintf "%s/%s" "https://github.com" gitOwner
let gitName = "AnnotationPrinciples"

let sitePath = Path.getFullName "./src"
let publicPath = Path.getFullName "./src/_public"

let runDotNet cmd workingDir =
    let result =
        Fake.DotNet.DotNet.exec (Fake.DotNet.DotNet.Options.withWorkingDirectory workingDir) cmd ""
    if result.ExitCode <> 0 then failwithf "'dotnet %s' failed in %s" cmd workingDir


Target.create "All" ignore

Target.create "GenerateLoadScript" (fun _ ->
    runDotNet "paket restore" "./"
    runDotNet "paket generate-load-scripts -t fsx" "./"
)

//Target.create "Clean" (fun _ ->
//    runDotNet "fornax clean" sitePath 
//)

Target.create "BuildSite" (fun _ ->
    runDotNet "fornax build" sitePath
)

Target.create "ReleaseDocsToGhPages" (fun _ ->
    let tempDocsDir = "temp/gh-pages"
    Shell.cleanDir tempDocsDir |> ignore
    Fake.Tools.Git.Repository.cloneSingleBranch "" (gitHome + "/" + gitName + ".git") "gh-pages" tempDocsDir
    Shell.copyRecursive publicPath tempDocsDir true |> printfn "%A"
    Fake.Tools.Git.Staging.stageAll tempDocsDir
    Fake.Tools.Git.Commit.exec tempDocsDir (sprintf "Update docs (%A)" System.DateTime.UtcNow)
    Fake.Tools.Git.Branches.push tempDocsDir
)


Target.create "Watch" (fun _ ->
    runDotNet "fornax watch" sitePath
    )

//"Clean"
"GenerateLoadScript"
    ==> "Watch"

"BuildSite"
    ==>"ReleaseDocsToGhPages"

Target.runOrDefaultWithArguments "All"
