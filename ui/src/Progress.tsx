import React from 'react'

export const Progress = () => (
    <div className='progressControl'>
        <svg className="svg" viewBox="0 0 32 32">
            {/* <circle v-bind:style="{ strokeDasharray: progress + ' 100' }" r="16" cx="16" cy="16" /> */}
            <circle className="progressCircle" style={{strokeDasharray: 140}} r="16" cx="16" cy="16" />
        </svg>
    </div> 
)




    
