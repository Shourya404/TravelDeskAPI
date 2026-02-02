# TravelDesk Project - Auto-Generation Prompt

## 📌 IMPORTANT INSTRUCTIONS FOR AI

You are about to receive:
1. **Project Name** - The name I created in Visual Studio
2. **Two Workflow Diagram Pictures** - Visual representation of the system flows
3. **Clarifying Question Answers** - Specifications for this project

**YOUR TASK:**
- Review all provided information
- Ask clarifying questions ONLY if genuinely confused
- Generate complete .NET 10 Web API backend
- Generate complete Angular 18+ frontend
- Include all features based on workflows
- Make project production-ready

---

## 🎯 What I'm Providing

### 1. Project Name
`[PROJECT_NAME_WILL_BE_PROVIDED]`

### 2. Workflow Diagram Pictures
- **Diagram 1:** Travel Request Workflow
- **Diagram 2:** User Role Workflow

### 3. Clarifying Question Answers

#### Database Schema Clarifications
Q: Additional User table fields?
A: `[ANSWER_WILL_BE_PROVIDED]`

Q: Additional TravelRequest fields?
A: `[ANSWER_WILL_BE_PROVIDED]`

Q: Additional entities/tables?
A: `[ANSWER_WILL_BE_PROVIDED]`

#### Workflow-Specific Questions
Q: All possible status values?
A: `[ANSWER_WILL_BE_PROVIDED]`

Q: Can Employee modify after submission?
A: `[ANSWER_WILL_BE_PROVIDED]`

Q: Can Manager reassign to another Manager?
A: `[ANSWER_WILL_BE_PROVIDED]`

Q: Approval chains or single-level?
A: `[ANSWER_WILL_BE_PROVIDED]`

Q: Automatic status changes?
A: `[ANSWER_WILL_BE_PROVIDED]`

Q: Rejection reasons/feedback required?
A: `[ANSWER_WILL_BE_PROVIDED]`

#### Feature Questions
Q: Save drafts and come back later?
A: `[ANSWER_WILL_BE_PROVIDED]`

Q: Request cancellation feature?
A: `[ANSWER_WILL_BE_PROVIDED]`

Q: Soft-delete or hard-delete?
A: `[ANSWER_WILL_BE_PROVIDED]`

Q: Approval deadline/expiry?
A: `[ANSWER_WILL_BE_PROVIDED]`

Q: Notifications/emails at status change?
A: `[ANSWER_WILL_BE_PROVIDED]`

Q: Reporting/analytics features?
A: `[ANSWER_WILL_BE_PROVIDED]`

#### Document & File Questions
Q: Other file types besides PDF, DOC, DOCX, JPG, PNG?
A: `[ANSWER_WILL_BE_PROVIDED]`

Q: Documents required or optional?
A: `[ANSWER_WILL_BE_PROVIDED]`

Q: Keep document version history?
A: `[ANSWER_WILL_BE_PROVIDED]`

Q: Upload after submission?
A: `[ANSWER_WILL_BE_PROVIDED]`

#### API Design Questions
Q: Pagination needed?
A: `[ANSWER_WILL_BE_PROVIDED]`

Q: Filtering/search capabilities?
A: `[ANSWER_WILL_BE_PROVIDED]`

Q: Sorting options?
A: `[ANSWER_WILL_BE_PROVIDED]`

Q: Standard response format wrapper?
A: `[ANSWER_WILL_BE_PROVIDED]`

Q: Error codes needed?
A: `[ANSWER_WILL_BE_PROVIDED]`

#### Security & Validation Questions
Q: Password complexity rules?
A: `[ANSWER_WILL_BE_PROVIDED]`

Q: Session timeout?
A: `[ANSWER_WILL_BE_PROVIDED]`

Q: Rate limiting?
A: `[ANSWER_WILL_BE_PROVIDED]`

Q: Additional authorization checks?
A: `[ANSWER_WILL_BE_PROVIDED]`

Q: Audit logging needed?
A: `[ANSWER_WILL_BE_PROVIDED]`

#### Angular Frontend Questions
Q: Angular Material or Bootstrap?
A: `[ANSWER_WILL_BE_PROVIDED]`

Q: Responsive/mobile-friendly?
A: `[ANSWER_WILL_BE_PROVIDED]`

Q: Frontend data caching?
A: `[ANSWER_WILL_BE_PROVIDED]`

Q: Auto-save drafts?
A: `[ANSWER_WILL_BE_PROVIDED]`

Q: Real-time notifications?
A: `[ANSWER_WILL_BE_PROVIDED]`

#### Deployment & Environment Questions
Q: Dev, Staging, Production configs?
A: `[ANSWER_WILL_BE_PROVIDED]`

Q: Environment-specific connection strings?
A: `[ANSWER_WILL_BE_PROVIDED]`

Q: Different API keys per environment?
A: `[ANSWER_WILL_BE_PROVIDED]`

---

## 🔧 Pre-Configured Defaults (Can Be Overridden)

### Technology Stack
- **Backend:** .NET Core 10.0 Web API
- **Frontend:** Angular 18+
- **Database:** MySQL 8.0+
- **Authentication:** JWT + BCrypt
- **ORM:** Entity Framework Core 9.0

