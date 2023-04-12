import { authOptions } from '@/pages/api/auth/[...nextauth]';
import { getServerSession } from 'next-auth';

import React from 'react';

export default async function UsersList() {
  const session = await getServerSession(authOptions);
  if (session) {
    const data = await fetch('https://localhost:7131/api/user', {
      headers: {
        'Content-Type': 'application/json',
        Authorization: 'Bearer ' + session.user.accessToken
      },
      cache: 'no-store'
    }).catch((err) => {
      throw new Error(err.message);
    });

    const users = await data.json();

    console.log(users);

    return (
      <div>
        <h1>Lista de elementos:</h1>
        <ul>
          {users.map((user) => (
            <li key={user.id}>
              {user.firstName} {user.lastName}
            </li>
          ))}
        </ul>
      </div>
    );
  }

  return <div>Debe iniciar sesion</div>;
}
