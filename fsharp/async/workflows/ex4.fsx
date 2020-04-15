open System

// creating and nesting asynchronous workflows

let sleepWorkflow =
    async {
        printfn "Starting sleep workflow at %O" DateTime.Now.TimeOfDay
        do! Async.Sleep 2000
        printfn "Finished sleep workflow at %O" DateTime.Now.TimeOfDay
    }

// Async.RunSynchronously sleepWorkflow

let nestedWorkflow =
    async {
        printfn "Starting parent"
        let! childWorkflow = Async.StartChild sleepWorkflow

        // give teh child a chance and then keep working
        do! Async.Sleep 100
        printfn "Doing something useful while waiting "

        // block on the child
        let! result = childWorkflow

        // done
        printfn "Finished parent"
    }

// run the whole workflow
Async.RunSynchronously nestedWorkflow
