import React, { useState, useLayoutEffect, useRef, useEffect } from 'react'
import 'virtual-table-react/dist/index.css'

export interface LogViewItem extends VirtualTableItem {
    item: string,
    index: number,
    fileIndex: number,
    highlightedItems?: TextPart[] 
}

export type ItemsSource = {
    count: number
    indexToSelect: number
    getItems: (start: number, end: number)=>Promise<LogViewItem[]>
}

import { 
    Column, 
    VirtualTable, 
    VirtualTableItem, 
    setVirtualTableItems,
    VirtualTableItems
} from 'virtual-table-react'

import { TextItem } from './TextItem'
import { LogItemResult, TextPart } from './App'

export type LogViewProps = {
    id: string
    itemSource: ItemsSource
}

export const LogView = ({id, itemSource }: LogViewProps) => {
    const [cols, setCols] = useState([{ name: "Eine Spalte" }] as Column[])

    const [focused, setFocused] = useState(false)
    const [items, setItems ] = useState(setVirtualTableItems({count: 0, getItems: async (s, e) =>[]}) as VirtualTableItems)

    const input = useRef("")
        
    const onColsChanged = (cols: Column[])=> {}
    const onSort = ()=> {}

    const onFocused = (val: boolean) => setFocused(val)

    const refresh = (indexToSelect: number | null) => setItems(setVirtualTableItems({count: itemSource.count, getItems: itemSource.getItems, currentIndex: indexToSelect || 0 }))

    useLayoutEffect(() => {
        refresh(itemSource.indexToSelect)
        setFocused(true)
    }, [itemSource])

    const onRestrictionsChanged = (evt: React.ChangeEvent<HTMLInputElement>) => {
        input.current = evt.target.value
    }

    const onKeydown = async (sevt: React.KeyboardEvent) => {
        const evt = sevt.nativeEvent
        if (evt.which == 13) { // Enter
            const data = await fetch(`http://localhost:9865/setrestrictions?id=${id}&restriction=${input.current}`)
            await data.json()
            refresh(items.currentIndex || 0)
            setFocused(true)
        }
    }

    const onTableKeydown = async (sevt: React.KeyboardEvent) => {
        const evt = sevt.nativeEvent
        if (evt.which == 114) { // F3

            // TODO do this in F# when sending new itensSource per websocket
            const currentItem = await itemSource.getItems(items.currentIndex || 0, items.currentIndex || 0)
            const indexToSelect = currentItem && currentItem[0].fileIndex

			const data = await fetch(`http://localhost:9865/toggleview?id=${id}&indexToSelect=${indexToSelect}`)
			const lineItems = (await data.json() as LogItemResult)
			evt.stopImmediatePropagation()
			evt.preventDefault()
			evt.stopPropagation()
		}
    }

    const itemRenderer = (item: VirtualTableItem) => [ <TextItem item={item as LogViewItem} /> ]

    return (
        <div className='containerVirtualTable' onKeyDown={onTableKeydown}>
            <VirtualTable 
                columns={cols} 
                isColumnsHidden={true}
                onColumnsChanged={onColsChanged} 
                onSort={onSort} 
                items={items}
                onItemsChanged={setItems}
                itemRenderer={itemRenderer}
                focused={focused}
                onFocused={onFocused} />
            <input type="text" onChange={onRestrictionsChanged} onKeyDown={onKeydown} />
        </div>
    )
}