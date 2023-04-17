'use client';
import { signIn, useSession } from 'next-auth/react';
import React, { useState } from 'react';
import Link from 'next/link';
import { redirect } from 'next/navigation';

const Login = () => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const { data: session } = useSession();
  const handleSubmit = async (e) => {
    e.preventDefault();
    const res = await signIn('credentials', {
      username: email,
      password,
      redirect: true,
      callbackUrl: '/'
    });

    console.log(res);
  };

  if (session) return redirect('/');
  return (
    <div className="h-screen flex justify-center items-center bg-gray-200">
      <div className="flex flex-col items-center mb-8">
        <h1 className="text-4xl font-bold mb-2">¡Bienvenidos!</h1>
        <form className="flex flex-col items-center bg-white p-6 rounded-lg">
          <input
            type="text"
            name=""
            id=""
            className="px-4 py-2 border rounded-lg mb-4"
            onChange={(e) => setEmail(e.target.value)}
          />
          <input
            type="password"
            name=""
            id=""
            className="px-4 py-2 border rounded-lg mb-4"
            onChange={(e) => setPassword(e.target.value)}
          />
          <div className="flex flex-col items-center gap-y-5">
            <button
              className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded"
              onClick={(e) => handleSubmit(e)}>
              LOGIN
            </button>

            <Link
              href="/auth/register"
              className="bg-red-500 p-2 text-white font-bold flex items-center justify-center rounded">
              Registrarse
            </Link>
          </div>
        </form>
      </div>
    </div>
  );
};

export default Login;
