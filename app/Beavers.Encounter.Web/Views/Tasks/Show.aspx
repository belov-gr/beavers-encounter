<%@ Page Title="Task Details" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" 
	Inherits="System.Web.Mvc.ViewPage<Task>" %>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

    <div class="columnsContainer">
	    
        <div class="leftColumn">
	        
            <h2>Свойства задания</h2>

            <ul>
		        <li>
                    <%= Model.Render<Task>(Html, x => x.Name)%>
		        </li>
		        <li>
                    <%= Model.Render<Task>(Html, x => x.Priority)%>
		        </li>
		        <li>
                    <%= Model.Render<Task>(Html, x => x.StreetChallendge)%>
		        </li>
		        <li>
                    <%= Model.Render<Task>(Html, x => x.Agents)%>
		        </li>
		        <li>
                    <%= Model.Render<Task>(Html, x => x.Locked)%>
		        </li>
		        <li>
			        <label for="Task_NotAfterTasks">Не после:</label>
                    <span id="Task_NotAfterTasks">
                    <% foreach (Task task in Model.NotAfterTasks) { %>
                        <%= Html.ActionLink<TasksController>(c => c.Show(task.Id), task.Name)%>
                    <% } %>
                    </span>
		        </li>
		        <li>
			        <label for="Task_NotOneTimeTasks">Не вместе:</label>
                    <span id="Task_NotOneTimeTasks">
                    <% foreach (Task task in Model.NotOneTimeTasks) { %>
                        <%= Html.ActionLink<TasksController>(c => c.Show(task.Id), task.Name)%>
                    <% } %>
                    </span>
		        </li>
	        </ul>
        	
            <p><%= Html.ActionLink<TasksController>(c => c.Edit(Model.Id), "Изменить")%></p>
        	
        </div>

        <div class="rightColumn">
            
	        <% Html.RenderPartial("TaskTips", ViewData.Model); %>

        	<br />
        	
        	<% Html.RenderPartial("TaskCodes", ViewData.Model); %>

        </div>
    
    </div>
    
</asp:Content>
