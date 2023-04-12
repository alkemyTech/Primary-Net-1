import { authOptions } from '@/pages/api/auth/[...nextauth]';
import { getServerSession } from 'next-auth';
import axios from 'axios';

import React from 'react';

export default async function UsersList() {
  const session = await getServerSession(authOptions);
  const users = await axios
    .get('https://localhost:7131/api/user', {
      headers: {
        'Content-Type': 'application/json',
        Authorization: 'Bearer ' + session.accessToken
      }
    })
    .catch((err) => {
      throw new Error(err.message);
    });
  if (session) {
    return (
      <div>
        <h1>Lista de elementos:</h1>
        <ul>
          {users.data.map((user) => (
            <li key={user.id}>{user.firstName}</li>
          ))}
        </ul>
      </div>
    );
  }
}
