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
            if (Session["role"] == null || Session["role"].ToString() != "Admin")
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                SetNavbarByRole();
                LoadGrid();
                LoadRoleDropdown();
                LoadDeptDropdown();
                LoadManagerDropdown();
            }
            
            else
            {
                EnforceRoleRules(); // ✅ CRITICAL
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

            string q = $"exec SoftDeleteEmpWithLogin {eid}";
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
            int eRole = Convert.ToInt32(ddlRole.SelectedValue);
            // Determine role
            string roleName = ddlRole.SelectedItem.Text;

            // Manager should NEVER have a manager
            object eManager = DBNull.Value;

            if (roleName != "Manager" && ddlManager.SelectedValue != "0")
            {
                eManager = ddlManager.SelectedItem.Text;
            }


            int eDept = ddlDept.Enabled
                ? Convert.ToInt32(ddlDept.SelectedValue)
                : 0;

            int eDesignation = 0;

            if (ddlDesignation.Enabled && int.TryParse(ddlDesignation.SelectedValue, out int desig))
            {
                eDesignation = desig;
            }


            int eid = string.IsNullOrEmpty(hfEmpId.Value)
                      ? 0
                      : Convert.ToInt32(hfEmpId.Value);



            string eName = txtEmpName.Text.Trim();
            string eContact = txtContact.Text.Trim();
            string eEmail = txtEmail.Text.Trim();
            DateTime eDOJ, eDOB;

            bool isDOJValid = DateTime.TryParse(txtDOJ.Text, out eDOJ);
            bool isDOBValid = DateTime.TryParse(txtDOB.Text, out eDOB);

            if (!isDOJValid || !isDOBValid)
            {
               
                return;
            }


            string eStatus = ddlStatus.SelectedValue;

            if (eRole == 0)
                return;

            bool deptDisabled = hfDeptDisabled.Value == "1";
            bool desigDisabled = hfDesignationDisabled.Value == "1";

            if (!deptDisabled && eDept == 0)
                return;

            if (!desigDisabled && eDesignation == 0)
                return;



            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmd;

                if (eid == 0)
                {
                 
                    cmd = new SqlCommand("InsertEmp", con);
                }
                else
                {
           
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
                cmd.Parameters.AddWithValue("@eDept",
                    ddlDept.Enabled && ddlDept.SelectedValue != "0"
                        ? (object)ddlDept.SelectedValue
                        : DBNull.Value
                );
                cmd.Parameters.AddWithValue("@eDesignation",
                    ddlDesignation.Enabled && ddlDesignation.SelectedValue != "0"
                        ? (object)ddlDesignation.SelectedValue
                        : DBNull.Value
                );
                cmd.Parameters.AddWithValue("@eManager", eManager); // ✅ ONCE
                cmd.Parameters.AddWithValue("@eStatus", eStatus);


                con.Open();
                cmd.ExecuteNonQuery();
            }

            int eDept1 = ddlDept.Enabled ? Convert.ToInt32(ddlDept.SelectedValue) : 0;
            int eDesignation1 = 0;

            if (ddlDesignation.Enabled &&
                ddlDesignation.SelectedValue != "0" &&
                !string.IsNullOrEmpty(ddlDesignation.SelectedValue))
            {
                eDesignation1 = Convert.ToInt32(ddlDesignation.SelectedValue);
            }
          

  
            hfEmpId.Value = "";
            LoadGrid();
            LoadManagerDropdown();
            ClearForm();
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
         

            LinkButton btn = (LinkButton)sender;
            int eid = Convert.ToInt32(btn.CommandArgument);

            hfEmpId.Value = eid.ToString();

         
            LoadEmployeeForEdit(eid);

            ScriptManager.RegisterStartupScript(
                this,
                GetType(),
                "roleChange",
                "$('#" + ddlRole.ClientID + "').trigger('change');",
                true
            );

            ScriptManager.RegisterStartupScript(
                this, GetType(),
                "ShowModal",
                "$('#deptModal').modal('show');",
                true);
        }
        private void EnforceRoleRules()
        {
            if (ddlRole.SelectedItem == null) return;

            string role = ddlRole.SelectedItem.Text;

            // RESET ENABLE STATE
            ddlDept.Enabled = true;
            ddlDesignation.Enabled = true;
            ddlManager.Enabled = true;

            // ===== ADMIN =====
            if (role == "Admin")
            {
                ddlDept.Enabled = false;
                ddlDesignation.Enabled = false;
                ddlManager.Enabled = false;

                ddlDept.SelectedIndex = 0;

                if (ddlDesignation.Items.Count > 0)
                    ddlDesignation.SelectedIndex = 0;

                if (ddlManager.Items.Count > 0)
                    ddlManager.SelectedIndex = 0;
            }

            // ===== MANAGER =====
            else if (role == "Manager")
            {
                ddlDept.Enabled = true;
                ddlDesignation.Enabled = false;
                ddlManager.Enabled = false;

                // SAFE resets
                if (ddlDesignation.Items.Count > 0)
                    ddlDesignation.SelectedIndex = 0;

                if (ddlManager.Items.Count > 0)
                    ddlManager.SelectedIndex = 0;
            }

            // ===== EMPLOYEE =====
            else if (role == "Employee")
            {
                ddlDept.Enabled = true;
                ddlDesignation.Enabled = true;
                ddlManager.Enabled = true;
            }
        }


        protected void ddlDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            int deptId = Convert.ToInt32(ddlDept.SelectedValue);

            ddlDesignation.Items.Clear();

            // Only EMPLOYEE needs designation
            if (ddlRole.SelectedItem.Text == "Employee" && deptId > 0)
            {
                LoadDesignationByDept(deptId);
            }
            else
            {
                ddlDesignation.Items.Insert(0, new ListItem("-- Select Designation --", "0"));
            }

            // 🔐 CRITICAL: always enforce role rules
            EnforceRoleRules();

            ScriptManager.RegisterStartupScript(
                this,
                GetType(),
                "ShowModalAfterDeptChange",
                "$('#deptModal').modal('show');",
                true
            );
        }

        private void LoadEmployeeForEdit(int eid)
        {
            string q = "SELECT * FROM Emp WHERE eid = @eid AND isActive = 1";

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

                    ddlStatus.SelectedValue = dr["eStatus"].ToString();

                    // ===== ROLE =====
                    string roleVal = dr["eRole"].ToString();
                    ddlRole.SelectedValue = ddlRole.Items.FindByValue(roleVal) != null
                        ? roleVal
                        : "0";

                    /// ===== DEPARTMENT =====
                    if (dr["eDept"] != DBNull.Value)
                    {
                        string deptVal = dr["eDept"].ToString();
                        ddlDept.SelectedValue = deptVal;

                        // 🔑 IMPORTANT: load designation list FIRST
                        LoadDesignationByDept(Convert.ToInt32(deptVal));
                    }
                    else
                    {
                        ddlDept.SelectedIndex = 0;
                        ddlDesignation.Items.Clear();
                        ddlDesignation.Items.Insert(0, new ListItem("-- Select Designation --", "0"));
                    }

                    // ===== DESIGNATION =====
                    if (dr["eDesignation"] != DBNull.Value)
                    {
                        string desigVal = dr["eDesignation"].ToString();

                        // ✅ SAFE assignment
                        if (ddlDesignation.Items.FindByValue(desigVal) != null)
                        {
                            ddlDesignation.SelectedValue = desigVal;
                        }
                        else
                        {
                            ddlDesignation.SelectedIndex = 0;
                        }
                    }
                    else
                    {
                        ddlDesignation.SelectedIndex = 0;
                    }


                    // ===== MANAGER =====
                    string managerName = dr["eManager"] == DBNull.Value
                        ? ""
                        : dr["eManager"].ToString();

                    if (!string.IsNullOrEmpty(managerName))
                    {
                        ListItem item = ddlManager.Items.FindByText(managerName);
                        ddlManager.SelectedIndex = item != null
                            ? ddlManager.Items.IndexOf(item)
                            : 0;
                    }
                    else
                    {
                        ddlManager.SelectedIndex = 0;
                    }
                }
            }
            EnforceRoleRules();

        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            int eid = Convert.ToInt32(btn.CommandArgument);

            string q = "exec SoftDeleteEmpWithLogin  @eid";

            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(q, con);
                cmd.Parameters.AddWithValue("@eid", eid);

                con.Open();
                cmd.ExecuteNonQuery();
            }

      
            LoadGrid();
        }

        private void ClearForm()
        {
            txtEmpName.Text = "";
            txtContact.Text = "";
            txtEmail.Text = "";
            txtDOJ.Text = "";
            txtDOB.Text = "";

            ddlManager.SelectedIndex = 0;
            ddlRole.SelectedIndex = 0;
            ddlDept.SelectedIndex = 0;
            ddlDesignation.SelectedIndex = 0;
            ddlStatus.SelectedIndex = 0;
        }


        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = GridView1.Rows[e.RowIndex];
            int eid = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value);

            string eName = ((TextBox)row.Cells[1].Controls[0]).Text;
            string eContact = ((TextBox)row.FindControl("txtContact")).Text;
            string eEmail = ((TextBox)row.Cells[3].Controls[0]).Text;
            DateTime eDOJ = Convert.ToDateTime(((TextBox)row.Cells[4].Controls[0]).Text);
            DateTime eDOB = Convert.ToDateTime(((TextBox)row.Cells[5].Controls[0]).Text);
            string eManager = ddlManager.SelectedItem.Text;
            string eStatus = ((DropDownList)row.FindControl("ddlEditStatus")).SelectedValue;

            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand("UpdateEmp", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@eid", eid);
                cmd.Parameters.AddWithValue("@eName", eName);
                cmd.Parameters.AddWithValue("@eContact", eContact);
                cmd.Parameters.AddWithValue("@eEmail", eEmail);
                cmd.Parameters.AddWithValue("@eDOJ", eDOJ);
                cmd.Parameters.AddWithValue("@eDOB", eDOB);
                cmd.Parameters.AddWithValue("@eRole", ddlRole.SelectedValue);
                cmd.Parameters.AddWithValue("@eDept", ddlDept.SelectedValue);
                cmd.Parameters.AddWithValue("@eDesignation", ddlDesignation.SelectedValue);
                cmd.Parameters.AddWithValue("@eManager", eManager);
                cmd.Parameters.AddWithValue("@eStatus", eStatus);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            GridView1.EditIndex = -1;
            LoadGrid();
        }


        private void LoadManagerDropdown()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlDataAdapter da = new SqlDataAdapter("FetchManagers", con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlManager.DataSource = dt;
                ddlManager.DataTextField = "eName";
                ddlManager.DataValueField = "eid";
                ddlManager.DataBind();
            }

            ddlManager.Items.Insert(0, new ListItem("-- Select Manager --", "0"));
        }

        private void LoadDesignationByDept(int deptId)
        {
            ddlDesignation.Items.Clear();

            if (deptId == 0)
            {
                ddlDesignation.Items.Insert(0, new ListItem("-- Select Designation --", "0"));
                return;
            }

            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand("FetchDesignationByDept", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@deptid", deptId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlDesignation.DataSource = dt;
                ddlDesignation.DataTextField = "deName";
                ddlDesignation.DataValueField = "deid";
                ddlDesignation.DataBind();
            }

            ddlDesignation.Items.Insert(0, new ListItem("-- Select Designation --", "0"));
        }


    }
}

