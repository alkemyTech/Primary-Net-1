import { authOptions } from '@/pages/api/auth/[...nextauth]';
import { getServerSession } from 'next-auth';
import React from 'react';
import Navbar from '../../../components/Nav'
const fetchData = async (token, id) => {
  return await fetch(`https://localhost:7131/api/fixedTerm/userAccount/${id}`, {
    headers: { Authorization: 'Bearer ' + token }
  });
};

export default async function FixedTerms() {
  const session = await getServerSession(authOptions);
  const data = await fetchData(session.user.accessToken, session.user.id);
  const fixedTerms = await data.json();
  console.log(fixedTerms);
  return (
    <div>
      {/* Navbar */}
      <Navbar />
      <div class="bg-gray-100 p-4 h-screen flex items-center justify-center">
        <div class="bg-white p-4 rounded-lg shadow-md w-full items-center justify-center">
        <p class="text-lg font-bold mb-2">Estas son sus transacciones:</p>
        <ul>
          {fixedTerms.data.map((ft) => (
            <li key={ft.id}>
              <li class="mb-2"><span class="font-bold">Numero:</span> {ft.id} <span class="font-bold">Monto:</span> {ft.amount}  <span class="font-bold">Fecha de cierre:</span> {ft.closingDate}</li>
            </li>
        ))}
        </ul>
        </div>
      </div>
    </div>

  );
}
