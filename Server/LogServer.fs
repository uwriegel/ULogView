module LogServer

open FSharpTools
open System.IO
open Session

let mutable private send: ((obj->unit) option) = None

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
        | "affen" ->
            let test = requestSession.Query.Value
            let param1 = test.Query "param1" 
            let param2 = test.Query "param2"
            let param3 = test.Query "param41"

            let command = {
                Cmd = "Command"
                RequestId = "RequestIDValue"
                Count= 45L
            }
            //System.Threading.Thread.Sleep 3
            do! requestSession.AsyncSendJson (command :> obj)
            return true
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