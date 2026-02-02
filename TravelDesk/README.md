# TravelDesk - Travel Request Management System

## Project Overview
TravelDesk is a web-based travel request management system built for ABC Global IT Services. It streamlines the travel request process from submission through approval and booking, replacing manual email-based tracking.

## Technology Stack
- **Frontend**: React 18.2.0
- **Backend**: .NET Core 6.0 Web API
- **Database**: Microsoft SQL Server

## Project Structure

```
TravelDesk/
├── Backend/
│   └── TravelDeskAPI/
│       ├── Controllers/          # API endpoints
│       ├── Models/              # Database models
│       ├── Data/                # Database context and initializer
│       ├── Services/            # Business logic services
│       ├── DTOs/                # Data transfer objects
│       ├── Migrations/          # EF Core migrations
│       ├── Program.cs           # Application startup
│       ├── appsettings.json     # Configuration
│       └── TravelDeskAPI.csproj # Project file
│
└── Frontend/
    └── traveldesk-app/
        ├── public/              # Static files
        ├── src/
        │   ├── components/      # Reusable components
        │   ├── pages/          # Page components
        │   ├── services/       # API services
        │   ├── context/        # React contexts
        │   ├── styles/         # CSS stylesheets
        │   ├── App.js          # Root component
        │   └── index.js        # Entry point
        └── package.json        # Dependencies
```

## Database Models

### User
- Id (PK)
- FirstName, LastName
- Email (Unique), Password
- EmployeeID (Unique)
- Department
- Role (Admin, HRTravelAdmin, Employee, Manager)
- ManagerName, ManagerId
- CreatedDate, IsActive

### TravelRequest
- Id (PK)
- RequestNumber (Unique, Auto-generated)
- EmployeeId (FK), EmployeeID, EmployeeName
- ProjectName, DepartmentName
- ReasonForTravelling
- TypeOfBooking (DomesticFlight, InternationalFlight, Hotel, FlightAndHotel)
- Status (Draft, SubmittedToManager, ApprovedByManager, etc.)
- AadharNumber, PassportNumber
- TravelDate, DaysOfStay
- MealRequired, MealPreference
- ManagerId (FK)
- CreatedDate, SubmittedDate, ModifiedDate, DeletedDate, IsDeleted

### Document
- Id (PK)
- TravelRequestId (FK)
- FileName, FileURL
- DocumentType (AadharCard, Passport, Visa, Ticket, HotelConfirmation, Other)
- Description
- UploadedDate, IsDeleted

### Comment
- Id (PK)
- TravelRequestId (FK)
- UserId (FK)
- CommentText
- CreatedDate, IsDeleted

## API Endpoints

### Authentication
- `POST /api/auth/login` - User login
- `POST /api/auth/register` - User registration

### Users (Admin Only)
- `GET /api/users/total` - Get total user count
- `GET /api/users/grid` - Get paginated user list
- `GET /api/users/{id}` - Get user by ID
- `POST /api/users/add` - Add new user
- `PUT /api/users/edit/{id}` - Edit user
- `DELETE /api/users/delete/{id}` - Delete user
- `PUT /api/users/assign-role/{id}` - Assign role to user

### Travel Requests
- `POST /api/travelrequests/create` - Create new request
- `GET /api/travelrequests/{id}` - Get request details
- `POST /api/travelrequests/{id}/submit` - Submit request
- `POST /api/travelrequests/{id}/delete` - Delete request
- `POST /api/travelrequests/{id}/approve` - Approve request
- `POST /api/travelrequests/{id}/disapprove` - Disapprove request
- `POST /api/travelrequests/{id}/return-to-employee` - Return to employee

### Comments
- `POST /api/comments/{travelRequestId}` - Add comment
- `GET /api/comments/{travelRequestId}` - Get all comments

### Documents
- `POST /api/documents/upload/{travelRequestId}` - Upload document
- `GET /api/documents/{travelRequestId}` - Get documents
- `DELETE /api/documents/{id}` - Delete document

## Role-Based Workflows

### Admin
- Add, Edit, Delete Users
- Assign Roles
- View User Grid with pagination
- See total user count

### Employee
- Create Travel Requests (Draft)
- Fill conditional form fields based on booking type
- Upload required documents
- Submit requests to Manager
- View request status and comments
- Edit returned requests
- Delete draft requests

### Manager
- View assigned requests
- Approve requests (with mandatory comments)
- Disapprove requests (with mandatory comments)
- Return requests to Employee (with mandatory comments)
- View request status (visible to both manager and employee)
- Receive notifications on status changes

### HR Travel Admin
- View all requests in history
- Book tickets/arrange travel (upload documents + comments)
- Return to Manager or Employee (with comments)
- Close requests with "Complete" status
- Handle reassigned requests

## Setup Instructions

### Backend Setup

1. **Install Dependencies**
   ```bash
   cd Backend/TravelDeskAPI
   dotnet restore
   ```

2. **Configure Database Connection**
   - Update `appsettings.json` with your SQL Server connection string

3. **Apply Migrations**
   ```bash
   dotnet ef database update
   ```

4. **Run the Application**
   ```bash
   dotnet run
   ```

The API will be available at `http://localhost:5000`

### Frontend Setup

1. **Install Dependencies**
   ```bash
   cd Frontend/traveldesk-app
   npm install
   ```

2. **Configure API URL**
   - Update the API base URL in `src/services/api.js` if needed

3. **Run the Application**
   ```bash
   npm start
   ```

The app will open at `http://localhost:3000`

## Key Features

✅ Role-Based Access Control
✅ Multi-step Travel Request Workflow
✅ Conditional Form Fields (based on booking type)
✅ Document Upload & Management
✅ Comment System for Requests
✅ Email Notifications (infrastructure ready)
✅ Pagination & Filtering
✅ User Management
✅ Request Status Tracking
✅ JWT Authentication

## Pending Implementation

- Email notification service integration
- Manager assignment logic (auto-assign based on employee's manager)
- Employee request history filtering
- Dashboard statistics
- Request search and advanced filtering
- File download functionality
- Export to PDF/Excel
- User activity logs
- Database backup scripts

## Non-Functional Requirements

| NFR | Requirement |
|-----|-------------|
| NFR 1 | Page Response Time: 3-10 seconds |
| NFR 2 | Browser Support: Chrome, Safari, Edge (latest versions) |
| NFR 3 | Grid Pagination: 20, 50, 100 items per page |
| NFR 4 | Error Messages: Appropriate error messages on failures |
| NFR 5 | Date Format: MM/DD/YYYY |
| NFR 6 | Time Format: 24 Hours |
| NFR 7 | Search/Filter: Available on all grids |
| NFR 8 | Notifications: Email system ready for live environment |

## Testing Credentials

Admin Account:
- Email: `admin@traveldesk.com`
- Password: `Admin@123`

## Configuration

### JWT Configuration (appsettings.json)
```json
"Jwt": {
  "Key": "your-secret-key-here-must-be-at-least-32-characters-long",
  "Issuer": "TravelDeskAPI",
  "Audience": "TravelDeskApp"
}
```

### Database Connection String
```
Server=.;Database=TravelDeskDB;Trusted_Connection=true;TrustServerCertificate=true;
```

## Future Enhancements

1. Mobile application
2. Multi-language support
3. Advanced reporting and analytics
4. Integration with booking services
5. Machine learning for optimal travel suggestions
6. Real-time notifications via SignalR
7. Two-factor authentication
8. Audit logging and compliance reports

## Support & Contact

For issues, questions, or contributions, please contact the development team.

---

**Version**: 1.0.0  
**Last Updated**: January 30, 2026
