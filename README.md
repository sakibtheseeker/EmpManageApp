---

# HRDesk – Employee Management System

HRDesk is a **role-based Employee Management System** built using **ASP.NET Web Forms** and **SQL Server**.
It is designed to help organizations manage employees, departments, roles, leaves, events, documents, and login access in a structured and secure manner.

This project demonstrates **real-world HR workflows**, **clean role separation**, and **database-driven logic**, making it ideal for **academic submission**, **portfolio**, and **job interviews**.

---

## 🚀 Key Features

### 👥 Role-Based Access Control

HRDesk supports three user roles:

* **Admin**
* **Manager**
* **Employee**

Each role has **strictly controlled access** to pages and actions.

---

### 🔐 Authentication & Login

* Separate **Employee Login** and **Admin Login**
* Admin login handled via a dedicated `AdminLogin.aspx`
* Employees cannot self-register
* Admin creates employee login credentials
* Login credentials are **emailed via SMTP**
* Session-based authentication with role validation

---

### 🧑‍💼 Employee Management (Admin)

* Add, edit, soft-delete employees
* Assign:

  * Role
  * Department
  * Designation
  * Manager
* Enforced business rules:

  * Admin → No department/designation/manager
  * Manager → Department allowed, no manager/designation
  * Employee → Full assignment
* Prevents invalid role combinations

---

### 🏢 Department & Designation Management

* Add / update / soft-delete departments
* Add designations mapped to departments
* Prevents duplicate department/designation entries
* Dropdowns dynamically load based on department selection

---

### 🧾 Role Management

* Add, update, soft-delete roles
* Prevent duplicate roles (e.g., only one “Admin”)
* Used consistently across the system

---

### 📆 Leave Management

**Employee**

* Apply leave
* Edit or delete pending leave
* View leave balance badges
* Overlap and balance validations

**Manager**

* Approve / Reject employee leaves
* Add rejection reason
* Leaves filtered by reporting manager

**Admin**

* Define leave types
* Configure department-wise leave limits

---

### 📅 Event Management

* Create event types (with color coding)
* Create and manage event calendar
* Soft delete supported
* Clean separation of Event Type and Event Calendar

---

### 📂 Document Management

* Upload employee documents
* Store files in structured folders
* View documents in browser (new tab)
* Download documents securely
* Employees can only view **their own documents**
* Managers and admins restricted as per rules

---

### ✉️ Email Integration (SMTP)

* Automatic email sent when:

  * Admin creates login credentials
* Includes:

  * Username
  * Password
* Built using `System.Net.Mail` with Gmail SMTP

---

## 🧱 Project Structure

```text
HRDesk (EmpManageApp)
│
├── AddDocument.aspx              # Upload employee documents
├── ViewDocuments.aspx            # View & download documents
│
├── Login.aspx                    # Employee login
├── AdminLogin.aspx               # Admin-only login
├── AdminSignup.aspx              # Admin creates employee login
│
├── Emp.aspx                      # Employee management (Admin)
├── Dept.aspx                     # Department management
├── Designation.aspx              # Designation management
├── Role.aspx                     # Role management
│
├── LeaveType.aspx                # Leave types (Admin)
├── AddLeave.aspx                 # Department leave allocation
├── ApplyLeave.aspx               # Employee applies leave
├── ApproveLeave.aspx             # Manager approves leave
│
├── EventType.aspx                # Event type management
├── EventCalender.aspx            # Event calendar
│
├── Employee Documents/           # Uploaded documents storage
│
├── packages.config               # NuGet packages
└── Web.config                    # Connection strings & SMTP config
```

---

## 🗄️ Database Overview

**Key Tables**

* `Emp`
* `Login`
* `Dept`
* `Designation`
* `Role`
* `LeaveType`
* `DeptLeave`
* `EmpLeave`
* `EmpLeaveBalance`
* `EventType`
* `EventCalendar`
* `EmpDocument`

**Design Principles**

* Soft delete using `isActive`
* Strong foreign key relationships
* Unique constraints for data integrity
* Stored procedures for all operations

---

## 🧠 Technical Highlights

* ASP.NET Web Forms (Code-Behind pattern)
* SQL Server Stored Procedures
* Bootstrap 4 UI
* jQuery for dynamic behavior
* Session-based authentication
* Server-side validation + business rules
* Modular and scalable architecture

---

## 🔐 Security Practices

* Role-based page access checks
* No direct employee signup
* Admin-controlled login creation
* Document access restricted by employee ID
* Soft delete instead of destructive delete
* Prevents duplicate and invalid records

---

## 🎯 Use Case Flow

1. **Admin logs in**
2. Creates departments, roles, designations
3. Adds employees
4. Creates employee login credentials
5. Credentials emailed to employee
6. Employee logs in → applies leave, views documents
7. Manager logs in → approves/rejects leaves

---

## 📌 Future Enhancements

* Password hashing (BCrypt)
* Change password feature
* Dashboard analytics
* Role-based document visibility
* Audit logs
* API layer (Web API)

---

## 👨‍💻 Developed By

**Sakib Tamboli**

* 📍 India
* 🎓 BSc IT
* 💻 ASP.NET | SQL Server | Web Forms
* 🚀 Actively building real-world projects

---
