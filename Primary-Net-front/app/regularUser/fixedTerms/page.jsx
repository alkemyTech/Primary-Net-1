import { authOptions } from '@/pages/api/auth/[...nextauth]';
import { getServerSession } from 'next-auth';
import React from 'react';
import FixedTermForm from './fixedTermForm';

export default async function FixedTerm() {
  const session = await getServerSession(authOptions);
  return (
    <div>
      <FixedTermForm session={session} />
    </div>
  );
}
