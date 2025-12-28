<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Emp.aspx.cs" Inherits="EmpManageApp.Emp" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Role</title>

    <!-- Bootstrap CSS -->

    <link rel="stylesheet"
          href="https://cdn.jsdelivr.net/npm/bootstrap@4.0.0/dist/css/bootstrap.min.css" />

    <!-- ✅ jQuery FIRST -->
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>


    <!-- ✅ Bootstrap JS -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@4.0.0/dist/js/bootstrap.min.js"></script>

    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.12.9/umd/popper.min.js"></script>
</head>


    <script>
        $(document).ready(function () {

            function handleRoleChange() {
                var roleText = $("#<%= ddlRole.ClientID %> option:selected").text();

        // ENABLE ALL FIRST
        $("#<%= ddlDept.ClientID %>").prop("disabled", false);
        $("#<%= ddlDesignation.ClientID %>").prop("disabled", false);
        $("#<%= ddlManager.ClientID %>").prop("disabled", false);

        $("#<%= hfDeptDisabled.ClientID %>").val("0");
        $("#<%= hfDesignationDisabled.ClientID %>").val("0");

        // ===== ADMIN =====
        if (roleText === "Admin") {

            $("#<%= ddlDept.ClientID %>").val("0");
            $("#<%= ddlDesignation.ClientID %>").val("0");
            $("#<%= ddlManager.ClientID %>").val("0");

            $("#<%= ddlDept.ClientID %>").prop("disabled", true);
            $("#<%= ddlDesignation.ClientID %>").prop("disabled", true);
            $("#<%= ddlManager.ClientID %>").prop("disabled", true);

            $("#<%= hfDeptDisabled.ClientID %>").val("1");
            $("#<%= hfDesignationDisabled.ClientID %>").val("1");
        }

        // ===== MANAGER =====
        else if (roleText === "Manager") {

            $("#<%= ddlDesignation.ClientID %>").val("0");
            $("#<%= ddlManager.ClientID %>").val("0");

            $("#<%= ddlDesignation.ClientID %>").prop("disabled", true);
            $("#<%= ddlManager.ClientID %>").prop("disabled", true);

            $("#<%= hfDesignationDisabled.ClientID %>").val("1");
        }
    }

    $("#<%= ddlRole.ClientID %>").on("change", handleRoleChange);
});
    </script>




  <script>
      function openAddEmpModal() {

         
          $("#<%= hfEmpId.ClientID %>").val("");

    
    $("#<%= txtEmpName.ClientID %>").val("");
    $("#<%= txtContact.ClientID %>").val("");
    $("#<%= txtEmail.ClientID %>").val("");
    $("#<%= txtDOJ.ClientID %>").val("");
    $("#<%= txtDOB.ClientID %>").val("");
    $("#<%= ddlManager.ClientID %>").val("0");


    $("#<%= ddlRole.ClientID %>").val("0").trigger("change");
    $("#<%= ddlDept.ClientID %>").val("0");
    $("#<%= ddlDesignation.ClientID %>").val("0");
    $("#<%= ddlStatus.ClientID %>").val("Active");

          $("#deptModal").modal("show");
      }
  </script>

    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>

<script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.12.9/umd/popper.min.js"></script>

<script src="https://cdn.jsdelivr.net/npm/bootstrap@4.0.0/dist/js/bootstrap.min.js"></script>


<body>

<form id="form2" runat="server">
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
      <asp:HiddenField ID="hfEmpId" runat="server" />
    <asp:HiddenField ID="hfDeptDisabled" runat="server" />
