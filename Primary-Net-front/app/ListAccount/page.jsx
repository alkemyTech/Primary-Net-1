import { authOptions } from '@/pages/api/auth/[...nextauth]';
import { getServerSession } from 'next-auth';
import { redirect } from 'next/navigation';

import React from 'react';

export default async function UsersList() {
  const session = await getServerSession(authOptions);
  if (session) {
    const data = await fetch('https://localhost:7131/api/account', {
      headers: {
        'Content-Type': 'application/json',
        Authorization: 'Bearer ' + session.user.accessToken
      },
      cache: 'no-store'
    }).catch((err) => {
      throw new Error(err.message);
    });

    const accounts = await data.json();

    console.log(accounts);

    return (
      <div>
        <h1>Lista de elementos:</h1>
        <ul>
          {accounts.data.items.map((item) => (
            <li key={item.id}>
              {item.id} {item.money}
            </li>
          ))}
        </ul>
      </div>
    );
  }
  return redirect('/');
}
