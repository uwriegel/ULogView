import React, { useState, useEffect, useRef } from 'react'
import {ItemsSource, LogView, LogViewItem} from './LogView'

var requestId = 0

type LogFileItem = {
	id: string
	lineCount: number
}

type LogItem = {
    text: string
    index?: number
    fileIndex: number
}

type LogItemResult = {
	request: number
	items: LogItem[]
}

function App() {
	const [itemSource, setItemSource] = useState({count: 0, getItems: async (s,e)=>null} as ItemsSource)
	const [id, setId] = useState("")

    const getItem = (text: string, index?: number) => ({ 
        item: text, 
        index: index || 0 
    } as LogViewItem)

    const getItems = async (id: string, start: number, end: number) => {
		const data = await fetch(`http://localhost:9865/getitems?id=${id}&req=${++requestId}&start=${start}&end=${end}`)
		const lineItems = (await data.json() as LogItemResult)
		return requestId == lineItems.request
			? lineItems.items.map(n => getItem(n.text, n.fileIndex))
			: null
	}
	
	useEffect(() => {
		const ws = new WebSocket("ws://localhost:9865/websocketurl")
		ws.onclose = () => console.log("Closed")
		ws.onmessage = p => { 
			const logFileItem = JSON.parse(p.data) as LogFileItem
			setId(logFileItem.id)
			setItemSource({count: logFileItem.lineCount, getItems: (s, e) => getItems(logFileItem.id, s, e) })
		}
	}, [])

  	return (
    	<div className="App">
			<LogView itemSource={itemSource} id={id} />
  		</div>
  	)
}

export default App
