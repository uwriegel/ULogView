import React from 'react'
import './App.css'

function App() {

	const onclick = async () =>{
		var data = await fetch("http://localhost:9865/affen")
		console.log("data", data)
		const json = await data.json()
		console.log("data json", json)
	}

  	return (
    	<div className="App">
      		<header className="App-header">
				<p>
					Edit <code>src/App.tsx</code> and save to reload.
				</p>
				<button onClick={onclick}>Abfrage</button>
      		</header>
  		</div>
  	)
}

export default App
