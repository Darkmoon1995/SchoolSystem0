import React from 'react';
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";

export default function SchoolDetails({ schoolDetails }) {
  return (
    <Card className="bg-gray-800 text-gray-100">
      <CardHeader>
        <CardTitle>School Details</CardTitle>
      </CardHeader>
      <CardContent>
        <p><strong>School Name:</strong> {schoolDetails.schoolName}</p>
        <p><strong>Class Name:</strong> {schoolDetails.className}</p>
        <p><strong>Grade:</strong> {schoolDetails.grade}</p>
      </CardContent>
    </Card>
  );
}