import React  from 'react'
import { LogViewItem } from './LogView'

export type TextItemProps = {
    item: LogViewItem
    restrictions: string[]
}

export const TextItem = ({item, restrictions }: TextItemProps) => {
    return restrictions.length == 0
        ?  <td className='textitem' key={1}>{item.item}</td> 
        :  <td className='textitem' key={1}> Schön {item.item}</td> 
}



// const getRestricted = item => {
//     function getRestricted(itemToRestrict, res) {
//         const seplen = res.length
//         // get next not highlighted part, and highlighted part
//         function* getParts() {
//             let index = 0
//             while (true) {
//                 const pos = itemToRestrict.item.toLowerCase().indexOf(res.toLowerCase(), index)
//                 yield pos != -1
//                     ? { 
//                         item: itemToRestrict.item.substr(index, pos - index), 
//                         sep: itemToRestrict.item.substr(pos - index, seplen) 
//                     }
//                     : { 
//                         item: itemToRestrict.item.substr(index), 
//                         sep: "" 
//                     }
//                 if (pos == -1)
//                     break                            
//                 index = pos + seplen
//             }
//         }
//         function getHighlighted() {
//             let result = ""
//             const parts = Array.from(getParts())
//             parts.forEach(n => {
//                 result += n.item
//                 if (n.sep.length > 0)
//                     result += `<span class='event-log-vue-selected${itemToRestrict.index}'>${n.sep}</span>`
//             })
//             return result;
//         }
//         return { 
//             item: getHighlighted(), 
//             index: itemToRestrict.index <= 3 ? itemToRestrict.index + 1 : 4
//         }
//     }
    
//     const result = this.restrictions.reduce((acc, res) => getRestricted(acc, res), { item, index: 1 })
//     return result.item
// }
