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
    <div>
      <select name="" id="" onChange={(e) => setDeletedAccount(e.target.value)}>
        {accounts.map((acc) => (
          <option value={acc.id} key={acc.id}>
            {acc.id}
          </option>
        ))}
      </select>
      <button onClick={(e) => handleRegister(e)}>delete</button>
    </div>
  );
}

export default DeleteAccount;
