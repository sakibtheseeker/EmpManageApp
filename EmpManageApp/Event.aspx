<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Event.aspx.cs" Inherits="EmpManageApp.Event" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
        <!-- Bootstrap CSS -->
    <link rel="stylesheet"
        href="https://cdn.jsdelivr.net/npm/bootstrap@4.0.0/dist/css/bootstrap.min.css" />
</head>
    <script src="https://code.jquery.com/jquery-3.2.1.slim.min.js"></script>
<script src="https://cdn.jsdelivr.net/npm/bootstrap@4.0.0/dist/js/bootstrap.min.js"></script>
</head>
<body>
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
            <ul class="navbar-nav mr-auto">

                <li class="nav-item">
                    <a class="nav-link" href="Dept.aspx">Department</a>
                </li>

                <li class="nav-item">
                    <a class="nav-link " href="Designation.aspx">Designation</a>
                </li>

                <li class="nav-item">
                    <a class="nav-link " href="Role.aspx">Role</a>
                </li>

                <li class="nav-item">
                    <a class="nav-link" href="Emp.aspx">Employee</a>
                </li>


                 <li class="nav-item">
                 <a class="nav-link active" href="Event.aspx">Event</a>
                 </li>

                 <li class="nav-item">
                 <a class="nav-link" href="#">Leave</a>
                 </li>

            </ul>
        </div>
    </div>
</nav>
    <form id="form1" runat="server">
        <div>
        </div>
    </form>
</body>
</html>
