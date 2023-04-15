import React from 'react';
import { getServerSession } from 'next-auth';
import { authOptions } from '@/pages/api/auth/[...nextauth]';
import DeleteAccountForm from './DeleteAccountForm';
export default async function DeleteAccount() {
  const session = await getServerSession(authOptions);
  if (session) {
    const data = await fetch('https://localhost:7131/api/account', {
      headers: {
        'Content-Type': 'application/json',
        Authorization: 'Bearer ' + session.user.accessToken
      },
      cache: 'no-store'
    }).catch((err) => {
      throw new Error(err.message);
    });

    const accounts = await data.json();
    return (
      <div>
        <ul>
          <DeleteAccountForm accounts={accounts.data.items} session={session} />
        </ul>
      </div>
    );
  }
}
