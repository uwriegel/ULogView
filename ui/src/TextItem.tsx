import React  from 'react'
import { TextPart } from './App'
import { LogViewItem } from './LogView'

export type TextItemProps = {
    item: LogViewItem
}

export const TextItem = ({item }: TextItemProps) => {
    const getHighlichtClass = (part: TextPart) => `highlight${part.restrictionIndex}`

    const getHighlightedParts = () => 
        item.highlightedItems!!.map(part => (
            <span className={getHighlichtClass(part)}>{part.text}</span>
        ))    

    return item.highlightedItems
        ? <td className='textitem' key={1}>{getHighlightedParts()}</td> 
        : <td className='textitem' key={1}>{item.item}</td> 
}



    
