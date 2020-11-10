#time "on"

#r "nuget:Akka.Fsharp"
#r "nuget:Akka.Testkit"

open System
open System.Diagnostics
open Akka.Actor
open Akka.Configuration
open Akka.FSharp
open Akka.TestKit

// #Using Actor
// Actors are one of Akka's concurrent models.
// An Actor is a like a thread instance with a mailbox. 
// It can be created with system.ActorOf: use receive to get a message, and <! to send a message.
// This example is an EchoServer which can receive messages then print them.

let nCores = Environment.ProcessorCount
let myClock = Stopwatch()
myClock.Start()
let processId = Process.GetCurrentProcess()
let cpuStartTime = processId.TotalProcessorTime 
let system = ActorSystem.Create("FSharp")

let lucasSqr s k =
   
    let _last1 = k-1
    let _last=s+_last1
    let myS:int64 = int64 s
    let myK:int64 = int64 k
    let _list1 = [myS..myS+myK-int64(1)]
    let _list2 = [ for i in _list1 -> i*i ]
    let sum = List.sumBy (fun elem -> elem) _list2
    let sq=float(sum) |> sqrt
    let st=truncate sq
    st = sq

type ChildActor =
        inherit Actor

        override x.OnReceive message =
            let threadId = Threading.Thread.CurrentThread.ManagedThreadId
            match message with
            | :? string as msg ->
                
                let aray=msg.Split [|'_'|]
                let start=aray.[0] |> int
                let k=aray.[1] |> int
                let ans= lucasSqr start k
                  
                if ans = true then
                    printfn"\n %A \n"start 
                                         
            | _ ->  failwith "unknown message"
            


type BossActor =
    inherit Actor

    override x.OnReceive message =
        match message with
        | :? string as msg -> 
             let arr = msg.Split [|'_'|]
             let n = arr.[0] |> int
             let k = arr.[1] |> int
             let total = 100
             let workUnitDoub = ceil (double n/double total)
             let workUnit:int = int workUnitDoub
             let childActors = 
                   [0  .. total]
                   |> List.map(fun id ->  system.ActorOf(Props(typedefof<ChildActor>, Array.empty)))
             for id in [0 .. workUnit] do
                let start = id*total  
                for j in [1 .. total] do
                  let current = start + j
                  if (current <= n && j <= n) then
                     j |> List.nth childActors <! sprintf "%d_%d" current k                       
                
        | _ ->  failwith "unknown message"
        let finalRTime = myClock.ElapsedMilliseconds
        printfn "Real Time for calculation %d ms" finalRTime
        let finalCpuTime = int64 (processId.TotalProcessorTime - cpuStartTime).TotalMilliseconds
        printfn "CPU Time --> %d ms" finalCpuTime
        if finalCpuTime > finalRTime then
               printfn "Number of Cores = %d, Cpu Time / Real Time = %f" nCores ((float finalCpuTime)/(float finalRTime))
        printfn "end"
    

let bossActor = system.ActorOf(Props(typedefof<BossActor>, Array.empty))
let args : string array = fsi.CommandLineArgs |> Array.tail
let final=args.[0] |> int
let kk=args.[1] |> int
let inp = sprintf "%d_%d"final kk
bossActor <! inp 
     
System.Console.ReadLine() |> ignore
       