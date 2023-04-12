'use client';
import { useSession } from 'next-auth/react';
import UserInformation from '../../components/UserInformation';
import axios from 'axios';
import React, { useState, useEffect } from 'react';

const UsersList = () => {
  const { data: session } = useSession();
  const [users, setUsers] = useState([]); // Estado para almacenar los usuarios

  useEffect(() => {
    if (session) {
      const token = session.accessToken;
      axios
        .get('https://localhost:7131/api/user', {
          headers: {
            'Content-Type': 'application/json',
            Authorization: 'Bearer ' + token
          }
        })
        .then((res) => setUsers(res.data)) // Actualizar el estado con los datos de los usuarios
        .catch((err) => {
          throw new Error(err.message);
        });
    }
  }, [session]);

  console.log(users);
  if (session) {
    return (
      <>
        <UserInformation data={session?.isAdmin} />
        <div>
          <h1>Lista de elementos:</h1>
          <ul>
            {users.map((user) => (
              <li key={user.id}>{user.firstName}</li>
            ))}
          </ul>
        </div>
      </>
    );
  } else {
    return null; // Puedes retornar lo que quieras en caso de que no haya sesión
  }
};

export default UsersList;