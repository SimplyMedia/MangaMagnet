/** @type {import('next').NextConfig} */
const nextConfig = {
	output: 'standalone',
	reactStrictMode: false, // Moved to _app.tsx, otherwise providers are called twice
	images: {
		remotePatterns: [{
			protocol: 'https',
			hostname: 'uploads.mangadex.org',
			pathname: '**',
		}],
	},
	env: {
		NEXT_PUBLIC_API_BASE: 'http://localhost:5248'
	}
}

module.exports = nextConfig
