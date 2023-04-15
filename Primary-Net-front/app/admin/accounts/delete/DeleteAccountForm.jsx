'use client';
import React from 'react';
import axios from 'axios';

const DeleteAccountForm = ({ session, account }) => {
  const handleDelete = async () => {
    await axios
      .delete(
        'https://localhost:7131/api/account/4',
        {},
        {
          headers: {
            Authorization: `Bearer ${session.user.accessToken}`
          }
        }
      )
      .then((res) => console.log(res.data));
  };
  return (
    <div>
      <button onClick={() => handleDelete()}>eliminar</button>
    </div>
  );
};

export default DeleteAccountForm;
