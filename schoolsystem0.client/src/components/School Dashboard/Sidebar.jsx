import React from 'react';
import { Home, Book, DollarSign, Truck, Library } from 'lucide-react';

export default function Sidebar() {
  return (
    <div className="w-64 bg-gray-800 text-white p-6">
      <h1 className="text-2xl font-bold mb-6">Student Dashboard</h1>
      <nav>
        <ul className="space-y-2">
          <li>
            <a href="#" className="flex items-center space-x-2 p-2 hover:bg-gray-700 rounded">
              <Home size={20} />
              <span>Home</span>
            </a>
          </li>
          <li>
            <a href="#" className="flex items-center space-x-2 p-2 hover:bg-gray-700 rounded">
              <Book size={20} />
              <span>Classes</span>
            </a>
          </li>
          <li>
            <a href="#" className="flex items-center space-x-2 p-2 hover:bg-gray-700 rounded">
              <DollarSign size={20} />
              <span>Expenses</span>
            </a>
          </li>
          <li>
            <a href="#" className="flex items-center space-x-2 p-2 hover:bg-gray-700 rounded">
              <Truck size={20} />
              <span>Taxi Service</span>
            </a>
          </li>
          <li>
            <a href="#" className="flex items-center space-x-2 p-2 hover:bg-gray-700 rounded">
              <Library size={20} />
              <span>Library</span>
            </a>
          </li>
        </ul>
      </nav>
    </div>
  );
}