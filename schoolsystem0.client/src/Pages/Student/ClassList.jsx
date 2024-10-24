import React from 'react';
import { useOutletContext } from 'react-router-dom';

export default function ClassList() {
  const { student } = useOutletContext();

  // Sample class list data
  const classList = [
    { id: 1, name: "John Doe" },
    { id: 2, name: "Jane Smith" },
    { id: 3, name: "Alice Johnson" },
    { id: 4, name: "Bob Williams" },
    { id: 5, name: "Charlie Brown" },
  ];

  if (!student?.schoolDetails?.className) {
    return <h1 className="text-2xl font-bold">Class list not available</h1>;
  }

  return (
    <div>
      <h1 className="text-2xl font-bold mb-4">Class List</h1>
      <p className="mb-4">Your class: {student.schoolDetails.className}</p>
      <ul className="space-y-2">
        {classList.map((classmate) => (
          <li key={classmate.id} className="bg-white p-2 rounded shadow">
            {classmate.name}
          </li>
        ))}
      </ul>
    </div>
  );
}