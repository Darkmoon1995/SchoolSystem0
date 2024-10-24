import React, { useEffect, useState } from 'react'
import { Card } from "@/components/ui/card"
import { Button } from "@/components/ui/button"
import { AlertCircle, MapPin } from "lucide-react"
import { Alert, AlertDescription, AlertTitle } from "@/components/ui/alert"

const API_URL = 'https://localhost:7287/api/Students/group-near-school'
const MAP_KEY = "web.57011b28fe9242c2a8903262bb2bfd1e" // Replace with your actual API key

const loadScript = (url) => {
    return new Promise((resolve, reject) => {
        const script = document.createElement('script')
        script.src = url
        script.async = true
        script.onload = () => resolve()
        script.onerror = reject
        document.body.appendChild(script)
    })
}

export default function TaxiServices() {
    const [mapData, setMapData] = useState(null)
    const [error, setError] = useState(null)

    useEffect(() => {
        const initializeMap = async () => {
            try {
                await loadScript("https://static.neshan.org/sdk/mapboxgl/v1.13.2/neshan-sdk/v1.1.1/index.js")
                await loadScript("https://cdnjs.cloudflare.com/ajax/libs/mapbox-polyline/1.2.1/polyline.js")
                await fetchMapData()
            } catch (error) {
                setError("Failed to load map resources. Please try again later.")
            }
        }

        initializeMap()
    }, [])

    const fetchMapData = async () => {
        const token = localStorage.getItem('token')
        if (!token) {
            setError("Authentication token not found. Please log in.")
            return
        }

        try {
            const response = await fetch(`${API_URL}?schoolLat=35&schoolLon=50`, {
                headers: {
                    'accept': '*/*',
                    'Authorization': `Bearer ${token}`
                }
            })

            if (!response.ok) throw new Error('Failed to fetch map data')

            const data = await response.json()
            setMapData(data)
        } catch (error) {
            setError("Failed to fetch map data. Please try again later.")
        }
    }

    useEffect(() => {
        if (mapData && window.nmp_mapboxgl) {
            const neshanMap = new window.nmp_mapboxgl.Map({
                mapType: window.nmp_mapboxgl.Map.mapTypes.neshanVector,
                container: "map",
                zoom: 15,
                center: [50, 35],
                minZoom: 2,
                maxZoom: 21,
                pitch: 0,
                mapKey: MAP_KEY,
                poi: false,
                traffic: false,
            })

            const routes = []
            const points = []

            mapData.routes.forEach(route => {
                route.legs.forEach(leg => {
                    leg.steps.forEach(step => {
                        const route = window.polyline.decode(step.polyline, 5).map(item => item.reverse())
                        routes.push(route)
                        points.push(step.start_location)
                    })
                })
            })

            const routeObj = {
                type: 'FeatureCollection',
                features: [{ type: 'Feature', geometry: { type: 'MultiLineString', coordinates: routes } }]
            }

            const pointsObj = {
                type: 'FeatureCollection',
                features: [{ type: "Feature", geometry: { type: "MultiPoint", coordinates: points } }]
            }

            neshanMap.on('load', () => {
                neshanMap.addSource('route', { type: 'geojson', data: routeObj })
                neshanMap.addSource('points', { type: 'geojson', data: pointsObj })

                neshanMap.addLayer({
                    id: 'route-line',
                    type: 'line',
                    source: 'route',
                    layout: { 'line-join': 'round', 'line-cap': 'round' },
                    paint: { 'line-color': '#250ECD', 'line-width': 4 }
                })

                neshanMap.addLayer({
                    id: 'route-points',
                    type: 'circle',
                    source: 'points',
                    paint: {
                        "circle-color": "#9fbef9",
                        "circle-stroke-color": "#FFFFFF",
                        "circle-stroke-width": 2,
                        "circle-radius": 5
                    }
                })

                // Hide the map type control
                const style = document.createElement('style')
                style.textContent = '.control.map-type-control.pos-relative { display: none !important; }'
                document.head.appendChild(style)
            })
        }
    }, [mapData])

    if (error) {
        return (
            <Alert variant="destructive">
                <AlertCircle className="h-4 w-4" />
                <AlertTitle>Error</AlertTitle>
                <AlertDescription>{error}</AlertDescription>
            </Alert>
        )
    }

    return (
        <Card className="w-full h-[calc(100vh-4rem)] overflow-hidden">
            <div id="map" className="w-full h-full" />
            {!mapData && (
                <div className="absolute inset-0 flex items-center justify-center bg-background/50">
                    <Button disabled>
                        <MapPin className="mr-2 h-4 w-4 animate-pulse" /> Loading Map...
                    </Button>
                </div>
            )}
        </Card>
    )
}