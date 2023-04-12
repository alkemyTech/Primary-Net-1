'use client';
import { useSession } from 'next-auth/react';
import UserInformation from '../../components/UserInformation';
import axios from 'axios';
import React, { useState, useEffect } from 'react';

const TransactionList = () => {
  const { data: session } = useSession();
  const [transactions, setTransactions] = useState([]); // Estado para almacenar los usuarios

  useEffect(() => {
    if (session) {
      const token = session.accessToken;
      axios
        .get('https://localhost:7131/api/Transaction', {
          headers: {
            'Content-Type': 'application/json',
            Authorization: 'Bearer ' + token
          }
        })
        .then((res) => setTransactions(res.data)) // Actualizar el estado con los datos de los usuarios
        .catch((err) => {
          throw new Error(err.message);
        });
    }
  }, [session]);

  console.log(transactions.items);
  if (session) {
    const items = transactions.items
    return (
      <>
        <UserInformation data={session?.isAdmin} />
        <div>
          <h1>Lista de elementos:</h1>
          <ul>
            {items && items.map((item) => (
              <li key={item.id}>{item.concept}</li>
            ))}
          </ul>
        </div>
      </>
    );
  } else {
    return null; // Puedes retornar lo que quieras en caso de que no haya sesión
  }
};

export default TransactionList;