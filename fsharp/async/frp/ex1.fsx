// a simple event stream
open System
open System.Threading

/// create a timer and register an event handler
/// then run the timer for five seconds
let createTimer timerInterval eventHandler =
    // setup a timer
    let timer = new System.Timers.Timer(float timerInterval)
    timer.AutoReset <- true

    // add an event handler
    timer.Elapsed.Add eventHandler

    // return an async task
    async {
        // start timer
        timer.Start()
        // ...run for five seconds
        do! Async.Sleep 5000
        // ... and stop
        timer.Stop()
    }

// create a handler. The event args are ignored
let basicHandler _ = printfn "tick %A" DateTime.Now

// register the handler
let basicTimer1 = createTimer 1000 basicHandler

// run the task now
// Async.RunSynchronously basicTimer1

let createTimerAndObservable timerInterval =
    // setup a timer
    let timer = new System.Timers.Timer(float timerInterval)
    timer.AutoReset <- true

    // events are automatically IObservable
    let observable = timer.Elapsed

    // return an async task
    let task = async {
        // start timer
        timer.Start()
        // ...run for five seconds
        do! Async.Sleep 5000
        // ... and stop
        timer.Stop()
    }

    (task,observable)

// create the timer and corresponding observable
// let basicTimer2, timerEventStream = createTimerAndObservable 1000

// register that every time something happens on the event stream, print the time
// timerEventStream
// |> Observable.subscribe (fun _ -> printfn "tick %A" DateTime.Now)

// run the task now
// Async.RunSynchronously basicTimer2

/// Counting events
/// Create a timer that ticks every 500ms. 
/// At each tick, print the number of ticks so far and the current time.

// let timerCount, timerEventStream = createTimerAndObservable 500

// // set up the transformations on the event stream
// timerEventStream
// |> Observable.scan (fun count _ -> count + 1) 0
// |> Observable.subscribe (fun count -> printfn "timer ticked with count %i" count)

// // run the task now
// Async.RunSynchronously timerCount

/// Merging multiple event streams

// Create two timers, called '3' and '5'. The '3' timer ticks every 300ms and the '5' timer ticks 
// every 500ms. 

// Handle the events as follows:
// a) for all events, print the id of the time and the time
// b) when a tick is simultaneous with a previous tick, print 'FizzBuzz'
// otherwise:
// c) when the '3' timer ticks on its own, print 'Fizz'
// d) when the '5' timer ticks on its own, print 'Buzz'

type FizzBuzzEvent = {label:int; time: DateTime}

let areSimultaneous (earlierEvent,laterEvent) =
    let {label=_;time=t1} = earlierEvent
    let {label=_;time=t2} = laterEvent
    t2.Subtract(t1).Milliseconds < 50

let timer3, timerEventStream3 = createTimerAndObservable 300
let timer5, timerEventStream5 = createTimerAndObservable 500

let eventStream3 =
    timerEventStream3
    |> Observable.map (fun _ -> {label=3; time=DateTime.Now})

let eventStream5 =
    timerEventStream5
    |> Observable.map (fun _ -> {label=5; time=DateTime.Now})

// combine the two streams
let combinedStream =
    Observable.merge eventStream3 eventStream5

// make pairs of events
let pairwiseStream =
    combinedStream |> Observable.pairwise

// split the stream based on whether the pairs are simultaneous
let simultaneousStream, nonSimultaneousStream =
    pairwiseStream |> Observable.partition areSimultaneous

// split the non-simultaneous stream based on the id
let fizzStream, buzzStream =
    nonSimultaneousStream
    // convert pair of events to the first event
    |> Observable.map (fun (ev1,_) -> ev1)
    // split on whether the event id is three
    |> Observable.partition (fun {label=id} -> id=3)

// print events from the combinedStream
combinedStream
|> Observable.subscribe (fun {label=id;time=t} ->
                             printf "[%i] %i.%03i " id t.Second t.Millisecond)

// print events from the simultaneous stream
simultaneousStream
|> Observable.subscribe (fun _ -> printfn "FizzBuzz")

// print events from the nonSimultaneous streams
fizzStream
|> Observable.subscribe (fun _ -> printfn "Fizz")

buzzStream
|> Observable.subscribe (fun _ -> printfn "Buzz")

[timer3;timer5]
|> Async.Parallel
|> Async.RunSynchronously
