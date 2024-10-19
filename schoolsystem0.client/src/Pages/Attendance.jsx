import React, { useState, useEffect } from 'react';
import { useOutletContext } from 'react-router-dom';
import { Calendar } from '@/components/ui/calendar';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';

export default function Attendance() {
  const { student } = useOutletContext();
  const [absences, setAbsences] = useState([]);

  useEffect(() => {
    const fetchAbsences = async () => {
      try {
        const response = await fetch(`https://localhost:7287/api/Students/${student.id}/absences`, {
          headers: {
            'Authorization': `Bearer ${localStorage.getItem('token')}`
          }
        });
        if (response.ok) {
          const data = await response.json();
          setAbsences(data.map(date => new Date(date)));
        }
      } catch (error) {
        console.error('Error fetching absences:', error);
      }
    };

    fetchAbsences();
  }, [student.id]);

  return (
    <Card className="w-full max-w-3xl mx-auto">
      <CardHeader>
        <CardTitle className="text-2xl font-bold">Attendance Calendar</CardTitle>
      </CardHeader>
      <CardContent>
        <Calendar
          mode="multiple"
          selected={absences}
          className="rounded-md border"
          modifiers={{
            absent: (date) => absences.some(absentDate => 
              date.getDate() === absentDate.getDate() &&
              date.getMonth() === absentDate.getMonth() &&
              date.getFullYear() === absentDate.getFullYear()
            )
          }}
          modifiersStyles={{
            absent: { backgroundColor: 'rgb(239 68 68)', color: 'white' }
          }}
        />
        <p className="mt-4 text-sm text-gray-500">Red dates indicate absences</p>
      </CardContent>
    </Card>
  );
}