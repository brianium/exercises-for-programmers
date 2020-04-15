open System
open Microsoft.FSharp.Control

// Asynchronous workflows

let userTimerWithAsync =

    // create a timer and associated async event
    let timer = new System.Timers.Timer(2000.0)
    let timerEvent = Async.AwaitEvent (timer.Elapsed) |> Async.Ignore

    // start
    printfn "Waiting for timer at %O" DateTime.Now.TimeOfDay
    timer.Start()

    // keep working
    printfn "Doing something useful while waiting for an event"

    // block on the timer event now by waiting for the async to complete
    Async.RunSynchronously timerEvent

    // done
    printfn "Timer ticked at %O" DateTime.Now.TimeOfDay

