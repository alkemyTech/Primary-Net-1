'use client';
import axios from 'axios';
import React, { useState } from 'react';

const CreateRoleForm = (token) => {
  const [description, setDescription] = useState();
  const [name, setName] = useState();

  const handleSubmit = async (e) => {
    await axios
      .post(
        'https://localhost:7131/api/role',
        {
          Description: description,
          Name: name
        },
        { headers: { Authorization: 'Bearer ' + token.token } }
      )
      .then((res) => console.log(res.data))
      .catch((err) => {
        throw new Error(err.message);
      });
  };
  return (
    <div>
      <form action="">
        <input
          type="text"
          name="name"
          id="name"
          placeholder="nombre"
          onChange={(e) => setName(e.target.value)}
        />
        <input
          type="text"
          name="description"
          id="desc"
          placeholder="descripcion"
          onChange={(e) => setDescription(e.target.value)}
        />
        <button onClick={() => handleSubmit()}>CREAR ROL</button>
      </form>
    </div>
  );
};

export default CreateRoleForm;
