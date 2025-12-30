<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="AdminSignup.aspx.cs"
    Inherits="EmpManageApp.AdminSignup" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Admin Signup</title>

    <!-- Bootstrap CSS -->
    <link rel="stylesheet"
          href="https://cdn.jsdelivr.net/npm/bootstrap@4.0.0/dist/css/bootstrap.min.css" />
</head>
<body class="bg-light">

<form id="form1" runat="server">
                  <nav class="navbar navbar-expand-lg navbar-dark bg-primary">
    <div class="container-fluid">
        <a class="navbar-brand" href="Emp.aspx">EmpManage</a>

        <button class="navbar-toggler" type="button"
                data-toggle="collapse"
                data-target="#navbarSupportedContent"
                aria-controls="navbarSupportedContent"
                aria-expanded="false"
                aria-label="Toggle navigation">
            <span class="navbar-toggler-icon"></span>
        </button>

        <div class="collapse navbar-collapse" id="navbarSupportedContent">
                 <ul class="navbar-nav ml-auto">

    <!-- ADMIN MENU -->
    <li class="nav-item" runat="server" id="liDept">
        <a class="nav-link" href="Dept.aspx">Department</a>
    </li>

    <li class="nav-item" runat="server" id="liDesignation">
        <a class="nav-link" href="Designation.aspx">Designation</a>
    </li>

    <li class="nav-item" runat="server" id="liRole">
        <a class="nav-link" href="Role.aspx">Role</a>
    </li>

    <li class="nav-item dropdown" runat="server" id="liEmp">
    <a class="nav-link dropdown-toggle"
       href="#"
       id="employeeDropdown"
       role="button"
       data-toggle="dropdown"
       aria-haspopup="true"
       aria-expanded="false">
        Employee
    </a>

    <div class="dropdown-menu dropdown-menu-right"
         aria-labelledby="employeeDropdown">

        <!-- Add Employee -->
        <a class="dropdown-item"
           href="Emp.aspx">
            Add / Manage Employee
        </a>

        <!-- Create Login -->
        <a class="dropdown-item"
           href="AdminSignup.aspx">
            Create Employee Login
        </a>

    </div>
</li>


    <!-- EVENT (ADMIN) -->
    <li class="nav-item dropdown" runat="server" id="liEvent">
        <a class="nav-link dropdown-toggle" href="#" data-toggle="dropdown">
            Event
        </a>
        <div class="dropdown-menu">
            <a class="dropdown-item" href="EventType.aspx">Event Type</a>
            <a class="dropdown-item" href="EventCalender.aspx">Event Calender</a>
        </div>
    </li>

    <!-- LEAVE MENU -->
    <li class="nav-item dropdown" runat="server" id="liLeave">
        <a class="nav-link dropdown-toggle" href="#" data-toggle="dropdown">
            Leave
        </a>
        <div class="dropdown-menu">

            <a class="dropdown-item" runat="server" id="liLeaveType" href="LeaveType.aspx">
                Leave Type
            </a>

            <a class="dropdown-item" runat="server" id="liAddLeave" href="AddLeave.aspx">
                Add Leave
            </a>

        </div>
    </li>

                           <!-- DOCUMENTS MENU -->
<li class="nav-item dropdown" runat="server" id="liDocuments">
    <a class="nav-link dropdown-toggle" href="#"
       data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
        Documents
    </a>

    <div class="dropdown-menu">

        <!-- ADMIN -->
        <a class="dropdown-item"
           runat="server"
           id="liAddDocument"
           href="AddDocument.aspx">
            Add Document
        </a>

        <!-- MANAGER -->
        <a class="dropdown-item"
           runat="server"
           id="liViewDocuments"
           href="ViewDocuments.aspx">
            View Documents
        </a>

    </div>
</li>
     <li class="nav-item" runat="server" id="liLogout">
    <asp:LinkButton
        ID="btnLogout"
        runat="server"
        CssClass="nav-link text-white"
        OnClick="btnLogout_Click">
        Logout
    </asp:LinkButton>
</li>


</ul>
        </div>
    </div>
</nav>
<div class="container mt-5">
    <div class="row justify-content-center">
        <div class="col-md-5">

            <div class="card shadow">
                <div class="card-header bg-primary text-white text-center">
                    <h5>Create Login for Employee</h5>
                </div>

                <div class="card-body">
                    <asp:Label
                        ID="lblMessage"
                        runat="server"
                        CssClass="alert alert-success d-block"
                        Visible="false">
                    </asp:Label>

                    <!-- EMPLOYEE -->
                    <div class="form-group">
                        <label>Employee</label>
                        <asp:DropDownList
                            ID="ddlEmployee"
                            runat="server"
                            CssClass="form-control"
                            AutoPostBack="true"
                            OnSelectedIndexChanged="ddlEmployee_SelectedIndexChanged" />
                    </div>

                    <!-- EMAIL -->
                    <div class="form-group">
                        <label>Email</label>
                        <asp:TextBox
                            ID="txtEmail"
                            runat="server"
                            CssClass="form-control"
                            ReadOnly="true" />
                    </div>

                    <!-- ROLE -->
                    <div class="form-group">
                        <label>Role</label>
                        <asp:DropDownList
                            ID="ddlRole"
                            runat="server"
                            CssClass="form-control">
                            <asp:ListItem Text="Employee" />
                            <asp:ListItem Text="Manager" />
                        </asp:DropDownList>
                    </div>

                    <!-- PASSWORD -->
                    <div class="form-group">
                        <label>Password</label>
                        <asp:TextBox
                            ID="txtPassword"
                            runat="server"
                            CssClass="form-control" />
                    </div>

                    <asp:Button
                        ID="btnCreateLogin"
                        runat="server"
                        Text="Create Login & Send Email"
                        CssClass="btn btn-success btn-block"
                        OnClick="btnCreateLogin_Click" />

                </div>
            </div>

        </div>
    </div>
</div>

</form>

<!-- Bootstrap JS -->
<script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
<script src="https://cdn.jsdelivr.net/npm/bootstrap@4.0.0/dist/js/bootstrap.min.js"></script>

</body>
</html>
