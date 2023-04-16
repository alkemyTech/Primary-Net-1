import { authOptions } from '@/pages/api/auth/[...nextauth]';
import { getServerSession } from 'next-auth';
import { redirect } from 'next/navigation';
import Navbar from '../../components/Nav'
import Footer from '../../components/Footer'

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
        <Navbar /> {/* Agrega el componente Navbar aquí */}
        <h1>Lista de elementos:</h1>
        <ul>
          {users.map((user) => (
            <li key={user.id}>
              {user.firstName} {user.lastName}
            </li>
          ))}
        </ul>
        <Footer/>
      </div>
    );
  }
  return redirect('/');
}
