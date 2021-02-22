import React, { useState, useLayoutEffect } from 'react'
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


    onChangeArray: () =>void



}

export const LogView = ({itemSource, onChangeArray}: LogViewProps) => {
    const [cols, setCols] = useState([{ name: "Eine Spalte" }] as Column[])

    const [focused, setFocused] = useState(false)
    const [items, setItems ] = useState(setVirtualTableItems({count: 0, getItems: async (s, e) =>[], itemRenderer: i=>[]}) as VirtualTableItems)
        
    const onColsChanged = (cols: Column[])=> {}
    const onSort = ()=> {}

    const itemRenderer = (item: VirtualTableItem) => {
        const tableItem = item as LogViewItem
        return [ <td key={1}>{tableItem.item}</td> ]
    }

    const onSetFocus = () => setFocused(true)   

    const onFocused = (val: boolean) => setFocused(val)

    useLayoutEffect(() => 
        setItems(setVirtualTableItems({count: itemSource.count, getItems: itemSource.getItems, itemRenderer, currentIndex: 45}))
    , [itemSource])

    return (
        <div className='rootVirtualTable'>
            <h1>Virtual Table</h1>
            <button onClick={onChangeArray}>Fill array</button>
            <button onClick={onSetFocus}>Set Focus</button>
            <div className='containerVirtualTable'>
                <VirtualTable 
                    columns={cols} 
                    isColumnsHidden={true}
                    onColumnsChanged={onColsChanged} 
                    onSort={onSort} 
                    items={items}
                    onItemsChanged ={setItems}
                    focused={focused}
                    onFocused={onFocused} />
            </div>
        </div>
    )
}