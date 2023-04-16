import React from 'react';
import { getServerSession } from 'next-auth';
import { authOptions } from '@/pages/api/auth/[...nextauth]';

const fetchTransactionData = async (id, accessToken) => {
  return await fetch(`https://localhost:7131/api/transaction/${id}`, {
    headers: {
      'Content-Type': 'application/json',
      Authorization: 'Bearer ' + accessToken
    }
  }).then((res) => res.json());
};

export default async function TransactionDetail({ params }) {
  const session = await getServerSession(authOptions);
  const { id } = params;
  const transactions = await fetchTransactionData(id, session.user.accessToken);
  console.log(transactions);
  return (
    <div>
      El detalle del catalogue es:
      {/* <ul>
        <li>{catalogue.data.name}</li>
        <li>{catalogue.data.image}</li>
        <li>{catalogue.data.points}</li>
      </ul> */}
    </div>
  );
}

// export default async function TransactionList() {
//   const session = await getServerSession(authOptions);
//   if (session) {
//     const data = await fetch('https://localhost:7131/api/transaction/id', {
//       headers: {
//         'Content-Type': 'application/json',
//         Authorization: 'Bearer ' + session.user.accessToken
//       },
//       cache: 'no-store'
//     }).catch((err) => {
//       throw new Error(err.message);
//     });

//     const transactions = await data.json();

//     console.log(transactions);

//     return (
//       <div>
//         <h1>Lista de elementos:</h1>
//         <ul>
//           {transactions.data.items.map((item) => (
//             <li key={item.id}>
//               Amount:{item.amount} concepto:{item.concept} tipo:{item.type}
//             </li>
//           ))}
//         </ul>
//       </div>
//     );
//   }
//   return redirect('/');
// }