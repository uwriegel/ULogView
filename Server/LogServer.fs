module LogServer

open FSharpTools
open System.IO

let mutable private send: ((obj->unit) option) = None

let private onSocketSession (session: Types.Session) =
    let onReceive (payload: Stream) =
        use tr = new StreamReader (payload)
        ()

    let onClose () = send <- None            

    let sendBytes = session.Start onReceive onClose

    let sendObject = Json.serializeToBuffer >> sendBytes
    send <- Some sendObject

let private configuration = Configuration.create {
    Configuration.createEmpty() with 
        Port = 9865
        Requests = [ Websocket.useWebsocket "/websocketurl" onSocketSession ]
}
let private server = Server.create configuration 

let start () = server.start ()    

let sendEvent msg = 
    match send with
    | Some send -> send msg
    | None -> ()