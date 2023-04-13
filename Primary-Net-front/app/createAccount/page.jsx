'use client';
import { useSession } from 'next-auth/react';
import React from 'react';
import axios from 'axios';

const AccountInsert = () => {
  const { data: session } = useSession();

  const handleSubmit = () => {
    if (session) {
      axios
        .post(
          'https://localhost:7131/api/account',
          {},
          { headers: { Authorization: 'Bearer ' + session.accessToken } }
        )
        .then((res) => console.log(res.data));
    }
  };
  

  return (
    <div>
      AccountInsert
      <button onClick={() => handleSubmit()}>SUBMIT</button>
    </div>
  );
};

export default AccountInsert;