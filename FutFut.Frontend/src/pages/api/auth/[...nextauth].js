import NextAuth from "next-auth"
import { jwtDecode } from "jwt-decode";

export const authOptions = {
    providers: [
        {
            id: "identityServer",
            name: "FutFutIdentity",
            type: "oauth",
            clientId: "nextjs",
            wellKnown: "https://localhost:5001/.well-known/openid-configuration",
            authorization: { params: { scope: "openid profile IdentityServerApi profile.fullaccess notify.fullaccess billing.fullaccess" } },
            idToken: true,
            checks: ["pkce", "state"],
            clientSecret: "AHAHAHAHHANEXTJSFRONT",
            profile(profile) {
                return {
                    id: profile.sub,
                    name: profile.name,
                    email: profile.email,
                    image: profile.picture,
                }
            },
        }
    ],

    callbacks: {
        async jwt({ token, account }) {
            if (account) {
                token.accessToken = account.access_token;
                token.idToken = account.id_token;
                token.sub = account.sub;
            }
            return token;
        },

        async session({ session, token }) {
            session.accessToken = token.accessToken;
            session.idToken = token.idToken;
            const decoded = jwtDecode(token.accessToken)

            session.sub = decoded.sub;
            return session;
        },
    },

    session: {
        strategy: 'jwt',
    },
}

export default NextAuth(authOptions)