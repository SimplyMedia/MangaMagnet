import { OpenAPI } from '@/services/openapi'
import '@/styles/globals.css'
import type { AppProps } from 'next/app'

OpenAPI.BASE = '';

export default function App({ Component, pageProps }: AppProps) {
  return <Component {...pageProps} />
}
