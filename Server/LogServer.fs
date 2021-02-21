module LogServer

open System.Collections.Concurrent
open System.IO

open FSharpTools
open Session
open ULogViewServer

let mutable private send: ((obj->unit) option) = None

let mutable private sessionIdGenerator = 0
let private logSessions = ConcurrentDictionary<string, LineItem[]>()

let private onSocketSession (session: Types.Session) =
    let onReceive (payload: Stream) =
        use tr = new StreamReader (payload)
        ()

    let onClose () = send <- None            

    let sendBytes = session.Start onReceive onClose

    let sendObject = Json.serializeToBuffer >> sendBytes
    send <- Some sendObject

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
                logSessions.[string sessionIdGenerator] <- lines 
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

let sendEvent msg = 
    match send with
    | Some send -> send msg
    | None -> ()

let indexFile logFile =
    // TODO Send Loading...
    let lines = LogFile.readLog logFile true
    sessionIdGenerator <- sessionIdGenerator + 1
    logSessions.[string sessionIdGenerator] <- lines 
    // TODO Send Loading finished