import React from 'react'
import { BrowserRouter as Router, Route, Routes, Navigate } from 'react-router-dom'
import Login from './Pages/Login.jsx'
import News from './Pages/News'
import Expenses from './Pages/Expenses'
import ClassList from './Pages/ClassList'
import TaxiServices from './Pages/TaxiServices'
import Library from './Pages/Library'
import Attendance from './Pages/Attendance'
import Grades from './Pages/Grades'
import Layout from './Pages/Layout'

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
                        path="/"
                        element={
                            <PrivateRoute>
                                <Layout />
                            </PrivateRoute>
                        }
                    >
                        <Route index element={<Navigate to="/dashboard" replace />} />
                        <Route path="dashboard" element={<News />} />
                        <Route path="expenses" element={<Expenses />} />
                        <Route path="class-list" element={<ClassList />} />
                        <Route path="taxi-services" element={<TaxiServices />} />
                        <Route path="library" element={<Library />} />
                        <Route path="attendance" element={<Attendance />} />
                        <Route path="grades" element={<Grades />} />
                    </Route>
                </Routes>
            </div>
        </Router>
    )
}