<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ApproveLeave.aspx.cs" Inherits="EmpManageApp.ApproveLeave" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Approve Leave</title>

    <!-- Bootstrap CSS -->
    <link rel="stylesheet"
          href="https://cdn.jsdelivr.net/npm/bootstrap@4.0.0/dist/css/bootstrap.min.css" />

    <!-- jQuery -->
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>

    <!-- Bootstrap JS -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@4.0.0/dist/js/bootstrap.min.js"></script>
<script>
    function openRejectModal(leaveId) {
        $('#<%= hfRejectLeaveId.ClientID %>').val(leaveId);
        $('#rejectModal').modal('show');
    }
</script>

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
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <asp:HiddenField ID="hfRejectLeaveId" runat="server" />
  

    <div class="container mt-4">
        <h4 class="mb-3">Pending Leave Requests</h4>

        <asp:GridView ID="gvApproveLeave" runat="server"
            CssClass="table table-bordered table-striped"
            AutoGenerateColumns="false"
            DataKeyNames="leaveId"
            OnRowCommand="gvApproveLeave_RowCommand">

            <Columns>

                <asp:BoundField DataField="employeeName" HeaderText="Employee" />

                <asp:BoundField DataField="fromDate" HeaderText="From"
                    DataFormatString="{0:yyyy-MM-dd}" />

                <asp:BoundField DataField="toDate" HeaderText="To"
                    DataFormatString="{0:yyyy-MM-dd}" />

                <asp:BoundField DataField="leaveDays" HeaderText="Days" />

                <asp:BoundField DataField="reason" HeaderText="Reason" />

                <asp:TemplateField HeaderText="Action">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnApprove"
                            runat="server"
                            Text="Approve"
                            CssClass="btn btn-sm btn-success mr-2"
                            CommandName="Approve"
                            CommandArgument='<%# Eval("leaveId") %>' />

                        <asp:LinkButton ID="btnReject"
                        runat="server"
                        Text="Reject"
                        CssClass="btn btn-sm btn-danger"
                         OnClientClick='<%# "openRejectModal(" + Eval("leaveId") + "); return false;" %>' />


                    </ItemTemplate>
                </asp:TemplateField>

            </Columns>
        </asp:GridView>

    </div>
    <div class="modal fade" id="rejectModal" tabindex="-1">
    <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content">

            <div class="modal-header bg-danger text-white">
                <h5 class="modal-title">Reject Leave</h5>
                <button type="button" class="close text-white" data-dismiss="modal">
                    <span>&times;</span>
                </button>
            </div>

            <div class="modal-body">
                <div class="form-group">
                    <label>Rejection Reason</label>
                    <asp:TextBox ID="txtRejectReason"
                        runat="server"
                        CssClass="form-control"
                        TextMode="MultiLine"
                        Rows="4" />
                </div>
            </div>

            <div class="modal-footer">
                <button type="button"
                    class="btn btn-secondary"
                    data-dismiss="modal">
                    Cancel
                </button>

                <asp:Button ID="btnConfirmReject"
                    runat="server"
                    Text="Reject Leave"
                    CssClass="btn btn-danger"
                    OnClick="btnConfirmReject_Click" />
            </div>

        </div>
    </div>
</div>

</form>

</body>
</html>
