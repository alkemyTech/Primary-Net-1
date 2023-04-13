import { authOptions } from "@/pages/api/auth/[...nextauth]";
import { getServerSession } from "next-auth";

import React from "react";

export default async function TransactionList(){
    const session = await getServerSession(authOptions);
    if(session){
        const data = await fetch('https://localhost:7131/api/transaction',{
            headers:{
                'Content-Type': 'application/json',
                Authorization: 'Baearer ' + session.user.accessToken
            },
            cache: 'no-store'
        }).catch((err)=>{
            throw new Error(err.message);
        });

        const transactions = await data.json();

        return(
            <div>
                <ul>
                    {transactions.data.items.map((item)=>(
                        <li key={item.id}>{item.concept}</li>
                    ))}
                </ul>
            </div>
        )
    }
}