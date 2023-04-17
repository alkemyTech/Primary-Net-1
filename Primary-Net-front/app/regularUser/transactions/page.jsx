import { authOptions } from '@/pages/api/auth/[...nextauth]';
import React from 'react';
import { getServerSession } from 'next-auth';

import Navbar from '../../components/Nav';

const fetchTransactions = async (token) => {
  return await fetch('https://localhost:7131/api/transaction', {
    headers: {
      Authorization: 'Bearer ' + token
    }
  });
};

export default async function Transactions() {
  const session = await getServerSession(authOptions);
  const data = await fetchTransactions(session.user.accessToken);
  const transactions = await data.json();

  return (
    <div>
      {/* Navbar */}
      <Navbar />
      <div className="bg-gray-100 p-4 h-screen flex items-center justify-center">
        <div className="bg-white p-4 rounded-lg shadow-md w-full items-center justify-center">
          <p className="text-lg font-bold mb-2">Estas son sus transacciones:</p>
          <ul>
            {transactions.data.items.map((tran) => (
              <li className="mb-2" key={tran.id}>
                <span className="font-bold">Concepto: </span>
                {tran.concept} |<span className="font-bold"> Monto: </span>
                {tran.amount}|<span className="font-bold"> Fecha: </span>{' '}
                {tran.date}
              </li>
            ))}
          </ul>
        </div>
      </div>
    </div>
  );
}
