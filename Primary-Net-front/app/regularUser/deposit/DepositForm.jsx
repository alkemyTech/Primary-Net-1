'use client';
import React, { useState } from 'react';
import axios from 'axios';
import Navbar from '../../components/Nav'
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
    <div>
      {/* Navbar */}
      <Navbar />
      <div className="h-screen flex justify-center items-center bg-gray-100"> 
        <div className="flex flex-col items-center mb-8">
          <form className="flex flex-col items-center bg-white p-6 rounded-lg">
            <span class="text-gray-700 font-bold">Cuenta:</span>
              <input
                type="number"
                name=""
                id=""
                className="px-4 py-2 border rounded-lg mb-4"
                onChange={(e) => setAmount(e.target.value)}
              />
              <input
                type="text"
                name=""
                id=""
                className="px-4 py-2 border rounded-lg mb-4"
                onChange={(e) => setConcept(e.target.value)}
              />
            <button onClick={(e) => handleDeposit(e)} class="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded">DEPOSITAR</button>
          </form> 
        </div>
      </div>
    </div>
  );
};

export default DepositForm;
