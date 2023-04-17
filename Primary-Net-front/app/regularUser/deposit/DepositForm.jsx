'use client';
import React, { useState } from 'react';
import axios from 'axios';

const DepositForm = (session) => {
  const [amount, setAmount] = useState();
  const [concept, setConcept] = useState();
  const handleDeposit = async (e) => {
    e.preventDefault();
    await axios
      .post(
        `https://localhost:7131/api/account/deposit/${session.session.user.id}`,
        {
          concept,
          AumentoSaldo: amount
        },
        {
          headers: {
            Authorization: 'Bearer ' + session.session.user.accessToken
          }
        }
      )
      .then((res) => console.log(res));
  };
  return (
    <div className="w-screen h-screen flex items-center justify-center">
      <div className="flex flex-col">
        <label htmlFor="Ingrese monto">Ingrese monto de deposito</label>
        <input
          type="number"
          name=""
          id=""
          className="border border-black rounded"
          onChange={(e) => setAmount(e.target.value)}
        />
        <input
          type="text"
          name=""
          id=""
          className="border border-black rounded"
          onChange={(e) => setConcept(e.target.value)}
        />
        <button onClick={(e) => handleDeposit(e)}>DEPOSITAR</button>
      </div>
    </div>
  );
};

export default DepositForm;
