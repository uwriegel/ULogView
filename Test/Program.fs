open FSharpTools
open ULogViewServer
open LogServer
open System.Threading
open System

let test = "Hallo Info und USER und cache dsdkajdk info jakjd alk cachett"

let getTextParts restrictionKey text restrictionIndex =
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

let test1 = getTextParts "User" test RestrictionIndex.Restricted1
let test2 = getTextParts "cache" test RestrictionIndex.Restricted1

let test3 = getTextParts "User" "user ksdjfkdsj" RestrictionIndex.Restricted3
let test4 = getTextParts "User" "dfd user ksduserjfkdsj" RestrictionIndex.Restricted3
let test5 = getTextParts "User" "dfd user ksduserjfkdsj user" RestrictionIndex.Restricted3

let rec getParts restrictionKey parts restrictionIndex =
    match parts with
    | head :: tail when head.RestrictionIndex = RestrictionIndex.NotRestricted ->
        let parts = getTextParts restrictionKey head.Text restrictionIndex
        parts @ getParts restrictionKey tail restrictionIndex
    | head :: tail when head.RestrictionIndex <> RestrictionIndex.NotRestricted ->
        head :: getParts restrictionKey tail restrictionIndex
    | [] -> []
    | _ -> []

let test6 = getParts "User" [{ Text = test
                               RestrictionIndex = RestrictionIndex.NotRestricted 
                            }] RestrictionIndex.Restricted1

let test7 = getParts "cache"  test6 RestrictionIndex.Restricted2

let getRestrictionsParts restrictionKeys text =
    let rec getRestrictionsParts restrictionKeys parts index =

        let nextIndex index = 
            match index with
            | RestrictionIndex.Restricted1 -> RestrictionIndex.Restricted2
            | RestrictionIndex.Restricted2 -> RestrictionIndex.Restricted3
            | _ -> RestrictionIndex.Restricted4

        match restrictionKeys with
        | head :: tail -> 
            let parts = getParts head parts index
            getRestrictionsParts tail parts (nextIndex index)
        | [] -> parts
    getRestrictionsParts restrictionKeys [{ Text = text
                                            RestrictionIndex = RestrictionIndex.NotRestricted 
                                         }] RestrictionIndex.Restricted1


let getRestriction text = 
    let getAndRestriction andPart = 
        match andPart |> String.split " && " with
        | andParts when andParts.Length > 1
            -> AndRestriction (andParts |> Array.map (fun p -> Text p))
        | andPart when andPart.Length = 1
            -> Text andPart.[0]
        | _ -> Text ""
    
    match text |> String.split " OR " with
    | orParts when orParts.Length > 1
        -> OrRestriction (orParts |> Array.map getAndRestriction)
    | orPart when orPart.Length = 1
        -> Text orPart.[0]
    | _ -> Text ""

let getKeywords = String.splitMulti [|" OR "; " && "|]
let restrictionString0 = "Das ist OR Julias && Peter OR Hans && UTE OR nix" 
let restriction0  = restrictionString0 |> getRestriction
let restrictionsKeys0 = restrictionString0 |> getKeywords    

let restrictionString = "user && cache OR info" 
let restriction  = restrictionString |> getRestriction
let restrictionKeys = restrictionString |> getKeywords |> Array.toList   


let test8 = getRestrictionsParts restrictionKeys test






let id = createSessionId ()
createSession id (fun a -> ()) 
@"C:\Users\urieg\Desktop\CaesarProxy.log" |> indexFile
let session = logSessions.Item(id)
let result = session.Items.[..30]


//let getTextparts restrictionKeys text =
//    let rec getTextparts restrictionKeys textParts =
//        let rec getTextparts restrictionKey restrictedTextParts textParts =
//            match textParts with
//            | head::tail -> 
//                restrictedTextParts
//                getTextparts restrictionKey tail
//            | [] -> 
//        8
//    getTextparts restrictionKeys [text]
        
//let parts = getTextparts restrictionKeys { Text = test; RestrictionIndex = RestrictionIndex.NotRestricted }



//let restrict restrction items =


//let restricted =
//    result
//    |> Array.map


let ende = 0