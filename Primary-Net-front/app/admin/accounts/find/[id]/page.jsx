import React from 'react';
import { getServerSession } from 'next-auth';
import { authOptions } from '@/pages/api/auth/[...nextauth]';

const fetchAccountData = async (id, accessToken) => {
  return await fetch(`https://localhost:7131/api/account/${id}`, {
    headers: {
      'Content-Type': 'application/json',
      Authorization: 'Bearer ' + accessToken
    }
  }).then((res) => res.json());
};

export default async function TransactionDetail({ params }) {
  const session = await getServerSession(authOptions);
  const { id } = params;
  const account = await fetchAccountData(id, session.user.accessToken);
  console.log(account);
  return (
    <div class="bg-gray-200 p-4 h-screen flex items-center justify-center">
      <div class="bg-white p-4 rounded-lg shadow-md w-full items-center justify-center">
        <p class="text-lg font-bold mb-2">Detalle de la cuenta</p>
        <ul>
          <li class="mb-2"><span class="font-bold">ID:</span> {account.data.id}</li>
          <li class="mb-2"><span class="font-bold">Fecha de creación:</span> {account.data.creationDate}</li>
          <li><span class="font-bold">Saldo:</span> {account.data.money}</li>
        </ul>
      </div>
    </div>
  );
}