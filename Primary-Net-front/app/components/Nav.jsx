import Image from 'next/image';
import logo from "./assets/logo.png";

const Navbar = () => {
  return (
    <nav className="flex justify-between items-center px-4 py-3 bg-gradient-to-l from-blue-500 to-blue-300 text-white text-lg font-montserrat">
      <div className="flex items-center">
        <Image src={logo} alt="Logo" width={48} height={48} className="mr-4" style={{background: 'none'}} />
        <h1 className="font-bold"></h1>
      </div>
      <ul className="flex list-none">
        <li className="px-4">
          <a href="#" className="text-white no-underline hover:text-gray-300">Home</a>
        </li>
        <li className="px-4">
          <a href="#" className="text-white no-underline hover:text-gray-300">About Us</a>
        </li>
        <li className="px-4">
          <a href="#" className="text-white no-underline hover:text-gray-300">Services</a>
        </li>
        <li className="px-4">
          <a href="#" className="text-white no-underline hover:text-gray-300">Blog</a>
        </li>
        <li className="px-4">
          <a href="#" className="text-white no-underline hover:text-gray-300">Contact Us</a>
        </li>
      </ul>
    </nav>
  )
}

export default Navbar;
