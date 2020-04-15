open System.Net
open System
open System.IO
open Microsoft.FSharp.Control.CommonExtensions

// async web downloader

let fetchUrl url =
    async {
        let req = WebRequest.Create(Uri(url))
        use! resp = req.AsyncGetResponse()
        use stream = resp.GetResponseStream()
        use reader = new IO.StreamReader(stream)
        let html = reader.ReadToEnd()
        printfn "finished downloading %s" url
    }

let sites = ["http://www.bing.com";
             "http://www.google.com";
             "http://www.microsoft.com";
             "http://www.amazon.com";
             "http://www.yahoo.com"]

#time
sites
|> List.map fetchUrl
|> Async.Parallel
|> Async.RunSynchronously
#time
