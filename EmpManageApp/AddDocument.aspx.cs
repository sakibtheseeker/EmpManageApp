using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EmpManageApp
{
    public partial class AddDocument : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["empmanage"].ConnectionString;

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
                LoadEmployees();
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
            liApplyLeave.Visible = false;
            liApproveLeave.Visible = false;
            liLogout.Visible = Session["role"] != null;

            liDocuments.Visible = false;
            liAddDocument.Visible = false;
            liViewDocuments.Visible = false;

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
                liDocuments.Visible = true;
                liAddDocument.Visible = true;
            }

            // EMPLOYEE
            else if (role == "Employee")
            {
                liApplyLeave.Visible = true;
            }

            // MANAGER
            else if (role == "Manager")
            {
                liApproveLeave.Visible = true;
                liDocuments.Visible = true;
                liViewDocuments.Visible = true;
            }
        }
        protected void btnLogout_Click(object sender, EventArgs e)
        {
            // Clear all session data
            Session.Clear();
            Session.Abandon();

            // Extra safety: prevent back navigation
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();

            Response.Redirect("Login.aspx");
        }
        private void LoadEmployees()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT eid, eName FROM Emp WHERE isActive = 1", con);

                con.Open();
                ddlEmployee.DataSource = cmd.ExecuteReader();
                ddlEmployee.DataTextField = "eName";
                ddlEmployee.DataValueField = "eid";
                ddlEmployee.DataBind();
            }

            ddlEmployee.Items.Insert(0, new ListItem("-- Select Employee --", "0"));
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {

            if (!fuDocument.HasFile)
                return;

            string docType = ddlDocType.SelectedValue;
            int empId = Convert.ToInt32(ddlEmployee.SelectedValue);

            // Create emp-wise folder
            string basePath = Server.MapPath("~/EmployeeDocuments/");
            string empFolder = Path.Combine(basePath, empId.ToString());

            // Ensure folder exists
            if (!Directory.Exists(empFolder))
            {
                Directory.CreateDirectory(empFolder);
            }

            // Save file
            string fileName = Path.GetFileName(fuDocument.FileName);

            string ext = Path.GetExtension(fileName).ToLower(); if (ext != ".pdf" && ext != ".jpg" && ext != ".png")
            {
                ClientScript.RegisterStartupScript(
                    GetType(),
                    "invalidFile",
                    "alert('Only PDF, JPG and PNG files are allowed');",
                    true
                );
                return;

            }
                string fullPath = Path.Combine(empFolder, fileName);

            fuDocument.SaveAs(fullPath);

            // Store relative path in DB
            string dbPath = $"~/EmployeeDocuments/{empId}/{fileName}";



            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand("InsertEmpDocument", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@empId", empId);
                cmd.Parameters.AddWithValue("@docType", ddlDocType.SelectedValue);
                cmd.Parameters.AddWithValue("@fileName", fileName);
                cmd.Parameters.AddWithValue("@filePath", dbPath);
                cmd.Parameters.AddWithValue("@uploadedBy", Convert.ToInt32(Session["empId"]));

                con.Open();
                cmd.ExecuteNonQuery();
            }



            // Optional success message
            ClientScript.RegisterStartupScript(
                GetType(),
                "success",
                "alert('Document uploaded successfully');",
                true
            );
        }


    }
}