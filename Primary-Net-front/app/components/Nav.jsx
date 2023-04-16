'use client';
import Image from 'next/image';
import logo from "./assets/logo.png";
import React from 'react';
import { signOut } from 'next-auth/react';

const Navbar = () => {
  return (
    <nav className="flex justify-between items-center px-4 py-3 bg-gradient-to-l from-blue-500 to-blue-300 text-white text-lg font-montserrat">
      <div className="flex items-center">
        <Image src={logo} alt="Logo" width={48} height={48} className="mr-4" style={{background: 'none'}} />
        <h1 className="font-bold"></h1>
      </div>
      <ul className="flex list-none">
        <li className="px-4">
          <a href="/home" className="text-white no-underline hover:text-gray-300">Home</a>
        </li>
        <li className="px-4">
          <a href="/regularUser/accounts/find" className="text-white no-underline hover:text-gray-300">Mi Cuenta</a>
        </li>
        <li className="px-4">
          <a href="/regularUser/catalogues/find" className="text-white no-underline hover:text-gray-300">Catalogo</a>
        </li>
        <li className="px-4">
          <a href="#" className="text-white no-underline hover:text-gray-300">Deposito</a>
        </li>
        <li className="px-4">
          <a href="/regularUser/transfer" className="text-white no-underline hover:text-gray-300">Transacion</a>
        </li>
        <li className="px-4">
          <a href="#" className="text-white no-underline hover:text-gray-300">Plazo Fijo</a>
        </li>
        <li className="px-4">
          {/* <a href="#" className="text-white no-underline hover:text-gray-300">Log Out</a> */}
          <button onClick={() => signOut()} className="text-white no-underline hover:text-gray-300" >Sign Out</button>
        </li>
      </ul>
    </nav>
  )
}

export default Navbar;
