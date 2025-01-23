# *Droped

---
# School System
This project is a full-stack application with a **React** frontend and an **ASP.NET Core** backend.The idea was simple Do everything a school needs in simple project.

## Features
- A multi Login Feature (When login will Choose the user based on the Role) ✘
### Students
 - News/Announcement ✔
 - Expensess (Half Done)
 - Class List ✔
 - Taxi Services/Routing  ✔ ==> change to google map for it to work in the whole world 
 - Library ✔
 - Tests ✔
 - Attendance ✔
 - Grades ✔

### Teachers
 - add Tests ✘
 - Can Get their Classes ✘
 - Can set User Attendance To appsent (For day) ✘
 - Can set Grades ✘
---
## Prerequisites

Before you begin, ensure you have the following installed:

- **Node.js** (version 14 or higher) and **npm** (version 6 or higher) for the React frontend.
- **.NET SDK** (version 6.0 or higher) for the ASP.NET backend.
- **API** I am using [Neshan Api](https://platform.neshan.org/sdk/) for routing. 
---

## Step 1: Clone the Repository

Clone the project repository and navigate into the project directory:

```bash
git clone https://github.com/Darkmoon1995/SchoolSystem0.git
cd SchoolSystem0
```
---
### Backend Installation (ASP.NET Core)
 - Change to the backend directory:
 ```bash
cd SchoolSystem0.Server
```
 - Restore Dependencies:
Use the .NET CLI to restore the project dependencies:

```bash
dotnet restore
```
Start the ASP.NET server
```bash
dotnet run
```
---
### Frontend Installation (React)
- Navigate to the React Frontend Folder:
Change to the frontend directory:
```bash 
cd ../schoolsystem0.client
```

Install Dependencies
- Install the project dependencies using npm
```bash
npm install
```
Start the React development server:
```bash
npm start
```
---
### Neshan API
 Go to the neshan website and create ApiKey and sdk key
The ApiKey has to be added to SchoolSystem0.Server/appsettings.json
```bash 
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "ApplicationDbContextConnection": "Data Source=MyLocalDatabase.db"
  },
  "Jwt": {
    "Key": "DarkmoonSuperSecretKeyHere123456789",
    "Issuer": "https://localhost:7226",
    "Audience": "https://localhost:5173",
    "ExpiryDurationInMinutes": 60
  },
  "Neshan": {
    "ApiKey": "**THE API KEY**"
  },
  "SmtpSettings": {
    "Server": "szm",
    "Port": 587,
    "Username": "wertyuioIsntAvailable@hotmail.com",
    "Password": "ThisIs#123Password#123"
  }
}

```
Also You must add the web.api (SDKKEY) to the schoolsystem0.client/src/Pages/TaxiServices.jsx

```bash 
import React, { useEffect, useState } from 'react'
import { Card } from "@/components/ui/card"
import { Button } from "@/components/ui/button"
import { AlertCircle, MapPin } from "lucide-react"
import { Alert, AlertDescription, AlertTitle } from "@/components/ui/alert"

const API_URL = 'https://localhost:7287/api/Students/group-near-school'
const MAP_KEY = "Map Key" // Replace with your actual API key

```

