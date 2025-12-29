<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddDocument.aspx.cs" Inherits="EmpManageApp.AddDocument" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
         <link rel="stylesheet"
    href="https://cdn.jsdelivr.net/npm/bootstrap@4.0.0/dist/css/bootstrap.min.css" />

<script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
<script src="https://cdn.jsdelivr.net/npm/bootstrap@4.0.0/dist/js/bootstrap.min.js"></script>

</head>
<body>
                             
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

            <a class="dropdown-item" runat="server" id="liApplyLeave" href="ApplyLeave.aspx">
                Apply Leave
            </a>

            <a class="dropdown-item" runat="server" id="liApproveLeave" href="ApproveLeave.aspx">
                Approve Leave
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

     <li class="nav-item text-danger" runat="server" id="liLogout" >
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
       <div class="card shadow-sm">
<div class="card-header bg-primary text-white text-center">
                    <h5 class="mb-0">Add Employee Document</h5>
                </div>

                <div class="card-body">

                    <!-- DOCUMENT TYPE -->
                    <div class="form-group">
                        <label>Document Type</label>
                        <asp:DropDownList
                            ID="ddlDocType"
                            runat="server"
                            CssClass="form-control form-control-sm">
                            <asp:ListItem Text="-- Select --" Value="" />
                            <asp:ListItem>Appointment Letter</asp:ListItem>
                            <asp:ListItem>Offer Letter</asp:ListItem>
                            <asp:ListItem>Confirmation Letter</asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <!-- EMPLOYEE -->
                    <div class="form-group">
                        <label>Employee</label>
                        <asp:DropDownList
                            ID="ddlEmployee"
                            runat="server"
                            CssClass="form-control form-control-sm" />
                    </div>

                    <!-- FILE -->
                    <div class="form-group">
                        <label>Upload File</label>
                        <asp:FileUpload
                            ID="fuDocument"
                            runat="server"
                            CssClass="form-control form-control-sm" />
                    </div>

                    <!-- BUTTON -->
                    <div class="text-center mt-3">
                        <asp:Button
                            ID="Button1"
                            runat="server"
                            Text="Upload"
                            CssClass="btn btn-success btn-sm px-4"
                            OnClick="btnUpload_Click" />
                    </div>
</div>
           </div>
                        </div>
        </div>
    </div>
</form>
</body>
</html>
