'use client';
import React, { useState } from 'react';
import axios from 'axios';
const FixedTermForm = ({ session }) => {
  const [date, setDate] = useState();
  const [amount, setAmount] = useState();
  const [accountId, setAccountId] = useState();

  const chnageDate = (e) => {
    setDate(e.target.value);
    console.log(date);
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    await axios
      .post(
        'https://localhost:7131/api/fixedTerm/deposit',
        {
          newFixedTermDeposit: {
            closingDate: date,
            amount,
            accountId
          },
          interestRate: 35
        },
        {
          headers: {
            Authorization: 'Bearer ' + session.user.accessToken
          }
        }
      )
      .then((res) => console.log(res.data));
  };

  return (
    <div>
      <input
        type="date"
        name="date"
        id="date_input"
        onChange={(e) => chnageDate(e)}
        className="border border-black"
      />
      <input
        type="number"
        name=""
        id=""
        onChange={(e) => setAmount(e.target.value)}
        className="border border-black"
      />
      <input
        type="number"
        className="border border-black"
        onChange={(e) => setAccountId(e.target.value)}
      />

      <button className="border border-black" onClick={(e) => handleSubmit(e)}>
        enviar
      </button>
    </div>
  );
};

export default FixedTermForm;
