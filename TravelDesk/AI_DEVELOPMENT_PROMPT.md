# AI Prompt for TravelDesk Project Development

## 🎯 Project Setup Instructions

I will be creating a **.NET Core Web API project** in **Visual Studio** using **.NET 10**. 

Follow these steps carefully:

### Step 1: Project Name
I will provide you with the **project name** once I create it in Visual Studio.

### Step 2: Workflow Diagrams
I will share **two workflow diagram pictures** showing:
1. **Travel Request Workflow** - The complete flow of creating, submitting, approving, and completing travel requests
2. **User Role Workflow** - How different roles (Admin, Employee, Manager, HR Travel Admin) interact with the system

### Step 3: Your Tasks Before Proceeding

Before you make ANY changes to the project:

**IMPORTANT: ASK ME THESE CLARIFYING QUESTIONS:**

1. **Database Schema Clarifications**
   - Are there any additional fields in the User table beyond: FirstName, LastName, Email, Password, Role, Department, EmployeeID, ManagerName, ManagerId?
   - Should TravelRequest have any additional fields beyond: RequestNumber, Destination, StartDate, EndDate, Purpose, BudgetAmount, Status?
   - Are there any additional entities/tables needed beyond: User, TravelRequest, Document, Comment?

2. **Workflow-Specific Questions** (based on the pictures I provide)
   - What are ALL the possible status values for a travel request?
   - Can an Employee modify a request after submission?
   - Can a Manager reassign a request to another Manager?
   - Should there be approval chains (multiple approvals) or single-level approval?
   - What triggers automatic status changes, if any?
   - Are there any rejection reasons/feedback required?

3. **Feature Questions**
   - Should users be able to save drafts and come back later?
   - Should there be a request cancellation feature?
   - Should deleted requests be soft-deleted or hard-deleted?
   - Should there be an approval deadline/expiry date?
   - Should there be notifications/emails at each status change?
   - Should there be reporting/analytics features?

4. **Document & File Questions**
   - Besides the approved file types (PDF, DOC, DOCX, JPG, PNG), are there any other types needed?
   - Should documents be required or optional for submission?
   - Should old versions of documents be kept in history?
   - Can documents be uploaded after submission?

5. **API Design Questions**
   - Should the API have pagination for list endpoints?
   - Should there be filtering/search capabilities?
   - Should there be sorting options?
   - Should API responses have a standard format wrapper?
   - What error codes should be returned for different scenarios?

6. **Security & Validation Questions**
   - Are there any specific password complexity rules?
   - Should users have session timeouts?
   - Should there be rate limiting on API endpoints?
   - Should certain endpoints have additional authorization checks?
   - Should there be audit logging for sensitive operations?

7. **Angular Frontend Questions**
   - Should the frontend use Angular Material or Bootstrap for UI?
   - Should the frontend be responsive (mobile-friendly)?
   - Should there be any data caching on the frontend?
   - Should forms have auto-save drafts functionality?
   - Should there be real-time notifications?

8. **Deployment & Environment Questions**
   - Are there different configurations for Development, Staging, and Production?
   - Should connection strings be environment-specific?
   - Should there be different API keys for different environments?

---

## 📋 How I Will Proceed

**ONLY AFTER you provide:**
1. ✅ Project name
2. ✅ Two workflow diagram pictures
3. ✅ Answers to ALL clarifying questions above

**THEN I will:**

### Phase 1: Analyze & Confirm
1. Study the workflow diagrams carefully
2. Map the workflows to database schema
3. Design API endpoints based on workflows
4. Create data models matching the workflows

### Phase 2: Backend Development (.NET 10)
1. Create/modify database entities based on diagrams
2. Add properties to User entity (if needed)
3. Add properties to TravelRequest entity (if needed)
4. Create any new entities (if needed)
5. Update DbContext with new entities
6. Create database migrations
7. Implement all API endpoints matching the workflows
8. Add business logic for status transitions
9. Implement validation rules
10. Add error handling
11. Configure CORS, Authentication, Authorization
12. Add Swagger documentation

### Phase 3: Frontend Development (Angular)
1. Create Angular project structure
2. Design components based on user roles
3. Implement pages for each workflow step
4. Create services for API communication
5. Implement authentication and route guards
6. Build forms with validation
7. Add status tracking UI
8. Implement document upload
9. Add comments/collaboration features
10. Style with responsive design

### Phase 4: Testing & Validation
1. Verify all endpoints work correctly
2. Test all workflow transitions
3. Validate all business rules
4. Test role-based access control
5. Verify error handling

### Phase 5: Documentation
1. Update API documentation
2. Create workflow documentation
3. Add deployment guide
4. Create user guide

---

## 📸 What I Need From You

### Picture 1: Travel Request Workflow Diagram
Should show:
- ✅ All status states (Draft → Submitted → Approved/Rejected → In Progress → Completed)
- ✅ Who can perform each action
- ✅ Conditions for status transitions
- ✅ Approval flow
- ✅ Rejection flow
- ✅ Any feedback/revision loops

### Picture 2: User Role Workflow Diagram
Should show:
- ✅ All user roles
- ✅ Permissions for each role
- ✅ Actions each role can perform
- ✅ Data access restrictions per role
- ✅ Navigation flow for each role

---

## ✅ Checklist Before Sending

Before you upload the diagrams and answers, make sure you have:

- [ ] Project name decided
- [ ] Workflow diagram 1 ready (Travel Request workflow)
- [ ] Workflow diagram 2 ready (User Role workflow)
- [ ] Answers to ALL 8 clarifying question sections
- [ ] Any additional requirements or specifications
- [ ] Any specific business rules or constraints

---

## 📝 Information I Already Know

Based on your previous requirements:

### Tech Stack
- **Backend:** .NET Core 10.0 Web API
- **Frontend:** Angular 18+
- **Database:** MySQL 8.0+
- **Authentication:** JWT + BCrypt
- **API Format:** RESTful with standard response wrapper
- **Request ID Format:** TR-YYYY-#### (auto-increment)
- **Document Storage:** File system at `wwwroot/uploads/documents/`
- **Document Limits:** Max 10MB, Types: PDF, DOC, DOCX, JPG, PNG, JPEG

### Default Entities (May Change Based on Diagrams)
- User (with roles)
- TravelRequest
- Document
- Comment

### Roles (May Change Based on Diagrams)
1. Admin - Full system access
2. Employee - Create requests, upload docs, comment
3. Manager - View team requests, approve/reject, comment
4. HR Travel Admin - View approved requests, process, report

### API Response Format (Unless Different)
```json
{
  "success": true/false,
  "data": { /* response data */ },
  "message": "Operation message",
  "statusCode": 200
}
```

---

## 🚀 Ready When You Are!

### Send Me:

1. **Project Name:** (e.g., "TravelDesk", "TravelRequestSystem", etc.)

2. **Workflow Diagram 1 Image:** (Travel Request workflow)

3. **Workflow Diagram 2 Image:** (User Role workflow)

4. **Answers to Clarifying Questions:** 
   - Copy all 8 sections and provide your answers
   - If a question doesn't apply, write "N/A"

5. **Any Additional Notes/Requirements**

---

**Once you provide all this information, I will:**
- Ask any follow-up questions if needed
- Confirm my understanding of the workflows
- Outline the exact implementation plan
- Begin development with complete clarity on your requirements

---

**Status:** ⏳ Waiting for your input

**Next Action:** Upload project name → diagrams → answers → clarifying questions
