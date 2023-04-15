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
    <div>
      Registrarse:
      <form action="">
        <input
          type="text"
          name="firstName"
          placeholder="nombre"
          id="firstName"
          onChange={(e) => setfirstName(e.target.value)}
        />
        <input
          type="text"
          name="lastName"
          placeholder="apellido"
          id="lastName"
          onChange={(e) => setLastname(e.target.value)}
        />
        <input
          type="email"
          name="email"
          placeholder="email"
          id="email"
          onChange={(e) => setEmail(e.target.value)}
        />
        <input
          type="password"
          name="password"
          placeholder="contraseña"
          id="password"
          onChange={(e) => setPassword(e.target.value)}
        />
        <button onClick={(e) => handleRegister(e)}>Registrarme</button>
      </form>
    </div>
  );
};

export default RegisterForm;
