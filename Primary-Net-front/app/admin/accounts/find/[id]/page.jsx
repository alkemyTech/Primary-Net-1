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
    <div>
      El detalle del account es:
      <ul>
        <li>{account.data.id}</li>
        <li>{account.data.creationDate}</li>
        <li>{account.data.money}</li>
      </ul>
    </div>
  );
}