module LogServer

open System.Collections.Concurrent
open System.IO
open System.Threading

open FSharpTools
open Session
open ULogViewServer

type LogSession = {
    Send: (obj->unit)
    Items: LineItem[] 
}

let mutable private sessionIdGenerator = 0
let mutable logSessions = Map.empty<string, LogSession>

let private onSocketSession (session: Types.Session) =
    let onReceive (payload: Stream) = ()
    let id = string (Interlocked.Increment &sessionIdGenerator)
    let onClose () = logSessions <- logSessions.Remove id 
    let sendBytes = session.Start onReceive onClose
    let sendObject = Json.serializeToBuffer >> sendBytes
    logSessions <- logSessions.Add (id, { Send = sendObject; Items = [||]})
    
type Command = {
    Cmd: string
    RequestId: string
    Count: int64
}

let request (requestSession: RequestSession) =
    async {
        let request = requestSession.Query.Value
        match requestSession.Query.Value.Request with
        | "getitems" ->
            let test = requestSession.Query.Value
            match test.Query "id", test.Query "start", test.Query "end" with
            | Some id, Some startIndex, Some endIndex ->
                let session = logSessions.Item(id)
                let result = session.Items.[int startIndex..int endIndex]
                do! requestSession.AsyncSendJson (result :> obj)
                return true
            | _ -> return false
        | _ -> return false
    }

let private configuration = Configuration.create {
    Configuration.createEmpty() with 
        Port = 9865
        AllowOrigins = Some [| "http://localhost:3000"; |]
        Requests = [ 
            Websocket.useWebsocket "/websocketurl" onSocketSession
            request
        ]
}
let private server = Server.create configuration 

let start () = server.start ()    

let indexFile logFile =
    // TODO Send Loading...
    let lines = LogFile.readLog logFile true
    
    logSessions <- logSessions |> Map.map (fun k item  -> { item with Items = lines })
    logSessions |> Map.iter (fun key item -> item.Send ({ Id = key; LineCount = lines.Length } :> obj)) 
    // TODO Send Loading finished