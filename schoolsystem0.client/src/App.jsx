import React from 'react'
import { BrowserRouter as Router, Route, Routes, Navigate } from 'react-router-dom'
import Login from './Pages/Login.jsx'
import StudentDashboard from './Pages/StudentDashboard.jsx'

const PrivateRoute = ({ children }) => {
    const token = localStorage.getItem('token')
    return token ? children : <Navigate to="/login" replace />
}

export default function App() {
    return (
        <Router>
            <div className="min-h-screen bg-gray-900">
                <Routes>
                    <Route path="/login" element={<Login />} />
                    <Route
                        path="/dashboard"
                        element={
                            <PrivateRoute>
                                <StudentDashboard />
                            </PrivateRoute>
                        }
                    />
                    <Route path="/" element={<Navigate to="/login" replace />} />
                </Routes>
            </div>
        </Router>
    )
}