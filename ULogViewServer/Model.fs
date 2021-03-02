namespace ULogViewServer
open System

type EventType = 
    LogFileSession = 0
    | Progress = 1

type LogFileSession = {
    Id: string
    LineCount: int
    IndexToSelect: int
    EventType: EventType
}   

type Progress = {
    Progress: int64
    Loading: bool
    EventType: EventType
}

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

type LogItem = {
    HighlightedText: TextPart[] 
    Text: string
    Index: int
    FileIndex: int
}

type LogItemResult = {
    Request: int
    Items: LogItem[]
}

type LineItem = {
    Text: string
    Index: int
    FileIndex: int
}

