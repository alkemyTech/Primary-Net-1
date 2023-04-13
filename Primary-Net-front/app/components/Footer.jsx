import Image from 'next/image';
import logo from "./assets/logo.png";
import google from "./assets/google.svg";
import appmac from "./assets/appmac.svg";

const Footer = () => {
  return (
    <footer className="flex justify-between items-center px-4 py-3 bg-gradient-to-l from-blue-500 to-blue-300 text-white text-lg font-montserrat"
      style={{ position: 'fixed', bottom: 0, left: 0, right: 0 }}>
      <div>
        <h3 className="text-xl font-semibold mb-2 text-white">Descarga nuestra App</h3>
        <p className="text-sm mb-6 text-white">¡Paga en línea de forma rápida y segura, desde cualquier lugar!</p>
        <div className="flex items-center">
          <Image src={google} alt="Google Play" width="25" height="25" className="mr-2" />
          <Image src={appmac} alt="App Store" width="25" height="25" />
        </div>
      </div>
      <div className="flex-grow"></div> {/* Espacio vacío en el centro */}
      <div>
        <h3 className="text-xl font-semibold mb-2 text-white">Ayuda y Soporte</h3>
        <ul className="list-none">
          <li className="mb-2"><a href="#" className="text-white hover:text-gray-400 no-underline">Centro de ayuda</a></li>
          <li className="mb-2"><a href="#" className="text-white hover:text-gray-400 no-underline">Contacto</a></li>
          {/* <li className="mb-2"><a href="#" className="text-white hover:text-black no-underline">Términos y Condiciones</a></li> */}
          <li><a href="#" className="text-white hover:text-gray-400 no-underline">Política de privacidad</a></li>
        </ul>
      </div>
    </footer>
  );
};

export default Footer;
