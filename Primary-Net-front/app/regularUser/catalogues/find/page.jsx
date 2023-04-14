import { authOptions } from '@/pages/api/auth/[...nextauth]';
import { getServerSession } from 'next-auth';
import { redirect } from 'next/navigation';

import React from 'react';

export default async function CataloguesList() {
  const session = await getServerSession(authOptions);
  if (session) {
    const data = await fetch('https://localhost:7131/api/catalogue', {
      headers: {
        'Content-Type': 'application/json',
        Authorization: 'Bearer ' + session.user.accessToken
      },
      cache: 'no-store'
    }).catch((err) => {
      throw new Error(err.message);
    });

    const catalogues = await data.json();
    
    return (
      <div>
        <h1>Lista de elementos:</h1>
        <ul>
          {catalogues.data.items.map((catalogue) => (
            <li key={catalogue.id}>
              {catalogue.name} {catalogue.image} {catalogue.points} 
            </li>
          ))}
        </ul>
      </div>
    );
  }
  return redirect('/');
}