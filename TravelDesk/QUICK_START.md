# Quick Start Guide

## 🚀 Get Started in 5 Minutes

This guide helps you get TravelDesk running locally for development.

---

## Prerequisites

- ✅ .NET 6.0 SDK ([Download](https://dotnet.microsoft.com/en-us/download/dotnet/6.0))
- ✅ Node.js 14+ ([Download](https://nodejs.org/))
- ✅ SQL Server 2019+ (Local or remote)
- ✅ A code editor (VS Code, Visual Studio, or similar)

Verify installation:
```bash
dotnet --version
node --version
npm --version
```

---

## Step 1: Backend Setup (2-3 minutes)

### 1.1 Navigate to Backend
```bash
cd "DotNet Project\TravelDesk\Backend\TravelDeskAPI"
```

### 1.2 Update Database Connection
Edit `appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=TravelDeskDB;Trusted_Connection=true;TrustServerCertificate=true;"
}
```

Replace `YOUR_SERVER` with:
- `(localdb)\mssqlLocalDB` for local development
- `.` for default SQL Server instance
- Your server name for remote database

### 1.3 Restore & Run
```bash
dotnet restore
dotnet run
```

**Expected Output:**
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:5001
      Now listening on: http://localhost:5000
```

✅ API is running! Visit `http://localhost:5000/swagger` to see API docs.

---

## Step 2: Frontend Setup (2-3 minutes)

### 2.1 Open New Terminal (keep backend running)
```bash
cd "DotNet Project\TravelDesk\Frontend\traveldesk-app"
```

### 2.2 Install Dependencies
```bash
npm install
```

This installs React, routing, Axios, and other dependencies (~2-3 min).

### 2.3 Start Development Server
```bash
npm start
```

**Expected Output:**
- Compiled successfully message
- Browser opens automatically at `http://localhost:3000`
- You see the TravelDesk login page

✅ Frontend is running!

---

## Step 3: Test the Application (1 minute)

### 3.1 Login with Admin Account
- Email: `admin@traveldesk.com`
- Password: `Admin@123`

### 3.2 Navigate Admin Dashboard
You should see:
- "Total Users" card showing user count
- User grid table with columns: First Name, Last Name, Employee ID, Department, Role, Manager
- "+ Add User" button
- Pagination controls

### 3.3 Test User Management
1. Click "+ Add User"
2. Fill form with sample data
3. Click "Create User"
4. See new user in grid

✅ Everything is working!

---

## Common Issues & Solutions

### Issue: "Database connection failed"
**Solution:**
```bash
# Test connection string
sqlcmd -S your_server -Q "SELECT @@VERSION"

# Update appsettings.json with correct server name
```

### Issue: "Port 5000 already in use"
**Solution:**
```bash
# Change port in appsettings.json or kill process
# Windows:
netstat -ano | findstr :5000
taskkill /PID <PID> /F
```

### Issue: "npm ERR!"
**Solution:**
```bash
# Clear npm cache and reinstall
npm cache clean --force
rm -r node_modules package-lock.json
npm install
```

### Issue: "Can't connect to API from frontend"
**Ensure:**
1. Backend is running at http://localhost:5000
2. CORS is enabled (already configured)
3. Check browser console for errors (F12)

---

## What's Running

### Backend (http://localhost:5000)
- RESTful API with JWT authentication
- Swagger UI at `/swagger`
- Database: TravelDeskDB

### Frontend (http://localhost:3000)
- React single-page application
- Connects to backend API
- Hot reload on file changes

### Database
- TravelDeskDB on your SQL Server
- Auto-created tables on first run
- Admin user seeded automatically

---

## Next Steps

### 1. Explore the API
Visit `http://localhost:5000/swagger` and try endpoints:
- GET `/users/grid` - See all users
- POST `/auth/login` - Test login

### 2. Review Code Structure
Key files to understand:
- Backend: `TravelDeskAPI/Controllers/UsersController.cs`
- Frontend: `traveldesk-app/src/pages/AdminDashboardPage.js`

### 3. Read Full Documentation
- **README.md** - Architecture & features
- **API_DOCUMENTATION.md** - All endpoints
- **SETUP.md** - Detailed setup guide

### 4. Try Different Roles
Create and test users with different roles:
- Employee - Create travel requests
- Manager - Approve/disapprove requests
- HR Travel Admin - Book and complete requests

---

## Development Workflow

### Making Changes

**Backend:**
```bash
# Make changes to .cs files
# Auto-recompiles on save (watch mode available)
dotnet watch run
```

**Frontend:**
```bash
# Make changes to .js or .css files
# Auto-reloads in browser (hot reload enabled)
# Just save and see changes instantly
```

### Adding New Feature Example

1. Create API endpoint in Backend Controller
2. Test with Swagger at http://localhost:5000/swagger
3. Add service method in Frontend `src/services/api.js`
4. Use in React component
5. Test in browser

---

## Database Management

### View Database (SQL Server Management Studio)
```
Server: (your server name)
Database: TravelDeskDB
Connection: Windows Auth (or SQL Auth)
```

### Reset Database
```bash
# Option 1: Delete database and let app recreate
# Option 2: Run migrations
dotnet ef database drop --force
dotnet ef database update
```

### Seed Additional Data
Edit `TravelDeskAPI/Data/DbInitializer.cs` and add more seed data.

---

## Useful Commands

### Backend Commands
```bash
# Run in development mode with watch
dotnet watch run

# Build only
dotnet build

# Run tests (add xUnit later)
dotnet test

# View migrations
dotnet ef migrations list
```

### Frontend Commands
```bash
# Start development server
npm start

# Build for production
npm run build

# Run tests
npm test

# Eject configuration (one-way, be careful!)
npm eject
```

---

## Environment Setup Checklist

- ✅ .NET 6.0 SDK installed
- ✅ Node.js 14+ installed
- ✅ SQL Server running
- ✅ Connection string configured in appsettings.json
- ✅ Backend running on http://localhost:5000
- ✅ Frontend running on http://localhost:3000
- ✅ Database created automatically
- ✅ Admin user seeded (admin@traveldesk.com)
- ✅ Can login successfully

---

## Getting Help

### Troubleshooting Resources
1. Check browser console: F12 → Console tab
2. Check backend logs in terminal
3. See error details in Network tab (F12 → Network)
4. Review SETUP.md troubleshooting section

### Common Debugging

**Frontend not connecting to API:**
- Check backend is running: `http://localhost:5000/swagger`
- Check network requests in browser (F12 → Network)
- Look for CORS errors

**Database not accessible:**
- Verify connection string in appsettings.json
- Ensure SQL Server is running
- Check Windows firewall

**Port conflicts:**
- Change port in appsettings.json
- Or kill existing process on that port

---

## Project Structure at a Glance

```
TravelDesk/
├── Backend/TravelDeskAPI/        ← .NET API
│   ├── Controllers/              ← Endpoints
│   ├── Models/                   ← Database entities
│   └── Data/                     ← Database context
│
├── Frontend/traveldesk-app/      ← React app
│   └── src/
│       ├── pages/                ← Full page components
│       ├── components/           ← Reusable components
│       └── services/             ← API integration
│
└── Documentation/                ← All guides (this folder)
```

---

## Keeping Things Running

### Restart Without Full Setup
**Terminal 1 (Backend):**
```bash
cd Backend/TravelDeskAPI
dotnet run
```

**Terminal 2 (Frontend):**
```bash
cd Frontend/traveldesk-app
npm start
```

Both will auto-reload on file changes!

---

## Next Time You Open Project

1. Open two terminals in project root
2. Terminal 1: `cd Backend/TravelDeskAPI && dotnet run`
3. Terminal 2: `cd Frontend/traveldesk-app && npm start`
4. Frontend opens automatically at http://localhost:3000
5. Login with admin account
6. Start developing!

---

## Tips & Tricks

- **Keyboard Shortcut (Frontend)**: Press `r` to restart server
- **API Testing**: Use Swagger at http://localhost:5000/swagger
- **Database Debugging**: Use SQL Server Management Studio
- **Network Issues**: Check CORS policy in `Program.cs`
- **Performance**: Check browser DevTools Lighthouse

---

**Version**: 1.0.0  
**Last Updated**: January 30, 2026

Happy coding! 🎉
