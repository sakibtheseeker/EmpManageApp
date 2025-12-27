<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="EmpManageApp.Login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>

    <!-- Bootstrap CSS -->
    <link rel="stylesheet"
          href="https://cdn.jsdelivr.net/npm/bootstrap@4.0.0/dist/css/bootstrap.min.css" />
</head>

<body class="bg-light">

<form id="form1" runat="server">

    <div class="container">
        <div class="row justify-content-center mt-5">
            <div class="col-md-4">

                <div class="card shadow">
                    <div class="card-header bg-primary text-white text-center">
                        <h5>EmpManage Login</h5>
                    </div>

                    <div class="card-body">

                        <asp:Label ID="lblError"
                            runat="server"
                            CssClass="text-danger"
                            Visible="false" />

                        <div class="form-group">
                            <label>Username</label>
                            <asp:TextBox ID="txtUsername"
                                runat="server"
                                CssClass="form-control" />
                        </div>

                        <div class="form-group">
                            <label>Password</label>
                            <asp:TextBox ID="txtPassword"
                                runat="server"
                                TextMode="Password"
                                CssClass="form-control" />
                        </div>

                        <asp:Button ID="btnLogin"
                            runat="server"
                            Text="Login"
                            CssClass="btn btn-primary btn-block"
                            OnClick="btnLogin_Click" />

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