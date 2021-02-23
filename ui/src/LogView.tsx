import React, { useState, useLayoutEffect, useRef } from 'react'
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

export type LogViewProps = {
    itemSource: ItemsSource
}

export const LogView = ({itemSource }: LogViewProps) => {
    const [cols, setCols] = useState([{ name: "Eine Spalte" }] as Column[])

    const [focused, setFocused] = useState(false)
    const [items, setItems ] = useState(setVirtualTableItems({count: 0, getItems: async (s, e) =>[], itemRenderer: i=>[]}) as VirtualTableItems)

    const input = useRef("")
        
    const onColsChanged = (cols: Column[])=> {}
    const onSort = ()=> {}

    const selectItems = () => {
        return 'textitem'
    }

    const itemRenderer = (item: VirtualTableItem) => {
        const tableItem = item as LogViewItem
        return [ <td className={selectItems()} key={1}>{tableItem.item}</td> ]
    }

    const onFocused = (val: boolean) => setFocused(val)

    useLayoutEffect(() => {
        setItems(setVirtualTableItems({count: itemSource.count, getItems: itemSource.getItems, itemRenderer }))
        setFocused(true)
    }, [itemSource])

    const onRestrictionsChanged = (evt: React.ChangeEvent<HTMLInputElement>) => {
        input.current = evt.target.value
    }

    const onKeydown = (sevt: React.KeyboardEvent) => {
        const evt = sevt.nativeEvent
        if (evt.which == 13) { // Enter
            console.log(input.current)
            setFocused(true)
        }
    }

    return (
        <div className='containerVirtualTable' onKeyDown={onKeydown}>
            <VirtualTable 
                columns={cols} 
                isColumnsHidden={true}
                onColumnsChanged={onColsChanged} 
                onSort={onSort} 
                items={items}
                onItemsChanged ={setItems}
                focused={focused}
                onFocused={onFocused} />
            <input type="text" onChange={onRestrictionsChanged} />
        </div>
    )
}