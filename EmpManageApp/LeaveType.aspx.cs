using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EmpManageApp
{
    public partial class LeaveType : System.Web.UI.Page
    {
        string connStr =
            ConfigurationManager.ConnectionStrings["empmanage"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if(Session["empId"] == null)
{
                Response.Redirect("Login.aspx");
            }

            if (!IsPostBack)
            {
                LoadGrid();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string leaveType = txtLeaveType.Text.Trim();
            string status = ddlStatus.SelectedValue;

            int id = string.IsNullOrEmpty(hfLeaveTypeId.Value)
                ? 0
                : Convert.ToInt32(hfLeaveTypeId.Value);

            if (leaveType == "")
                return;

            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmd;

                if (id == 0)
                {
                    cmd = new SqlCommand("InsertLeaveType", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@leaveTypeName", leaveType);
                    cmd.Parameters.AddWithValue("@status", status);
                }
                else
                {
                    cmd = new SqlCommand(
                        "UPDATE LeaveType SET leaveTypeName=@leaveTypeName, status=@status WHERE leaveTypeId=@id",
                        con);

                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@leaveTypeName", leaveType);
                    cmd.Parameters.AddWithValue("@status", status);
                }

                con.Open();
                cmd.ExecuteNonQuery();
            }

            hfLeaveTypeId.Value = "";
            txtLeaveType.Text = "";

            LoadGrid();

            ScriptManager.RegisterStartupScript(
                this, GetType(),
                "hideModal",
                "$('#leaveModal').modal('hide');",
                true);
        }



        private void LoadGrid()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlDataAdapter da =
                    new SqlDataAdapter("FetchLeaveType", con);

                DataTable dt = new DataTable();
                da.Fill(dt);

                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
        }


        protected void btnEdit_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(
                ((System.Web.UI.WebControls.LinkButton)sender).CommandArgument);

            hfLeaveTypeId.Value = id.ToString();

            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmd =
                    new SqlCommand(
                        "SELECT * FROM LeaveType WHERE leaveTypeId=@id",
                        con);

                cmd.Parameters.AddWithValue("@id", id);
                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    txtLeaveType.Text = dr["leaveTypeName"].ToString();
                    ddlStatus.SelectedValue = dr["status"].ToString();
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
            int id = Convert.ToInt32(
                ((System.Web.UI.WebControls.LinkButton)sender).CommandArgument);

            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmd =
                    new SqlCommand("SoftDeleteLeaveType", con);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            LoadGrid();
        }

    }
}