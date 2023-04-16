'use client';
import { signIn } from 'next-auth/react';
import React, { useState } from 'react';

const Login = () => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const handleSubmit = async (e) => {
    e.preventDefault();
    const res = await signIn('credentials', {
      username: email,
      password,
      redirect: true,
      callbackUrl: '/home'
    });

    console.log(res);
  };

  return (
    <div class="h-screen flex justify-center items-center bg-gray-200">
      <div class="flex flex-col items-center mb-8">
        <h1 class="text-4xl font-bold mb-2">¡Bienvenido!</h1>
        <form class="flex flex-col items-center bg-white p-6 rounded-lg">
          <input
            type="text"
            name=""
            id=""
            class="px-4 py-2 border rounded-lg mb-4"
            onChange={(e) => setEmail(e.target.value)}
          />
          <input
           type="password"
            name=""
            id=""
            class="px-4 py-2 border rounded-lg mb-4"
            onChange={(e) => setPassword(e.target.value)}
          />
          <button 
            class="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded"
            onClick={(e) => handleSubmit(e)}>
              LOGIN
          </button>
        </form>
      </div>
    </div>
  );
};

export default Login;
