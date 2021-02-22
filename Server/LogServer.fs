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
let private logSessions = ConcurrentDictionary<string, LogSession>()

let private onSocketSession (session: Types.Session) =
    let onReceive (payload: Stream) = ()
    let id = string (Interlocked.Increment &sessionIdGenerator)
    let onClose () = logSessions.TryRemove(id) |> ignore 
    let sendBytes = session.Start onReceive onClose
    let sendObject = Json.serializeToBuffer >> sendBytes
    logSessions.[id] <- { Send = sendObject; Items = [||]}
    
type Command = {
    Cmd: string
    RequestId: string
    Count: int64
}

let request (requestSession: RequestSession) =
    async {
        let request = requestSession.Query.Value
        match requestSession.Query.Value.Request with
        | "initialize" ->
            let test = requestSession.Query.Value
            let logFile = test.Query "file" 
            let isAnsi = test.Query "ansi" = Some "true"
            match logFile with 
            | Some logFile -> 
                let lines = LogFile.readLog logFile isAnsi
                sessionIdGenerator <- sessionIdGenerator + 1
                //logSessions.[string sessionIdGenerator] <- lines 
                let command = {
                    Cmd = "Command"
                    RequestId = "RequestIDValue"
                    Count= 45L
                }
                do! requestSession.AsyncSendJson (command :> obj)
                return true
            | None -> 
                return false
        | "getitem" ->
            let test = requestSession.Query.Value
            match test.Query "id", test.Query "index" with
            | Some id, Some index ->
                return false
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
    
    for key in logSessions.Keys do
        let session = logSessions.Item(key)    
        let updatedSession = { session with Items = lines }
        logSessions.TryUpdate(key, updatedSession, updatedSession) |> ignore
        updatedSession.Send ({ Id = key; LineCount = lines.Length } :> obj)
    // TODO Send Loading finished