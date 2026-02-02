# TravelDesk Project - Step-by-Step Setup Guide for Visual Studio

## 📋 Prerequisites
- Visual Studio 2022 (Community/Professional/Enterprise)
- .NET 8.0 SDK installed
- Node.js 18+ and npm installed
- MySQL 8.0+ installed and running
- Git installed (optional)

---

## ✅ Step 1: Verify Prerequisites

### 1.1 Check .NET SDK
```bash
dotnet --version
```
Should show: `8.0.xxx`

### 1.2 Check Node.js
```bash
node --version
npm --version
```
Should show versions for both

### 1.3 Check MySQL
```bash
mysql --version
```
Should show: `mysql Ver 8.0.xxx`

**If any are missing, download and install from:**
- .NET SDK: https://dotnet.microsoft.com/download
- Node.js: https://nodejs.org/
- MySQL: https://dev.mysql.com/downloads/mysql/

---

## 🗂️ Step 2: Create Project Structure

### 2.1 Create Main Folder
```bash
# Create the main project folder
mkdir "c:\Users\YourUsername\Desktop\DotNet Project\TravelDesk"
cd "c:\Users\YourUsername\Desktop\DotNet Project\TravelDesk"
```

### 2.2 Create Subfolders
```bash
mkdir Backend
mkdir Frontend
mkdir Documentation
```

**Your folder structure should look like:**
```
TravelDesk/
├── Backend/
├── Frontend/
└── Documentation/
```

---

## 🎯 Step 3: Create .NET Core Backend in Visual Studio

### 3.1 Open Visual Studio 2022
1. Launch **Visual Studio 2022**
2. Click **"Create a new project"**

### 3.2 Create Web API Project
1. Search for **"ASP.NET Core Web API"**
2. Select it and click **"Next"**
3. Configure project:
   - **Project name:** TravelDeskAPI
   - **Location:** `c:\Users\YourUsername\Desktop\DotNet Project\TravelDesk\Backend`
   - **Solution name:** TravelDesk
   - Click **"Next"**

### 3.3 Additional Information
- **Framework:** .NET 8.0
- **Authentication type:** None
- **Configure for HTTPS:** ✓ (checked)
- **Enable OpenAPI support:** ✓ (checked - for Swagger)
- Click **"Create"**

### 3.4 Wait for Project Creation
Visual Studio will create the project and install dependencies.

---

## 📦 Step 4: Configure Backend Project

### 4.1 Open Package Manager Console
In Visual Studio:
- **Tools** → **NuGet Package Manager** → **Package Manager Console**

Or right-click on project → **Manage NuGet Packages**

### 4.2 Install Required NuGet Packages
Run these commands in Package Manager Console:

```powershell
# Entity Framework Core and MySQL
Install-Package Microsoft.EntityFrameworkCore -Version 8.0.0
Install-Package Microsoft.EntityFrameworkCore.Tools -Version 8.0.0
Install-Package Pomelo.EntityFrameworkCore.MySql -Version 8.0.0

# Authentication
Install-Package System.IdentityModel.Tokens.Jwt -Version 7.1.2
Install-Package Microsoft.AspNetCore.Authentication.JwtBearer -Version 8.0.0

# Password hashing
Install-Package BCrypt.Net-Next -Version 4.0.3

# Swagger (usually pre-installed)
Install-Package Swashbuckle.AspNetCore -Version 6.5.0
```

**Or use Manage NuGet Packages UI:**
1. Right-click project → **Manage NuGet Packages**
2. Search for each package and install

### 4.3 Verify Installation
Open **TravelDeskAPI.csproj** and verify all packages are listed:
```xml
<PackageReference Include="Microsoft.EntityFrameworkCore" Version="8.0.0" />
<PackageReference Include="Pomelo.EntityFrameworkCore.MySql" Version="8.0.0" />
<PackageReference Include="System.IdentityModel.Tokens.Jwt" Version="7.1.2" />
```

---

## 🗄️ Step 5: Set Up MySQL Database

### 5.1 Start MySQL Service
**Windows:**
```bash
net start MySQL80
```

