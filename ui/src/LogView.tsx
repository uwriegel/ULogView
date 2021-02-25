import React, { useState, useLayoutEffect, useRef, useEffect } from 'react'
import 'virtual-table-react/dist/index.css'

export interface LogViewItem extends VirtualTableItem {
    item: string
}

export type ItemsSource = {
    count: number
    getItems: (start: number, end: number)=>Promise<LogViewItem[]|null>
}

import { 
    Column, 
    VirtualTable, 
    VirtualTableItem, 
    setVirtualTableItems,
    VirtualTableItems
} from 'virtual-table-react'

import { TextItem } from './TextItem'

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

    const refresh = () => setItems(setVirtualTableItems({count: itemSource.count, getItems: itemSource.getItems }))

    useLayoutEffect(() => {
        refresh()
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
            refresh()
            setFocused(true)
        }
    }

    const itemRenderer = (item: VirtualTableItem) => [ <TextItem item={item as LogViewItem} /> ]

    return (
        <div className='containerVirtualTable' onKeyDown={onKeydown}>
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
            <input type="text" onChange={onRestrictionsChanged} />
        </div>
    )
}