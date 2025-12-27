using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EmpManageApp
{
    public partial class ApproveLeave : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["empmanage"].ConnectionString;

     


        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["role"] == null || Session["role"].ToString() != "Manager")
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadPendingLeaves();
            }
        }

        string GetManagerName()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(@"
            SELECT DISTINCT eManager
            FROM Emp
            WHERE eManager IS NOT NULL
              AND eManager IN (
                  SELECT eManager
                  FROM Emp
                  WHERE eid = @empId
              )
        ", con);

                cmd.Parameters.AddWithValue("@empId", Session["empId"]);

                con.Open();
                return Convert.ToString(cmd.ExecuteScalar());
            }
        }

        void LoadPendingLeaves()
        {
            string managerName = GetManagerName();

            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(@"
            SELECT 
                l.leaveId,
                emp.eName AS employeeName,
                l.fromDate,
                l.toDate,
                DATEDIFF(day, l.fromDate, l.toDate) + 1 AS leaveDays,
                l.reason
            FROM EmpLeave l
            JOIN Emp emp ON l.empId = emp.eid
            WHERE l.status = 'Pending'
              AND emp.eManager = @managerName
        ", con);

                cmd.Parameters.AddWithValue("@managerName", managerName);

                con.Open();
                gvApproveLeave.DataSource = cmd.ExecuteReader();
                gvApproveLeave.DataBind();
            }
        }






        protected void gvApproveLeave_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int leaveId = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "Approve")
            {
                UpdateLeaveStatus(leaveId, "Approved");
            }
            else if (e.CommandName == "Reject")
            {
                UpdateLeaveStatus(leaveId, "Rejected");
            }

            // PRG pattern
            Response.Redirect("ApproveLeave.aspx");
        }

        int CalculateWorkingDays(DateTime start, DateTime end)
        {
            int days = 0;

            for (DateTime date = start; date <= end; date = date.AddDays(1))
            {
                if (date.DayOfWeek != DayOfWeek.Saturday &&
                    date.DayOfWeek != DayOfWeek.Sunday)
                {
                    days++;
                }
            }

            return days;
        }


        void UpdateLeaveStatus(int leaveId, string status, string rejectionReason = null)
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();

                // 🔴 REFUND ONLY IF REJECTED
                if (status == "Rejected")
                {
                    SqlCommand cmdGet = new SqlCommand(@"
                SELECT empId, leaveTypeId, fromDate, toDate
                FROM EmpLeave
                WHERE leaveId = @leaveId
                  AND status = 'Pending'", con);

                    cmdGet.Parameters.AddWithValue("@leaveId", leaveId);

                    SqlDataReader dr = cmdGet.ExecuteReader();
                    if (!dr.Read()) return;

                    int empId = Convert.ToInt32(dr["empId"]);
                    int leaveTypeId = Convert.ToInt32(dr["leaveTypeId"]);
                    DateTime from = Convert.ToDateTime(dr["fromDate"]);
                    DateTime to = Convert.ToDateTime(dr["toDate"]);
                    dr.Close();

                    int leaveDays = CalculateWorkingDays(from, to);

                    SqlCommand cmdRefund = new SqlCommand(@"
                UPDATE EmpLeaveBalance
                SET usedLeaves = usedLeaves - @days
                WHERE empId = @empId
                  AND leaveTypeId = @leaveTypeId", con);

                    cmdRefund.Parameters.AddWithValue("@days", leaveDays);
                    cmdRefund.Parameters.AddWithValue("@empId", empId);
                    cmdRefund.Parameters.AddWithValue("@leaveTypeId", leaveTypeId);
                    cmdRefund.ExecuteNonQuery();
                }

                // ✅ UPDATE STATUS (for both approve & reject)
                SqlCommand cmdUpdate = new SqlCommand(@"
            UPDATE EmpLeave
            SET status = @status,
                rejectionReason = @rejectionReason,
                approvedOn = GETDATE()
            WHERE leaveId = @leaveId
              AND status = 'Pending'", con);

                cmdUpdate.Parameters.AddWithValue("@status", status);
                cmdUpdate.Parameters.AddWithValue("@leaveId", leaveId);
                cmdUpdate.Parameters.AddWithValue("@rejectionReason",
                    string.IsNullOrEmpty(rejectionReason) ? (object)DBNull.Value : rejectionReason);

                cmdUpdate.ExecuteNonQuery();
            }
        }




        protected void btnConfirmReject_Click(object sender, EventArgs e)
        {
            int leaveId = Convert.ToInt32(hfRejectLeaveId.Value);
            string reason = txtRejectReason.Text.Trim();

            if (string.IsNullOrEmpty(reason))
                return;

            // ✅ USE SAME METHOD (refund included)
            UpdateLeaveStatus(leaveId, "Rejected", reason);

            Response.Redirect("ApproveLeave.aspx");
        }



    }
}
