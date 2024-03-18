import type { NextRequest } from 'next/server'

export function middleware(request: NextRequest) {
    const session = request.cookies.get('session')?.value
    console.log('session', session)
    console.log('request.nextUrl.pathname', request.nextUrl.pathname)

    if (session && !request.nextUrl.pathname.startsWith('/')) {
        return Response.redirect(new URL('/', request.url))
    }

    if (!session && !request.nextUrl.pathname.startsWith('/login')) {
        return Response.redirect(new URL('/login', request.url))
    }
}

export const config = {
    matcher: ['/((?!api|_next/static|_next/image|.*\\.png$).*)'],
}