import { authOptions } from '@/pages/api/auth/[...nextauth]';
import React from 'react';
import { getServerSession } from 'next-auth';

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
      <ul>
        {transactions.data.items.map((tran) => (
          <li key={tran.id}>{tran.concept}</li>
        ))}
      </ul>
    </div>
  );
}
