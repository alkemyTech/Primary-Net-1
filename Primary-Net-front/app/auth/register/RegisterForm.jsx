'use client';
import React, { useState } from 'react';
import axios from 'axios';
import { signIn } from 'next-auth/react';

const RegisterForm = () => {
  const [firstName, setfirstName] = useState();
  const [lastName, setLastname] = useState();
  const [email, setEmail] = useState();
  const [password, setPassword] = useState();

  const RegisterSignIn = async () => {
    await signIn('credentials', {
      username: email,
      password,
      redirect: true,
      callbackUrl: '/'
    });
  };

  const handleRegister = (e) => {
    e.preventDefault();
    axios
      .post('https://localhost:7131/api/register', {
        FirstName: firstName,
        LastName: lastName,
        Email: email,
        Password: password
      })
      .then((res) => {
        RegisterSignIn();
      })
      .catch((err) => {
        throw new Error(err.message);
      });
  };
  return (
    <div class="h-screen flex justify-center items-center bg-gray-200">
  <div class="bg-white p-8 rounded-lg shadow-md">
    <h1 class="text-2xl font-bold mb-4">Registrarse:</h1>
    <form class="flex flex-col gap-4">
      <input
        type="text"
        name="firstName"
        placeholder="Nombre"
        id="firstName"
        class="px-4 py-2 border rounded-lg"
        onChange={(e) => setfirstName(e.target.value)}
      />
      <input
        type="text"
        name="lastName"
        placeholder="Apellido"
        id="lastName"
        class="px-4 py-2 border rounded-lg"
        onChange={(e) => setLastname(e.target.value)}
      />
      <input
        type="email"
        name="email"
        placeholder="Email"
        id="email"
        class="px-4 py-2 border rounded-lg"
        onChange={(e) => setEmail(e.target.value)}
      />
      <input
        type="password"
        name="password"
        placeholder="Contraseña"
        id="password"
        class="px-4 py-2 border rounded-lg"
        onChange={(e) => setPassword(e.target.value)}
      />
      <button
        class="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded"
        onClick={(e) => handleRegister(e)}
      >
        Registrarme
      </button>
    </form>
  </div>
</div>
  );
};

export default RegisterForm;
