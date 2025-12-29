using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EmpManageApp
{
    public partial class ApplyLeave : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["empmanage"].ConnectionString;
        int EmpId => Convert.ToInt32(Session["empId"]);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["role"] == null || Session["role"].ToString() != "Employee")
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                // ✅ SAFE TO CALL ALWAYS
                SetNavbarByRole();
                EnsureLeaveBalance();
                LoadLeaveTypes();
                LoadLeaveBadges();
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

            liDocuments.Visible = false;

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

                // Admin can ADD document
                liDocuments.Visible = true;
             
            }

            // EMPLOYEE
            else if (role == "Employee")
            {
                liApplyLeave.Visible = true;

                // Employee can VIEW documents
                liDocuments.Visible = true;
                liViewDocuments.Visible = true;
            }

            // MANAGER
            else if (role == "Manager")
            {
                liApproveLeave.Visible = true;

                // ❌ Manager should NOT see documents
                // (intentionally left blank)
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

        /* CREATE BALANCE IF NOT EXISTS */
        void EnsureLeaveBalance()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(@"
        DECLARE @deptId INT

        SELECT @deptId = eDept
        FROM Emp
        WHERE eid = @empId

        INSERT INTO EmpLeaveBalance (empId, leaveTypeId, totalLeaves, usedLeaves)
        SELECT 
            @empId,
            dl.leaveTypeId,
            dl.totalLeaves,
            0
        FROM DeptLeave dl
        WHERE dl.deptId = @deptId
        AND NOT EXISTS (
            SELECT 1
            FROM EmpLeaveBalance b
            WHERE b.empId = @empId
              AND b.leaveTypeId = dl.leaveTypeId
        )", con);

                cmd.Parameters.AddWithValue("@empId", EmpId);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }



        void LoadLeaveTypes()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT leaveTypeId, leaveTypeName FROM LeaveType WHERE isActive=1", con);

                con.Open();
                ddlLeaveType.DataSource = cmd.ExecuteReader();
                ddlLeaveType.DataTextField = "leaveTypeName";
                ddlLeaveType.DataValueField = "leaveTypeId";
                ddlLeaveType.DataBind();
            }

            ddlLeaveType.Items.Insert(0, new ListItem("--Select--", "0"));
        }

        void LoadLeaveBadges()
        {

            leaveBadges.InnerHtml = ""; // clear old badges

            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(@"
            SELECT lt.leaveTypeName,
                   (b.totalLeaves - b.usedLeaves) AS availableLeaves
            FROM EmpLeaveBalance b
            JOIN LeaveType lt ON b.leaveTypeId = lt.leaveTypeId
            WHERE b.empId = @empId", con);

                cmd.Parameters.AddWithValue("@empId", EmpId);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                string[] badgeColors = { "primary", "success", "warning", "info", "danger" };
                int i = 0;

                while (dr.Read())
                {
                    string leaveType = dr["leaveTypeName"].ToString();
                    int available = Convert.ToInt32(dr["availableLeaves"]);

                    string color = badgeColors[i % badgeColors.Length];
                    i++;

                    leaveBadges.InnerHtml += $@"
                <span class='badge badge-{color} p-3 mr-2 mb-2' style='font-size:14px;'>
                    {leaveType}: <strong>{available}</strong>
                </span>";
                }
            }
        }


        void LoadLeaveGrid()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(@"
                SELECT l.leaveId, lt.leaveTypeName,
                       l.fromDate, l.toDate, l.reason, l.status
                FROM EmpLeave l
                JOIN LeaveType lt ON l.leaveTypeId=lt.leaveTypeId
                WHERE l.empId=@empId", con);

                cmd.Parameters.AddWithValue("@empId", EmpId);

                con.Open();
                gvLeave.DataSource = cmd.ExecuteReader();
                gvLeave.DataBind();
            }
        }

        bool HasOverlappingLeave(DateTime from, DateTime to, int leaveId = 0)
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(@"
            SELECT COUNT(*)
            FROM EmpLeave
            WHERE empId = @empId
              AND leaveId <> @leaveId
              AND status <> 'Rejected'
              AND @fromDate <= toDate
              AND @toDate >= fromDate", con);

                cmd.Parameters.AddWithValue("@empId", EmpId);
                cmd.Parameters.AddWithValue("@leaveId", leaveId); // 0 for new
                cmd.Parameters.AddWithValue("@fromDate", from);
                cmd.Parameters.AddWithValue("@toDate", to);

                con.Open();
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        int CalculateWorkingDays(DateTime start, DateTime end)
        {
            int days = 0;

            for (DateTime date = start; date <= end; date = date.AddDays(1))
            {
                // Skip Saturday & Sunday
                if (date.DayOfWeek != DayOfWeek.Saturday &&
                    date.DayOfWeek != DayOfWeek.Sunday)
                {
                    days++;
                }
            }

            return days;
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            int leaveTypeId;
            if (!int.TryParse(ddlLeaveType.SelectedValue, out leaveTypeId) || leaveTypeId == 0)
                return;

            // 🔹 Detect NEW vs EDIT
            bool isNewLeave = string.IsNullOrEmpty(hfLeaveId.Value);

            DateTime fromDate = Convert.ToDateTime(txtFromDate.Text);
            DateTime toDate = Convert.ToDateTime(txtToDate.Text);

            if (toDate < fromDate)
            {
                ScriptManager.RegisterStartupScript(
                    this, GetType(), "dateErr",
                    "alert('To Date cannot be earlier than From Date');", true);
                return;
            }

            int leaveDays = CalculateWorkingDays(fromDate, toDate);
            // 🚫 Block weekend-only leave
            if (leaveDays <= 0)
            {
                ScriptManager.RegisterStartupScript(
                    this, GetType(), "noWorkDay",
                    "alert('Selected date range does not contain any working days');", true);
                return;
            }
            // 🔹 Check available balance
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmdCheck = new SqlCommand(@"
            SELECT (totalLeaves - usedLeaves)
            FROM EmpLeaveBalance
            WHERE empId=@empId AND leaveTypeId=@leaveTypeId", con);

                cmdCheck.Parameters.AddWithValue("@empId", EmpId);
                cmdCheck.Parameters.AddWithValue("@leaveTypeId", leaveTypeId);

                con.Open();
                int available = Convert.ToInt32(cmdCheck.ExecuteScalar());

                if (isNewLeave && available < leaveDays)
                {
                    ScriptManager.RegisterStartupScript(
                        this, GetType(), "noLeave",
                        "alert('Not enough leave balance');", true);
                    return;
                }
            }

            if (HasOverlappingLeave(fromDate, toDate,
        string.IsNullOrEmpty(hfLeaveId.Value) ? 0 : Convert.ToInt32(hfLeaveId.Value)))
            {
                ScriptManager.RegisterStartupScript(
                    this, GetType(), "overlap",
                    "alert('You already have a leave applied for this date range');", true);
                return;
            }

            // 🔹 INSERT or UPDATE leave
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmd;

                if (isNewLeave)
                {
                    cmd = new SqlCommand(@"
            INSERT INTO EmpLeave
            (empId, leaveTypeId, fromDate, toDate, reason, status, appliedOn)
            VALUES
            (@empId, @leaveTypeId, @fromDate, @toDate, @reason, 'Pending', GETDATE())", con);
                }
                else
                {
                    // 🔹 Fetch OLD dates to calculate delta
                    DateTime oldFrom, oldTo;

                    using (SqlConnection con2 = new SqlConnection(connStr))
                    {
                        SqlCommand cmdOld = new SqlCommand(@"
                SELECT fromDate, toDate
                FROM EmpLeave
                WHERE leaveId=@leaveId AND empId=@empId", con2);

                        cmdOld.Parameters.AddWithValue("@leaveId", hfLeaveId.Value);
                        cmdOld.Parameters.AddWithValue("@empId", EmpId);

                        con2.Open();
                        SqlDataReader dr = cmdOld.ExecuteReader();
                        dr.Read();

                        oldFrom = Convert.ToDateTime(dr["fromDate"]);
                        oldTo = Convert.ToDateTime(dr["toDate"]);
                    }

                    int oldDays = CalculateWorkingDays(oldFrom, oldTo);
                    int newDays = CalculateWorkingDays(fromDate, toDate);
                    int diffDays = newDays - oldDays;

                    // 🔒 Validate available balance during EDIT
                    using (SqlConnection conChk = new SqlConnection(connStr))
                    {
                        SqlCommand cmdAvail = new SqlCommand(@"
        SELECT (totalLeaves - usedLeaves)
        FROM EmpLeaveBalance
        WHERE empId=@empId AND leaveTypeId=@leaveTypeId", conChk);

                        cmdAvail.Parameters.AddWithValue("@empId", EmpId);
                        cmdAvail.Parameters.AddWithValue("@leaveTypeId", leaveTypeId);

                        conChk.Open();
                        int available = Convert.ToInt32(cmdAvail.ExecuteScalar());

                        // ❌ Not enough balance for extension
                        if (diffDays > 0 && available < diffDays)
                        {
                            ScriptManager.RegisterStartupScript(
                                this, GetType(), "editErr",
                                "alert('Not enough leave balance to extend leave duration');", true);
                            return;
                        }
                    }

                    // 🔹 Adjust balance ONLY if days changed
                    if (diffDays != 0)
                    {
                        using (SqlConnection con3 = new SqlConnection(connStr))
                        {
                            SqlCommand cmdAdj = new SqlCommand(@"
                    UPDATE EmpLeaveBalance
                    SET usedLeaves = usedLeaves + @diff
                    WHERE empId=@empId AND leaveTypeId=@leaveTypeId", con3);

                            cmdAdj.Parameters.AddWithValue("@empId", EmpId);
                            cmdAdj.Parameters.AddWithValue("@leaveTypeId", leaveTypeId);
                            cmdAdj.Parameters.AddWithValue("@diff", diffDays);

                            con3.Open();
                            cmdAdj.ExecuteNonQuery();
                        }
                    }

                    // 🔹 Update leave record
                    cmd = new SqlCommand(@"
            UPDATE EmpLeave
            SET leaveTypeId=@leaveTypeId,
                fromDate=@fromDate,
                toDate=@toDate,
                reason=@reason
            WHERE leaveId=@leaveId AND empId=@empId", con);

                    cmd.Parameters.AddWithValue("@leaveId", hfLeaveId.Value);
                }

                cmd.Parameters.AddWithValue("@empId", EmpId);
                cmd.Parameters.AddWithValue("@leaveTypeId", leaveTypeId);
                cmd.Parameters.AddWithValue("@fromDate", fromDate);
                cmd.Parameters.AddWithValue("@toDate", toDate);
                cmd.Parameters.AddWithValue("@reason", txtReason.Text);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            // 🔹 UPDATE balance ONLY for NEW leave
            if (isNewLeave)
            {
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    SqlCommand cmdUpdate = new SqlCommand(@"
                UPDATE EmpLeaveBalance
                SET usedLeaves = usedLeaves + @days
                WHERE empId=@empId AND leaveTypeId=@leaveTypeId", con);

                    cmdUpdate.Parameters.AddWithValue("@empId", EmpId);
                    cmdUpdate.Parameters.AddWithValue("@leaveTypeId", leaveTypeId);
                    cmdUpdate.Parameters.AddWithValue("@days", leaveDays);

                    con.Open();
                    cmdUpdate.ExecuteNonQuery();
                }
            }

            // 🔹 RESET + PRG Redirect
            ClearForm();
            Response.Redirect("ApplyLeave.aspx");
        }


        protected void btnDelete_Click(object sender, EventArgs e)
        {
            int leaveId = Convert.ToInt32(((LinkButton)sender).CommandArgument);

            int leaveTypeId = 0;
            DateTime fromDate;
            DateTime toDate;

            // 🔹 Step 1: Get leave details
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmdGet = new SqlCommand(@"
            SELECT leaveTypeId, fromDate, toDate
            FROM EmpLeave
            WHERE leaveId=@id AND empId=@empId", con);

                cmdGet.Parameters.AddWithValue("@id", leaveId);
                cmdGet.Parameters.AddWithValue("@empId", EmpId);

                con.Open();
                SqlDataReader dr = cmdGet.ExecuteReader();

                if (!dr.Read())
                    return;

                leaveTypeId = Convert.ToInt32(dr["leaveTypeId"]);
                fromDate = Convert.ToDateTime(dr["fromDate"]);
                toDate = Convert.ToDateTime(dr["toDate"]);
            }

            int leaveDays = CalculateWorkingDays(fromDate, toDate);



            // 🔹 Step 2: Refund correct number of days
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmdUpdate = new SqlCommand(@"
            UPDATE EmpLeaveBalance
            SET usedLeaves = usedLeaves - @days
            WHERE empId=@empId AND leaveTypeId=@leaveTypeId", con);

                cmdUpdate.Parameters.AddWithValue("@empId", EmpId);
                cmdUpdate.Parameters.AddWithValue("@leaveTypeId", leaveTypeId);
                cmdUpdate.Parameters.AddWithValue("@days", leaveDays);

                con.Open();
                cmdUpdate.ExecuteNonQuery();
            }

            // 🔹 Step 3: Delete leave
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmdDel = new SqlCommand(
                    "DELETE FROM EmpLeave WHERE leaveId=@id AND empId=@empId", con);

                cmdDel.Parameters.AddWithValue("@id", leaveId);
                cmdDel.Parameters.AddWithValue("@empId", EmpId);

                con.Open();
                cmdDel.ExecuteNonQuery();
            }

            // 🔹 Step 4: Redirect (PRG pattern)
            Response.Redirect("ApplyLeave.aspx");
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(((LinkButton)sender).CommandArgument);
            hfLeaveId.Value = id.ToString();

            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT leaveTypeId, fromDate, toDate, reason " +
                    "FROM EmpLeave WHERE leaveId=@id AND empId=@empId", con);

                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@empId", EmpId);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    ddlLeaveType.SelectedValue = dr["leaveTypeId"].ToString();
                    txtFromDate.Text = Convert.ToDateTime(dr["fromDate"]).ToString("yyyy-MM-dd");
                    txtToDate.Text = Convert.ToDateTime(dr["toDate"]).ToString("yyyy-MM-dd");
                    txtReason.Text = dr["reason"].ToString();
                }
            }

            // 🔹 Just show modal – NOTHING else
            ScriptManager.RegisterStartupScript(
                this, GetType(), "show",
                "$('#leaveModal').modal('show');", true);
        }

        protected void gvLeave_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;

            string status = DataBinder.Eval(e.Row.DataItem, "status").ToString();

            var badge = (System.Web.UI.HtmlControls.HtmlGenericControl)
                e.Row.FindControl("statusBadge");

            if (badge == null) return;

            if (status == "Pending")
                badge.InnerHtml = "<span class='badge badge-warning'>Pending</span>";
            else if (status == "Approved")
                badge.InnerHtml = "<span class='badge badge-success'>Approved</span>";
            else if (status == "Rejected")
                badge.InnerHtml = "<span class='badge badge-danger'>Rejected</span>";
        }

      

       

        private int GetColumnIndex(string headerText)
        {
            foreach (DataControlField col in gvLeave.Columns)
            {
                if (col.HeaderText == headerText)
                    return gvLeave.Columns.IndexOf(col);
            }
            return -1;
        }

        void LoadGrid()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(@"
            SELECT 
                l.leaveId,
                lt.leaveTypeName,
                l.fromDate,
                l.toDate,
                l.reason,
                l.status,
                l.rejectionReason
            FROM EmpLeave l
            JOIN LeaveType lt ON l.leaveTypeId = lt.leaveTypeId
            WHERE l.empId = @empId
            ORDER BY l.appliedOn DESC", con);

                cmd.Parameters.AddWithValue("@empId", EmpId);

                con.Open();
                gvLeave.DataSource = cmd.ExecuteReader();
                gvLeave.DataBind();
            }
        }


        void ClearForm()
        {
            ddlLeaveType.SelectedIndex = 0;
            txtFromDate.Text = "";
            txtToDate.Text = "";
            txtReason.Text = "";
            hfLeaveId.Value = "";
        }

    }
}