<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <h2><%= Convert.ToString(Application["AppTitle"]) %></h2>
    <h3>Что дальше?</h3>
    <%
    if (!((User) User).Identity.IsAuthenticated) 
    { %>
    <p>
        Для входа в систему перейдите по ссылке <%= Html.ActionLink<AccountController>(c => c.LogOn(), "Войти") %> в верхнем правом углу страницы.
    </p>
    <p>
        Если вы еще не зарегистрированны, то зарегистрируйтесь здесь <%= Html.ActionLink<AccountController>(c => c.Register(), "Регистрация")%>.
    </p>
    <%
    }
    else if (((User)User).Role.IsAuthor)
    { %>
    <p>
        <div>Вы автор игры &quot;<%= Html.ActionLink<GamesController>(c => c.Edit(((User)User).Game.Id), ((User)User).Game.Name)%>&quot;.</div>
        <div>Не пытайтесь создавать от своего имени новую игру, так как вы не сможете редактировать эту.</div>
    </p>
    <%
    }
    else if (((User)User).Team == null)
    { %>
    <p>
        <div>Вы можете вступить в существующую команду.</div>
        <div>Если Вы станете первым игроком, вступившим в команду, то Вы автоматически станете ее капитаном.</div>
        <p>
        Для вступления в команду нужен ключ доступа. Капитану стедует получить его у автора игры, первым войти по нему в команду и после этого сообщить его игрокам своей команды, чтобы они тажке могли в ней зарегистрироваться.
        </p>
        <div>Если есть подозрения в том, что код доступа стал известен третьим лицам, следует обратиться к авторам игры с просьбой изменить код доступа.</div>
    </p>
    <%
    }
    else if (((User)User).Team != null && ((User)User).Role.IsTeamLeader)
    { %>
    <p>
        <div>Вы капитан команды &quot;<%= Html.Encode(((User)User).Team.Name)%>&quot;.</div>
        <div>Уточните, зарегистрирована ли Ваша команда на участие в интересующей Вас игре.</div>
        <p>
        Вы можете управлять списком игроков в Вашей команде.<br />
        </p>
        <div>Вы можете сложить капитанские полномочия, в этом случае Капитаном станет игрок, стоящий в списке игроков сразу за Вами.</div>
    </p>
    <%
    } 
    else if (((User)User).Team != null && ((User)User).Role.IsPlayer)
    { %>
    <p>
        <div>Вы игрок команды &quot;<%= Html.Encode(((User)User).Team.Name)%>&quot;.</div>
        <div>Если Ваша команда зарегистрирована на участие в какой-либо игре, то ожидайте начала игры.</div>
        <p>
        При желании Вы можете покинуть команду, но об этом лучше поставить в известность Вашего капитана. Для повторного вступления в команду Вам потребуется код доступа.
        </p>
    </p>
    <%
    } %>
</asp:Content>
