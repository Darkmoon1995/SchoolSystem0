import React from 'react';
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table";

export default function ExpensesList({ costs }) {
  return (
    <Card className="bg-gray-800 text-gray-100 md:col-span-2">
      <CardHeader>
        <CardTitle>Costs</CardTitle>
      </CardHeader>
      <CardContent>
        <Table>
          <TableHeader>
            <TableRow>
              <TableHead className="text-gray-100">Description</TableHead>
              <TableHead className="text-gray-100">Amount</TableHead>
              <TableHead className="text-gray-100">Date Incurred</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {costs.map((cost) => (
              <TableRow key={cost.id}>
                <TableCell className="text-gray-100">{cost.description}</TableCell>
                <TableCell className="text-gray-100">${cost.amount.toFixed(2)}</TableCell>
                <TableCell className="text-gray-100">{new Date(cost.dateIncurred).toLocaleDateString()}</TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </CardContent>
    </Card>
  );
}