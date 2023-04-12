import React from 'react';
import { getServerSession } from 'next-auth';
import { authOptions } from '@/pages/api/auth/[...nextauth]';

const fetchUserData = (id, accessToken) => {
  return fetch(`https://localhost:7131/api/user/${id}`, {
    headers: {
      'Content-Type': 'application/json',
      Authorization: 'Bearer ' + accessToken
    }
  }).then((res) => res.json());
};

async function UserDetail({ params }) {
  const session = await getServerSession(authOptions);
  const { id } = params;
  const user = await fetchUserData(id, session.user.accessToken);

  return (
    <div>
      Este es el :
      <ul>
        <li>{user.firstName}</li>
        <li>{user.lastName}</li>
        <li>{user.email}</li>
        <li>Puntos: {user.points}</li>
      </ul>
    </div>
  );
}

export default UserDetail;
