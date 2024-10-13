import React, { useState, useEffect } from 'react';
import Sidebar from '../components/School Dashboard/Sidebar.jsx';
import ProfileSection from '../components/School Dashboard/ProfileSection.jsx';
import ContactInformation from '../components/School Dashboard/ContactInformation.jsx';
import SchoolDetails from '../components/School Dashboard/SchoolDetails.jsx';
import GradesSection from '../components/School Dashboard/GradesSection.jsx';
import ExpensesList from '../components/School Dashboard/ExpensesList.jsx';
import NewsSection from '../components/School Dashboard/NewsSection.jsx';
import ClassList from '../components/School Dashboard/ClassList.jsx';
import TaxiService from '../components/School Dashboard/TaxiService.jsx';
import Library from '../components/School Dashboard/Library.jsx';
import { Loader2, AlertCircle } from "lucide-react";

export default function Layout() {
    const [student, setStudent] = useState(null);
    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState('');

    useEffect(() => {
        const fetchStudentData = async () => {
            try {
                const token = localStorage.getItem('token');
                if (!token) {
                    throw new Error('No token found');
                }

                const response = await fetch('https://localhost:7287/api/Students/Myself', {
                    headers: {
                        'accept': 'text/plain',
                        'Authorization': `Bearer ${token}`
                    }
                });

                if (!response.ok) {
                    throw new Error('Failed to fetch student data');
                }

                const data = await response.json();
                setStudent(data);
            } catch (err) {
                setError('Error fetching student data. Please try again later.');
            } finally {
                setIsLoading(false);
            }
        };

        fetchStudentData();
    }, []);

    if (isLoading) {
        return (
            <div className="flex items-center justify-center min-h-screen bg-gray-900">
                <Loader2 className="h-8 w-8 animate-spin text-blue-500" />
            </div>
        );
    }

    if (error) {
        return (
            <div className="flex items-center justify-center min-h-screen bg-gray-900 text-white">
                <div className="flex items-center space-x-2">
                    <AlertCircle className="h-6 w-6 text-red-500" />
                    <span>{error}</span>
                </div>
            </div>
        );
    }

    if (!student) {
        return (
            <div className="flex items-center justify-center min-h-screen bg-gray-900 text-white">
                No student data available.
            </div>
        );
    }

    return (
        <div className="flex min-h-screen bg-gray-900">
            <Sidebar />
            <div className="flex-1 p-8">
                <h1 className="text-3xl font-bold mb-8 text-gray-100">Student Dashboard</h1>
                <div className="grid grid-cols-1 md:grid-cols-2 gap-8">
                    <ProfileSection student={student} />
                    <ContactInformation contactInfo={student.contactInformation} />
                    <SchoolDetails schoolDetails={student.schoolDetails} />
                    <GradesSection grades={student.grades} />
                    <ExpensesList costs={student.costs} />
                    <NewsSection />
                    <ClassList />
                    <TaxiService />
                    <Library />
                </div>
            </div>
        </div>
    );
}