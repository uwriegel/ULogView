module LogFile

open System.Globalization
open System.IO
open System.Text

open ULogViewServer

let readLog filePath isUtf8 sendProgress = 
    Encoding.RegisterProvider CodePagesEncodingProvider.Instance 
    let encoding = if isUtf8 then Encoding.UTF8 else Encoding.GetEncoding (CultureInfo.CurrentCulture.TextInfo.ANSICodePage)
    use stream = new FileStream (filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite) 
    use reader = new StreamReader (stream, encoding)

    let rec readLines progress index = 
        seq {
            let textline = reader.ReadLine ()
            let newProgress = 
                if index > 1000 && index % 1000 = 0 then
                    let length = reader.BaseStream.Length
                    let currentPosition = reader.BaseStream.Position
                    let newProgress = int64 (double currentPosition / double length * 100.0)
                    if newProgress > progress then sendProgress newProgress
                    newProgress
                else
                    progress
            match isNull textline with
            | false -> 
                    yield textline
                    yield! readLines newProgress (index + 1)
            | true -> sendProgress 100L
        }
        
    readLines 0L 0
    |> Seq.mapi (fun i n -> { Text = n; Index = i; FileIndex = i; })
    |> Seq.toArray
        
        