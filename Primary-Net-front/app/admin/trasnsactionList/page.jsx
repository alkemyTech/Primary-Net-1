import { authOptions } from '@/pages/api/auth/[...nextauth]';
import { getServerSession } from 'next-auth';
import { redirect } from 'next/navigation';

import React from 'react';

export default async function FixedTermList() {
  const session = await getServerSession(authOptions);
  if (session) {
    const data = await fetch('https://localhost:7131/api/transaction', {
      headers: {
        'Content-Type': 'application/json',
        Authorization: 'Bearer ' + session.user.accessToken
      },
      cache: 'no-store'
    }).catch((err) => {
      throw new Error(err.message);
    });

    const transactions = await data.json();

    console.log(transactions);

    return (
      <div>
        <h1>Lista de elementos:</h1>
        <ul>
          {transactions.data.items.map((item) => (
            <li key={item.id}>
              Amount:{item.amount} concepto:{item.concept} tipo:{item.type}
            </li>
          ))}
        </ul>
      </div>
    );
  }
  return redirect('/');
}
