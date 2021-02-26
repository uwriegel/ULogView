module Restriction

open System

open FSharpTools
open ULogViewServer


let getRestriction restrictionString = 
    let getAndRestriction andPart = 
        match andPart |> String.split " AND " with
        | andParts when andParts.Length > 1
            -> AndRestrictions (andParts |> Array.map (fun p -> Text p))
        | andPart when andPart.Length = 1
            -> Text andPart.[0]
        | _ -> Text ""

    let getKeywords = String.splitMulti [|" OR "; " AND "|]
    
    {
        Restrictions = 
            match restrictionString |> String.split " OR " with
            | orParts when orParts.Length > 1
                -> OrRestrictions (orParts |> Array.map getAndRestriction)
            | orPart when orPart.Length = 1
                -> Text orPart.[0]
            | _ -> Text ""
        Keywords = restrictionString |> getKeywords |> Array.toList   
    }

let getHighlightedPartsFromText restrictionKey text restrictionIndex =
    let rec restrict text =
        match text |> String.indexOfCompare restrictionKey StringComparison.CurrentCultureIgnoreCase with
        | Some index when index = 0 -> 
            let res = { Text = text.[..index + restrictionKey.Length - 1]; RestrictionIndex = restrictionIndex } 
            res :: restrict text.[index + restrictionKey.Length..]
        | Some index when index = text.Length - restrictionKey.Length -> 
            let first = { Text = text.[..index-1]; RestrictionIndex = RestrictionIndex.NotRestricted } 
            let res = { Text = text.[index..index + restrictionKey.Length - 1]; RestrictionIndex = restrictionIndex } 
            first :: [ res ]
        | None ->  [{ Text = text; RestrictionIndex = RestrictionIndex.NotRestricted }]
        | Some index -> 
            let first = { Text = text.[..index-1]; RestrictionIndex = RestrictionIndex.NotRestricted } 
            let res = { Text = text.[index..index + restrictionKey.Length - 1]; RestrictionIndex = restrictionIndex } 
            first :: [ res ] @ restrict text.[index + restrictionKey.Length..]
    restrict text

let rec getHighlightedPartsFromParts restrictionKey parts restrictionIndex =
    match parts with
    | head :: tail when head.RestrictionIndex = RestrictionIndex.NotRestricted ->
        let parts = getHighlightedPartsFromText restrictionKey head.Text restrictionIndex
        parts @ getHighlightedPartsFromParts restrictionKey tail restrictionIndex
    | head :: tail when head.RestrictionIndex <> RestrictionIndex.NotRestricted ->
        head :: getHighlightedPartsFromParts restrictionKey tail restrictionIndex
    | [] -> []
    | _ -> []

let getHighlightedParts restrictionKeys text =
    let rec getRestrictionsParts restrictionKeys parts index =

        let nextIndex index = 
            match index with
            | RestrictionIndex.Restricted1 -> RestrictionIndex.Restricted2
            | RestrictionIndex.Restricted2 -> RestrictionIndex.Restricted3
            | _ -> RestrictionIndex.Restricted4

        match restrictionKeys with
        | head :: tail -> 
            let parts = getHighlightedPartsFromParts head parts index
            getRestrictionsParts tail parts (nextIndex index)
        | [] -> parts
    getRestrictionsParts restrictionKeys [{ Text = text
                                            RestrictionIndex = RestrictionIndex.NotRestricted 
                                         }] RestrictionIndex.Restricted1

let rec filterRestriction text restrictions = 
    match restrictions with
    | Text restriction -> 
        text |> String.containsComparison restriction StringComparison.CurrentCultureIgnoreCase 
    | OrRestrictions orr -> 
        orr |> Array.exists (filterRestriction text)
    | _ -> false

// TODO AND