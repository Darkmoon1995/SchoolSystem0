import React, { useState, useEffect } from 'react'
import { Outlet, Link, useLocation } from 'react-router-dom'
import { Loader2 } from 'lucide-react'

export default function StudentDashboard() {
    const [student, setStudent] = useState(null)
    const [isLoading, setIsLoading] = useState(true)
    const [error, setError] = useState('')
    const location = useLocation()

    useEffect(() => {
        const fetchStudentData = async () => {
            try {
                const token = localStorage.getItem('token')
                if (!token) {
                    throw new Error('No token found')
                }

                const response = await fetch('https://localhost:7287/api/Students/Myself', {
                    headers: {
                        'accept': 'text/plain',
                        'Authorization': `Bearer ${token}`
                    }
                })

                if (!response.ok) {
                    throw new Error('Failed to fetch student data')
                }

                const data = await response.json()
                setStudent(data)
            } catch (err) {
                setError('Error fetching student data. Please try again later.')
            } finally {
                setIsLoading(false)
            }
        }

        fetchStudentData()
    }, [])

    if (isLoading) {
        return (
            <div className="flex items-center justify-center min-h-screen bg-gray-900">
                <Loader2 className="h-8 w-8 animate-spin text-blue-500" />
            </div>
        )
    }

    if (error) {
        return <div className="text-red-500 text-center mt-4">{error}</div>
    }

    const isActive = (path) => location.pathname === `/dashboard${path}`

    return (
        <div className="flex h-screen bg-gray-100">
            {/* Sidebar */}
            <div className="w-64 bg-white shadow-md">
                <div className="p-4">
                    <h2 className="text-xl font-semibold">{student?.fullName}</h2>
                    <p className="text-sm text-gray-600">{student?.schoolDetails?.grade}</p>
                    <p className="text-sm mt-2">
                        {student?.isPresent ? (
                            <span className="text-green-600">Present</span>
                        ) : (
                            <span className="text-red-600">Absent</span>
                        )}
                    </p>
                </div>
                <nav className="mt-4">
                    <Link to="/dashboard" className={`block py-2 px-4 ${isActive('') ? 'bg-gray-200' : 'hover:bg-gray-100'}`}>News</Link>
                    <Link to="/dashboard/expenses" className={`block py-2 px-4 ${isActive('/expenses') ? 'bg-gray-200' : 'hover:bg-gray-100'}`}>Expenses</Link>
                    <Link to="/dashboard/class-list" className={`block py-2 px-4 ${isActive('/class-list') ? 'bg-gray-200' : 'hover:bg-gray-100'}`}>Class List</Link>
                    <Link to="/dashboard/taxi-services" className={`block py-2 px-4 ${isActive('/taxi-services') ? 'bg-gray-200' : 'hover:bg-gray-100'}`}>Taxi Services</Link>
                    <Link to="/dashboard/library" className={`block py-2 px-4 ${isActive('/library') ? 'bg-gray-200' : 'hover:bg-gray-100'}`}>Library</Link>
                    <Link to="/dashboard/Tests" className={`block py-2 px-4 ${isActive('/library') ? 'bg-gray-200' : 'hover:bg-gray-100'}`}>Library</Link>
                </nav>
            </div>

            {/* Main content */}
            <div className="flex-1 p-8 overflow-auto">
                <Outlet context={{ student }} />
            </div>
        </div>
    )
}