namespace ULogViewServer
open System

type LineItemIndex = {
    Index: int
    LineIndex: int
    Item: string
}

type Restriction = 
    | OrRestriction of Restriction[]
    | AndRestriction of Restriction[]
    | Text of string

type RestrictionIndex = 
    NotRestricted = 0
    | Restricted1 = 1 
    | Restricted2 = 2 
    | Restricted3 = 3 
    | Restricted4 = 4 

type TextPart = {
    Text: string
    RestrictionIndex: RestrictionIndex
}

type LogItem = {
    TextParts: TextPart[]
    Index: int
    FileIndex: int
}

type LineItem = {
    Text: string
    Index: int
    FileIndex: int
}
type LogFileSession = {
    Id: string
    LineCount: int
}   