<asp:HiddenField ID="hfDesignationDisabled" runat="server" />

    <div class="container mt-3">
        <h4>Employee</h4>

    
       

        <br /><br />


        <asp:GridView ID="GridView1" runat="server"
    CssClass="table table-bordered"
    AutoGenerateColumns="false"
    DataKeyNames="eid">

    <Columns>

    <asp:BoundField DataField="eid" HeaderText="ID" />
    <asp:BoundField DataField="eName" HeaderText="Name" />
    <asp:BoundField DataField="eContact" HeaderText="Contact" />
    <asp:BoundField DataField="eEmail" HeaderText="Email" />
    <asp:BoundField DataField="eDOJ" HeaderText="DOJ"
        DataFormatString="{0:yyyy-MM-dd}" />
    <asp:BoundField DataField="eDOB" HeaderText="DOB"
        DataFormatString="{0:yyyy-MM-dd}" />
    <asp:BoundField DataField="eManager" HeaderText="Manager" />

    <asp:BoundField DataField="eStatus" HeaderText="Status" />

       <asp:TemplateField HeaderText="Action">
        <ItemTemplate>
            <asp:LinkButton runat="server"
                Text="Edit"
                CssClass="btn btn-sm btn-warning mr-1"
                
                CommandArgument='<%# Eval("eid") %>'
                CausesValidation="false"
UseSubmitBehavior="false"

                OnClick="btnEdit_Click" />

            <asp:LinkButton runat="server"
                Text="Delete"
                CssClass="btn btn-sm btn-danger"
                CommandArgument='<%# Eval("eid") %>'
                OnClick="btnDelete_Click"
                CausesValidation="false"
UseSubmitBehavior="false"

                OnClientClick="return confirm('Delete this employee?');" />
        </ItemTemplate>
    </asp:TemplateField>

</Columns>

</asp:GridView>

   <button type="button" class="btn btn-primary"
        onclick="openAddEmpModal()">
    Add Emp
</button>

        
    </div>


    <div class="modal fade" id="deptModal" tabindex="-1" role="dialog">
        <div class="modal-dialog modal-dialog-centered" role="document">
            <div class="modal-content">


                <div class="modal-header bg-primary text-white">
                    <h5 class="modal-title">Add Emp</h5>
                    <button type="button" class="close text-white" data-dismiss="modal">
                        <span>&times;</span>
                    </button>
                </div>

                <div class="modal-body">

    <div class="form-group">
        <label>Name</label>
        <asp:TextBox ID="txtEmpName" runat="server" CssClass="form-control" />
    </div>

    <div class="form-group">
        <label>Contact</label>
        <asp:TextBox ID="txtContact" runat="server" CssClass="form-control" />
    </div>

    <div class="form-group">
        <label>Email</label>
        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" />
    </div>

    <div class="form-group">
        <label>Date of Joining</label>
        <asp:TextBox ID="txtDOJ" runat="server"
            TextMode="Date" CssClass="form-control" />
    </div>

    <div class="form-group">
        <label>Date of Birth</label>
        <asp:TextBox ID="txtDOB" runat="server"
            TextMode="Date" CssClass="form-control" />
    </div>

    <div class="form-group">
        <label>Role</label>
        <asp:DropDownList ID="ddlRole" runat="server" CssClass="form-control" />
    </div>

    <div class="form-group">
    <label>Department</label>
    <asp:DropDownList 
    ID="ddlDept"
    runat="server"
    CssClass="form-control"
    AutoPostBack="true"
    OnSelectedIndexChanged="ddlDept_SelectedIndexChanged" />
</div>

    <div class="form-group">
        <label>Designation</label>
        <asp:DropDownList ID="ddlDesignation" runat="server" CssClass="form-control" />
    </div>

   <div class="form-group">
    <label>Manager</label>
    <asp:DropDownList ID="ddlManager" runat="server" CssClass="form-control" />
</div>


    <div class="form-group">
        <label>Status</label>
        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
            <asp:ListItem>Active</asp:ListItem>
            <asp:ListItem>Inactive</asp:ListItem>
        </asp:DropDownList>
    </div>

                    <div class="modal-footer">
    <button type="button"
        class="btn btn-secondary"
        data-dismiss="modal">
        Close
    </button>

    <asp:Button ID="btnSave"
        runat="server"
        Text="Save"
        CssClass="btn btn-success"
        OnClick="btnSave_Click" />
</div>

</div>
        </div>
    </div>

</form>

    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
<script src="https://cdn.jsdelivr.net/npm/bootstrap@4.0.0/dist/js/bootstrap.min.js"></script>

</body>
</html>

