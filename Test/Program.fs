open FSharpTools
open ULogViewServer
open LogServer
open Restriction
open System.Threading
open System

//let testRestriction = { Restrictions = Text "module"; Keywords = [] }
//let t1 = "Das Module ist groß" |> filterRestriction testRestriction
//let t2 = "Das Programm ist groß" |> filterRestriction testRestriction


let test = "Hallo Info und USER und cache dsdkajdk info jakjd alk cachett"

let test1 = getHighlightedPartsFromText "User" test RestrictionIndex.Restricted1
let test2 = getHighlightedPartsFromText "cache" test RestrictionIndex.Restricted1

let test3 = getHighlightedPartsFromText "User" "user ksdjfkdsj" RestrictionIndex.Restricted3
let test4 = getHighlightedPartsFromText "User" "dfd user ksduserjfkdsj" RestrictionIndex.Restricted3
let test5 = getHighlightedPartsFromText "User" "dfd user ksduserjfkdsj user" RestrictionIndex.Restricted3



let test6 = getHighlightedPartsFromParts "User" [{ Text = test
                                                   RestrictionIndex = RestrictionIndex.NotRestricted 
                                                }] RestrictionIndex.Restricted1

let test7 = getHighlightedPartsFromParts "cache"  test6 RestrictionIndex.Restricted2






let restrictionString0 = "Das ist OR Julias AND Peter OR Hans AND UTE OR nix" 
let restriction0  = restrictionString0 |> Restriction.getRestriction

let restrictionString = "user AND cache OR info" 
let restriction  = restrictionString |> Restriction.getRestriction


let test8 = getHighlightedParts restriction.Keywords test






let id = createSessionId ()
createSession id (fun a -> ()) 
@"C:\Users\urieg\Desktop\CaesarProxy.log" |> indexFile
let session = logSessions.Item(id)
let result = session.Items.[140..170]




let texte () = 
    result
    |> Array.map (fun n -> getHighlightedParts restriction.Keywords n.Text)

let affen = texte ()

let texte2 = 
    result
    |> Array.Parallel.map (fun n -> getHighlightedParts restriction.Keywords n.Text)


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