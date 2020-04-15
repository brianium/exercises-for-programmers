// managing shared state with locks - no thanks (but I think that is the point!)
open System
open System.Threading
open System.Diagnostics

// a utility function
type Utility() =
    static let rand = Random()

    static member RandomSleep() =
        let ms = rand.Next(1, 10)
        Thread.Sleep ms

// implementation of a shared counter using locks
type LockedCounter() =

    static let _lock = Object()

    static let mutable count = 0
    static let mutable sum = 0

    static let updateState i =
        // increment the counters and...
        sum <- sum + i
        count <- count + 1
        printfn "Count is: %i. Sum is: %i" count sum

        // ..emulate a short delay
        Utility.RandomSleep()

    static member Add i =
        // see how long a client has to wait
        let stopwatch = Stopwatch()
        stopwatch.Start()

        // start lock. Same as C# lock {}
        lock _lock (fun () ->
            // see how long the wait was
            stopwatch.Stop()
            printfn "Client waited %i" stopwatch.ElapsedMilliseconds

            // do the core logic
            updateState i)

// test in isolation
// LockedCounter.Add 4
// LockedCounter.Add 5

let makeCountingTask addFunction taskId =
    async {
        let name = sprintf "Task%i" taskId
        for i in [ 1 .. 3 ] do
            addFunction i
    }

// test in isolation
// let task = makeCountingTask LockedCounter.Add 1
// Async.RunSynchronously task

let lockedExample5 = 
    [1..10]
        |> List.map (fun i -> makeCountingTask LockedCounter.Add i)
        |> Async.Parallel
        |> Async.RunSynchronously
        |> ignore
