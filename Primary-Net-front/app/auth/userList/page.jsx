'use client';
import UserList from '@/pages/api/user/user_list';
import { useSession, signIn, signOut} from 'next-auth/react';
import UserInformation from '../../components/UserInformation';
import axios from "axios";


const UsersList = () => {
  const { data: session } = useSession();

  // una peticion a la api para obtener los datos de los usuarios
  console.log(session)
  const token = session.accessToken;
  console.log(token)
  const users = axios.get(
                'https://localhost:7131/api/user',
                {},
                {
                  headers: {
                    'Authorization': `Bearer ${token}`
        
                  }
                }
              )
              .then((res) => {
                return res.data;
              })
              .catch((err) => {
                throw new Error(err.message);
              });
  console.log(users)
  console.log(session);
  if (session) {
    return (
      <>
        <UserInformation data={session.isAdmin} />

        <div>
        <h1>Lista de elementos:</h1>
          {/* <ul>
            {users.map(users => (
              <li key={users.id}>{users.firstName}</li>
            ))}
          </ul> */}
        </div>

      </>
    );
  }
};

export default UsersList;