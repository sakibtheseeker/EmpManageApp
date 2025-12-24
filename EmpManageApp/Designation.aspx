
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Designation.aspx.cs" Inherits="EmpManageApp.Designation" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Designation</title>

    <!-- Bootstrap CSS -->
    <link rel="stylesheet"
        href="https://cdn.jsdelivr.net/npm/bootstrap@4.0.0/dist/css/bootstrap.min.css" />
</head>
    <script src="https://code.jquery.com/jquery-3.2.1.slim.min.js"></script>
<script src="https://cdn.jsdelivr.net/npm/bootstrap@4.0.0/dist/js/bootstrap.min.js"></script>

<body>
<form id="form2" runat="server">

    <div class="container mt-3">
        <h4>Designation</h4>

        <!-- ✅ HTML BUTTON (NO POSTBACK) -->
       

        <br /><br />


        <asp:GridView ID="GridView1" runat="server"
    CssClass="table table-bordered"
    AutoGenerateColumns="false"
    DataKeyNames="deid"

    OnRowEditing="GridView1_RowEditing"
    OnRowUpdating="GridView1_RowUpdating"
    OnRowCancelingEdit="GridView1_RowCancelingEdit"
    OnRowDeleting="GridView1_RowDeleting">

    <Columns>

        <asp:BoundField DataField="deid"
            HeaderText="ID" ReadOnly="true" />

        <asp:BoundField DataField="deName"
            HeaderText="Department Name" />

        <asp:BoundField DataField="destatus"
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
                    OnClientClick="return confirm('Delete this designation?');">
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
     Add Designation
 </button>
        
    </div>


    <div class="modal fade" id="deptModal" tabindex="-1" role="dialog">
        <div class="modal-dialog modal-dialog-centered" role="document">
            <div class="modal-content">

                <div class="modal-header bg-primary text-white">
                    <h5 class="modal-title">Add Designation</h5>
                    <button type="button" class="close text-white" data-dismiss="modal">
                        <span>&times;</span>
                    </button>
                </div>

                <div class="modal-body">
                    <div class="form-group">
                        <label>Department</label>
                        <asp:DropDownList ID="ddlDept" runat="server"
                            CssClass="form-control" />
                     </div>

                <div class="modal-body">
                    <div class="form-group">
                        <label>Designation Name</label>
                        <asp:TextBox ID="txtDesignation" runat="server"
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
