import axios from 'axios';
import { useSession, signIn, signOut } from 'next-auth/react';

export const UserList = ()=> 
{
  const { data: session } = useSession();
  let token = session.accessToken
  console.log(token)
  const data = axios
          .get(
            'https://localhost:7131/api/user',
            {},
            {
              Headers: {
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
}

export default UserList;

