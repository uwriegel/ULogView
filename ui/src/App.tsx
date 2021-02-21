import React from 'react'
import './App.css'
import {LogView} from './LogView'

const ws = new WebSocket("ws://localhost:9865/websocketurl")
ws.onclose = () => console.log("Closed")
ws.onmessage = p => console.log(JSON.parse(p.data))
 
function App() {

	const onclick = async () =>{
		var data = await fetch("http://localhost:9865/affen")
		console.log("data", data)
		const json = await data.json()
		console.log("data json", json)
	}

  	return (
    	<div className="App">
			<button onClick={onclick}>Abfrage</button>
			<LogView />
  		</div>
  	)
}

export default App
