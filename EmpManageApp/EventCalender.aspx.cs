using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EmpManageApp
{
    public partial class EventCalender : System.Web.UI.Page
    {
        string connStr =
            ConfigurationManager.ConnectionStrings["empmanage"].ConnectionString;

        private DataTable calendarEvents;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["username"] == null)
            {
                Response.Redirect("Login.aspx");
            }
            if (!IsPostBack)
            {
                LoadEventTypes();
                LoadCalendarEvents();
            }
        }

        private void LoadCalendarEvents()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlDataAdapter da = new SqlDataAdapter("FetchEventCalendar", con);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;

                calendarEvents = new DataTable();
                da.Fill(calendarEvents);
            }

            ViewState["CalendarEvents"] = calendarEvents;
        }
        protected void Calendar1_DayRender(object sender, DayRenderEventArgs e)
        {
            if (ViewState["CalendarEvents"] == null)
                return;

            DataTable dt = (DataTable)ViewState["CalendarEvents"];

            DateTime cellDate = e.Day.Date;

            DataRow[] events = dt.Select(
                $"eventDate = #{cellDate:MM/dd/yyyy}#"
            );

            foreach (DataRow row in events)
            {
                string eventName = row["eventName"].ToString();
                string color = row["colorCode"].ToString();

                Label lbl = new Label();
                lbl.Text = eventName;
                lbl.ForeColor = System.Drawing.Color.White;
                lbl.BackColor = System.Drawing.ColorTranslator.FromHtml(color);
                lbl.CssClass = "badge d-block mt-1";
                lbl.Style.Add("font-size", "12px");

                e.Cell.Controls.Add(new LiteralControl("<br/>"));
                e.Cell.Controls.Add(lbl);
            }
        }

        private void LoadEventTypes()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT eventTypeId, eventTypeName FROM EventType WHERE isActive = 1",
                    con);

                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlEventType.DataSource = dt;
                ddlEventType.DataTextField = "eventTypeName";
                ddlEventType.DataValueField = "eventTypeId";
                ddlEventType.DataBind();
            }

            ddlEventType.Items.Insert(0,
                new System.Web.UI.WebControls.ListItem("-- Select Event Type --", "0"));
        }

        protected void btnAddEvent_Click(object sender, EventArgs e)
        {
            int eventTypeId = Convert.ToInt32(ddlEventType.SelectedValue);
            string eventName = txtEventName.Text.Trim();
            DateTime eventDate;

            if (eventTypeId == 0 || string.IsNullOrEmpty(eventName))
                return;

            if (!DateTime.TryParse(txtEventDate.Text, out eventDate))
                return;

            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand("InsertEventCalendar", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@eventTypeId", eventTypeId);
                cmd.Parameters.AddWithValue("@eventName", eventName);
                cmd.Parameters.AddWithValue("@eventDate", eventDate);

                con.Open();
                cmd.ExecuteNonQuery();
            }
            LoadCalendarEvents();

     
            ScriptManager.RegisterStartupScript(
                this,
                GetType(),
                "eventAdded",
                "alert('Event added successfully!');",
                true
            );

        }

        protected void Calendar1_SelectionChanged(object sender, EventArgs e)
        {
            txtEventDate.Text =
                Calendar1.SelectedDate.ToString("yyyy-MM-dd");
        }

       
     
    }
}
