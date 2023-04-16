import { authOptions } from '@/pages/api/auth/[...nextauth]';
import { getServerSession } from 'next-auth';
import React from 'react';
import AccountList from './AccountList';
import { redirect } from 'next/navigation';

const fetchData = async (token) => {
  return await fetch('https://localhost:7131/api/account', {
    headers: {
      'Content-Type': 'application/json',
      Authorization: 'Bearer ' + token
    }
  }).catch((err) => {
    throw new Error(err.message);
  });
};

export default async function Transfer() {
  const session = await getServerSession(authOptions);

  if(session){
    const data = await fetchData(session.user.accessToken);

    const accounts = await data.json();
  
    console.log(accounts);
  
    return (
      <div>
        <AccountList accounts={accounts.data.items} session={session} />
      </div>
    );
  }
  return redirect('/auth/login');
  
}
