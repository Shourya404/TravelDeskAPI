# TravelDesk Project Generation Prompt

## ⚠️ CRITICAL: WORKFLOW DIAGRAMS REQUIRED

**DO NOT PROCEED** until you have:
1. ✅ Project name from user
2. ✅ Workflow Diagram Picture 1 (Travel Request workflow)
3. ✅ Workflow Diagram Picture 2 (User Role workflow)
4. ✅ User answers to ALL clarifying questions

**If any are missing: ASK FOR THEM FIRST**

---

## 🎯 Project Overview
Build a complete Travel Request Management System with Angular frontend and **.NET Core 10.0** Web API backend using MySQL database.

---

## 📋 Technology Stack

### Backend
- **.NET Core 10.0** Web API ⭐ LATEST
- **Entity Framework Core 9.0** with MySQL (Pomelo provider)
- **MySQL 8.0+** Database
- **JWT Authentication** with BCrypt password hashing
- **Swagger/OpenAPI** documentation

### Frontend
- **Angular 18+** (latest stable)
- **TypeScript**
- **RxJS** for reactive programming
- **Angular Material** or Bootstrap for UI
- **HttpClientModule** for API communication
- **Angular Forms** (Reactive Forms)
- **Angular Router** for navigation

### Development
- Node.js 18+
- npm or yarn
- Visual Studio Code

---

## 🏗️ Architecture

**PROJECT_NAME:** `[USER_PROVIDED_PROJECT_NAME]`

```
[PROJECT_NAME]/
├── Backend/
│   └── [PROJECT_NAME]API/          (.NET Core 10.0 Web API)
│       ├── Controllers/
│       ├── Models/
│       ├── Services/
│       ├── Data/
│       ├── DTOs/
│       ├── wwwroot/uploads/documents/
│       ├── appsettings.json
│       └── Program.cs
│
├── Frontend/
│   └── [project-name]-app/          (Angular 18+ SPA)
│       ├── src/app/
│       ├── src/styles/
│       ├── src/assets/
│       └── angular.json
│
└── Documentation/
```

---

## � WORKFLOW DIAGRAMS (MUST HAVE)

### Workflow Diagram 1: Travel Request Workflow
**Image provided by user showing:**
- All status states and transitions
- Who can perform each action
- Conditions for moving between states
- Approval/rejection flows

**[DIAGRAM 1 IMAGE - USER PROVIDED]**

### Workflow Diagram 2: User Role Workflow
**Image provided by user showing:**
- All user roles
- Permissions for each role
- Actions each role can perform
- Data access restrictions

**[DIAGRAM 2 IMAGE - USER PROVIDED]**

---

## ✅ PRE-GENERATION CHECKLIST

**BEFORE generating anything, confirm you have:**

- [ ] Project name from user
- [ ] Workflow Diagram 1 image
- [ ] Workflow Diagram 2 image
- [ ] Database schema clarifications
- [ ] Workflow questions answered
- [ ] Feature requirements confirmed
- [ ] Document handling confirmed
- [ ] API design preferences confirmed
- [ ] Security requirements confirmed
- [ ] Angular preferences confirmed

**If ANY are missing: ASK USER FOR THEM FIRST**

---

## �🗄️ Database Schema

### Users Table
```sql
CREATE TABLE Users (
  Id INT PRIMARY KEY AUTO_INCREMENT,
  FirstName VARCHAR(100) NOT NULL,
  LastName VARCHAR(100) NOT NULL,
  Email VARCHAR(255) UNIQUE NOT NULL,
  PasswordHash VARCHAR(255) NOT NULL,
  Role ENUM('Admin', 'Employee', 'Manager', 'HRTravelAdmin') NOT NULL,
  IsActive BOOLEAN DEFAULT TRUE,
  CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
  UpdatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);
```

