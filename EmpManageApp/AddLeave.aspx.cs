using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EmpManageApp
{
    public partial class AddLeave : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["empmanage"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadGrid();
                LoadDepartment();
                LoadLeaveType();
            }
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
            int id = string.IsNullOrEmpty(hfDeptLeaveId.Value)
                ? 0
                : Convert.ToInt32(hfDeptLeaveId.Value);

            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmd;

                if (id == 0)
                {
                    cmd = new SqlCommand("InsertDeptLeave", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                }
                else
                {
                    cmd = new SqlCommand(
                        "UPDATE DeptLeave SET deptId=@deptId, leaveTypeId=@leaveTypeId, totalLeaves=@totalLeaves WHERE deptLeaveId=@id",
                        con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@id", id);
                }

                cmd.Parameters.AddWithValue("@deptId", ddlDept.SelectedValue);
                cmd.Parameters.AddWithValue("@leaveTypeId", ddlLeaveType.SelectedValue);
                cmd.Parameters.AddWithValue("@totalLeaves", txtTotalLeaves.Text);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            hfDeptLeaveId.Value = "";
            txtTotalLeaves.Text = "";

            LoadGrid();

            ScriptManager.RegisterStartupScript(
                this, GetType(),
                "hideModal",
                "$('#leaveModal').modal('hide'); alert('Leave saved successfully');",
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
