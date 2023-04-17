import React from 'react';
import DepositForm from './DepositForm';
import { getServerSession } from 'next-auth';
import { authOptions } from '@/pages/api/auth/[...nextauth]';

export default async function Deposit() {
  const session = await getServerSession(authOptions);
  return (
    <div>
      <DepositForm session={session} />
    </div>
  );
}