### TravelRequests Table
```sql
CREATE TABLE TravelRequests (
  Id INT PRIMARY KEY AUTO_INCREMENT,
  RequestNumber VARCHAR(50) UNIQUE NOT NULL,  -- Format: TR-YYYY-####
  EmployeeId INT NOT NULL,
  ManagerId INT,
  Destination VARCHAR(255) NOT NULL,
  StartDate DATE NOT NULL,
  EndDate DATE NOT NULL,
  Purpose VARCHAR(1000) NOT NULL,
  BudgetAmount DECIMAL(10,2),
  Status ENUM('Draft', 'Submitted', 'Approved', 'Rejected', 'InProgress', 'Completed', 'Cancelled') DEFAULT 'Draft',
  SubmittedAt DATETIME,
  ApprovedAt DATETIME,
  ApprovedBy INT,
  Comments TEXT,
  CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
  UpdatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  FOREIGN KEY (EmployeeId) REFERENCES Users(Id),
  FOREIGN KEY (ManagerId) REFERENCES Users(Id),
  FOREIGN KEY (ApprovedBy) REFERENCES Users(Id)
);
```

### Documents Table
```sql
CREATE TABLE Documents (
  Id INT PRIMARY KEY AUTO_INCREMENT,
  TravelRequestId INT NOT NULL,
  FileName VARCHAR(255) NOT NULL,
  FileType VARCHAR(50),
  FileSize INT,
  FilePath VARCHAR(500),
  UploadedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
  FOREIGN KEY (TravelRequestId) REFERENCES TravelRequests(Id) ON DELETE CASCADE
);
```

### Comments Table
```sql
CREATE TABLE Comments (
  Id INT PRIMARY KEY AUTO_INCREMENT,
  TravelRequestId INT NOT NULL,
  UserId INT NOT NULL,
  CommentText TEXT NOT NULL,
  CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
  UpdatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  FOREIGN KEY (TravelRequestId) REFERENCES TravelRequests(Id) ON DELETE CASCADE,
  FOREIGN KEY (UserId) REFERENCES Users(Id)
);
```

---

## 🔐 Authentication & Authorization

### JWT Configuration
- **Token Expiration:** 24 hours
- **Issuer:** TravelDeskAPI
- **Audience:** TravelDeskApp
- **Secret Key:** Min 32 characters (store in appsettings.json)

