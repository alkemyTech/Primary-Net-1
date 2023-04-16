'use client';
import { useSession } from 'next-auth/react';
import React from 'react';
import axios from 'axios';
import Navbar from '../../../components/Nav'

const AccountInsert = () => {
  const { data: session } = useSession();

  const handleSubmit = () => {
    if (session) {
      axios
        .post(
          'https://localhost:7131/api/account',
          {},
          { headers: { Authorization: 'Bearer ' + session.user.accessToken } }
        )
        .then(() => {

        });
    }
  };
  

  return (
    <div className="min-h-screen bg-gray-100 font-montserrat">
      {/* Navbar */}
      <Navbar />
      {/* Contenido principal */}
      <div className="flex justify-center items-center h-screen">
        <div className="bg-white p-6 rounded-lg shadow-md max-w-md w-full flex justify-center items-center">
          <div className="text-center">
            <h1 className="text-3xl font-semibold mb-4">¡Crea tu cuenta!</h1>
            <p className="text-gray-600 mb-6">
              Bienvenido a nuestra plataforma. Haz clic en el botón para crear tu cuenta y comenzar a disfrutar de nuestros servicios.
            </p>
            <button
              className="bg-blue-500 hover:bg-blue-600 text-white py-2 px-4 rounded-md cursor-pointer"
              onClick={() => handleSubmit()}
            >
              CREAR CUENTA
            </button>
          </div>
        </div>
      </div>

    </div>
  );
};

export default AccountInsert;