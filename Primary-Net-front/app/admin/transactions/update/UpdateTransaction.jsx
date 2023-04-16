'use client';
import React, { useState } from 'react';

const UpdateTransaction = ({ session, transactions }) => {
  const [selectedTran, setSelectedTran] = useState();
  const [concept, setConcept] = useState();

  const handleUpdate = () => {
    fetch(`https://localhost:7131/api/account/${selectedTran}`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
        Authorization: 'Bearer ' + session.user.accessToken
      },
      body: JSON.stringify({
        concept
      })
    })
      .then((res) => res.json())
      .then((res) => console.log(res))
      .catch((err) => {
        throw new Error(err.message);
      });
  };

  return (
    <div className="flex flex-col items-center w-1/2 gap-y-6">
      <select onChange={(e) => setSelectedTran(e.target.value)}>
        {transactions.map((tran) => (
          <option value={tran.id} key={tran.id}>
            {tran.concept}
          </option>
        ))}
      </select>

      <input
        type="text"
        name=""
        id=""
        placeholder="concept"
        className="border border-black rounded-lg w-64 text-center"
        onChange={(e) => setConcept(e.target.value)}
      />

      <button
        onClick={() => handleUpdate()}
        className="border border-black bg-red-400 rounded-lg">
        actualizar
      </button>
    </div>
  );
};

export default UpdateTransaction;
