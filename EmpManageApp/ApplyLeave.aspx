<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ApplyLeave.aspx.cs" Inherits="EmpManageApp.ApplyLeave" %>

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

           <li class="nav-item dropdown">
             <a class="nav-link dropdown-toggle" href="#" id="navbarDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
               Event
             </a>
             <div class="dropdown-menu" aria-labelledby="navbarDropdown">
               <a class="dropdown-item" href="EventType.aspx">Event Type</a>
               <a class="dropdown-item" href="EventCalender.aspx">Event Calender</a>
                 </div>
               </li>

             <li class="nav-item dropdown">
               <a class="nav-link active dropdown-toggle" href="#" id="navbarDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                 Leave
               </a>
               <div class="dropdown-menu" aria-labelledby="navbarDropdown">
                 <a class="dropdown-item" href="LeaveType.aspx">Leave Type</a>
                 <a class="dropdown-item" href="AddLeave.aspx">Add Leave</a>
                   <a class="dropdown-item" href="ApplyLeave.aspx">Apply Leave</a>
                   </div>
                </li>


        </ul>
        </nav>
    <<form id="form1" runat="server">

<asp:HiddenField ID="hfLeaveId" runat="server" />

<div class="container mt-4">
 

<div id="leaveBadges" class="mb-4" runat="server">
</div>

    <h4>Apply Leave</h4>

    <!-- GRID -->
    <asp:GridView ID="gvLeave"
        runat="server"
        CssClass="table table-bordered"
        AutoGenerateColumns="false"
        DataKeyNames="leaveId">

        <Columns>
            <asp:BoundField DataField="leaveTypeName" HeaderText="Leave Type" />
            <asp:BoundField DataField="fromDate" HeaderText="From Date"
                DataFormatString="{0:dd-MM-yyyy}" />
            <asp:BoundField DataField="toDate" HeaderText="To Date"
                DataFormatString="{0:dd-MM-yyyy}" />
            <asp:BoundField DataField="reason" HeaderText="Reason" />
            <asp:BoundField DataField="status" HeaderText="Status" />

            <asp:TemplateField HeaderText="Action">
                <ItemTemplate>
                    <asp:LinkButton runat="server"
                        Text="Edit"
                        CssClass="btn btn-sm btn-warning"
                        CommandArgument='<%# Eval("leaveId") %>'
                        OnClick="btnEdit_Click" />

                         <asp:LinkButton runat="server"
                            Text="Delete"
                            CssClass="btn btn-sm btn-danger"
                            CommandArgument='<%# Eval("leaveId") %>'
                            OnClick="btnDelete_Click"
                            OnClientClick="return confirm('Are you sure you want to delete this leave?');" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

    <button type="button" class="btn btn-primary"
        data-toggle="modal" data-target="#leaveModal">
        Apply Leave
    </button>
</div>

<!-- MODAL -->
<div class="modal fade" id="leaveModal" tabindex="-1">
<div class="modal-dialog modal-dialog-centered">
<div class="modal-content">

<div class="modal-header bg-primary text-white">
    <h5 class="modal-title">Apply Leave</h5>
    <button type="button" class="close text-white" data-dismiss="modal">&times;</button>
</div>

<div class="modal-body">

    <div class="form-group">
        <label>Leave Type</label>
        <asp:DropDownList ID="ddlLeaveType"
            runat="server"
            CssClass="form-control">
            <asp:ListItem Text="--Select--" Value="0" />
        </asp:DropDownList>
    </div>

    <div class="form-group">
        <label>From Date</label>
        <asp:TextBox ID="txtFromDate"
            runat="server"
            CssClass="form-control"
            TextMode="Date" />
    </div>

    <div class="form-group">
        <label>To Date</label>
        <asp:TextBox ID="txtToDate"
            runat="server"
            CssClass="form-control"
            TextMode="Date" />
    </div>

    <div class="form-group">
        <label>Reason</label>
        <asp:TextBox ID="txtReason"
            runat="server"
            CssClass="form-control"
            TextMode="MultiLine" />
    </div>

</div>

<div class="modal-footer">
    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>

    <asp:Button ID="btnSave"
        runat="server"
        Text="Submit"
        CssClass="btn btn-success"
        OnClick="btnSave_Click" />
</div>

</div>
</div>
</div>

</form>
</body>
</html>
