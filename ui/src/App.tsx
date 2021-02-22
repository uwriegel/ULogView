import React, { useState, useEffect,



useRef


} from 'react'
import { setVirtualTableItems, VirtualTableItem, VirtualTableItems } from 'virtual-table-react'
import {ItemsSource, LogView, LogViewItem} from './LogView'

function App() {
	const [itemSource, setItemSource] = useState({count: 0, getItems: async (s,e)=>[]} as ItemsSource)

    const getItem = (index: number) => ({ 
        item: `Name ${index}`, 
        index: index 
    } as LogViewItem)

    const getItems = (start: number, end: number) => 
        new Promise<LogViewItem[]>(res => setTimeout(() => 
            res(Array.from(Array(end - start + 1).keys()).map(i => getItem(i + start))), 30))



	const tableItems = useRef([] as VirtualTableItem[])
	const onChangeArray = () => {
		tableItems.current = Array.from(Array(60).keys()).map(index => getItem(index))
		setItemSource({count: tableItems.current.length, getItems})
	}
	
	const onclick = async () =>{
		var data = await fetch("http://localhost:9865/affen")
		console.log("data", data)
		const json = await data.json()
		console.log("data json", json)
	}

	useEffect(() => {
		const ws = new WebSocket("ws://localhost:9865/websocketurl")
		ws.onclose = () => console.log("Closed")
		ws.onmessage = p => console.log(JSON.parse(p.data))
	}, [])

  	return (
    	<div className="App">
			<button onClick={onclick}>Abfrage</button>
			<LogView itemSource={itemSource} onChangeArray={onChangeArray}/>
  		</div>
  	)
}

export default App
