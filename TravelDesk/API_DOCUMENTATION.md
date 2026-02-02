# API Documentation

## Base URL
```
http://localhost:5000/api
```

## Authentication
All endpoints (except login/register) require a JWT token in the Authorization header:
```
Authorization: Bearer <your_jwt_token>
```

## Endpoints

### Auth Endpoints

#### Login
**POST** `/auth/login`

Request:
```json
{
  "email": "admin@traveldesk.com",
  "password": "Admin@123"
}
```

Response:
```json
{
  "message": "Login successful",
  "data": {
    "userId": 1,
    "email": "admin@traveldesk.com",
    "firstName": "Admin",
    "lastName": "User",
    "role": "Admin",
    "token": "eyJhbGc..."
  }
}
```

#### Register
**POST** `/auth/register`

Request:
```json
{
  "firstName": "John",
  "lastName": "Doe",
  "email": "john@example.com",
  "password": "SecurePassword123",
  "employeeID": "EMP001",
  "department": "IT",
  "role": "Employee"
}
```

---

### User Endpoints (Admin Only)

#### Get Total Users
**GET** `/users/total`

Response:
```json
{
  "totalUsers": 45
}
```

#### Get Users Grid
**GET** `/users/grid?pageNumber=1&pageSize=20`

Response:
```json
{
  "data": [
    {
      "id": 1,
      "firstName": "John",
      "lastName": "Doe",
      "employeeID": "EMP001",
      "department": "IT",
      "role": "Employee",
      "managerName": "Manager Name"
    }
  ],
  "totalCount": 45,
  "pageNumber": 1,
  "pageSize": 20
}
```

#### Get User by ID
**GET** `/users/{id}`

Response:
```json
{
  "id": 1,
  "firstName": "John",
  "lastName": "Doe",
  "email": "john@example.com",
  "employeeID": "EMP001",
  "department": "IT",
  "role": "Employee",
  "managerName": "Manager Name",
  "isActive": true
}
```

#### Add User
**POST** `/users/add`

Request:
```json
{
  "firstName": "Jane",
  "lastName": "Smith",
  "email": "jane@example.com",
  "password": "SecurePassword123",
  "employeeID": "EMP002",
  "department": "HR",
  "role": "Employee",
  "managerName": "John Doe",
  "managerId": 1
}
```

#### Edit User
**PUT** `/users/edit/{id}`

Request:
```json
{
  "firstName": "Jane",
  "lastName": "Smith",
  "department": "HR",
  "role": "Manager",
  "managerName": null,
  "managerId": null
}
```

#### Delete User
**DELETE** `/users/delete/{id}`

#### Assign Role
**PUT** `/users/assign-role/{id}`

Request:
```json
{
  "role": "Manager"
}
```

---

### Travel Request Endpoints

#### Create Travel Request
**POST** `/travelrequests/create`

Request:
```json
{
  "employeeID": "EMP001",
  "employeeName": "John Doe",
  "projectName": "Project Alpha",
  "departmentName": "IT",
  "reasonForTravelling": "Client meeting and training",
  "typeOfBooking": "FlightAndHotel",
  "aadharNumber": "1234567890123456",
  "passportNumber": "K1234567",
  "travelDate": "2024-02-15",
  "daysOfStay": 3,
  "mealRequired": "Both",
  "mealPreference": "Veg"
}
```

Response:
```json
{
  "message": "Travel request created successfully",
  "data": {
    "requestId": 1,
    "requestNumber": "TR-20240130-ABC12345"
  }
}
```

#### Get Travel Request
**GET** `/travelrequests/{id}`

Response:
```json
{
  "id": 1,
  "requestNumber": "TR-20240130-ABC12345",
  "employeeID": "EMP001",
  "employeeName": "John Doe",
  "projectName": "Project Alpha",
  "departmentName": "IT",
  "reasonForTravelling": "Client meeting",
  "typeOfBooking": "FlightAndHotel",
  "status": "Draft",
  "travelDate": "2024-02-15",
  "daysOfStay": 3,
  "mealRequired": "Both",
  "mealPreference": "Veg",
  "createdDate": "2024-01-30T10:00:00",
  "submittedDate": null
}
```

#### Submit Travel Request
**POST** `/travelrequests/{id}/submit`

Response:
```json
{
  "message": "Travel request submitted successfully"
}
```

#### Delete Travel Request
**POST** `/travelrequests/{id}/delete`

#### Approve Travel Request
**POST** `/travelrequests/{id}/approve`

Request:
```json
{
  "commentText": "Request approved. Please proceed with booking."
}
```

#### Disapprove Travel Request
**POST** `/travelrequests/{id}/disapprove`

Request:
```json
{
  "commentText": "Please provide more details about the travel purpose."
}
```

#### Return to Employee
**POST** `/travelrequests/{id}/return-to-employee`

Request:
```json
{
  "commentText": "Please update the hotel preferences."
}
```

---

### Comments Endpoints

#### Add Comment
**POST** `/comments/{travelRequestId}`

Request:
```json
{
  "commentText": "Updated details as requested."
}
```

#### Get Comments
**GET** `/comments/{travelRequestId}`

Response:
```json
[
  {
    "id": 1,
    "travelRequestId": 1,
    "userName": "John Doe",
    "commentText": "Need more information",
    "createdDate": "2024-01-30T10:15:00"
  }
]
```

---

### Documents Endpoints

#### Upload Document
**POST** `/documents/upload/{travelRequestId}`

Form Data:
- `file`: [multipart file]
- `documentType`: "Passport" | "Visa" | "AadharCard" | "Ticket" | "HotelConfirmation" | "Other"

Response:
```json
{
  "message": "Document uploaded successfully",
  "data": {
    "documentId": 1
  }
}
```

#### Get Documents
**GET** `/documents/{travelRequestId}`

Response:
```json
[
  {
    "id": 1,
    "travelRequestId": 1,
    "fileName": "passport.pdf",
    "fileURL": "/uploads/1_20240130101500_passport.pdf",
    "documentType": "Passport",
    "uploadedDate": "2024-01-30T10:15:00"
  }
]
```

#### Delete Document
**DELETE** `/documents/{id}`

---

## Error Responses

### 400 Bad Request
```json
{
  "message": "Invalid request"
}
```

### 401 Unauthorized
```json
{
  "message": "Please enter the correct Email & Password"
}
```

### 403 Forbidden
```json
{
  "message": "User does not have permission to access this resource"
}
```

### 404 Not Found
```json
{
  "message": "Resource not found"
}
```

### 500 Internal Server Error
```json
{
  "message": "An error occurred while processing your request"
}
```

---

## Status Codes

| Code | Meaning |
|------|---------|
| 200 | Success |
| 201 | Created |
| 400 | Bad Request |
| 401 | Unauthorized |
| 403 | Forbidden |
| 404 | Not Found |
| 500 | Server Error |

---

## Pagination

For endpoints that support pagination (e.g., `/users/grid`):

Query Parameters:
- `pageNumber` (default: 1)
- `pageSize` (default: 20, options: 20, 50, 100)

Example:
```
GET /users/grid?pageNumber=2&pageSize=50
```

---

**Last Updated**: January 30, 2026
