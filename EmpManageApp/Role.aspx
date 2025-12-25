<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Role.aspx.cs" Inherits="EmpManageApp.Role" %>

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
                    <a class="nav-link active" href="Role.aspx">Role</a>
                </li>

                <li class="nav-item">
                    <a class="nav-link" href="Emp.aspx">Employee</a>
                </li>

                 <li class="nav-item">
                 <a class="nav-link" href="Event.aspx">Event</a>
                 </li>

                 <li class="nav-item">
                 <a class="nav-link" href="#">Leave</a>
                 </li>

            </ul>
        </div>
    </div>
</nav>
<form id="form2" runat="server">

    <div class="container mt-3">
        <h4>Role</h4>

        <!-- ✅ HTML BUTTON (NO POSTBACK) -->
       

        <br /><br />


        <asp:GridView ID="GridView1" runat="server"
    CssClass="table table-bordered"
    AutoGenerateColumns="false"
    DataKeyNames="rid"

    OnRowEditing="GridView1_RowEditing"
    OnRowUpdating="GridView1_RowUpdating"
    OnRowCancelingEdit="GridView1_RowCancelingEdit"
    OnRowDeleting="GridView1_RowDeleting">

    <Columns>

        <asp:BoundField DataField="rid"
            HeaderText="ID" ReadOnly="true" />

        <asp:BoundField DataField="rName"
            HeaderText="Role Name" />

        <asp:BoundField DataField="rstatus"
            HeaderText="Status" />

   
        <asp:TemplateField HeaderText="Action">
            <ItemTemplate>
                <asp:LinkButton runat="server"
                    CommandName="Edit"
                    CssClass="btn btn-sm btn-warning mr-1">
                    Edit
                </asp:LinkButton>

                <asp:LinkButton runat="server"
                    CommandName="Delete"
                    CssClass="btn btn-sm btn-danger"
                    OnClientClick="return confirm('Delete this Role?');">
                    Delete
                </asp:LinkButton>
            </ItemTemplate>

            <EditItemTemplate>
                <asp:LinkButton runat="server"
                    CommandName="Update"
                    CssClass="btn btn-sm btn-success mr-1">
                    Update
                </asp:LinkButton>

                <asp:LinkButton runat="server"
                    CommandName="Cancel"
                    CssClass="btn btn-sm btn-secondary">
                    Cancel
                </asp:LinkButton>
            </EditItemTemplate>
        </asp:TemplateField>

    </Columns>
</asp:GridView>

         <button type="button" class="btn btn-primary"
     data-toggle="modal" data-target="#deptModal">
     Add Role
 </button>
        
    </div>


    <div class="modal fade" id="deptModal" tabindex="-1" role="dialog">
        <div class="modal-dialog modal-dialog-centered" role="document">
            <div class="modal-content">

                <div class="modal-header bg-primary text-white">
                    <h5 class="modal-title">Add Role</h5>
                    <button type="button" class="close text-white" data-dismiss="modal">
                        <span>&times;</span>
                    </button>
                </div>

                <div class="modal-body">
                    <div class="form-group">
                        <label>Role Name</label>
                        <asp:TextBox ID="txtRole" runat="server"
                            CssClass="form-control" />
                    </div>

                    <div class="form-group">
                        <label>Status</label>
                        <asp:DropDownList ID="ddlStatus" runat="server"
                            CssClass="form-control">
                            <asp:ListItem>Active</asp:ListItem>
                            <asp:ListItem>Inactive</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>

                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">
                        Close
                    </button>

                  
                    <asp:Button ID="btnSave" runat="server"
                        Text="Save" CssClass="btn btn-success"
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
