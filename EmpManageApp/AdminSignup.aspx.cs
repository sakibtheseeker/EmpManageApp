using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.UI;

namespace EmpManageApp
{
    public partial class AdminSignup : Page
    {
        string connStr =
            ConfigurationManager.ConnectionStrings["empmanage"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["role"] == null || Session["role"].ToString() != "Admin")
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                SetNavbarByRole();
                LoadEmployeesWithoutLogin();
            }

        }

        private void SetNavbarByRole()
        {
            string role = Session["role"].ToString();

            // Hide everything first
            liDept.Visible = false;
            liDesignation.Visible = false;
            liRole.Visible = false;
            liEmp.Visible = false;
            liEvent.Visible = false;

            liLeaveType.Visible = false;
            liAddLeave.Visible = false;
            liViewDocuments.Visible = false;
            liLogout.Visible = Session["role"] != null;

            // ADMIN
            if (role == "Admin")
            {
                liDept.Visible = true;
                liDesignation.Visible = true;
                liRole.Visible = true;
                liEmp.Visible = true;
                liEvent.Visible = true;
                liLeaveType.Visible = true;
                liAddLeave.Visible = true;
                liAddDocument.Visible = true;
            }

           
        }

        private void LoadEmployeesWithoutLogin()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(@"
                    SELECT e.eid, e.eName, e.eEmail
                    FROM Emp e
                    LEFT JOIN Login l ON e.eid = l.empId
                    WHERE l.empId IS NULL
                      AND e.isActive = 1", con);

                con.Open();
                ddlEmployee.DataSource = cmd.ExecuteReader();
                ddlEmployee.DataTextField = "eName";
                ddlEmployee.DataValueField = "eid";
                ddlEmployee.DataBind();
            }

            ddlEmployee.Items.Insert(0,
                new System.Web.UI.WebControls.ListItem("-- Select Employee --", "0"));
        }

        protected void ddlEmployee_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlEmployee.SelectedValue == "0")
                return;

            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmd =
                    new SqlCommand("SELECT eEmail FROM Emp WHERE eid=@id", con);
                cmd.Parameters.AddWithValue("@id", ddlEmployee.SelectedValue);

                con.Open();
                txtEmail.Text = Convert.ToString(cmd.ExecuteScalar());
            }

            // Auto-generate password
            txtPassword.Text = Guid.NewGuid().ToString("N").Substring(0, 8);
        }

        protected void btnCreateLogin_Click(object sender, EventArgs e)
        {
            if (ddlEmployee.SelectedValue == "0")
                return;

            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(@"
            IF NOT EXISTS (SELECT 1 FROM Login WHERE empId=@empId)
            BEGIN
                INSERT INTO Login (empId, username, password, role)
                VALUES (@empId, @username, @password, 'Employee')
            END", con);

                cmd.Parameters.AddWithValue("@empId", ddlEmployee.SelectedValue);
                cmd.Parameters.AddWithValue("@username", txtEmail.Text.Trim());
                cmd.Parameters.AddWithValue("@password", txtPassword.Text.Trim());

                con.Open();
                cmd.ExecuteNonQuery();
            }

            // Send email
            SendCredentialsMail(txtEmail.Text, txtPassword.Text);

            // ✅ SUCCESS MESSAGE
            lblMessage.Text =
                "Login credentials created successfully and sent to <b>" +
                txtEmail.Text + "</b>.";
            lblMessage.Visible = true;

            // Reset form
            ddlEmployee.SelectedIndex = 0;
            txtEmail.Text = "";
            txtPassword.Text = "";

            LoadEmployeesWithoutLogin();
        }


        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();

            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();

            Response.Redirect("Login.aspx");
        }

        private void SendCredentialsMail(string toEmail, string password)
        {
            string fromEmail = ConfigurationManager.AppSettings["FROM_EMAIL"];
            string fromPassword = ConfigurationManager.AppSettings["FROM_PASSWORD"];

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(fromEmail);
            mail.To.Add(toEmail);

            mail.Subject = "EmpManage - Login Credentials";

            mail.Body =
                "Dear Employee,\n\n" +
                "Your EmpManage account has been created.\n\n" +
                "Login Details:\n" +
                "Username: " + toEmail + "\n" +
                "Password: " + password + "\n\n" +
                "Please change your password after first login.\n\n" +
                "Regards,\nEmpManage Admin";

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential(fromEmail, fromPassword);
            smtp.EnableSsl = true;
            smtp.Send(mail);
        }
    }
}
