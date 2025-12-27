
# 🧑‍💼 EmpManageApp

**Employee Management System (ASP.NET Web Forms)**

EmpManageApp is a **web-based employee management system** designed to manage employees, departments, roles, and leave workflows efficiently.
The application focuses on **backend logic, role-based access control, and database-driven operations** using **ASP.NET and SQL Server**.

---

## 🚀 Features

### 👤 Employee Module

* Employee creation and management
* Role-based access (Admin / Employee)
* Department and designation mapping

### 🏢 Department & Role Management

* Add / update departments
* Assign roles dynamically
* Active / inactive state handling

### 🗓️ Leave Management

* Apply leave with date validation
* Leave approval / rejection by Admin
* Rejection reason support
* Leave status tracking (Pending / Approved / Rejected)

### 📊 Leave Balance

* Automatic leave balance creation per employee
* Prevents negative leave balance
* Leave usage calculation based on approvals

### 🔐 Authentication & Authorization

* Session-based authentication
* Admin and Employee access segregation
* Unauthorized access protection

---

## 🛠️ Tech Stack

| Layer       | Technology                              |
| ----------- | --------------------------------------- |
| Frontend    | ASP.NET Web Forms, HTML, CSS, Bootstrap |
| Backend     | C# (.NET Framework)                     |
| Database    | SQL Server                              |
| Data Access | ADO.NET                                 |
| UI Styling  | Bootstrap 4                             |

---

## 🗂️ Database Design

### Main Tables

* `Emp`
* `Dept`
* `Role`
* `LeaveType`
* `EmpLeave`
* `EmpLeaveBalance`
* `DeptLeave`

### Key Constraints

* Foreign keys for referential integrity
* Unique constraints on employee-leave mapping
* Date validation using CHECK constraints
* Triggers for leave balance handling

---

## 📸 Screenshots (Add Later)

> Recommended screenshots to add:

* Login Page
* Employee Dashboard
* Leave Application Page
* Admin Leave Approval Page

```md
![Login Page](screenshots/login.png)
![Leave Approval](screenshots/leave-approval.png)
```

---

## ⚙️ How to Run Locally

### Prerequisites

* Visual Studio (2019 or later)
* SQL Server + SSMS
* .NET Framework installed

### Steps

1. Clone the repository

   ```bash
   git clone https://github.com/sakibtheseeker/EmpManageApp.git
   ```

2. Open the `.sln` file in Visual Studio

3. Restore database

   * Execute SQL scripts from `/Database` folder
   * Update connection string in `Web.config`

4. Run the project

   * Press `Ctrl + F5` or click **Start**

---

## 🔐 Default Roles

| Role     | Access                       |
| -------- | ---------------------------- |
| Admin    | Full control, leave approval |
| Employee | Apply leave, view status     |

---

## 📌 Learning Outcomes

* Practical understanding of **ASP.NET Web Forms**
* Real-world **CRUD operations**
* SQL constraints and joins
* Session handling & role-based authorization
* Debugging runtime exceptions (NullReference, DataBinding issues)

---

## 🚧 Future Enhancements

* Convert to ASP.NET MVC
* API-based backend
* Angular frontend integration
* Email notifications
* Dashboard analytics

---

## 👨‍💻 Author

**Sakib Tamboli**
📧 Email: [sakibtamboliwork@gmail.com](mailto:sakibtamboliwork@gmail.com)
🌐 Portfolio: [https://sakib-tamboli.netlify.app](https://sakib-tamboli.netlify.app)
🔗 GitHub: [https://github.com/sakibtheseeker](https://github.com/sakibtheseeker)

---

## ⭐ If you like this project

Give it a ⭐ on GitHub — it motivates me to build more!

---

