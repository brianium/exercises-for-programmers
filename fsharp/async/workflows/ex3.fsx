open System
open Microsoft.FSharp.Control

// async file write by wrapping the IAsyncResult

let fileWriteWithAsync =

    // create a stream to write to
    use stream = new IO.FileStream("test.txt", System.IO.FileMode.Create)

    // start
    printfn "Starting async write"
    let asyncResult = stream.BeginWrite(Array.empty,0,0,null,null)

    // create an async wrapper around an IAsyncResult
    let async = Async.AwaitIAsyncResult(asyncResult) |> Async.Ignore

    // keep working
    printfn "Doing something useful while waiting for the write to complete"

    // block on the result now by waiting for the async to complete
    Async.RunSynchronously async

    // done
    printfn "Async write completed"
