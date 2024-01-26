/** @type {import('next').NextConfig} */
const nextConfig = {
	output: 'standalone',
	reactStrictMode: true,
	images: {
		remotePatterns: [{
			protocol: 'https',
			hostname: 'uploads.mangadex.org',
			pathname: '**',
		}],
	}
}

module.exports = nextConfig