### Password Requirements
- Minimum 6 characters
- At least 1 uppercase letter
- At least 1 number
- At least 1 special character (!@#$%^&*)

### Roles & Permissions
```
1. Admin
   - Manage all users
   - View all travel requests
   - System configuration

2. Employee
   - Create travel requests
   - Upload documents
   - View own requests
   - Add comments

3. Manager
   - View team member requests
   - Approve/Reject requests
   - Add comments

4. HR Travel Admin
   - View all approved requests
   - Update request status
   - Generate reports
```

---

## 📡 API Endpoints (23 Total)

### Authentication (3)
- `POST /api/auth/register` - Register new user
- `POST /api/auth/login` - Login with email/password
- `POST /api/auth/refresh-token` - Refresh JWT token

### Users (5)
- `GET /api/users` - Get all users (Admin only)
- `GET /api/users/{id}` - Get user by ID
- `POST /api/users` - Create new user (Admin only)
- `PUT /api/users/{id}` - Update user
- `DELETE /api/users/{id}` - Delete user (Admin only)

### Travel Requests (10)
- `GET /api/travelrequests` - Get all requests (filtered by role)
- `GET /api/travelrequests/{id}` - Get request details
- `POST /api/travelrequests` - Create new request
- `PUT /api/travelrequests/{id}` - Update request
- `PATCH /api/travelrequests/{id}/submit` - Submit request
- `PATCH /api/travelrequests/{id}/approve` - Approve request (Manager/Admin)
- `PATCH /api/travelrequests/{id}/reject` - Reject request (Manager/Admin)
- `PATCH /api/travelrequests/{id}/cancel` - Cancel request
- `DELETE /api/travelrequests/{id}` - Delete request
- `GET /api/travelrequests/{id}/export` - Export to PDF

### Documents (3)
- `POST /api/documents/upload` - Upload document
- `GET /api/documents/{id}` - Download document
- `DELETE /api/documents/{id}` - Delete document

### Comments (2)
- `POST /api/comments` - Add comment
- `GET /api/travelrequests/{id}/comments` - Get all comments

---

## 🎨 Frontend Pages (5)

### 1. Login Page
- Email and password input
- "Forgot Password" link
- "Remember me" checkbox
- Login button with loading state

### 2. Admin Dashboard
- User management table (Create, Edit, Delete users)
- System statistics
- All travel requests list
- User role assignment

### 3. Employee Dashboard
- Quick stats (pending, approved, completed)
- My travel requests table
- Create new request button
- Filter by status

### 4. Create/Edit Travel Request
- Form with fields: Destination, Dates, Purpose, Budget
- Document upload section
- Save as draft / Submit buttons
- Real-time validation

### 5. Travel Request Details
- Read-only view of request details
- Document list with download
- Comments section (add/view)
- Status timeline
- Approval/Rejection buttons (if Manager/Admin)

---

## 🔧 Configuration & Settings

### Request ID Generation
- Format: `TR-YYYY-####`
- Example: `TR-2026-0001`
- Auto-increment per year
- Database trigger or application-level generation

### Document Storage
- **Location:** `Backend/TravelDeskAPI/wwwroot/uploads/documents/`
- **Max file size:** 10 MB
- **Allowed types:** PDF, DOC, DOCX, JPG, PNG, JPEG
- **Naming:** `{RequestId}-{Timestamp}-{OriginalFileName}`

### Email Configuration
- **Service:** SMTP (Gmail or company SMTP)
- **From Address:** noreply@traveldesk.com
- **Templates:** 
  - Request submitted notification
  - Approval notification
  - Rejection notification
  - Status update notification

### Status Workflow
```
Draft → Submitted → Approved/Rejected → InProgress → Completed
                  ↓ Rejected
                  Cancelled
```

---

## ✨ Core Features

### 1. Authentication & Authorization
- JWT token-based auth
- Role-based access control
- Password hashing with BCrypt
- Token refresh mechanism
- Logout functionality

### 2. Travel Request Management
- Create, read, update, delete requests
- Status tracking through workflow
- Request submission with validation
- Approval/rejection by managers
- Draft save functionality

### 3. Document Management
- Upload documents to requests
- Download documents
- File type and size validation
- Automatic file cleanup
- Document preview (if applicable)

### 4. Collaboration & Comments
- Add comments to requests
- View comment history
- Comment timestamps and author info
- Edit/delete own comments

### 5. User Management
- Admin user CRUD
- Role assignment
- User activation/deactivation
- User listing and filtering

### 6. Notifications & Email
- Email on request submission
- Email on request approval
- Email on request rejection
- Status update emails
- Configurable SMTP settings

### 7. Reporting
- Travel request reports (filtered by date, status, employee)
- Export to CSV/PDF
- Dashboard statistics
- Approval trends

---

## 🚀 Implementation Phases

### Phase 1: Backend Setup
1. Create .NET Core 8.0 Web API project
2. Configure MySQL connection
3. Create entity models
4. Set up EF Core migrations
5. Implement JWT authentication
6. Create API controllers
7. Add Swagger documentation

### Phase 2: Frontend Setup
1. Create Angular project
2. Set up routing
3. Create layout components (header, navbar, footer)
4. Implement Angular Material theme

### Phase 3: Authentication
1. Create login page
2. Implement JWT service
3. Create auth guard
4. Add interceptors for token
5. Implement logout

### Phase 4: Core Features
1. Dashboard components
2. Travel request CRUD
3. Document upload/download
4. Comments system
5. Status management

### Phase 5: Admin Features
1. User management
2. Role assignment
3. System configuration

### Phase 6: Polish & Deployment
1. Error handling
2. Loading states
3. Form validation
4. Responsive design
5. Performance optimization

---

## 📝 Default Seed Data

### Users
```
Admin User:
  Email: admin@traveldesk.com
  Password: Admin@123
  Role: Admin

Employee User:
  Email: emp@traveldesk.com
  Password: Emp@123
  Role: Employee

Manager User:
  Email: mgr@traveldesk.com
  Password: Mgr@123
  Role: Manager

HR Admin User:
  Email: hr@traveldesk.com
  Password: Hr@123
  Role: HRTravelAdmin
```

---

## 🔗 API Response Format

### Success Response
```json
{
  "success": true,
  "data": { /* response data */ },
  "message": "Operation successful"
}
```

### Error Response
```json
{
  "success": false,
  "error": "Error message",
  "statusCode": 400
}
```

---

## ⚙️ Configuration Files

### appsettings.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;Database=TravelDeskDB;User=root;Password=your_password;"
  },
  "Jwt": {
    "Key": "your-secret-key-here-must-be-at-least-32-characters-long",
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
    "AllowedExtensions": [".pdf", ".doc", ".docx", ".jpg", ".jpeg", ".png"],
    "UploadPath": "wwwroot/uploads/documents"
  }
}
```

### environment.ts (Angular)
```typescript
export const environment = {
  production: false,
  apiUrl: 'http://localhost:5000/api',
  tokenKey: 'traveldesk_token'
};
```

---

## 📚 File Structure Summary

**Backend Files:** 19 files
- Controllers: 5 (Auth, Users, TravelRequests, Documents, Comments)
- Models: 4 (User, TravelRequest, Document, Comment)
- Services: 2 (IAuthenticationService, AuthenticationService)
- Data: 2 (TravelDeskDbContext, DbInitializer)
- DTOs: 1 (All DTOs in one file)
- Configuration: Multiple

**Frontend Files:** 20+ files
- Components: 5+ (Login, Dashboard, TravelRequest, etc.)
- Services: 3+ (AuthService, ApiService, etc.)
- Guards: 1 (AuthGuard)
- Styles: 5+ CSS files
- Models: Multiple TypeScript interfaces
- Pages: 5

**Total: 40+ files in complete solution**

---

## 🚀 IMPLEMENTATION WORKFLOW

### Step 1: Pre-Generation Confirmation
1. ✅ Review project name
2. ✅ Analyze both workflow diagrams
3. ✅ Review clarifying question answers
4. ✅ Ask follow-up questions ONLY if confused
5. ✅ Confirm understanding

### Step 2: Backend Development (.NET 10)
1. Create database entities based on diagrams
2. Setup DbContext with relationships
3. Create migrations
4. Implement all API endpoints
5. Add business logic for workflows
6. Setup authentication & authorization
7. Add validation & error handling
8. Configure Swagger
9. Remove all code comments
10. Ensure production-ready

### Step 3: Angular Frontend
**⚠️ BEFORE starting frontend, ask user:**
- Is backend implementation correct?
- Any changes needed before proceeding?
- Ready for Angular frontend?

**Then generate:**
1. Angular 18+ project structure
2. Routing based on roles (from diagrams)
3. All page components
4. API service with authentication
5. Auth guards and interceptors
6. Forms with validation
7. Workflow UI components
8. Responsive styling
9. Error handling & notifications
10. Remove all code comments

### Step 4: Final Documentation
1. API documentation
2. Setup guide
3. Deployment instructions

---

## 🎯 Success Criteria

- [ ] Backend API running on http://localhost:5000
- [ ] Frontend Angular app running on http://localhost:4200
- [ ] All API endpoints functional
- [ ] JWT authentication working
- [ ] Database migrations applied
- [ ] All pages rendering correctly
- [ ] CRUD operations working
- [ ] Workflows matching diagrams
- [ ] Role-based access control enforced
- [ ] Document upload/download working
- [ ] Comments system functional
- [ ] Swagger documentation accessible
- [ ] All validations in place
- [ ] Responsive design implemented
- [ ] No comments in any code
- [ ] Production-ready

---

## 📞 Support & Troubleshooting

Include common issues and solutions:
- Connection string errors
- Port conflicts (3000, 5000)
- CORS issues
- Authentication failures
- File upload issues
- Database migration problems

---

## 📄 Additional Notes

- All timestamps should be in UTC
- Implement proper error handling on both frontend and backend
- Add loading states and spinners
- Implement proper form validation
- Add confirmation dialogs for destructive actions
- Implement auto-logout after 24 hours
- Add session timeout warning
- Implement request cancellation if still in Draft/Submitted status
- Add activity audit logging for sensitive operations

---

**Version:** 1.0
**Last Updated:** February 2, 2026
**Status:** Ready for Implementation
