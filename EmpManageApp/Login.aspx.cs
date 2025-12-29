using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace EmpManageApp
{
    public partial class Login : Page
    {
        string connStr =
            ConfigurationManager.ConnectionStrings["empmanage"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
                lblError.Visible = false;
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand("ValidateLogin", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@username", txtUsername.Text.Trim());
                cmd.Parameters.AddWithValue("@password", txtPassword.Text.Trim());

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    string role = dr["role"].ToString();

                    // ❌ BLOCK ADMIN HERE
                    if (role == "Admin")
                    {
                        lblError.Text = "Admin login is not allowed here.";
                        lblError.Visible = true;
                        return;
                    }

                    Session["role"] = role;
                    Session["empId"] = dr["empId"];

                    if (role == "Employee")
                        Response.Redirect("ApplyLeave.aspx");
                    else if (role == "Manager")
                        Response.Redirect("ApproveLeave.aspx");
                }
                else
                {
                    lblError.Text = "Invalid credentials";
                    lblError.Visible = true;
                }
            }
        }


        protected void btnAdminSignup_Click(object sender, EventArgs e)
        {
            // Safety check (even though button is admin-only)
            if (Session["role"] == null || Session["role"].ToString() != "Admin")
            {
                Response.Redirect("Login.aspx");
                return;
            }

            Response.Redirect("AdminSignup.aspx");
        }

        protected void btnAdminLogin_Click(object sender, EventArgs e)
        {
            Response.Redirect("AdminLogin.aspx");
        }

    }
}
