<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LeaveType.aspx.cs" Inherits="EmpManageApp.LeaveType" %>

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
                       </div>
                    </li>


            </ul>
        </div>
    </div>
</nav>
  

   

    <form id="form1" runat="server">

    <asp:HiddenField ID="hfLeaveTypeId" runat="server" />

    <div class="container mt-4">
        <h4>Leave Type</h4>

        <asp:GridView ID="GridView1"
            runat="server"
            CssClass="table table-bordered"
            AutoGenerateColumns="false"
            DataKeyNames="leaveTypeId">

            <Columns>
                <asp:BoundField DataField="leaveTypeName" HeaderText="Leave Type" />
                <asp:BoundField DataField="status" HeaderText="Status" />

                <asp:TemplateField HeaderText="Action">
                    <ItemTemplate>
                        <asp:LinkButton runat="server"
                            Text="Edit"
                            CssClass="btn btn-sm btn-warning mr-1"
                            CommandArgument='<%# Eval("leaveTypeId") %>'
                            OnClick="btnEdit_Click" />

                        <asp:LinkButton runat="server"
                            Text="Delete"
                            CssClass="btn btn-sm btn-danger"
                            CommandArgument='<%# Eval("leaveTypeId") %>'
                            OnClick="btnDelete_Click"
                            OnClientClick="return confirm('Delete this leave type?');" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>

        <button type="button" class="btn btn-primary"
            data-toggle="modal" data-target="#leaveModal">
            Add Leave Type
        </button>
    </div>

    <!-- MODAL -->
    <div class="modal fade" id="leaveModal" tabindex="-1">
    <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content">

            <div class="modal-header bg-primary text-white">
                <h5 class="modal-title">Leave Type</h5>
                <button type="button" class="close text-white"
                        data-dismiss="modal">&times;</button>
            </div>

            <div class="modal-body">

                <div class="form-group">
                    <label>Leave Type</label>
                    <asp:TextBox 
                        ID="txtLeaveType"
                        runat="server"
                        CssClass="form-control" />
                </div>

                <div class="form-group">
                    <label>Status</label>
                    <asp:DropDownList 
                        ID="ddlStatus"
                        runat="server"
                        CssClass="form-control">
                        <asp:ListItem>Active</asp:ListItem>
                        <asp:ListItem>Inactive</asp:ListItem>
                    </asp:DropDownList>
                </div>

            </div>

            <div class="modal-footer">
                <button type="button"
                    class="btn btn-secondary"
                    data-dismiss="modal">Close</button>

                <asp:Button 
                    ID="btnSave"
                    runat="server"
                    Text="Save"
                    CssClass="btn btn-success"
                    OnClick="btnSave_Click" />
            </div>

        </div>
    </div>
</div>

</form>

</body>
</html>