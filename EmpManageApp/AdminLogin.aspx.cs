using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EmpManageApp
{
    public partial class AdminLogin : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["empmanage"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnAdminLogin_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(@"
            SELECT loginId
            FROM Login
            WHERE username=@u
              AND password=@p
              AND role='Admin'
              AND isActive=1", con);

                cmd.Parameters.AddWithValue("@u", txtAdminUser.Text.Trim());
                cmd.Parameters.AddWithValue("@p", txtAdminPass.Text.Trim());

                con.Open();
                object result = cmd.ExecuteScalar();

                if (result != null)
                {
                    Session["role"] = "Admin";
                    Response.Redirect("AdminSignup.aspx");
                }
                else
                {
                    lblError.Text = "Invalid admin credentials";
                    lblError.Visible = true;
                }
            }
        }

    }
}