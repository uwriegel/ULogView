import React, { useState, useLayoutEffect, useRef, useEffect } from 'react'
import 'virtual-table-react/dist/index.css'

export interface LogViewItem extends VirtualTableItem {
    item: string
}

export type ItemsSource = {
    count: number
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

export type LogViewProps = {
    itemSource: ItemsSource
}

export const LogView = ({itemSource }: LogViewProps) => {
    const [cols, setCols] = useState([{ name: "Eine Spalte" }] as Column[])

    const [focused, setFocused] = useState(false)
    const [items, setItems ] = useState(setVirtualTableItems({count: 0, getItems: async (s, e) =>[]}) as VirtualTableItems)
    const [restrictions, setRestrictions] = useState([] as string[])

    const input = useRef("")
        
    const onColsChanged = (cols: Column[])=> {}
    const onSort = ()=> {}

    const onFocused = (val: boolean) => setFocused(val)

    useLayoutEffect(() => {
        setItems(setVirtualTableItems({count: itemSource.count, getItems: itemSource.getItems }))
        setFocused(true)
    }, [itemSource])

    useEffect(() => {}, [restrictions])

    const onRestrictionsChanged = (evt: React.ChangeEvent<HTMLInputElement>) => {
        input.current = evt.target.value
    }

    const onKeydown = (sevt: React.KeyboardEvent) => {
        const evt = sevt.nativeEvent
        if (evt.which == 13) { // Enter
            const restrictions = 
                input.current   
                ? input.current.split(" OR ").map(n => n.split(" && ")).flat()
                : []
            setRestrictions(restrictions)
            setFocused(true)
        }
    }

    const itemRenderer = (item: VirtualTableItem) => [ <TextItem item={item as LogViewItem} restrictions={restrictions} /> ]

    return (
        <div className='containerVirtualTable' onKeyDown={onKeydown}>
            <VirtualTable 
                columns={cols} 
                isColumnsHidden={true}
                onColumnsChanged={onColsChanged} 
                onSort={onSort} 
                items={items}
                onItemsChanged ={setItems}
                itemRenderer={itemRenderer}
                focused={focused}
                onFocused={onFocused} />
            <input type="text" onChange={onRestrictionsChanged} />
        </div>
    )
}