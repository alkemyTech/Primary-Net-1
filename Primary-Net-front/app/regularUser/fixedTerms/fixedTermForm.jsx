'use client';
import React, { useState } from 'react';
import axios from 'axios';
import Navbar from '../../components/Nav'

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
      {/* Navbar */}
      <Navbar />
      <div className="h-screen flex justify-center items-center bg-gray-100"> 
        <div className="flex flex-col items-center mb-8">
          <form className="flex flex-col items-center bg-white p-6 rounded-lg">
            <input
              type="date"
              name="date"
              id="date_input"
              onChange={(e) => chnageDate(e)}
              className="px-4 py-2 border rounded-lg mb-4"
            />
            <input
              type="number"
              name=""
              id=""
              onChange={(e) => setAmount(e.target.value)}
              className="px-4 py-2 border rounded-lg mb-4"
            />
            <input
              type="number"
              className="px-4 py-2 border rounded-lg mb-4"
              onChange={(e) => setAccountId(e.target.value)}
            />

            <button class="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded" onClick={(e) => handleSubmit(e)}>
              enviar
            </button>
          </form>
        </div>
      </div>
    </div>
    
  );
};

export default FixedTermForm;
