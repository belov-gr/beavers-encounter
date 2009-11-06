<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Game>"%>
<%@ Import Namespace="Beavers.Encounter.Core"%>
<%@ Import Namespace="Beavers.Encounter.Web.Controllers"%>

<h2>���������� �����</h2>

	<%= Html.ActionLink<GamesController>(c => c.CurrentState(Model.Id), "��������� ����:")%>
    <%= Html.Encode(Enum.GetName(typeof(GameStates), Model.GameState))%>
    <% 
        if (Model.GameState == GameStates.Planned)
       { %>
   
        <%= Html.Button("btnStartupGame", "����������� � �������", HtmlButtonType.Button,
                        "window.location.href = '" + Html.BuildUrlFromExpression<GamesController>(c => c.StartupGame(Model.Id)) + "';")%>
    <% }
        else if (Model.GameState == GameStates.Started)
       { %>
        <%= Html.Button("btnStopGame", "���������� ����", HtmlButtonType.Button,
                        "window.location.href = '" + Html.BuildUrlFromExpression<GamesController>(c => c.StopGame(Model.Id)) + "';")%>
    <% }
        else if (Model.GameState == GameStates.Startup)
       { %>
        <%= Html.Button("btnStartGame", "��������� ����", HtmlButtonType.Button,
                        "window.location.href = '" + Html.BuildUrlFromExpression<GamesController>(c => c.StartGame(Model.Id)) + "';")%>
    <% }
        else if (Model.GameState == GameStates.Finished)
       { %>

        <%= Html.Button("btnCloseGame", "������� ����", HtmlButtonType.Button,
                        "window.location.href = '" + Html.BuildUrlFromExpression<GamesController>(c => c.CloseGame(Model.Id)) + "';")%>
    <% } %>

    <% if (Model.GameState == GameStates.Startup ||
           Model.GameState == GameStates.Finished ||
           Model.GameState == GameStates.Cloused)
       { %>
   
        <%= Html.Button("btnResetGame", "������������ ����", HtmlButtonType.Button,
                        "window.location.href = '" + Html.BuildUrlFromExpression<GamesController>(c => c.ResetGame(Model.Id)) + "';")%>
    <% } %>

        <div><%= Html.ActionLink<GameServiceController>(c => c.CalcState(Model.Id), "�������� ���������")%></div>
        <div><%= Html.ActionLink<GameServiceController>(c => c.GetState(Model.Id), "������� ��������� � ������")%></div>
    
