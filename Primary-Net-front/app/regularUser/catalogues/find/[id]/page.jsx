import React from 'react';
import { getServerSession } from 'next-auth';
import { authOptions } from '@/pages/api/auth/[...nextauth]';

const fetchCatalogueData = async (id, accessToken) => {
  return await fetch(`https://localhost:7131/api/catalogue/${id}`, {
    headers: {
      'Content-Type': 'application/json',
      Authorization: 'Bearer ' + accessToken
    }
  }).then((res) => res.json());
};

export default async function CatalogueDetail({ params }) {
  const session = await getServerSession(authOptions);
  const { id } = params;
  const catalogue = await fetchCatalogueData(id, session.user.accessToken);
  console.log(catalogue);
  return (
    <div class="bg-gray-200 p-4 h-screen flex items-center justify-center">
      <div class="bg-white p-4 rounded-lg shadow-md w-full items-center justify-center">
        <p class="text-lg font-bold mb-2">El detalle del catálogo es:</p>
        <ul>
          <li class="mb-2"><span class="font-bold">Nombre:</span> {catalogue.data.name}</li>
          <li class="mb-2"><span class="font-bold">Imagen:</span> {catalogue.data.image}</li>
          <li><span class="font-bold">Puntos:</span> {catalogue.data.points}</li>
        </ul>
      </div>
  </div>
  );
}