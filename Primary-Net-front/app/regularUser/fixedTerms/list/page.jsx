import { authOptions } from '@/pages/api/auth/[...nextauth]';
import { getServerSession } from 'next-auth';
import React from 'react';
import Navbar from '../../../components/Nav';
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
      <div className="bg-gray-100 p-4 h-screen flex items-center justify-center">
        <div className="bg-white p-4 rounded-lg shadow-md w-full items-center justify-center">
          <p className="text-lg font-bold mb-2">Estos son sus Plazos fijos:</p>
          <ul>
            {fixedTerms.data.map((ft) => (
              <li key={ft.id}>
                <li className="mb-2">
                  <span className="font-bold">Numero:</span> {ft.id} |
                  <span className="font-bold">Monto:</span> {ft.amount} |
                  <span className="font-bold">Fecha de cierre:</span>{' '}
                  {ft.closingDate}
                </li>
              </li>
            ))}
          </ul>
        </div>
      </div>
    </div>
  );
}