Or use Services (services.msc) and start MySQL80

### 5.2 Create Database
Open Command Prompt/PowerShell:
```bash
mysql -u root -p
```

Enter your MySQL root password, then run:
```sql
CREATE DATABASE TravelDeskDB;
CREATE USER 'traveldesk_user'@'localhost' IDENTIFIED BY 'YourStrongPassword123!';
GRANT ALL PRIVILEGES ON TravelDeskDB.* TO 'traveldesk_user'@'localhost';
FLUSH PRIVILEGES;
EXIT;
```

### 5.3 Verify Database
```bash
mysql -u root -p
SHOW DATABASES;
# Should show: TravelDeskDB
EXIT;
```

---

## ⚙️ Step 6: Configure Backend Connection String

### 6.1 Open appsettings.json
In Visual Studio, open `appsettings.json` in the root of TravelDeskAPI project

### 6.2 Update Connection String
Replace the content with:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;Database=TravelDeskDB;User=root;Password=your_actual_password;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "Jwt": {
    "Key": "your-secret-key-here-must-be-at-least-32-characters-long-for-security",
    "Issuer": "TravelDeskAPI",
    "Audience": "TravelDeskApp",
    "ExpirationMinutes": 1440
  },
  "Email": {
    "SmtpServer": "smtp.gmail.com",
    "SmtpPort": 587,
    "FromAddress": "noreply@traveldesk.com",
    "Username": "your-email@gmail.com",
    "Password": "your-app-password"
  },
  "FileUpload": {
    "MaxFileSizeBytes": 10485760,
    "AllowedExtensions": ".pdf,.doc,.docx,.jpg,.jpeg,.png",
    "UploadPath": "wwwroot/uploads/documents"
  }
}
```

**Replace:**
- `your_actual_password` - your MySQL root password
- `your-secret-key-here...` - any 32+ character string
- `your-email@gmail.com` - your email for SMTP

### 6.3 Create uploads folder
In Solution Explorer:
1. Right-click **TravelDeskAPI** project
2. **Add** → **New Folder**
3. Name it: `wwwroot`
4. Inside wwwroot, create: `uploads` folder
5. Inside uploads, create: `documents` folder

---

## 🔧 Step 7: Create Backend Entities & Database Context

### 7.1 Create Models Folder
1. Right-click **TravelDeskAPI** project
2. **Add** → **New Folder** → Name: `Models`

### 7.2 Add Model Classes
Right-click **Models** folder → **Add** → **Class** and create:

**User.cs**
```csharp
namespace TravelDeskAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; } // Admin, Employee, Manager, HRTravelAdmin
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public ICollection<TravelRequest> CreatedRequests { get; set; }
        public ICollection<TravelRequest> ManagedRequests { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
```

**TravelRequest.cs**
```csharp
namespace TravelDeskAPI.Models
{
    public class TravelRequest
    {
        public int Id { get; set; }
        public string RequestNumber { get; set; } // TR-YYYY-####
        public int EmployeeId { get; set; }
        public int? ManagerId { get; set; }
        public string Destination { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Purpose { get; set; }
        public decimal? BudgetAmount { get; set; }
        public string Status { get; set; } = "Draft"; // Draft, Submitted, Approved, Rejected, InProgress, Completed, Cancelled
        public DateTime? SubmittedAt { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public int? ApprovedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public User Employee { get; set; }
        public User Manager { get; set; }
        public User ApprovedByUser { get; set; }
        public ICollection<Document> Documents { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
```

**Document.cs**
```csharp
namespace TravelDeskAPI.Models
{
    public class Document
    {
        public int Id { get; set; }
        public int TravelRequestId { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public int FileSize { get; set; }
        public string FilePath { get; set; }
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public TravelRequest TravelRequest { get; set; }
    }
}
```

**Comment.cs**
```csharp
namespace TravelDeskAPI.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int TravelRequestId { get; set; }
        public int UserId { get; set; }
        public string CommentText { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public TravelRequest TravelRequest { get; set; }
        public User User { get; set; }
    }
}
```

### 7.3 Create Data Folder & DbContext
1. Right-click **TravelDeskAPI** → **Add** → **New Folder** → `Data`
2. Right-click **Data** → **Add** → **Class** → `TravelDeskDbContext.cs`

```csharp
using Microsoft.EntityFrameworkCore;
using TravelDeskAPI.Models;

namespace TravelDeskAPI.Data
{
    public class TravelDeskDbContext : DbContext
    {
        public TravelDeskDbContext(DbContextOptions<TravelDeskDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<TravelRequest> TravelRequests { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships
            modelBuilder.Entity<TravelRequest>()
                .HasOne(tr => tr.Employee)
                .WithMany(u => u.CreatedRequests)
                .HasForeignKey(tr => tr.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TravelRequest>()
                .HasOne(tr => tr.Manager)
                .WithMany(u => u.ManagedRequests)
                .HasForeignKey(tr => tr.ManagerId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Document>()
                .HasOne(d => d.TravelRequest)
                .WithMany(tr => tr.Documents)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.TravelRequest)
                .WithMany(tr => tr.Comments)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed default users
            modelBuilder.Entity<User>().HasData(
                new User 
                { 
                    Id = 1, 
                    FirstName = "Admin", 
                    LastName = "User", 
                    Email = "admin@traveldesk.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                    Role = "Admin",
                    IsActive = true
                },
                new User 
                { 
                    Id = 2, 
                    FirstName = "Employee", 
                    LastName = "User", 
                    Email = "emp@traveldesk.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Emp@123"),
                    Role = "Employee",
                    IsActive = true
                }
            );
        }
    }
}
```

---

## 🔑 Step 8: Configure Program.cs

### 8.1 Update Program.cs
Replace the entire content of `Program.cs`:

```csharp
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TravelDeskAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddDbContext<TravelDeskDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

// JWT Configuration
var jwtKey = builder.Configuration["Jwt:Key"];
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

// Apply migrations and seed data
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TravelDeskDbContext>();
    db.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
```

---

## 🚀 Step 9: Test Backend Project

### 9.1 Build Project
In Visual Studio:
- **Build** → **Build Solution** (Ctrl + Shift + B)

Wait for build to complete (should show "Build succeeded")

### 9.2 Create Migration
In **Package Manager Console**, run:
```powershell
Add-Migration InitialCreate
```

Then:
```powershell
Update-Database
```

### 9.3 Run Project
- Press **F5** or **Debug** → **Start Debugging**
- Wait for browser to open with Swagger UI
- You should see: `http://localhost:5000/swagger`

### 9.4 Verify Backend is Running
- Swagger page should load
- Navigate to http://localhost:5000/api/users (should return empty array)

---

## 🅰️ Step 10: Create Angular Frontend

### 10.1 Open Terminal/PowerShell
```bash
cd c:\Users\YourUsername\Desktop\DotNet Project\TravelDesk\Frontend
```

### 10.2 Create Angular Project
```bash
ng new traveldesk-app --routing --style=css --skip-git
```

When prompted:
- **Do you want to enable Server-Side Rendering (SSR)?** → **No**
- **Which AI tools do you want to configure?** → **None**

Wait for npm packages to install (5-10 minutes)

### 10.3 Verify Angular Installation
```bash
cd traveldesk-app
ng version
```

Should show Angular CLI version and Angular version 18+

---

## 📁 Step 11: Organize Angular Project Structure

### 11.1 Create Folders
In `src/app/` directory, create:
```
src/app/
├── pages/           (Page components)
├── components/      (Reusable components)
├── services/        (API services)
├── guards/          (Route guards)
├── models/          (TypeScript interfaces)
└── styles/          (CSS files)
```

Use VS Code or File Explorer to create these folders.

### 11.2 Update app.config.ts
Create a new file `src/app.config.ts`:
```typescript
import { ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { routes } from './app.routes';

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideHttpClient()
  ]
};
```

---

## 🔌 Step 12: Create Core Services

### 12.1 Create AuthService
File: `src/app/services/auth.service.ts`

```typescript
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'http://localhost:5000/api/auth';
  private tokenKey = 'traveldesk_token';
  private userSubject = new BehaviorSubject<any>(null);

  constructor(private http: HttpClient) {
    this.loadUser();
  }

  login(email: string, password: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/login`, { email, password });
  }

  register(userData: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/register`, userData);
  }

  logout(): void {
    localStorage.removeItem(this.tokenKey);
    this.userSubject.next(null);
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  setToken(token: string): void {
    localStorage.setItem(this.tokenKey, token);
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }

  private loadUser(): void {
    const token = this.getToken();
    if (token) {
      // Decode token to get user info
      try {
        const payload = JSON.parse(atob(token.split('.')[1]));
        this.userSubject.next(payload);
      } catch {
        this.logout();
      }
    }
  }
}
```

### 12.2 Create ApiService
File: `src/app/services/api.service.ts`

```typescript
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private apiUrl = 'http://localhost:5000/api';

  constructor(private http: HttpClient, private authService: AuthService) {}

  private getHeaders(): HttpHeaders {
    const token = this.authService.getToken();
    return new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    });
  }

  get<T>(endpoint: string): Observable<T> {
    return this.http.get<T>(`${this.apiUrl}/${endpoint}`, {
      headers: this.getHeaders()
    });
  }

  post<T>(endpoint: string, data: any): Observable<T> {
    return this.http.post<T>(`${this.apiUrl}/${endpoint}`, data, {
      headers: this.getHeaders()
    });
  }

  put<T>(endpoint: string, data: any): Observable<T> {
    return this.http.put<T>(`${this.apiUrl}/${endpoint}`, data, {
      headers: this.getHeaders()
    });
  }

  delete<T>(endpoint: string): Observable<T> {
    return this.http.delete<T>(`${this.apiUrl}/${endpoint}`, {
      headers: this.getHeaders()
    });
  }

  patch<T>(endpoint: string, data: any): Observable<T> {
    return this.http.patch<T>(`${this.apiUrl}/${endpoint}`, data, {
      headers: this.getHeaders()
    });
  }
}
```

---

## 🔒 Step 13: Create Auth Guard

### 13.1 Create AuthGuard
File: `src/app/guards/auth.guard.ts`

```typescript
import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) {}

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): boolean {
    if (this.authService.isLoggedIn()) {
      return true;
    }

    this.router.navigate(['/login']);
    return false;
  }
}
```

---

## 🎨 Step 14: Create Basic Pages

### 14.1 Create Login Component
File: `src/app/pages/login/login.component.ts`

```typescript
import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="login-container">
      <h2>TravelDesk Login</h2>
      <form (ngSubmit)="login()">
        <input [(ngModel)]="email" name="email" type="email" placeholder="Email" required />
        <input [(ngModel)]="password" name="password" type="password" placeholder="Password" required />
        <button type="submit">Login</button>
        <p *ngIf="error" class="error">{{ error }}</p>
      </form>
    </div>
  `,
  styles: [`
    .login-container {
      max-width: 400px;
      margin: 100px auto;
      padding: 20px;
      border: 1px solid #ddd;
      border-radius: 8px;
    }
    input {
      width: 100%;
      padding: 10px;
      margin: 10px 0;
      border: 1px solid #ddd;
      border-radius: 4px;
    }
    button {
      width: 100%;
      padding: 10px;
      background-color: #007bff;
      color: white;
      border: none;
      border-radius: 4px;
      cursor: pointer;
    }
    button:hover {
      background-color: #0056b3;
    }
    .error {
      color: red;
      margin-top: 10px;
    }
  `]
})
export class LoginComponent {
  email = '';
  password = '';
  error = '';

  constructor(private authService: AuthService, private router: Router) {}

  login(): void {
    this.authService.login(this.email, this.password).subscribe(
      (response: any) => {
        this.authService.setToken(response.data.token);
        this.router.navigate(['/dashboard']);
      },
      (error) => {
        this.error = error.error.error || 'Login failed';
      }
    );
  }
}
```

### 14.2 Create Dashboard Component
File: `src/app/pages/dashboard/dashboard.component.ts`

```typescript
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApiService } from '../../services/api.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="dashboard-container">
      <h2>Dashboard</h2>
      <p>Welcome to TravelDesk!</p>
      <div class="stats">
        <div class="stat-card">
          <h3>Total Requests</h3>
          <p>{{ totalRequests }}</p>
        </div>
        <div class="stat-card">
          <h3>Pending</h3>
          <p>{{ pendingRequests }}</p>
        </div>
        <div class="stat-card">
          <h3>Approved</h3>
          <p>{{ approvedRequests }}</p>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .dashboard-container {
      padding: 20px;
    }
    .stats {
      display: grid;
      grid-template-columns: repeat(3, 1fr);
      gap: 20px;
      margin-top: 20px;
    }
    .stat-card {
      padding: 20px;
      border: 1px solid #ddd;
      border-radius: 8px;
      text-align: center;
    }
  `]
})
export class DashboardComponent implements OnInit {
  totalRequests = 0;
  pendingRequests = 0;
  approvedRequests = 0;

  constructor(private apiService: ApiService) {}

  ngOnInit(): void {
    this.loadDashboardData();
  }

  loadDashboardData(): void {
    this.apiService.get('travelrequests').subscribe((response: any) => {
      this.totalRequests = response.data?.length || 0;
      this.pendingRequests = response.data?.filter((r: any) => r.status === 'Submitted').length || 0;
      this.approvedRequests = response.data?.filter((r: any) => r.status === 'Approved').length || 0;
    });
  }
}
```

---

## 🛣️ Step 15: Configure Routing

### 15.1 Create Routes
File: `src/app/app.routes.ts`

```typescript
import { Routes } from '@angular/router';
import { LoginComponent } from './pages/login/login.component';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { AuthGuard } from './guards/auth.guard';

export const routes: Routes = [
  { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'dashboard', component: DashboardComponent, canActivate: [AuthGuard] },
  { path: '**', redirectTo: 'dashboard' }
];
```

---

## 🧪 Step 16: Run Angular Application

### 16.1 Start Angular Dev Server
In the `Frontend/traveldesk-app` directory:

```bash
ng serve
# or
npm start
```

### 16.2 Access Application
Open browser and go to: `http://localhost:4200`

**Default Login Credentials:**
- Email: `admin@traveldesk.com`
- Password: `Admin@123`

---

## ✅ Step 17: Verify Complete Setup

### Checklist:
- [ ] MySQL database running
- [ ] Backend API running on http://localhost:5000
- [ ] Backend Swagger accessible at http://localhost:5000/swagger
- [ ] Angular app running on http://localhost:4200
- [ ] Can login with admin credentials
- [ ] Dashboard loads successfully
- [ ] No CORS errors in browser console

---

## 🐛 Troubleshooting

### Backend Won't Start
```bash
cd Backend/TravelDeskAPI
dotnet clean
dotnet restore
dotnet run
```

### Database Connection Error
- Verify MySQL is running: `net start MySQL80`
- Check connection string in appsettings.json
- Verify database exists: `mysql -u root -p` then `SHOW DATABASES;`

### Angular Port Already in Use
```bash
ng serve --port 4300
```

### CORS Errors
- Backend Program.cs should have CORS enabled (already configured)
- Make sure frontend URL matches allowed origins

### Can't Login
- Verify users are seeded in database
- Check API is returning token in response

---

## 📝 Next Steps After Setup

1. **Create remaining pages** (Admin Dashboard, Create Request, Request Details)
2. **Implement CRUD operations** for travel requests
3. **Add document upload** functionality
4. **Add comments system**
5. **Implement role-based UI** (show/hide based on user role)
6. **Add form validation**
7. **Implement error handling**
8. **Add loading states and spinners**
9. **Responsive design** with Bootstrap/Material
10. **Testing and debugging**

---

**Version:** 1.0  
**Date:** February 2, 2026  
**Status:** Complete Setup Guide

