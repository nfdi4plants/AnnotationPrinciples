#r "../_lib/Fornax.Core.dll"
#I "../../.paket/load/netcoreapp3.1"
#load "main.group.fsx"

open Newtonsoft.Json
open System.IO


type BuildingBlock = {
    Name            : string
    Description     : string
    DescriptionSub  : string
    BackgroudColor  : string
    Color           : string
    Image           : string
}

let contentDir = "buildingblocks"

let jsonFromFile<'T> path =
    JsonConvert.DeserializeObject<'T>(File.ReadAllText(path))
    

let loader (projectRoot: string) (siteContent: SiteContents) =

    let blocksPath = System.IO.Path.Combine(projectRoot, contentDir)
    System.IO.Directory.GetFiles blocksPath
    |> Array.filter (fun n -> n.EndsWith ".json")
    |> Array.map jsonFromFile<BuildingBlock>
    |> Array.iter (fun b -> siteContent.Add b)

    siteContent
