import React from 'react';
import { useOutletContext } from 'react-router-dom';

export default function Expenses() {
  const { student } = useOutletContext();

  if (!student?.costs || student.costs.length === 0) {
    return <h1 className="text-2xl font-bold">No expenses found</h1>;
  }

  return (
    <div>
      <h1 className="text-2xl font-bold mb-4">Expenses</h1>
      <ul className="space-y-2">
        {student.costs.map((cost, index) => (
          <li key={index} className="bg-white p-4 rounded shadow">
            <span className="font-semibold">{cost.description}:</span> ${cost.amount.toFixed(2)}
            <span className="text-gray-600 ml-2">
              ({new Date(cost.dateIncurred).toLocaleDateString()})
            </span>
          </li>
        ))}
      </ul>
    </div>
  );
}