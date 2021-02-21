namespace ULogViewServer
open System

type LineItemIndex = {
    Index: int
    LineIndex: int
    Item: string
}

type LineItem = {
    Text: string
    Index: int
    FileIndex: int
}

   