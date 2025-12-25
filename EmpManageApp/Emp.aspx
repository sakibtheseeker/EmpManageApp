<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Emp.aspx.cs" Inherits="EmpManageApp.Emp" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Role</title>

    <!-- Bootstrap CSS -->
    <link rel="stylesheet"
        href="https://cdn.jsdelivr.net/npm/bootstrap@4.0.0/dist/css/bootstrap.min.css" />
</head>
    <script src="https://code.jquery.com/jquery-3.2.1.slim.min.js"></script>
<script src="https://cdn.jsdelivr.net/npm/bootstrap@4.0.0/dist/js/bootstrap.min.js"></script>

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
                    <a class="nav-link" href="Designation.aspx">Designation</a>
                </li>

                <li class="nav-item">
                    <a class="nav-link" href="Role.aspx">Role</a>
                </li>

                <li class="nav-item">
                    <a class="nav-link active" href="Emp.aspx">Employee</a>
                </li>

            </ul>
        </div>
    </div>
</nav>


<form id="form2" runat="server">
      <asp:HiddenField ID="hfEmpId" runat="server" />
    <div class="container mt-3">
        <h4>Employee</h4>

        <!-- ✅ HTML BUTTON (NO POSTBACK) -->
       

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
                OnClick="btnEdit_Click" />

            <asp:LinkButton runat="server"
                Text="Delete"
                CssClass="btn btn-sm btn-danger"
                CommandArgument='<%# Eval("eid") %>'
                OnClick="btnDelete_Click"
                OnClientClick="return confirm('Delete this employee?');" />
        </ItemTemplate>
    </asp:TemplateField>

</Columns>

</asp:GridView>

         <button type="button" class="btn btn-primary"
     data-toggle="modal" data-target="#deptModal">
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
        <label>Department</label>
        <asp:DropDownList ID="ddlDept" runat="server" CssClass="form-control" />
    </div>

    <div class="form-group">
        <label>Role</label>
        <asp:DropDownList ID="ddlRole" runat="server" CssClass="form-control" />
    </div>

    <div class="form-group">
        <label>Designation</label>
        <asp:DropDownList ID="ddlDesignation" runat="server" CssClass="form-control" />
    </div>

    <div class="form-group">
        <label>Manager</label>
        <asp:TextBox ID="txtManager" runat="server" CssClass="form-control" />
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

<script src="https://code.jquery.com/jquery-3.2.1.slim.min.js"></script>
<script src="https://cdn.jsdelivr.net/npm/bootstrap@4.0.0/dist/js/bootstrap.min.js"></script>

</body>
</html>

