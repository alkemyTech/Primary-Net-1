'use client';
import { useSession } from 'next-auth/react';
import React from 'react';
import axios from 'axios';
import Navbar from '../components/Nav'
import Footer from '../components/Footer'

const Home = () => {
  const { data: session } = useSession();
  if(session){
    return (
        <div className="min-h-screen bg-gray-100 font-montserrat">
          {/* Navbar */}
          <Navbar />
    
          {/* Contenido principal */}
          <div className="flex justify-center items-center h-screen">
            <div className="bg-white p-6 rounded-lg shadow-md max-w-md w-full flex justify-center items-center">
              <div className="text-center">
                <p className="text-gray-600 mb-6">
                  Bienvenido a nuestra plataforma. Comienza a disfrutar de nuestros servicios.
                </p>
              </div>
            </div>
          </div>
    
          {/* Footer */}
          <Footer />
        </div>
      );
  }
  
};

export default Home;