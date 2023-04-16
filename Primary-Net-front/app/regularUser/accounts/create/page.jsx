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
          { headers: { Authorization: 'Bearer ' + session.user.accessToken } }
        )
        .then((res) => console.log(res.data));
    }
  };
  return (
    <div class="flex justify-center items-center h-screen bg-gray-200">
      <div class="bg-white p-8 rounded-lg shadow-md flex items-center">
        <h1 class="text-2xl font-bold mr-4">AccountInsert</h1>
        <button
          class="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded"
          onClick={() => handleSubmit()}
        >
          SUBMIT
        </button>
      </div>
    </div>
  );
};

export default AccountInsert;
