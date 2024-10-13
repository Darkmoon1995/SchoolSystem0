import React from 'react';
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";

export default function ProfileSection({ student }) {
  return (
    <Card className="bg-gray-800 text-gray-100">
      <CardHeader>
        <CardTitle>Personal Information</CardTitle>
      </CardHeader>
      <CardContent className="flex items-center space-x-4">
        <img
          src="/placeholder.svg?height=100&width=100"
          alt="Profile"
          className="w-24 h-24 rounded-full"
        />
        <div>
          <h2 className="text-2xl font-bold">{student.fullName}</h2>
          <p className="text-sm text-gray-400">Student</p>
          <p><strong>Date of Birth:</strong> {new Date(student.dateOfBirth).toLocaleDateString()}</p>
          <p><strong>Gender:</strong> {student.gender}</p>
          <p><strong>Student ID:</strong> {student.studentId}</p>
        </div>
      </CardContent>
    </Card>
  );
}