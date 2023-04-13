import { authOptions } from '@/pages/api/auth/[...nextauth]';
import { getServerSession } from 'next-auth';
import { redirect } from 'next/navigation';

import React from 'react';

export default async function RolesList() {
  const session = await getServerSession(authOptions);
  if (session) {
    const data = await fetch('https://localhost:7131/api/role', {
      headers: {
        'Content-Type': 'application/json',
        Authorization: 'Bearer ' + session.user.accessToken
      },
      cache: 'no-store'
    }).catch((err) => {
      throw new Error(err.message);
    });

    console.log(data);
    const roles = await data.json();
    
    return (
      <div>
        <h1>Lista de elementos:</h1>
        <ul>
          {roles.data.map((role) => (
            <li key={role.id}>
              {role.name} {role.description} {role.esEliminado} 
            </li>
          ))}
        </ul>
      </div>
    );
  }
  return redirect('/');
}