### File Structure
```
[PROJECT_NAME]/
├── Backend/
│   └── [PROJECT_NAME]API/          (.NET 10 Web API)
│       ├── Controllers/             (5 controllers)
│       ├── Models/                  (Entities based on diagrams)
│       ├── Services/                (Business logic)
│       ├── Data/                    (DbContext, Migrations)
│       ├── DTOs/                    (Data Transfer Objects)
│       ├── wwwroot/
│       │   └── uploads/documents/
│       ├── appsettings.json
│       ├── Program.cs
│       └── [PROJECT_NAME]API.csproj
│
├── Frontend/
│   └── [PROJECT_NAME]-app/          (Angular SPA)
│       ├── src/
│       │   ├── app/
│       │   │   ├── components/
│       │   │   ├── pages/
│       │   │   ├── services/
│       │   │   ├── guards/
│       │   │   ├── models/
│       │   │   ├── interceptors/
│       │   │   └── app.module.ts
│       │   ├── styles/
│       │   ├── assets/
│       │   └── environments/
│       ├── angular.json
│       └── package.json
│
└── Documentation/
    ├── README.md
    ├── API_DOCUMENTATION.md
    ├── SETUP_GUIDE.md
    └── DEPLOYMENT_GUIDE.md
```

### Database
- **Name:** `[PROJECT_NAME]DB`
- **Provider:** Pomelo.EntityFrameworkCore.MySql
- **Auto-migrations:** Enabled
- **Seed data:** Default users with test credentials

### Default Entities (Will Be Modified Based on Diagrams)
- User
- TravelRequest
- Document
- Comment

### Default Roles (Will Be Modified Based on Diagrams)
- Admin
- Employee
- Manager
- HR Travel Admin

### Default Configuration
- **JWT Expiration:** 24 hours
- **API Base Path:** `/api`
- **CORS:** Enabled for localhost:4200
- **Swagger:** Enabled at `/swagger`
- **Request ID Format:** `[PREFIX]-YYYY-####`
- **Document Storage:** File system
- **Document Max Size:** 10MB

---

## 🚀 Generation Workflow

### Phase 1: Confirmation (1-5 minutes)
1. Review provided project name
2. Analyze workflow diagrams
3. Parse clarifying question answers
4. **Ask follow-up questions ONLY if unclear**
5. Confirm understanding with user

### Phase 2: Backend Development (30-60 minutes)
1. Design database schema based on diagrams
2. Create .NET 10 models/entities
3. Setup EF Core DbContext
4. Create database migrations
5. Implement authentication service
6. Build all API controllers
7. Add business logic for workflows
8. Implement authorization checks
9. Add validation rules
10. Configure error handling
11. Add Swagger documentation
12. Setup CORS, middleware, configuration

### Phase 3: Frontend Development (60-120 minutes)
1. Create Angular project structure
2. Design routing based on user roles
3. Create layout components
4. Build page components
5. Create API service
6. Implement authentication guard
7. Build forms with validation
8. Add state management (if needed)
9. Create workflow UI components
10. Add styling (Bootstrap/Material)
11. Implement responsive design
12. Add error handling & notifications

### Phase 4: Integration & Testing (15-30 minutes)
1. Verify API endpoints
2. Test authentication flow
3. Validate workflow transitions
4. Check role-based access
5. Test document upload
6. Verify error handling

### Phase 5: Documentation (10-15 minutes)
1. Create README
2. API Documentation
3. Setup guide
4. Deployment instructions

---

## ✅ Generation Checklist

Backend:
- [ ] .NET 10 project created with correct name
- [ ] MySQL connection configured
- [ ] All entities created based on diagrams
- [ ] DbContext with migrations
- [ ] JWT authentication service
- [ ] All API controllers implemented
- [ ] Business logic for workflows
- [ ] Authorization/permissions
- [ ] Validation rules
- [ ] Error handling with proper status codes
- [ ] CORS configured
- [ ] Swagger documentation
- [ ] appsettings.json configured
- [ ] No comments in code
- [ ] Production-ready

Frontend:
- [ ] Angular 18+ project created
- [ ] Routing configured based on roles
- [ ] All pages created
- [ ] Components for all features
- [ ] API service with base URL
- [ ] Authentication service and guard
- [ ] Login page with validation
- [ ] Dashboard for each role
- [ ] Workflow UI components
- [ ] Document upload functionality
- [ ] Comments/collaboration features
- [ ] Responsive design
- [ ] Error handling and notifications
- [ ] Loading states and spinners
- [ ] No comments in code
- [ ] Production-ready

---

## 🎯 If You Have Questions

If anything is unclear about the diagrams or answers:
1. Ask specific clarifying questions
2. Request diagram clarification
3. Ask for additional context
4. DO NOT guess or assume

**But do NOT ask the user to provide what was already provided in answers.**

---

## 📋 Generation Output

You will create:

### Backend Files (30-40 files)
- Controllers (5-8)
- Models (4-8)
- Services (3-5)
- DTOs (1-2)
- Data layer (DbContext, Migrations)
- Configuration files

### Frontend Files (30-50 files)
- Components (8-12)
- Pages (5-8)
- Services (3-5)
- Guards (2-3)
- Models/Interfaces (5-10)
- Styles (5-10)
- Configuration files

### Documentation (4-5 files)
- README
- API Documentation
- Setup Guide
- Deployment Guide

---

## 🔐 Code Quality Standards

- ✅ No comments (clean code)
- ✅ Proper error handling
- ✅ Input validation
- ✅ SQL injection prevention
- ✅ XSS protection
- ✅ CSRF protection
- ✅ Password hashing (BCrypt)
- ✅ JWT token security
- ✅ Role-based access control
- ✅ Async/await patterns
- ✅ Dependency injection
- ✅ SOLID principles
- ✅ RESTful API design
- ✅ Production-ready

---

## 📞 Ready to Begin?

Waiting for:
1. ✅ Project name
2. ✅ Workflow diagram 1 (image)
3. ✅ Workflow diagram 2 (image)
4. ✅ Clarifying question answers

Once all provided → AI generates complete project

---

**Status:** ⏳ Awaiting Project Details

**Next Step:** Provide project name, diagrams, and answers
