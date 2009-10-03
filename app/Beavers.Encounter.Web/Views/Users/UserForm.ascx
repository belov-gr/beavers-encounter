<%@ Control Language="C#" AutoEventWireup="true"
	Inherits="System.Web.Mvc.ViewUserControl<Beavers.Encounter.Web.Controllers.UsersController.UserFormViewModel>" %>
<%@ Import Namespace="Beavers.Encounter.Core" %>
<%@ Import Namespace="Beavers.Encounter.Web.Controllers" %>
 

<% if (ViewContext.TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] != null) { %>
    <p id="pageMessage"><%= ViewContext.TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()]%></p>
<% } %>

<%= Html.ValidationSummary() %>

<% using (Html.BeginForm()) { %>
    <%= Html.AntiForgeryToken() %>
    <%= Html.Hidden("User.Id", (ViewData.Model.User != null) ? ViewData.Model.User.Id : 0)%>

    <ul>
		<li>
			<label for="User_Login">Login:</label>
			<div>
				<%= Html.TextBox("User.Login", 
					(ViewData.Model.User != null) ? ViewData.Model.User.Login.ToString() : "")%>
			</div>
			<%= Html.ValidationMessage("User.Login")%>
		</li>
		<li>
			<label for="User_Password">Password:</label>
			<div>
				<%= Html.TextBox("User.Password", 
					(ViewData.Model.User != null) ? ViewData.Model.User.Password.ToString() : "")%>
			</div>
			<%= Html.ValidationMessage("User.Password")%>
		</li>
		<li>
			<label for="User_Nick">Nick:</label>
			<div>
				<%= Html.TextBox("User.Nick", 
					(ViewData.Model.User != null) ? ViewData.Model.User.Nick.ToString() : "")%>
			</div>
			<%= Html.ValidationMessage("User.Nick")%>
		</li>
		<li>
			<label for="User_Phone">Phone:</label>
			<div>
				<%= Html.TextBox("User.Phone", 
					(ViewData.Model.User != null) ? ViewData.Model.User.Phone.ToString() : "")%>
			</div>
			<%= Html.ValidationMessage("User.Phone")%>
		</li>
		<li>
			<label for="User_Icq">Icq:</label>
			<div>
				<%= Html.TextBox("User.Icq", 
					(ViewData.Model.User != null) ? ViewData.Model.User.Icq.ToString() : "")%>
			</div>
			<%= Html.ValidationMessage("User.Icq")%>
		</li>
		<li>
			<label for="User_Team">Team:</label>
			<div>
				<%= Html.TextBox("User.Team",
    (ViewData.Model.User != null) ? ViewData.Model.User.Team == null ? "Not set" : ViewData.Model.User.Team.ToString() : "")%>
			</div>
			<%= Html.ValidationMessage("User.Team")%>
		</li>
	    <li>
            <%= Html.SubmitButton("btnSave", "Save User") %>
	        <%= Html.Button("btnCancel", "Cancel", HtmlButtonType.Button, 
				    "window.location.href = '" + Html.BuildUrlFromExpression<UsersController>(c => c.Index()) + "';") %>
        </li>
    </ul>
<% } %>
