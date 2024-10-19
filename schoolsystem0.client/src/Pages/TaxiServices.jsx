import React, { useEffect, useRef, useState } from 'react';
import PropTypes from 'prop-types';
import { Loader2, MapPin } from 'lucide-react';
import { Button } from "@/components/ui/button"
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"

function TaxiServices() {
    const mapContainer = useRef(null);
    const map = useRef(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [selectedGroup, setSelectedGroup] = useState(null);

    useEffect(() => {
        const loadNeshanSDK = () => {
            const script = document.createElement('script');
            script.src = 'https://static.neshan.org/sdk/leaflet/1.4.0/leaflet.js'; // Example Neshan SDK script
            script.async = true;
            script.onload = () => initMap(); // Call the function to initialize the map
            document.body.appendChild(script);
        };

        const initMap = () => {
            if (!mapContainer.current || map.current) return;

            map.current = new neshanMap.Map({
                key: API_KEY,
                center: [schoolLon, schoolLat],
                zoom: 14,
            });

            map.current.on('load', () => {
                setLoading(false);
                fetchStudentGroups(schoolLat, schoolLon);
            });
        };

        loadNeshanSDK();
    }, []);


    const fetchStudentGroups = async (schoolLat, schoolLon) => {
        try {
            const token = localStorage.getItem('token');
            const response = await fetch(
                `https://localhost:7287/api/Students/group-near-school?schoolLat=${schoolLat}&schoolLon=${schoolLon}`,
                {
                    headers: {
                        'accept': 'text/plain',
                        'Authorization': `Bearer ${token}`
                    }
                }
            );

            if (!response.ok) {
                throw new Error('Failed to fetch student groups');
            }

            const data = await response.json();
            addMarkersToMap(data, schoolLat, schoolLon);
        } catch (err) {
            setError('Error fetching student groups. Please try again later.');
            console.error('Error:', err);
        }
    };

    const addMarkersToMap = (studentGroups, schoolLat, schoolLon) => {
        // Add school marker
        new neshanMap.Marker()
            .setLngLat([schoolLon, schoolLat])
            .setPopup(new neshanMap.Popup().setHTML("<h3>School</h3>"))
            .addTo(map.current);

        // Add student group markers
        studentGroups.forEach((group, index) => {
            const avgLat = group.reduce((sum, student) => sum + student.contactInformation.latitude, 0) / group.length;
            const avgLon = group.reduce((sum, student) => sum + student.contactInformation.longitude, 0) / group.length;

            const marker = new neshanMap.Marker({ color: '#FF0000' })
                .setLngLat([avgLon, avgLat])
                .setPopup(new neshanMap.Popup().setHTML(`<h3>Group ${index + 1}</h3><p>${group.length} students</p>`))
                .addTo(map.current);

            marker.getElement().addEventListener('click', () => setSelectedGroup(group));
        });
    };

    const handleRequestTaxi = () => {
        if (selectedGroup) {
            alert(`Taxi requested for ${selectedGroup.length} students in this group.`);
            // Here you would typically make an API call to request the taxi
        }
    };

    if (loading) {
        return (
            <div className="flex items-center justify-center h-screen">
                <Loader2 className="h-8 w-8 animate-spin text-primary" />
            </div>
        );
    }

    if (error) {
        return <div className="text-red-500 p-4">{error}</div>;
    }

    return (
        <div className="h-screen flex flex-col p-4">
            <h1 className="text-2xl font-bold mb-4">Taxi Services</h1>
            <div className="flex flex-col md:flex-row gap-4 h-full">
                <div ref={mapContainer} className="flex-grow h-[400px] md:h-auto rounded-lg shadow-md" />
                <Card className="w-full md:w-64">
                    <CardHeader>
                        <CardTitle>Selected Group</CardTitle>
                    </CardHeader>
                    <CardContent>
                        {selectedGroup ? (
                            <>
                                <p className="mb-2">{selectedGroup.length} students in this group</p>
                                <ul className="list-disc pl-5 mb-4">
                                    {selectedGroup.map(student => (
                                        <li key={student.id}>{student.fullName}</li>
                                    ))}
                                </ul>
                                <Button onClick={handleRequestTaxi} className="w-full">
                                    <MapPin className="mr-2 h-4 w-4" /> Request Taxi
                                </Button>
                            </>
                        ) : (
                            <p>Select a group on the map to view details and request a taxi.</p>
                        )}
                    </CardContent>
                </Card>
            </div>
        </div>
    );
}

TaxiServices.propTypes = {
};

export default TaxiServices;