# TravelDesk - MySQL Setup Guide

**Database**: MySQL 8.0+  
**Provider**: Pomelo.EntityFrameworkCore.MySql  
**Last Updated**: January 30, 2026

---

## 📋 Prerequisites

### 1. Install MySQL Server
Download and install MySQL Server 8.0 or later:
- **Windows**: [MySQL Installer for Windows](https://dev.mysql.com/downloads/installer/)
- **Mac**: `brew install mysql`
- **Linux**: `sudo apt-get install mysql-server`

### 2. Verify MySQL Installation
```bash
mysql --version
```

Expected output: `mysql Ver 8.0.xx`

### 3. Start MySQL Service

**Windows:**
```bash
# Start MySQL Service
net start MySQL80

# Or use Services app (services.msc)
```

**Mac:**
```bash
brew services start mysql
```

**Linux:**
```bash
sudo systemctl start mysql
sudo systemctl enable mysql
```

---

## 🔧 Step-by-Step Setup

### Step 1: Configure MySQL Database

#### 1.1 Login to MySQL
```bash
mysql -u root -p
```
Enter your MySQL root password when prompted.

#### 1.2 Create Database and User
```sql
-- Create database
CREATE DATABASE TravelDeskDB;

-- Create dedicated user (optional but recommended)
CREATE USER 'traveldesk_user'@'localhost' IDENTIFIED BY 'YourStrongPassword123!';

-- Grant all privileges on TravelDeskDB
GRANT ALL PRIVILEGES ON TravelDeskDB.* TO 'traveldesk_user'@'localhost';

-- Apply changes
FLUSH PRIVILEGES;

-- Verify database was created
SHOW DATABASES;

-- Exit MySQL
EXIT;
```

### Step 2: Update Connection String

Edit `Backend/TravelDeskAPI/appsettings.json`:

#### Option A: Using root user (Development only)
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;Database=TravelDeskDB;User=root;Password=YOUR_ROOT_PASSWORD;"
  }
}
```

#### Option B: Using dedicated user (Recommended)
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;Database=TravelDeskDB;User=traveldesk_user;Password=YourStrongPassword123!;"
  }
}
```

**Important**: Replace the password with your actual MySQL password!

### Step 3: Install .NET Packages

Navigate to backend directory:
```bash
cd "c:\Users\shourya.saxena\Desktop\DotNet Project\TravelDesk\Backend\TravelDeskAPI"
```

Restore packages (includes MySQL provider):
```bash
dotnet restore
```

This installs:
- ✅ Pomelo.EntityFrameworkCore.MySql (MySQL provider)
- ✅ Entity Framework Core
- ✅ BCrypt.Net-Next (password hashing)
- ✅ JWT authentication packages

### Step 4: Create Database Tables

The application will automatically create tables on first run, but you can also use migrations:

#### Option A: Automatic (Recommended for beginners)
```bash
dotnet run
```
The application will create all tables automatically.

#### Option B: Using EF Core Migrations
```bash
# Create initial migration
dotnet ef migrations add InitialCreate

# Apply migration to database
dotnet ef database update
```

### Step 5: Run the Backend

```bash
dotnet run
```

**Expected Output:**
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:5001
      Now listening on: http://localhost:5000
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (50ms) [Parameters=[], CommandType='Text']
      CREATE TABLE `Users` ...
```

✅ Backend is running with MySQL!

Visit: http://localhost:5000/swagger

---

## 🚀 Complete Project Setup

### Terminal 1: Backend
```bash
cd "c:\Users\shourya.saxena\Desktop\DotNet Project\TravelDesk\Backend\TravelDeskAPI"

# First time only - restore packages
dotnet restore

# Run the application
dotnet run
```

### Terminal 2: Frontend (Open new terminal)
```bash
cd "c:\Users\shourya.saxena\Desktop\DotNet Project\TravelDesk\Frontend\traveldesk-app"

# First time only - install dependencies
npm install

