using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace EmpManageApp
{
    public partial class Designation : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["empmanage"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["username"] == null)
            {
                Response.Redirect("Login.aspx");
            }
            if (!IsPostBack)
            {
                LoadGrid();
                LoadDepartmentDropdown();
            }
        }

        private void LoadGrid()
        {
            string q = "exec FetchDesignation";
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlDataAdapter da = new SqlDataAdapter(q, con);

                DataTable dt = new DataTable();
                da.Fill(dt);

                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
        }

        private void LoadDepartmentDropdown()
        {
            string q = "exec FetchDeptForDropdown";

            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlDataAdapter da = new SqlDataAdapter(q, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlDept.DataSource = dt;
                ddlDept.DataTextField = "deptName";
                ddlDept.DataValueField = "deptid";
                ddlDept.DataBind();
            }

            ddlDept.Items.Insert(0, new ListItem("- Select Department -", "0"));
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
        int deptid = Convert.ToInt32(ddlDept.SelectedValue);
        string deName = txtDesignation.Text.Replace("'", "''");
        string destatus = ddlStatus.SelectedValue;


    if (deptid == 0 || string.IsNullOrWhiteSpace(deName))
    {
       
        return;
    }

            string q = $"exec InsertDesignation {deptid} ,'{deName}','{destatus}'";
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(q, con);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            LoadGrid();

       
            ddlDept.SelectedIndex = 0;
            txtDesignation.Text = "";
            ddlStatus.SelectedIndex = 0;
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

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = GridView1.Rows[e.RowIndex];
            int deid = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value);
            string deName = ((TextBox)row.Cells[1].Controls[0]).Text.Replace("'", "''");
            string destatus = ((TextBox)row.Cells[2].Controls[0]).Text;

            string q = $" exec UpdateDesignation {deid}, '{deName}' ,'{destatus}'";

            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(q, con);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            GridView1.EditIndex = -1;
            LoadGrid();
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int deid = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value);

            string q = $"exec SoftDeleteDesignation {deid}";
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(q, con);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            LoadGrid();
        }
    }
}

