
import { authOptions } from '@/pages/api/auth/[...nextauth]';
import { getServerSession } from 'next-auth';
import { redirect } from 'next/navigation';
import Navbar from '../../../components/Nav'

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
      <div className="min-h-screen flex flex-col">
        {/* Navbar */}
        <Navbar />
  
        {/* Contenedor de la lista de elementos */}
        <div className="flex-grow">
          <div className="grid grid-cols-4 gap-4">
            {catalogues.data.items.map((catalogue) => (
              <div key={catalogue.id} className="bg-gray-100 p-4 border border-gray-200 shadow-md items-center justify-center">
                <img src={catalogue.image} alt="Mi imagen" width="300" height="200"/><br></br>
                {catalogue.name}<br></br>{catalogue.points}                
              </div>
            ))}
          </div>
        </div>
      </div>
    );
  }
  return redirect('/auth/login');
}