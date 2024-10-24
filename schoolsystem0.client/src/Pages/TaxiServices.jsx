import React, { useEffect, useState } from 'react';

const loadScript = (url) => {
    return new Promise((resolve, reject) => {
        const script = document.createElement('script');
        script.src = url;
        script.async = true;
        script.onload = resolve;
        script.onerror = reject;
        document.body.appendChild(script);
    });
};

const MapComponent = () => {
    const [exampleResponse, setExampleResponse] = useState(null);

    // Define initializeMap before using it
    const initializeMap = (data) => {
        const neshanMap = new nmp_mapboxgl.Map({
            mapType: nmp_mapboxgl.Map.mapTypes.neshanVector,
            container: "map",
            zoom: 15,
            pitch: 0,
            center: [51.391165, 35.700956],
            minZoom: 2,
            maxZoom: 21,
            trackResize: true,
            mapKey: "web.57011b28fe9242c2a8903262bb2bfd1e", // Replace with your actual API key
            poi: false,
            traffic: false,
            mapTypeControllerOptions: {
                show: true,
                position: 'bottom-left'
            }
        });

        const routes = [];
        const points = [];

        for (let k = 0; k < data.routes.length; k++) {
            for (let j = 0; j < data.routes[k].legs.length; j++) {
                for (let i = 0; i < data.routes[k].legs[j].steps.length; i++) {
                    const step = data.routes[k].legs[j].steps[i]["polyline"];
                    const point = data.routes[k].legs[j].steps[i]["start_location"];

                    const route = polyline.decode(step, 5);
                    route.map(item => { item.reverse() });

                    routes.push(route);
                    points.push(point);
                }
            }
        }

        const routeObj = {
            type: 'FeatureCollection',
            features: [
                {
                    type: 'Feature',
                    geometry: {
                        type: 'MultiLineString',
                        coordinates: routes
                    }
                }
            ]
        };

        const pointsObj = {
            type: 'FeatureCollection',
            features: [
                {
                    type: "Feature",
                    geometry: {
                        "type": "MultiPoint",
                        "coordinates": points
                    }
                }
            ]
        };

        neshanMap.on('load', function () {
            neshanMap.addSource('route', {
                type: 'geojson',
                data: routeObj
            });
            neshanMap.addSource('points1', {
                type: 'geojson',
                data: pointsObj
            });

            neshanMap.addLayer({
                id: 'route-line',
                type: 'line',
                source: 'route',
                layout: {
                    'line-join': 'round',
                    'line-cap': 'round'
                },
                paint: {
                    'line-color': '#250ECD',
                    'line-width': 9
                }
            });

            neshanMap.addLayer({
                id: 'points1',
                type: 'circle',
                source: 'points1',
                paint: {
                    "circle-color": "#9fbef9",
                    "circle-stroke-color": "#FFFFFF",
                    "circle-stroke-width": 2,
                    "circle-radius": 5
                }
            });
        });
    };

    useEffect(() => {
        const token = localStorage.getItem('token'); // Get the token from localStorage

        const fetchData = async () => {
            try {
                const response = await fetch('https://localhost:7287/api/Students/group-near-school?schoolLat=50&schoolLon=50', {
                    method: 'GET',
                    headers: {
                        'accept': '*/*',
                        'Authorization': `Bearer ${token}`
                    }
                });

                if (!response.ok) {
                    throw new Error('Failed to fetch data');
                }

                const data = await response.json();
                setExampleResponse(data); // Set the fetched data to exampleResponse
            } catch (error) {
                console.error("Error fetching data", error);
            }
        };

        // Load Neshan SDK and Mapbox Polyline library dynamically
        const loadNeshanSDK = async () => {
            try {
                await loadScript("https://static.neshan.org/sdk/mapboxgl/v1.13.2/neshan-sdk/v1.1.1/index.js");
                await loadScript("https://cdnjs.cloudflare.com/ajax/libs/mapbox-polyline/1.2.1/polyline.js");
                fetchData();
            } catch (error) {
                console.error("Error loading scripts", error);
            }
        };

        loadNeshanSDK();
    }, []);

    useEffect(() => {
        if (exampleResponse) {
            initializeMap(exampleResponse); // Initialize the map once the data is fetched
        }
    }, [exampleResponse]);

    return (
        <div style={{ height: "100vh", width: "100vw", margin: 0 }}>
            <div id="map" style={{ height: "100%", width: "100%" }}></div>
        </div>
    );
}

export default MapComponent;
