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

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT empId, role FROM Login WHERE username=@u AND password=@p AND isActive=1", con);

                cmd.Parameters.AddWithValue("@u", txtUsername.Text.Trim());
                cmd.Parameters.AddWithValue("@p", txtPassword.Text.Trim());

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    Session["role"] = dr["role"].ToString();
                    Session["empId"] = dr["empId"] == DBNull.Value ? null : dr["empId"];

                    // ROLE BASED REDIRECTION
                    string role = dr["role"].ToString();

                    if (role == "Admin")
                        Response.Redirect("Dept.aspx");
                    else
                        Response.Redirect("ApplyLeave.aspx");
                }
                else
                {
                    lblError.Text = "Invalid login";
                    lblError.Visible = true;
                }
            }
        }

    }
}
