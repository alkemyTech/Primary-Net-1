'use client';
import React, { useState } from 'react';
import axios from 'axios';
import Navbar from '../../components/Nav';

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
            <span className="text-gray-700 font-bold">Ingrese la fecha de finalización:</span>
            <input
              type="date"
              name="date"
              id="date_input"
              onChange={(e) => chnageDate(e)}
              className="px-4 py-2 border rounded-lg mb-4"
            />

            <span className="text-gray-700 font-bold">Ingrese monto:</span>
            <input
              type="number"
              name=""
              id=""
              onChange={(e) => setAmount(e.target.value)}
              className="px-4 py-2 border rounded-lg mb-4"
            />
            <span className="text-gray-700 font-bold">
              Confirme su numero de cuenta:
            </span>
            <input
              type="number"
              className="px-4 py-2 border rounded-lg mb-4"
              onChange={(e) => setAccountId(e.target.value)}
            />

            <span className="text-gray-700 font-bold">
              Tasa de Interés Nominal Actual:
            </span>
            <input
              type="number"
              value="35" readOnly={true}
              style={{ width: '75px', textAlign:'center', backgroundColor: '#f4f4f4'}}
              className="px-4 py-2 border rounded-lg mb-4"
              onChange={(e) => setAccountId(e.target.value)}
            />

            <button
              className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded"
              onClick={(e) => handleSubmit(e)}>
              enviar
            </button>
          </form>
        </div>
      </div>
    </div>
  );
};

export default FixedTermForm;
