open System
open System.Threading

// cancelling workflows

let testLoop =
    async {
        for i in [1..100] do
            // do something
            printf "%i before.." i

            // sleep a bit
            do! Async.Sleep 10
            printfn "..after"
        }

// create a cancellation source
use cancellationSource = new CancellationTokenSource()

Async.Start (testLoop, cancellationSource.Token)

// wait a bit
Thread.Sleep(200)

// cancel after 200ms
cancellationSource.Cancel()
