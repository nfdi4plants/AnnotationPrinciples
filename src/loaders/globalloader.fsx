#r "../_lib/Fornax.Core.dll"

type SiteInfo = {
    title: string
    description: string
}

let loader (projectRoot: string) (siteContent: SiteContents) =
    siteContent.Add({title = "Sample Fornax blog"; description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit"})

    siteContent
