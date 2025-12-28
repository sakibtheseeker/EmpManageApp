using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EmpManageApp
{
    public partial class EventType : System.Web.UI.Page
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
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlDataAdapter da = new SqlDataAdapter("FetchEventType", con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string eventTypeName = txtEventType.Text.Trim();
            string status = ddlEventStatus.SelectedValue;
            string colorCode = txtColor.Text;

            if (string.IsNullOrEmpty(eventTypeName))
                return;

            int eventTypeId = 0;
            int.TryParse(hfEventTypeId.Value, out eventTypeId);

            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmd;

                if (eventTypeId == 0)
                {
               
                    cmd = new SqlCommand("InsertEventType", con);
                }
                else
                {
             
                    cmd = new SqlCommand("UpdateEventType", con);
                    cmd.Parameters.AddWithValue("@eventTypeId", eventTypeId);
                }

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@eventTypeName", eventTypeName);
                cmd.Parameters.AddWithValue("@status", status);
                cmd.Parameters.AddWithValue("@colorCode", colorCode);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            hfEventTypeId.Value = "";
            txtEventType.Text = "";
            txtColor.Text = "#000000";

            LoadGrid();
        }


        protected void btnDelete_Click(object sender, EventArgs e)
        {
            int eventTypeId = Convert.ToInt32(
                ((System.Web.UI.WebControls.LinkButton)sender).CommandArgument);

            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand("UPDATE EventType SET isActive=0 WHERE eventTypeId=@id", con);
                cmd.Parameters.AddWithValue("@id", eventTypeId);
                con.Open();
                cmd.ExecuteNonQuery();
            }

            LoadGrid();
        }


        protected void btnEdit_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            int eventTypeId = Convert.ToInt32(btn.CommandArgument);

            hfEventTypeId.Value = eventTypeId.ToString();

            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT * FROM EventType WHERE eventTypeId=@id", con);
                cmd.Parameters.AddWithValue("@id", eventTypeId);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    txtEventType.Text = dr["eventTypeName"].ToString();
                    ddlEventStatus.SelectedValue = dr["status"].ToString();
                    txtColor.Text = dr["colorCode"].ToString();
                }
            }

      
            ScriptManager.RegisterStartupScript(
                this,
                GetType(),
                "ShowEventModal",
                "$('#eventModal').modal('show');",
                true
            );
        }


    }
}