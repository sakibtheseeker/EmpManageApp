<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EventCalender.aspx.cs" Inherits="EmpManageApp.EventCalender" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet"
       href="https://cdn.jsdelivr.net/npm/bootstrap@4.0.0/dist/css/bootstrap.min.css" />

   <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
   <script src="https://cdn.jsdelivr.net/npm/bootstrap@4.0.0/dist/js/bootstrap.min.js"></script>
       <style>
        .calendar-style {
            width: 100%;
        }
        .custom-calendar {
    width: 100%;
}

/* EACH DATE CELL */
.calendar-day,
.calendar-weekend {
    height: 80px !important;
    width: 100px !important;
    vertical-align: top !important;
    font-size: 16px;
    padding: 5px;
}


/* WEEKEND COLOR */
.calendar-weekend {
    background-color: #f8f9fa;
}

/* HEADER (SUN MON TUE) */
.calendar-header {
    height: 40px;
    font-weight: bold;
    text-align: center;
}

/* MONTH TITLE */
.calendar-title {
    font-size: 22px;
    font-weight: bold;
    padding: 10px;
}

/* OTHER MONTH DAYS */
.other-month {
    color: #ccc;
}

           </style>

</head>
<body>
          <nav class="navbar navbar-expand-lg navbar-dark bg-primary">
    <div class="container-fluid">
        <a class="navbar-brand" href="Emp.aspx">EmpManage</a>

        <button class="navbar-toggler" type="button"
                data-toggle="collapse"
                data-target="#navbarSupportedContent"
                aria-controls="navbarSupportedContent"
                aria-expanded="false"
                aria-label="Toggle navigation">
            <span class="navbar-toggler-icon"></span>
        </button>

        <div class="collapse navbar-collapse" id="navbarSupportedContent">
            <ul class="navbar-nav mr-auto">

                <li class="nav-item">
                    <a class="nav-link" href="Dept.aspx">Department</a>
                </li>

                <li class="nav-item">
                    <a class="nav-link" href="Designation.aspx">Designation</a>
                </li>

                <li class="nav-item">
                    <a class="nav-link" href="Role.aspx">Role</a>
                </li>

                <li class="nav-item">
                    <a class="nav-link" href="Emp.aspx">Employee</a>
                </li>

                <li class="nav-item dropdown">
                <a class="nav-link active dropdown-toggle" href="#" id="navbarDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                  Event
                </a>
                <div class="dropdown-menu" aria-labelledby="navbarDropdown">
                  <a class="dropdown-item" href="EventType.aspx">Event Type</a>
                  <a class="dropdown-item" href="EventCalender.aspx">Event Calender</a>
                    </div>
                  </li>

                 <li class="nav-item dropdown">
                <a class="nav-link dropdown-toggle" href="#" id="navbarDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                  Leave
                </a>
                <div class="dropdown-menu" aria-labelledby="navbarDropdown">
                  <a class="dropdown-item" href="LeaveType.aspx">Leave Type</a>
                  <a class="dropdown-item" href="AddLeave.aspx">Add Leave</a>
                       <a class="dropdown-item" href="ApplyLeave.aspx">Apply Leave</a>
                    </div>
                 </li>
            </ul>
        </div>
    </div>
</nav>
   <form id="form1" runat="server">

<div class="container-fluid mt-4">
    <div class="row">

     
        <div class="col-md-4">
            <div class="card">
                <div class="card-header bg-primary text-white">
                    Add Event
                </div>

                <div class="card-body">

                    <div class="form-group">
                        <label>Event Type</label>
                        <asp:DropDownList ID="ddlEventType"
                            runat="server"
                            CssClass="form-control" />
                    </div>

                    <div class="form-group">
                        <label>Event Name</label>
                        <asp:TextBox ID="txtEventName"
                            runat="server"
                            CssClass="form-control" />
                    </div>

                    <div class="form-group">
                        <label>Event Date</label>
                        <asp:TextBox ID="txtEventDate"
                            runat="server"
                            TextMode="Date"
                            CssClass="form-control" />
                    </div>

                    <asp:Button ID="btnAddEvent"
                        runat="server"
                        Text="Save Event"
                        CssClass="btn btn-success btn-block"
                        OnClick="btnAddEvent_Click" />

                </div>
            </div>
        </div>

      
        <div class="col-md-8">
            <div class="card">
                <div class="card-header bg-dark text-white">
                    Event Calendar
                </div>

                <div class="card-body text-center">

                 <asp:Calendar ID="Calendar1"
                    runat="server"
                    CssClass="custom-calendar"

                    DayStyle-CssClass="calendar-day"
                    OtherMonthDayStyle-CssClass="calendar-day other-month"
                    TitleStyle-CssClass="calendar-title"
                    DayHeaderStyle-CssClass="calendar-header"
                    WeekendDayStyle-CssClass="calendar-weekend"

                    OnDayRender="Calendar1_DayRender">
                 </asp:Calendar>


                </div>
            </div>
        </div>

    </div>
</div>

</form>
</body>
</html>
