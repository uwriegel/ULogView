namespace ULogViewServer
open System

type LineItemIndex = {
    Index: int
    LineIndex: int
    Item: string
}

type Restrictions = 
    | OrRestrictions of Restrictions[]
    | AndRestrictions of Restrictions[]
    | Text of string

type Restriction = {
    Restrictions: Restrictions
    Keywords: string list
}

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

type LineItem = {
    HighlightedText: TextPart[] 
    Text: string
    Index: int
    FileIndex: int
}

type LogFileSession = {
    Id: string
    LineCount: int
}   