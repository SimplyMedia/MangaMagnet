import { OpenAPI } from '@/services/openapi'
import { WebSocketProvider } from '@/services/websocket'
import '@/styles/globals.css'
import React from 'react'
import type { AppProps } from 'next/app'

if (process.env.NEXT_PUBLIC_API_BASE) {
	OpenAPI.BASE = process.env.NEXT_PUBLIC_API_BASE
}

export default function App({Component, pageProps}: AppProps) {
	return (
		<WebSocketProvider>
			<React.StrictMode>
				<Component {...pageProps} />
			</React.StrictMode>
		</WebSocketProvider>
	)
}
