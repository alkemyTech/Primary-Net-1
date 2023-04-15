import { authOptions } from '@/pages/api/auth/[...nextauth]';
import { getServerSession } from 'next-auth';
import React from 'react';
import CreateRoleForm from './CreateRoleForm';

const fetchData = async (token) => {
  return await fetch('https://localhost:7131/api/role', {
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
  const data = await fetchData(session.user.accessToken).then((res) =>
    res.json()
  );

  const roles = data.data;

  return (
    <div>
      <ul>
        {roles.map((role) => (
          <li key={role.id}>{role.name}</li>
        ))}
        <CreateRoleForm token={session.user.accessToken} />
      </ul>
    </div>
  );
}
