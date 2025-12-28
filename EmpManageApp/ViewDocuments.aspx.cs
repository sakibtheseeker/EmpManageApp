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
    public partial class ViewDocuments : System.Web.UI.Page
    {
        string connStr = ConfigurationManager
            .ConnectionStrings["empmanage"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            // 🔐 MANAGER ONLY
            if (Session["role"] == null || Session["role"].ToString() != "Manager")
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                SetNavbarByRole();
                LoadDocuments();
            }
        }

        private void LoadDocuments()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlDataAdapter da =
                    new SqlDataAdapter("FetchEmpDocuments", con);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;

                DataTable dt = new DataTable();
                da.Fill(dt);

                gvDocuments.DataSource = dt;
                gvDocuments.DataBind();
            }
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            string relativePath =
                ((System.Web.UI.WebControls.LinkButton)sender).CommandArgument;

            string filePath = Server.MapPath(relativePath);

            if (!File.Exists(filePath))
                return;

            Response.Clear();
            Response.ContentType = "application/octet-stream";
            Response.AppendHeader("Content-Disposition",
                "attachment; filename=" + Path.GetFileName(filePath));
            Response.TransmitFile(filePath);
            Response.End();
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
            liAddDocument.Visible = false;
            liViewDocuments.Visible = false;

            liLogout.Visible = true;

            // MANAGER
            if (role == "Manager")
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
    }
}
