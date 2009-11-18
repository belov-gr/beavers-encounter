<%@ Page Title="Code Details" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" 
	Inherits="System.Web.Mvc.ViewPage<Code>" %>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

    <h1>Code Details</h1>

    <ul>
		<li>
		    <%= Model.Render<Code>(Html, x => x.Name) %>
		</li>
		<li>
		    <%= Model.Render<Code>(Html, x => x.Danger) %>
		</li>
		<li>
		    <%= Model.Render<Code>(Html, x => x.IsBonus) %>
		</li>
	</ul>

    <%= Html.Button("btnBack", "Back", HtmlButtonType.Button, 
        "window.location.href = '" + Html.BuildUrlFromExpression<CodesController>(c => c.Index()) + "';") %>

</asp:Content>
