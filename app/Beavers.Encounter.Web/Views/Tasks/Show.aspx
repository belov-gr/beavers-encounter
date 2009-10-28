<%@ Page Title="Task Details" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" 
	Inherits="System.Web.Mvc.ViewPage<Task>" %>
<%@ Import Namespace="Beavers.Encounter.Core"%>
<%@ Import Namespace="Beavers.Encounter.Web.Controllers" %>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

    <div class="columnsContainer">
	    
        <div class="leftColumn">
	        
            <h2>Свойства задания</h2>

            <ul>
		        <li>
			        <label for="Task_Name">Кодовое название:</label>
                    <span id="Task_Name" style="font-weight:bold"><%= Html.Encode(ViewData.Model.Name) %></span>
		        </li>
		        <li>
			        <label for="Task_Priority">Приоритет задания:</label>
                    <span id="Task_Priority"><%= ViewData.Model.Priority %></span>
		        </li>
		        <li>
			        <label for="Task_StreetChallendge">Street Challenge:</label>
                    <span id="Task_StreetChallendge"><%= ViewData.Model.StreetChallendge == 1 ? "Да" : "Нет" %></span>
		        </li>
		        <li>
			        <label for="Task_Agents">Задание с агентами:</label>
                    <span id="Task_Agents"><%= ViewData.Model.Agents == 1 ? "Да" : "Нет" %></span>
		        </li>
		        <li>
			        <label for="Task_Locked">Задание заблокированно:</label>
                    <span id="Task_Locked"><%= ViewData.Model.Locked == 1 ? "Да" : "Нет" %></span>
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
