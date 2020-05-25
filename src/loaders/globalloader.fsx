#r "../_lib/Fornax.Core.dll"

type SiteInfo = {
    title: string
    description: string
}

let loader (projectRoot: string) (siteContent: SiteContents) =
    siteContent.Add({title = "DataPLANT Data annotation best practices"; description = "Concise overview over best practices for data annotation going forward"})

    siteContent
