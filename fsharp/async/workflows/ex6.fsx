open System

// run workflows in series and parallel

let sleepWorkflowMs ms =
    async {
        printfn "%i ms workflow started" ms
        do! Async.Sleep ms
        printfn "%i ms workflow finished" ms
    }

let workflowInSeries =
    async {
        let! sleep1 = sleepWorkflowMs 1000
        printfn "Finished one"
        let! sleep2 = sleepWorkflowMs 2000
        printfn "Finished two"
    }

// Create workflows
let sleep1 = sleepWorkflowMs 1000
let sleep2 = sleepWorkflowMs 2000

#time
// Async.RunSynchronously workflowInSeries

// run in parallel
[sleep1; sleep2]
    |> Async.Parallel
    |> Async.RunSynchronously
#time
