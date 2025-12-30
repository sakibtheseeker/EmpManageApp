using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EmpManageApp
{
    public partial class Role : System.Web.UI.Page
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
                LoadGrid();
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

            // EMPLOYEE
            else if (role == "Employee")
            {
                liApplyLeave.Visible = true;
                liDocuments.Visible = true;
                liViewDocuments.Visible = true;
            }

            // MANAGER
            else if (role == "Manager")
            {
                liApproveLeave.Visible = true;
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

        private void LoadGrid()
        {
            string q = "exec FetchRole";
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlDataAdapter da = new SqlDataAdapter(q, con);

                DataTable dt = new DataTable();
                da.Fill(dt);

                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
        }



        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            LoadGrid();
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            LoadGrid();
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int rid = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value);

            string q = $"exec DeleteRole {rid}";
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(q, con);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            LoadGrid();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string rName = txtRole.Text.Trim();
            string rStatus = ddlStatus.SelectedValue;

            if (string.IsNullOrEmpty(rName))
                return;

            int count = 0;

            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT COUNT(*) FROM Role WHERE rName = @rName", con);

                cmd.Parameters.AddWithValue("@rName", rName);

                con.Open();
                count = (int)cmd.ExecuteScalar();
            }

            if (count > 0)
            {
                // ❌ Duplicate
                ScriptManager.RegisterStartupScript(
                    this, GetType(), "dup",
                    "alert('Role already exists');", true);
                return;
            }

            // ✅ Insert
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(
                    "InsertRole", con);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@rName", rName);
                cmd.Parameters.AddWithValue("@rStatus", rStatus);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            LoadGrid();
            txtRole.Text = "";
            ddlStatus.SelectedIndex = 0;
        }


        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = GridView1.Rows[e.RowIndex];
            int rid = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value);
            string rName = ((TextBox)row.Cells[1].Controls[0]).Text.Replace("'", "''");
            string rstatus = ((TextBox)row.Cells[2].Controls[0]).Text;

            string q = $" exec UpdateRole  {rid},'{rName}' ,'{rstatus}'";

            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(q, con);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            GridView1.EditIndex = -1;
            LoadGrid();
        }
    }

}