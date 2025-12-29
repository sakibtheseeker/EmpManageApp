<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="AdminLogin.aspx.cs"
    Inherits="EmpManageApp.AdminLogin" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Admin Login</title>

    <!-- Bootstrap CSS -->
    <link rel="stylesheet"
          href="https://cdn.jsdelivr.net/npm/bootstrap@4.0.0/dist/css/bootstrap.min.css" />
</head>

<body class="bg-light">

<form id="form1" runat="server">

    <div class="container">
        <div class="row justify-content-center mt-5">
            <div class="col-md-4">

                <div class="card shadow border-danger">
                    <div class="card-header bg-danger text-white text-center">
                        <h5>Admin Login</h5>
                    </div>

                    <div class="card-body">

                        <asp:Label ID="lblError"
                            runat="server"
                            CssClass="text-danger"
                            Visible="false" />

                        <div class="form-group">
                            <label>Admin Username</label>
                            <asp:TextBox ID="txtAdminUser"
                                runat="server"
                                CssClass="form-control" />
                        </div>

                        <div class="form-group">
                            <label>Password</label>
                            <asp:TextBox ID="txtAdminPass"
                                runat="server"
                                TextMode="Password"
                                CssClass="form-control" />
                        </div>

                        <asp:Button ID="btnAdminLogin"
                            runat="server"
                            Text="Login as Admin"
                            CssClass="btn btn-danger btn-block"
                            OnClick="btnAdminLogin_Click" />

                        <hr />

                        <asp:HyperLink
                            NavigateUrl="Login.aspx"
                            CssClass="btn btn-link btn-block text-center"
                            runat="server">
                            ← Back to Employee Login
                        </asp:HyperLink>

                    </div>
                </div>

            </div>
        </div>
    </div>

</form>

<!-- JS -->
<script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
<script src="https://cdn.jsdelivr.net/npm/popper.js@1.12.9/dist/umd/popper.min.js"></script>
<script src="https://cdn.jsdelivr.net/npm/bootstrap@4.0.0/dist/js/bootstrap.min.js"></script>

</body>
</html>
