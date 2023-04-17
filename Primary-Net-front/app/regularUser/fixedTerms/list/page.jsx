import { authOptions } from '@/pages/api/auth/[...nextauth]';
import { getServerSession } from 'next-auth';
import React from 'react';
const fetchData = async (token, id) => {
  return await fetch(`https://localhost:7131/api/fixedTerm/userAccount/${id}`, {
    headers: { Authorization: 'Bearer ' + token }
  });
};

export default async function FixedTerms() {
  const session = await getServerSession(authOptions);
  const data = await fetchData(session.user.accessToken, session.user.id);
  const fixedTerms = await data.json();
  console.log(fixedTerms);
  return (
    <div>
      <ul>
        {fixedTerms.data.map((ft) => (
          <li key={ft.id}>
            {ft.id} | {ft.amount} | {ft.closingDate}
          </li>
        ))}
      </ul>
    </div>
  );
}
