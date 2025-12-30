using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EmpManageApp
{
    public partial class AddLeave : System.Web.UI.Page
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
                LoadDepartment();
                LoadLeaveType();
            }
        }
        private void SetNavbarByRole()
        {
            // Hide all first
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
            liLogout.Visible = true;

            // Only ADMIN is allowed here anyway
            liDept.Visible = true;
            liDesignation.Visible = true;
            liRole.Visible = true;
            liEmp.Visible = true;
            liEvent.Visible = true;

            liLeaveType.Visible = true;
            liAddLeave.Visible = true;
            liAddDocument.Visible = true;
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
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlDataAdapter da = new SqlDataAdapter("FetchDeptLeave", con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
        }

        private void LoadDepartment()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlDataAdapter da = new SqlDataAdapter("FetchDeptForDropdown", con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlDept.DataSource = dt;
                ddlDept.DataTextField = "deptName";
                ddlDept.DataValueField = "deptid";
                ddlDept.DataBind();
            }

            ddlDept.Items.Insert(0, new ListItem("-- Select Department --", "0"));
        }

        private void LoadLeaveType()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlDataAdapter da = new SqlDataAdapter("FetchLeaveType", con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlLeaveType.DataSource = dt;
                ddlLeaveType.DataTextField = "leaveTypeName";
                ddlLeaveType.DataValueField = "leaveTypeId";
                ddlLeaveType.DataBind();
            }

            ddlLeaveType.Items.Insert(0, new ListItem("-- Select Leave Type --", "0"));
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int newLeaves = Convert.ToInt32(txtTotalLeaves.Text);
            int deptLeaveId = string.IsNullOrEmpty(hfDeptLeaveId.Value)
       ? 0
       : Convert.ToInt32(hfDeptLeaveId.Value);
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand("LeaveTypeValidation", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@deptLeaveId", deptLeaveId);
                cmd.Parameters.AddWithValue("@deptId", ddlDept.SelectedValue);
                cmd.Parameters.AddWithValue("@leaveTypeId", ddlLeaveType.SelectedValue);
                cmd.Parameters.AddWithValue("@newLeaves", newLeaves);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            txtTotalLeaves.Text = "";
            hfDeptLeaveId.Value = "";

            LoadGrid();

            ScriptManager.RegisterStartupScript(
                this, GetType(),
                "success",
                "$('#leaveModal').modal('hide'); alert('Leaves updated successfully');",
                true);
        }


        protected void btnEdit_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(((LinkButton)sender).CommandArgument);
            hfDeptLeaveId.Value = id.ToString();

            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT * FROM DeptLeave WHERE deptLeaveId=@id", con);
                cmd.Parameters.AddWithValue("@id", id);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    ddlDept.SelectedValue = dr["deptId"].ToString();
                    ddlLeaveType.SelectedValue = dr["leaveTypeId"].ToString();
                    txtTotalLeaves.Text = dr["totalLeaves"].ToString();
                }
            }

            ScriptManager.RegisterStartupScript(
                this, GetType(),
                "showModal",
                "$('#leaveModal').modal('show');",
                true);
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(((LinkButton)sender).CommandArgument);

            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(
                    "UPDATE DeptLeave SET isActive=0 WHERE deptLeaveId=@id", con);
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }

            LoadGrid();
        }
    }
}
