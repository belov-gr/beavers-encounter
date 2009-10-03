<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="Beavers.Encounter.Core"%>
<%@ Import Namespace="Beavers.Encounter.Web.Controllers" %>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <h2>ДВИГАТЕЛЬ ДОЗОРА</h2>
    <h3>Что дальше?</h3>
    <%
    if (!((User) User).Identity.IsAuthenticated) 
    { %>
    <p>
        Для входа в систему перейдите по ссылке <%= Html.ActionLink("Войти", "LogOn", "Account") %> в верхнем правом углу страницы.
    </p>
    <p>
        Если вы еще не зарегистрированны, то зарегистрируйтесь здесь <%= Html.ActionLink("Регистрация", "Register", "Account")%>.
    </p>
    <p>
        После входа в систему Вы можете создать свою команду либо вступить в уже существующую.
    </p>
    <p>
        Для участия команды в игре капитану команды необходимо зарегистрировать свою команду в игре.
    </p>
    <%
    }
    else if (((User)User).Role.IsAuthor)
    { %>
    <p>
        Вы являетесь автором игры: 
        <%= Html.ActionLink<GamesController>(c => c.Edit(((User)User).Game.Id), ((User)User).Game.Name)%>.
    </p>
    <p>
        Не выпускайте штурвал из рук)))
    </p>
    <%
    }
    else if (((User)User).Team == null)
    { %>
    <p>
        Вы можете стать членом существующей команды и принять участие в игре.
    </p>
    <p>
        Либо вы можете создать свою команду и стать ее капитаном.
    </p>
    <%
    }
    else if (((User)User).Team != null && ((User)User).Role.IsTeamLeader)
    { %>
    <p>
        Вы капитан команды и можете подать заявку на участие Вашей команды в игре либо отказаться от участия Вашей команды в игре.
    </p>
    <p>
        Так же Вы можете управлять списком игроков в Вашей команде.
    </p>
    <%
    } 
    else if (((User)User).Team != null && ((User)User).Role.IsPlayer)
    { %>
    <p>
        Вы член команды <%= Html.Encode(((User)User).Team.Name)%>, если команда зарегистрирована в какой-либо игре, то ожидайте начала игры.
    </p>
    <%
    } %>
</asp:Content>
