'use client';
import React, { useState } from 'react';

function DeleteAccount({ accounts, session }) {
  const [deletedAccount, setDeletedAccount] = useState();
  const handleRegister = async (e) => {
    fetch(`https://localhost:7131/api/account/${deletedAccount}`, {
      method: 'DELETE',
      headers: {
        'Content-Type': 'application/json',
        Authorization: 'Bearer ' + session.user.accessToken
      }
    })
      .then((res) => res.json())
      .then((res) => console.log(res))
      .catch((err) => {
        throw new Error(err.message);
      });
  };

  return (
    <div class="flex flex-col justify-center items-center bg-gray-200 h-screen">
      <div class="flex flex-col justify-center items-center bg-white shadow-md p-4 rounded-lg">
        <select class="border border-gray-400 p-2 rounded-md" name="" id="" onChange={(e) => setDeletedAccount(e.target.value)}>
          {accounts.map((acc) => (
            <option value={acc.id} key={acc.id}>
              {acc.id}
            </option>
          ))}
        </select>
        <button class="bg-red-500 hover:bg-red-700 text-white font-bold py-2 px-4 rounded mt-4" onClick={(e) => handleRegister(e)}>delete</button>
      </div>
    </div>
  );
}

export default DeleteAccount;
