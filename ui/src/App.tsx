import React, { useState, useEffect, useRef } from 'react'
import { CSSTransition } from 'react-transition-group'
import {ItemsSource, LogView, LogViewItem} from './LogView'
import {Progress} from './Progress'

var requestId = 0

type Event = {

}

type LogFileItem = {
	id: string
	lineCount: number
	indexToSelect: number
	loading: boolean
	progress: number
}

export type TextPart = {
    text: string
    restrictionIndex: number
}

type LogItem = {
	highlightedText?: TextPart[] 
    text: string
    index?: number
    fileIndex: number
}

export type LogItemResult = {
	request: number
	items: LogItem[]
}

function App() {
	const [itemSource, setItemSource] = useState({count: 0, indexToSelect:0, getItems: async (s,e)=>[]} as ItemsSource)
	const [id, setId] = useState("")
	const [progress, setProgress] = useState(0)
	const [progressTitle, setProgressTitle] = useState("")

    const getItem = (text: string, highlightedItems?: TextPart[], index?: number, fileIndex?: number) => ({ 
        item: text, 
		highlightedItems,
        index: index || 0,
		fileIndex: fileIndex || 0
    } as LogViewItem)

    const getItems = async (id: string, start: number, end: number) => {
		const data = await fetch(`http://localhost:9865/getitems?id=${id}&req=${++requestId}&start=${start}&end=${end}`)
		const lineItems = (await data.json() as LogItemResult)
		return requestId == lineItems.request
			? lineItems.items.map(n => getItem(n.text, n.highlightedText, n.index, n.fileIndex))
			: []
	}
	
	useEffect(() => {
		const ws = new WebSocket("ws://localhost:9865/websocketurl")
		ws.onclose = () => console.log("Closed")
		ws.onmessage = p => { 
			const logFileItem = JSON.parse(p.data) as LogFileItem
			if (logFileItem.progress)
				if (logFileItem.progress < 100) {
					setProgressTitle(logFileItem.loading ? "Indiziere Logdatei" : "Wende Einschränkungen an")
					setProgress(logFileItem.progress)	
				}
				else
				  	setProgress(0)
			else {
				setId(logFileItem.id)
				setItemSource({count: logFileItem.lineCount, getItems: (s, e) => getItems(logFileItem.id, s, e), indexToSelect: logFileItem.indexToSelect})
			}
		}
	}, [])

	return (
    	<div className="App" >
			<LogView itemSource={itemSource} id={id} />
			<CSSTransition
			    in={progress > 0}
        		timeout={300}
        		classNames="progress"
        		unmountOnExit >			
				<Progress progress={progress} title={progressTitle} />
			</CSSTransition>
  		</div>
  	)
}

export default App