# Run the application
npm start
```

**Access Points:**
- 🌐 Frontend: http://localhost:3000
- 🔌 Backend API: http://localhost:5000
- 📚 API Docs: http://localhost:5000/swagger

### Default Login Credentials
- Email: `admin@traveldesk.com`
- Password: `Admin@123`

---

## 🔍 Verify MySQL Connection

### Method 1: Check Application Logs
When you run `dotnet run`, look for:
```
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand ...
```

### Method 2: Check Database Directly
```bash
mysql -u root -p

# In MySQL prompt:
USE TravelDeskDB;
SHOW TABLES;
```

You should see:
```
+------------------------+
| Tables_in_TravelDeskDB |
+------------------------+
| Users                  |
| TravelRequests         |
| Documents              |
| Comments               |
+------------------------+
```

### Method 3: Check Admin User
```sql
USE TravelDeskDB;
SELECT * FROM Users;
```

You should see the seeded admin user.

---

## 🛠️ Troubleshooting

### Issue 1: "Unable to connect to MySQL server"

**Solution:**
```bash
# Check if MySQL is running
# Windows:
sc query MySQL80

# Mac/Linux:
sudo systemctl status mysql

# Start MySQL if not running
# Windows:
net start MySQL80

# Mac:
brew services start mysql

# Linux:
sudo systemctl start mysql
```

### Issue 2: "Access denied for user"

**Solution:**
1. Verify your password is correct in `appsettings.json`
2. Reset MySQL root password if needed:
```bash
# Windows (as Administrator):
net stop MySQL80
mysqld --skip-grant-tables

# In another terminal:
mysql -u root
ALTER USER 'root'@'localhost' IDENTIFIED BY 'NewPassword123!';
FLUSH PRIVILEGES;
EXIT;

# Restart MySQL normally
net start MySQL80
```

### Issue 3: "Database 'TravelDeskDB' doesn't exist"

**Solution:**
```bash
mysql -u root -p
CREATE DATABASE TravelDeskDB;
EXIT;

# Then run the app again
dotnet run
```

### Issue 4: Connection string format error

**Correct format:**
```json
"DefaultConnection": "Server=localhost;Port=3306;Database=TravelDeskDB;User=root;Password=your_password;"
```

**Common mistakes:**
- ❌ Missing semicolon at the end
- ❌ Wrong port (default is 3306)
- ❌ Spaces in connection string values
- ❌ Missing User or Password

### Issue 5: "Package Pomelo.EntityFrameworkCore.MySql not found"

**Solution:**
```bash
# Clear NuGet cache
dotnet nuget locals all --clear

# Restore packages
dotnet restore

# If still failing, manually add package
dotnet add package Pomelo.EntityFrameworkCore.MySql --version 6.0.2
```

---

## 📊 MySQL Connection String Options

### Basic Connection (Development)
```json
"Server=localhost;Port=3306;Database=TravelDeskDB;User=root;Password=yourpassword;"
```

### With Charset and SSL
```json
"Server=localhost;Port=3306;Database=TravelDeskDB;User=root;Password=yourpassword;CharSet=utf8mb4;SslMode=Required;"
```

### Remote MySQL Server
```json
"Server=192.168.1.100;Port=3306;Database=TravelDeskDB;User=admin;Password=yourpassword;"
```

### Connection Pooling (Production)
```json
"Server=localhost;Port=3306;Database=TravelDeskDB;User=root;Password=yourpassword;MinimumPoolSize=5;MaximumPoolSize=100;"
```

---

## 🔐 Security Best Practices

### 1. Never Commit Passwords
Add to `.gitignore`:
```
appsettings.json
appsettings.Development.json
appsettings.Production.json
```

### 2. Use Environment Variables (Production)
Update `Program.cs` to read from environment:
```csharp
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? Environment.GetEnvironmentVariable("MYSQL_CONNECTION_STRING");
```

Set environment variable:
```bash
# Windows (PowerShell)
$env:MYSQL_CONNECTION_STRING="Server=localhost;Port=3306;Database=TravelDeskDB;User=root;Password=yourpassword;"

