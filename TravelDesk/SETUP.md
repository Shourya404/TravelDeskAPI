# Setup Guide

## Prerequisites
- .NET 6.0 SDK or later
- Node.js 14+ and npm
- SQL Server 2019 or later
- Visual Studio 2022 or VS Code

## Backend Setup

### Step 1: Install .NET Prerequisites
```bash
# Verify .NET installation
dotnet --version
```

### Step 2: Navigate to Backend Directory
```bash
cd "DotNet Project\TravelDesk\Backend\TravelDeskAPI"
```

### Step 3: Restore NuGet Packages
```bash
dotnet restore
```

### Step 4: Configure Database Connection
Edit `appsettings.json` and update the connection string:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=TravelDeskDB;Trusted_Connection=true;TrustServerCertificate=true;"
}
```

### Step 5: Update JWT Secret
Edit `appsettings.json` and update JWT key:
```json
"Jwt": {
  "Key": "your-very-secure-secret-key-minimum-32-characters-long"
}
```

### Step 6: Create Database and Apply Migrations
```bash
# Create database and apply migrations automatically (configured in Program.cs)
dotnet run
```

The API will start at `https://localhost:5001` or `http://localhost:5000`

## Frontend Setup

### Step 1: Install Node.js
Verify Node.js installation:
```bash
node --version
npm --version
```

### Step 2: Navigate to Frontend Directory
```bash
cd "DotNet Project\TravelDesk\Frontend\traveldesk-app"
```

### Step 3: Install Dependencies
```bash
npm install
```

### Step 4: Update API URL (if needed)
Edit `src/services/api.js`:
```javascript
const API_BASE_URL = 'http://localhost:5000/api';
```

### Step 5: Start Development Server
```bash
npm start
```

The app will automatically open at `http://localhost:3000`

## Verification

### Backend Verification
1. Navigate to `http://localhost:5000/swagger/index.html` to see API documentation
2. You should see all the API endpoints

### Frontend Verification
1. The React app should open automatically
2. You should see the TravelDesk login page

## Troubleshooting

### Database Connection Issues
- Verify SQL Server is running
- Check connection string in `appsettings.json`
- Ensure user has appropriate permissions

### Port Already in Use
- Change port in `appsettings.json` for backend
- Or kill the process using the port

### NPM Install Issues
- Delete `node_modules` folder and `package-lock.json`
- Run `npm install` again
- Clear npm cache: `npm cache clean --force`

### CORS Issues
- Verify CORS is configured in `Program.cs`
- Check frontend URL matches CORS policy

## Next Steps

1. Log in with admin credentials:
   - Email: `admin@traveldesk.com`
   - Password: `Admin@123`

2. Create additional users through Admin panel

3. Test employee workflow by creating travel requests

4. Test manager approval workflow

5. Test HR Travel Admin booking workflow

---

For detailed API documentation, visit `http://localhost:5000/swagger`
