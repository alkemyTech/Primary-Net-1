import { authOptions } from '@/pages/api/auth/[...nextauth]';
import { getServerSession } from 'next-auth';
import { redirect } from 'next/navigation';
import Navbar from '../../../components/Nav'

import React from 'react';

export default async function AccountDetail() {
  const session = await getServerSession(authOptions);
  if (session) {
    const data = await fetch(`https://localhost:7131/api/account/userAccount/${session.user.id}`, {
      headers: {
        'Content-Type': 'application/json',
        Authorization: 'Bearer ' + session.user.accessToken
      },
      cache: 'no-store'
    }).catch((err) => {
      throw new Error(err.message);
    });

    
    const accountOfUser = await data.json();
    console.log(accountOfUser)

    return (
      <div className="min-h-screen flex flex-col">
        {/* Navbar */}
        <Navbar />
  
        {/* Contenedor de la lista de elementos */}
        <div class="bg-gray-100 p-4 h-screen flex items-center justify-center">
      <div class="bg-white p-4 rounded-lg shadow-md w-full items-center justify-center">
        <p class="text-lg font-bold mb-2">El detalle de la cuenta es:</p>
        <ul>
          <li class="mb-2"><span class="font-bold">Numero de cuenta:</span> {accountOfUser.data.value.id}</li>
          <li class="mb-2"><span class="font-bold">Fecha de Creacion:</span> {accountOfUser.data.value.creationDate}</li>
          <li class="mb-2"><span class="font-bold">Dinero:</span> {accountOfUser.data.value.money}</li>
        </ul>
      </div>
        </div>
      </div>
    );
  }
  return redirect('/auth/login');
}