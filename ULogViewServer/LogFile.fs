module LogFile

open System.Globalization
open System.IO
open System.Text

open ULogViewServer

let readLog filePath isUtf8 = 
    let encoding = if isUtf8 then Encoding.UTF8 else Encoding.GetEncoding (CultureInfo.CurrentCulture.TextInfo.ANSICodePage)
    let stream = new FileStream (filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite) 
    let reader = new StreamReader (stream, encoding)

    let rec readLines () = 
        seq {
            let textline = reader.ReadLine ()
            match isNull textline with
            | false -> 
                    yield textline
                    yield! readLines ()
            | true -> ()
        }
        
    readLines ()
    |> Seq.mapi (fun i n -> { Text = n; Index = i; FileIndex = i})
    |> Seq.toArray
        
        