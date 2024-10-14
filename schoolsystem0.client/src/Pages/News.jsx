import React from 'react';
import { useOutletContext } from 'react-router-dom';

export default function News() {
  const { student } = useOutletContext();

  // Sample news data
  const news = [
    { id: 1, title: "End of Year Exams Schedule", content: "The final exams will be held from June 15th to June 25th. Please check the detailed schedule on the school website." },
    { id: 2, title: "Summer School Registration Open", content: "Registration for summer school programs is now open. Visit the admin office to sign up before May 30th." },
    { id: 3, title: "New Library Books Arrived", content: "Our school library has received a new shipment of books. Check them out starting next week!" },
  ];

  return (
    <div>
      <h1 className="text-2xl font-bold mb-4">News</h1>
      <div className="space-y-4">
        {news.map((item) => (
          <div key={item.id} className="bg-white p-4 rounded shadow">
            <h2 className="text-xl font-semibold mb-2">{item.title}</h2>
            <p>{item.content}</p>
          </div>
        ))}
      </div>
    </div>
  );
}