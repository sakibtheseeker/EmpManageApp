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
    public partial class Emp : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["empmanage"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadGrid();
                LoadRoleDropdown();
                LoadDeptDropdown();
                LoadDesignationDropdown();
            }
                
        }

        private void LoadGrid()
        {
            string q = "exec FetchEmp";
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlDataAdapter da = new SqlDataAdapter(q, con);

                DataTable dt = new DataTable();
                da.Fill(dt);

                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
        }

        private void LoadDeptDropdown()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlDataAdapter da = new SqlDataAdapter("exec FetchDeptForDropdown", con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlDept.DataSource = dt;
                ddlDept.DataTextField = "deptName";
                ddlDept.DataValueField = "deptid";
                ddlDept.DataBind();
            }

            ddlDept.Items.Insert(0, new ListItem("-- Select Department --", "0"));
        }

        private void LoadRoleDropdown()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlDataAdapter da = new SqlDataAdapter("exec FetchRoleForDropdown", con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlRole.DataSource = dt;
                ddlRole.DataTextField = "rName";
                ddlRole.DataValueField = "rid";
                ddlRole.DataBind();
            }

            ddlRole.Items.Insert(0, new ListItem("-- Select Role --", "0"));
        }

        private void LoadDesignationDropdown()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlDataAdapter da = new SqlDataAdapter("exec FetchDesignationForDropdown", con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlDesignation.DataSource = dt;
                ddlDesignation.DataTextField = "deName";
                ddlDesignation.DataValueField = "deid";
                ddlDesignation.DataBind();
            }

            ddlDesignation.Items.Insert(0, new ListItem("-- Select Designation --", "0"));
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
            int eid = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value);

            string q = $"exec DeleteEmp {eid}";
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
            int eid = string.IsNullOrEmpty(hfEmpId.Value)
                      ? 0
                      : Convert.ToInt32(hfEmpId.Value);

            string eName = txtEmpName.Text.Trim();
            string eContact = txtContact.Text.Trim();
            string eEmail = txtEmail.Text.Trim();
            DateTime eDOJ = Convert.ToDateTime(txtDOJ.Text);
            DateTime eDOB = Convert.ToDateTime(txtDOB.Text);

            int eRole = Convert.ToInt32(ddlRole.SelectedValue);
            int eDept = Convert.ToInt32(ddlDept.SelectedValue);
            int eDesignation = Convert.ToInt32(ddlDesignation.SelectedValue);

            string eManager = txtManager.Text.Trim();
            string eStatus = ddlStatus.SelectedValue;

            if (eRole == 0 || eDept == 0 || eDesignation == 0)
                return;

            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmd;

                if (eid == 0)
                {
                    // INSERT
                    cmd = new SqlCommand("InsertEmp", con);
                }
                else
                {
                    // UPDATE
                    cmd = new SqlCommand("UpdateEmp", con);
                    cmd.Parameters.AddWithValue("@eid", eid);
                }

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@eName", eName);
                cmd.Parameters.AddWithValue("@eContact", eContact);
                cmd.Parameters.AddWithValue("@eEmail", eEmail);
                cmd.Parameters.AddWithValue("@eDOJ", eDOJ);
                cmd.Parameters.AddWithValue("@eDOB", eDOB);
                cmd.Parameters.AddWithValue("@eRole", eRole);
                cmd.Parameters.AddWithValue("@eDept", eDept);
                cmd.Parameters.AddWithValue("@eDesignation", eDesignation);
                cmd.Parameters.AddWithValue("@eManager", eManager);
                cmd.Parameters.AddWithValue("@eStatus", eStatus);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            // reset
            hfEmpId.Value = "";
            LoadGrid();
            ClearForm();
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            int eid = Convert.ToInt32(btn.CommandArgument);

            // store eid for update
            hfEmpId.Value = eid.ToString();

            // TODO: Load employee data into modal
            LoadEmployeeForEdit(eid);

            // show modal
            ScriptManager.RegisterStartupScript(
                this, GetType(),
                "ShowModal",
                "$('#deptModal').modal('show');",
                true);
        }

        private void LoadEmployeeForEdit(int eid)
        {
            string q = "SELECT * FROM Emp WHERE eid = @eid";

            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(q, con);
                cmd.Parameters.AddWithValue("@eid", eid);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    txtEmpName.Text = dr["eName"].ToString();
                    txtContact.Text = dr["eContact"].ToString();
                    txtEmail.Text = dr["eEmail"].ToString();
                    txtDOJ.Text = Convert.ToDateTime(dr["eDOJ"]).ToString("yyyy-MM-dd");
                    txtDOB.Text = Convert.ToDateTime(dr["eDOB"]).ToString("yyyy-MM-dd");
                    txtManager.Text = dr["eManager"].ToString();
                    ddlStatus.SelectedValue = dr["eStatus"].ToString();

                    ddlRole.SelectedValue = dr["eRole"].ToString();
                    ddlDept.SelectedValue = dr["eDept"].ToString();
                    ddlDesignation.SelectedValue = dr["eDesignation"].ToString();
                }
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            int eid = Convert.ToInt32(btn.CommandArgument);

            string q = "exec DeleteEmp @eid";

            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(q, con);
                cmd.Parameters.AddWithValue("@eid", eid);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            // Refresh grid
            LoadGrid();
        }

        private void ClearForm()
        {
            txtEmpName.Text = "";
            txtContact.Text = "";
            txtEmail.Text = "";
            txtDOJ.Text = "";
            txtDOB.Text = "";
            txtManager.Text = "";

            ddlRole.SelectedIndex = 0;
            ddlDept.SelectedIndex = 0;
            ddlDesignation.SelectedIndex = 0;
            ddlStatus.SelectedIndex = 0;
        }



        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = GridView1.Rows[e.RowIndex];

            int eid = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value);

            string eName = ((TextBox)row.Cells[1].Controls[0]).Text.Replace("'", "''");
            TextBox txtContact = (TextBox)row.FindControl("txtContact");
            string eContact = txtContact.Text;
            string eEmail = ((TextBox)row.Cells[3].Controls[0]).Text.Replace("'", "''");
            string eDOJ = ((TextBox)row.Cells[4].Controls[0]).Text;
            string eDOB = ((TextBox)row.Cells[5].Controls[0]).Text;
            TextBox txtManager =(TextBox)row.FindControl("txtEditManager");

            string eManager = txtManager.Text.Replace("'", "''");

            DropDownList ddlStatus =(DropDownList)row.FindControl("ddlEditStatus");

            string eStatus = ddlStatus.SelectedValue;


            string q = $@"
        exec UpdateEmp
        {eid},
        '{eName}','{eContact}','{eEmail}',
        '{eDOJ}','{eDOB}',
        {ddlRole.SelectedValue},
        {ddlDept.SelectedValue},
        {ddlDesignation.SelectedValue},
        '{eManager}','{eStatus}'
    ";

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