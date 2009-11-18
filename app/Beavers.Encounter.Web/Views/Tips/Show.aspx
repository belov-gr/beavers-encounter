<%@ Page Title="Tip Details" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" 
	Inherits="System.Web.Mvc.ViewPage<Tip>" %>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

    <h1>Tip Details</h1>

    <ul>
		<li>
            <%= Model.Render<Tip>(Html, x => x.Name)%>
		</li>
		<li>
            <%= Model.Render<Tip>(Html, x => x.SuspendTime)%>
		</li>
	    <li class="buttons">
            <%= Html.Button("btnBack", "Back", HtmlButtonType.Button, 
                "window.location.href = '" + Html.BuildUrlFromExpression<TipsController>(c => c.Index()) + "';") %>
        </li>
	</ul>

</asp:Content>
