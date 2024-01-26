import { NextRequest, NextResponse } from "next/server";

export const config = {
	matcher: ["/api/:path*"],
};

export function middleware(request: NextRequest) {
	const API_BASE = process.env.API_BASE ?? 'http://localhost:5248'

	return NextResponse.rewrite(
		new URL(
			`${API_BASE}${request.nextUrl.pathname}${request.nextUrl.search}`
		),
		{request}
	);
}
