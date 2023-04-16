'use client';
import React, { useState } from 'react';
import axios from 'axios';

import Navbar from '../../components/Nav'

const AccountList = ({ session, accounts }) => {

  const [selectedAccount, setSelectedAccount] = useState(0);
  const [amount, setAmount] = useState(0);
  const [concept, setConcept] = useState('');

  const handleSelect = (e) => {
    setSelectedAccount(e.target.value);
  };

  const handleTransfer = async () => {
    await axios
      .post(
        `https://localhost:7131/api/account/transfer/${session.user.id}`,
        {
          idReceptor: selectedAccount,
          MontoTransferido: amount,
          Concept: concept
        },
        { headers: { Authorization: 'Bearer ' + session.user.accessToken } }
      )
      .then((res) => console.log(res))
      .catch((err) => {
        throw new Error(err.message);
      });
  };
  return (
    <div>
      {/* Navbar */}
      <Navbar />
      <div class="flex items-center justify-center h-screen bg-gray-100"> 
        <div class="bg-white rounded-lg shadow-md p-8">
          <label class="block mb-4">
            <span class="text-gray-700 font-bold">Cuenta:</span>
            <select
              name="account"
              id="account_select"
              class="form-select block w-full mt-1"
              onChange={(e) => handleSelect(e)}
              defaultValue={0}
            >
              <option value={0}>0</option>
                {accounts.map((acc) => (
                  <option value={acc.id} key={acc.id}>
                    {acc.id}
                  </option>
                ))}
            </select>
          </label>
          <label class="block mb-4">
          <span class="text-gray-700 font-bold">Cantidad:</span>
          <input
            type="number"
            name=""
            id=""
            class="form-input mt-1 block w-full"
            onChange={(e) => setAmount(e.target.value)}
          />
          </label>
          <label class="block mb-4">
            <span class="text-gray-700 font-bold">Concepto:</span>
            <input
              type="text"
              name=""
              id=""
              placeholder="concepto"
              class="form-input mt-1 block w-full"
              onChange={(e) => setConcept(e.target.value)}
            />
            </label>
            <button
            class="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded"
            onClick={() => handleTransfer()}
            >
              Transferir
            </button>
          </div>
      </div>
    </div>
  )  
  
};

export default AccountList;
