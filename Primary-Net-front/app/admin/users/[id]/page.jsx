import React from 'react';
import { getServerSession } from 'next-auth';
import { authOptions } from '@/pages/api/auth/[...nextauth]';

const fetchUserData = async (id, accessToken) => {
  return await fetch(`https://localhost:7131/api/user/${id}`, {
    headers: {
      'Content-Type': 'application/json',
      Authorization: 'Bearer ' + accessToken
    }
  }).then((res) => res.json());
};

export default async function UserDetail({ params }) {
  const session = await getServerSession(authOptions);
  const { id } = params;
  const user = await await fetchUserData(id, session.user.accessToken);
  return (
    <div>
      Este es el User:
      <ul>
        <li>{user.firstName}</li>
      </ul>
    </div>
  );
}
