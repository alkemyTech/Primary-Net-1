'use client';
import React, { useState } from 'react';
import axios from 'axios';

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
      <select
        name="account"
        id="account_select"
        onChange={(e) => handleSelect(e)}
        defaultValue={0}>
        <option value={0}>0</option>
        {accounts.map((acc) => (
          <option value={acc.id} key={acc.id}>
            {acc.id}
          </option>
        ))}
      </select>
      <input
        type="number"
        name=""
        id=""
        onChange={(e) => setAmount(e.target.value)}
      />
      <input
        type="text"
        name=""
        id=""
        placeholder="concepto"
        onChange={(e) => setConcept(e.target.value)}
      />

      <button onClick={() => handleTransfer()}>Transfer</button>
    </div>
  );
};

export default AccountList;
