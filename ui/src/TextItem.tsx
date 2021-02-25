import React  from 'react'
import { LogViewItem } from './LogView'

export type TextItemProps = {
    item: LogViewItem
}

type restriction = {
    item: string,
    index: number
}

export const TextItem = ({item }: TextItemProps) => {
    
    //const renderRestricted = () => {
        // function getRestricted(itemToRestrict: restriction, res: restriction) {
        //     const seplen = res.item.length
        //     // get next not highlighted part, and highlighted part
        //     function* getParts() {
        //         let index = 0
        //         while (true) {
        //             const pos = itemToRestrict.item.toLowerCase().indexOf(res.item.toLowerCase(), index)
        //             yield pos != -1
        //                 ? { 
        //                     item: itemToRestrict.item.substr(index, pos - index), 
        //                     sep: itemToRestrict.item.substr(pos - index, seplen) 
        //                 }
        //                 : { 
        //                     item: itemToRestrict.item.substr(index), 
        //                     sep: "" 
        //                 }
        //             if (pos == -1)
        //                 break                            
        //             index = pos + seplen
        //         }
        //     }
        //     function getHighlighted() {
        //         let result = ""
        //         const parts = Array.from(getParts())
        //         parts.forEach(n => {
        //             result += n.item
        //             if (n.sep.length > 0)
        //                 result += `<span className='highlight${itemToRestrict.index}'>${n.sep}</span>`
        //         })
        //         return result;
        //     }
        //     return { 
        //         item: getHighlighted(), 
        //         index: itemToRestrict.index <= 3 ? itemToRestrict.index + 1 : 4
        //     }
        // }
    
        // const result = restrictions
        //     .map((n, i) => ({ item: n, index: i }))
        //     .reduce((acc, res) => getRestricted(acc, res), { item: item.item, index: 1 })

//        return <td className='textitem' key={1}>{result.item}</td> 
  //  }

    // return restrictions.length == 0
    //     ?  <td className='textitem' key={1}>{item.item}</td> 
    //     :  renderRestricted()
    return item.highlightedItems
        ? <td className='textitem' key={1}>{item.highlightedItems[0].text}</td> 
        : <td className='textitem' key={1}>{item.item}</td> 
}



    
