import { authOptions } from '@/pages/api/auth/[...nextauth]';
import { getServerSession } from 'next-auth';
import { redirect } from 'next/navigation';

import React from 'react';
import UpdateTransaction from './UpdateTransaction';

export default async function AccountsList() {
  const session = await getServerSession(authOptions);
  if (session) {
    const data = await fetch('https://localhost:7131/api/transaction', {
      headers: {
        'Content-Type': 'application/json',
        Authorization: 'Bearer ' + session.user.accessToken
      },
      cache: 'no-store'
    }).catch((err) => {
      throw new Error(err.message);
    });

    const transactions = await data.json();
    return (
      <div>
        <UpdateTransaction
          session={session}
          transactions={transactions.data.items}
        />
      </div>
    );
  }
  return redirect('/');
}
