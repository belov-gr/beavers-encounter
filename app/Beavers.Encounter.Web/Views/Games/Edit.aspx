<%@ Page Title="Edit Game" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" 
	Inherits="System.Web.Mvc.ViewPage<GamesController.GameFormViewModel>" %>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

<h1 style="color:Red">Тестируем <% = Html.ActionLink<GameboardController>(x => x.Show(Model.Game.Id), "новый редактор")%>!</h1>
<p>Жду пожеланий и отзывов о новом редакторе.</p>
<br />
    <div class="columnsContainer">
	    
        <div class="leftColumn">
	        
	        <% Html.RenderPartial("GameForm", ViewData); %>
    	
            <% Html.RenderPartial("RegisteredTeams", Model.Game); %>
            
        </div>

        <div class="rightColumn">
            
    	    <% Html.RenderPartial("GameStateControl", Model.Game); %>

            <% Html.RenderPartial("Tasks", Model.Game.Tasks); %>
            
            <% Html.RenderPartial("BonusTasks", Model.Game.BonusTasks); %>

        </div>
    	
    </div>

    
</asp:Content>
