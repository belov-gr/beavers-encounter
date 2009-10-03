<%@ Page Title="Edit Task" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" 
	Inherits="System.Web.Mvc.ViewPage<Beavers.Encounter.Web.Controllers.TasksController.TaskFormViewModel>" %>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

    <div class="columnsContainer">
	    
        <div class="leftColumn">
	        
	        <% Html.RenderPartial("TaskForm", ViewData); %>

        </div>

        <div class="rightColumn">
            
        	<% Html.RenderPartial("TaskTips", ViewData.Model.Task); %>

        	<br />
        	
	        <% Html.RenderPartial("TaskCodes", ViewData.Model.Task); %>

        </div>
    
    </div>
    
</asp:Content>
