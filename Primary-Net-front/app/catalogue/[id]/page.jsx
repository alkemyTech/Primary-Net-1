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
    <div>
      El detalle del catalogue es:
      <ul>
        <li>{catalogue.data.name}</li>
        <li>{catalogue.data.image}</li>
        <li>{catalogue.data.points}</li>
      </ul>
    </div>
  );
}