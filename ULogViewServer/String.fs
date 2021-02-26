namespace FSharpTools

module String = 

    open System
    
    /// <summary>
    /// Splits a string into parts, separator is one char
    /// If the string is null, an emtpy array is returned
    /// </summary>
    /// <param name="sep">The separator</param>
    /// <param name="str">String to be splitted</param>
    /// <returns>The trimmed string</returns>
    let split (sep: string) (str: string) =
        match isNull str, isNull sep with
        | false, false -> str.Split ([|sep|], StringSplitOptions.RemoveEmptyEntries)
        | false, true -> [|str|]
        |_ -> [||]

    /// <summary>
    /// Splits a string into parts, separator is one char
    /// If the string is null, an emtpy array is returned
    /// </summary>
    /// <param name="seps">The separators</param>
    /// <param name="str">String to be splitted</param>
    /// <returns>The trimmed string</returns>
    let splitMulti (seps: string[]) (str: string) =
        match isNull str, isNull seps with
        | false, false -> str.Split (seps, StringSplitOptions.RemoveEmptyEntries)
        | false, true -> [|str|]
        |_ -> [||]

    let containsComparison (test: string) comparison (str: string) =
        match isNull str, isNull test with
        | false, false -> 
            str.Contains(test, comparison)
        | _ -> false

