import React from 'react';

const UserInformation = ({ data }) => {
  return <pre>{JSON.stringify(data, null, 2)}</pre>;
};

export default UserInformation;
