#r "../_lib/Fornax.Core.dll"
#load "layout.fsx"

open Html

let generate' (ctx : SiteContents) (_: string) =

    let buildingBlocks =
        ctx.TryGetValues<Buildingblockloader.BuildingBlock> ()
        |> Option.defaultValue Seq.empty
        |> List.ofSeq


    Layout.layout ctx "Home" (
        buildingBlocks
        |> List.map (fun bb -> 
            div [
                Color bb.Color
                HtmlProperties.Style [BackgroundColor bb.BackgroudColor] 
                Class "section "
            ] [
                h1 [
                    Class "title has-text-centered is-size-1"
                    HtmlProperties.Style [CSSProperties.Custom("color",bb.Color)]
                ] [
                    !!bb.Name
                ]
                br []
                h3 [
                    Class "subtitle has-text-centered is-size-2"
                    HtmlProperties.Style [CSSProperties.Custom("color",bb.Color)]    
                ] [
                    !! bb.Description
                ]
                h3 [
                    Class "subtitle has-text-centered is-size-2"
                    HtmlProperties.Style [CSSProperties.Custom("color",bb.Color)]    
                ] [
                    !! bb.DescriptionSub
                ]
                div [Class "container"; HtmlProperties.Style [MaxWidth "50%"]] [
                    figure [Class "image is-2by1 is-centered"] [
                        img [Src bb.Image]
                    ]
                ]
            ] 
        )
    )
    

let generate (ctx : SiteContents) (projectRoot: string) (page: string) =
  generate' ctx page
  |> Layout.render ctx