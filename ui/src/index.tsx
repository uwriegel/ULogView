import React from 'react'
import ReactDOM from 'react-dom'
import './index.css'
import App from './App'

window.addEventListener("keydown", (evt) => {
	if (evt.which == 114) 
		evt.preventDefault()
})

ReactDOM.render(
  	<React.StrictMode>
    	<App />
  	</React.StrictMode>,
  	document.getElementById('root')
)
