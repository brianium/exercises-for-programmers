open System

// Traditional asynchronous programming

let userTimerWithCallback =
    // create an event to wait on
    let event = new System.Threading.AutoResetEvent(false)

    // create a timer and add an event handler that will signal the event
    let timer = new System.Timers.Timer(2000.0)
    timer.Elapsed.Add (fun _ -> event.Set() |> ignore)

    // start
    printfn "Waiting for timer at %O" DateTime.Now.TimeOfDay
    timer.Start()

    // keep working
    printfn "Doing something useful while waiting for event"

    // block on the timer via the AutoResetEvent
    event.WaitOne() |> ignore

    // done
    printfn "Timer ticket at %O" DateTime.Now.TimeOfDay
