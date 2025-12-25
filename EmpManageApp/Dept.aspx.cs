using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EmpManageApp
{
    public partial class Dept : System.Web.UI.Page
    {
        string connStr =ConfigurationManager.ConnectionStrings["empmanage"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadGrid();
            }
        }

        private void LoadGrid()
        {
            string q = "exec FetchDept";
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlDataAdapter da = new SqlDataAdapter(q, con);

                DataTable dt = new DataTable();
                da.Fill(dt);

                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            string deptName = txtDeptName.Text.Replace("'", "''");
            string deptstatus = ddlStatus.SelectedValue;

            string q = $"exec InsertDept {deptName} ,'{deptstatus}'";
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(q, con);

                con.Open();
                cmd.ExecuteNonQuery();
            }

           
            LoadGrid();

        
            txtDeptName.Text = "";
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
            int deptid = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value);
            string deptName = ((TextBox)row.Cells[1].Controls[0]).Text.Replace("'", "''");
            string deptstatus = ((TextBox)row.Cells[2].Controls[0]).Text;

            string q = $" exec UpdateDept {deptid}, '{deptName}' ,'{deptstatus}'";

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
            int deptid = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value);

            string q = $"exec SoftDeleteDept {deptid}";
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(q, con);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            LoadGrid();
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
