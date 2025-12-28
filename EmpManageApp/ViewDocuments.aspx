<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewDocuments.aspx.cs" Inherits="EmpManageApp.ViewDocuments" %>

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

<li class="nav-item" runat="server" id="liEmp">
    <a class="nav-link" href="Emp.aspx">Employee</a>
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
    <div class="container mt-4">
        <h4 class="mb-3">Employee Documents</h4>

        <asp:GridView ID="gvDocuments"
            runat="server"
            CssClass="table table-bordered table-striped"
            AutoGenerateColumns="false">

            <Columns>
                <asp:BoundField DataField="employeeName" HeaderText="Employee" />
                <asp:BoundField DataField="docType" HeaderText="Document Type" />
                <asp:BoundField DataField="fileName" HeaderText="File Name" />

                <asp:TemplateField HeaderText="Action">
    <ItemTemplate>

        <!-- VIEW (NEW TAB) -->
        <asp:HyperLink
            runat="server"
            Text="View"
            CssClass="btn btn-sm btn-info mr-2"
            NavigateUrl='<%# ResolveUrl(Eval("filePath").ToString()) %>'
            Target="_blank" />

        <!-- DOWNLOAD -->
        <asp:LinkButton
            runat="server"
            Text="Download"
            CssClass="btn btn-sm btn-success"
            CommandArgument='<%# Eval("filePath") %>'
            OnClick="btnDownload_Click" />

    </ItemTemplate>
</asp:TemplateField>

            </Columns>

        </asp:GridView>
    </div>

</form>
</body>
</html>
