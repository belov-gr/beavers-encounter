<%@ Page Title="BonusTask Details" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" 
	Inherits="System.Web.Mvc.ViewPage<BonusTask>" %>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

    <h1>Свойства бонусного задания</h1>

    <ul>
		<li>
		    <%= Model.Render<BonusTask>(Html, x => x.Name) %>
		</li>
		<li>
		    <%= Model.Render<BonusTask>(Html, x => x.TaskText) %>
		</li>
		<li>
		    <%= Model.Render<BonusTask>(Html, x => x.StartTime) %>
		</li>
		<li>
		    <%= Model.Render<BonusTask>(Html, x => x.FinishTime) %>
		</li>
	</ul>
    
    <%= Html.Button("btnBack", "Back", HtmlButtonType.Button, 
        "window.location.href = '" + Html.BuildUrlFromExpression<GamesController>(c => c.Edit(ViewData.Model.Game.Id)) + "';") %>

</asp:Content>