# Linux/Mac
export MYSQL_CONNECTION_STRING="Server=localhost;Port=3306;Database=TravelDeskDB;User=root;Password=yourpassword;"
```

### 3. Create Dedicated Database User
```sql
CREATE USER 'traveldesk_app'@'localhost' IDENTIFIED BY 'StrongP@ssw0rd!';
GRANT SELECT, INSERT, UPDATE, DELETE ON TravelDeskDB.* TO 'traveldesk_app'@'localhost';
FLUSH PRIVILEGES;
```

---

## 📦 Managing Migrations

### Create a New Migration
```bash
dotnet ef migrations add MigrationName
```

### Apply Migrations
```bash
dotnet ef database update
```

### Rollback Migration
```bash
# Rollback to specific migration
dotnet ef database update PreviousMigrationName

# Rollback all migrations
dotnet ef database update 0
```

### Remove Last Migration (if not applied)
```bash
dotnet ef migrations remove
```

### View Migration History
```bash
dotnet ef migrations list
```

---

## 🧪 Testing the Setup

### Test 1: Check API Health
```bash
# In browser or using curl
curl http://localhost:5000/swagger
```

### Test 2: Test Login Endpoint
```bash
curl -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -d "{\"email\":\"admin@traveldesk.com\",\"password\":\"Admin@123\"}"
```

### Test 3: Verify Database Tables
```sql
USE TravelDeskDB;

-- Show all tables
SHOW TABLES;

-- Check Users table structure
DESCRIBE Users;

-- Count records
SELECT COUNT(*) FROM Users;

-- View admin user
SELECT Id, FirstName, LastName, Email, Role FROM Users;
```

---

## 💡 Useful MySQL Commands

### Database Operations
```sql
-- List all databases
SHOW DATABASES;

-- Select database
USE TravelDeskDB;

-- List all tables
SHOW TABLES;

-- Show table structure
DESCRIBE Users;

-- Show table creation script
SHOW CREATE TABLE Users;
```

### Data Operations
```sql
-- View all users
SELECT * FROM Users;

-- View travel requests
SELECT * FROM TravelRequests;

-- View with joins
SELECT 
    tr.Id, 
    tr.RequestNumber, 
    u.FirstName, 
    u.LastName, 
    tr.Status 
FROM TravelRequests tr
JOIN Users u ON tr.EmployeeId = u.Id;
```

### Backup and Restore
```bash
# Backup database
mysqldump -u root -p TravelDeskDB > backup.sql

# Restore database
mysql -u root -p TravelDeskDB < backup.sql
```

---

## 🚀 Quick Start Checklist

- [ ] MySQL Server installed and running
- [ ] Database `TravelDeskDB` created
- [ ] Connection string updated in `appsettings.json`
- [ ] Packages restored (`dotnet restore`)
- [ ] Backend running (`dotnet run`) on port 5000
- [ ] Frontend installed (`npm install`)
- [ ] Frontend running (`npm start`) on port 3000
- [ ] Can access Swagger UI (http://localhost:5000/swagger)
- [ ] Can access Login page (http://localhost:3000)
- [ ] Can login with admin credentials
- [ ] Database tables created automatically
- [ ] Admin user seeded in database

---

## 📚 Additional Resources

- [MySQL Documentation](https://dev.mysql.com/doc/)
- [Pomelo EF Core Provider](https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [MySQL Workbench](https://www.mysql.com/products/workbench/) - GUI tool

---

## 🎯 Next Steps

1. ✅ Complete setup following this guide
2. ✅ Verify all tables are created
3. ✅ Login with admin credentials
4. ✅ Create test users
5. ✅ Test travel request workflow
6. ✅ Review API documentation
7. ✅ Start development!

---

**Version**: 1.0.0 - MySQL Edition  
**Last Updated**: January 30, 2026

For detailed API documentation, see [API_DOCUMENTATION.md](API_DOCUMENTATION.md)  
For general setup, see [SETUP.md](SETUP.md)
