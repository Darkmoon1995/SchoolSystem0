import React from 'react';
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";

export default function ContactInformation({ contactInfo }) {
  return (
    <Card className="bg-gray-800 text-gray-100">
      <CardHeader>
        <CardTitle>Contact Information</CardTitle>
      </CardHeader>
      <CardContent>
        <p><strong>Email:</strong> {contactInfo.email}</p>
        <p><strong>Phone:</strong> {contactInfo.phoneNumber}</p>
        <p><strong>Address:</strong> {contactInfo.address}</p>
      </CardContent>
    </Card>
  );
}