"use client"
import LoginBtn from "@/components/login-btn";
import {SessionProvider} from "next-auth/react";

export default function Home() {
  return (
    <div>
        <SessionProvider>
            <LoginBtn/>
        </SessionProvider>
    </div>
  );
}
