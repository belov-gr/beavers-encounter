<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<TeamGameboardController.TeamGameboardViewModel>" %>
<%@ Import Namespace="Beavers.Encounter.Common"%>
<%@ Import Namespace="Beavers.Encounter.Core.DataInterfaces"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <link href="<%= ResolveUrl("~") %>Content/Site.css" rel="stylesheet" type="text/css" />
    <title>Game board</title>
</head>
<body>
  <div class="teamGameboardPage">
    <h1><%= Html.Encode(Model.TeamName) %></h1>
    
    <div><%= Html.Encode(Model.Message) %></div>
    
    <% if (Model.TeamGameState != null && Model.ActiveTaskState != null) { %>
    
        <h2>������� �: <%= Model.TeamGameState.AcceptedTasks.Count %></h2>

        <% 
        // ������� ����� ����������� ������� � ���������
        int indx = 0;
        foreach (AcceptedTip tip in Model.ActiveTaskState.AcceptedTips)
        { %>
            <ul><li><div style="font-weight:bold; padding-bottom: 0px; margin-bottom 0px;">
                    <%= tip.Tip.SuspendTime == 0 ? "�������" : String.Format("��������� �{0}", indx)%></div></li></ul>
            <div><%= BBCode.ConvertToHtml(tip.Tip.Name)%></div>
            <div class="note">[��������: <%= tip.AcceptTime %>]</div>
        <%
            indx++;
        } %>

        <%
        // ������ ��� "���������" �������. �������� ������ ��� �������� �������.
        if (((User)User).Role.IsTeamLeader && 
            Model.TeamGameState.ActiveTaskState.Task.TaskType == TaskTypes.NeedForSpeed &&
            Model.TeamGameState.ActiveTaskState.AccelerationTaskStartTime == null) 
        { %>
            <p></p>
            <% 
            using (Html.BeginForm<TeamGameboardController>(c => c.AccelerateTask(Model.ActiveTaskState.Id), FormMethod.Post)) 
            { %>
                <%= Html.AntiForgeryToken() %>
                <div style="color:Yellow">��������! ����� ������� ������, �� ���������� ������� ��������� <%= Model.TeamGameState.Game.TimePerTask - Model.ActiveTaskState.Task.Tips.Last(tip => tip.SuspendTime > 0).SuspendTime%> �����. �� �������, ��� �� ������?</div>
                <div><%= Html.SubmitButton("btnAccelerateTask", "�� ������!", new Dictionary<string, object> { { "onclick", "return confirm('�� �������, ��� ������ � ���������?');" } })%></div>
            <% 
            } 
        } %>


        <%
        // ������ ��� ������ ���������.
        if (Model.SuggestTips != null) 
        { %>
            <p></p>
            <div style="color:Yellow"><%= Model.SuggestMessage %></div>
            <%
            foreach(Tip tip in Model.SuggestTips)
            { %>
                <span>
                <%
                string colorText = String.Empty;
                int tipPos = Model.ActiveTaskState.Task.Tips.TipPosition(tip);
                switch (tipPos)
                {
                    case 1: { colorText = "50x50"; break; }
                    case 2: { colorText = "������ �����"; break; }
                    case 3: { colorText = "��������� ����"; break; }
                }
                
                using (Html.BeginForm<TeamGameboardController>(c => c.SelectTip(Model.ActiveTaskState.Id, tip.Id), FormMethod.Post)) 
                { %>
                    <%= Html.AntiForgeryToken() %>
                    <%= Html.SubmitButton(String.Format("btnSelectTip{0}", tipPos), colorText)%>
                <% 
                } %> 
                </span>
            <%
            } %> 
            
        <% 
        } %>


        <p>��: 
        <% 
        
        // ������� ���� ��������� �������� �����
        foreach (Code code in Model.ActiveTaskState.Task.Codes)
        { 
            if (!code.IsBonus)
            { %>
            <span class="<%= Model.ActiveTaskState.AcceptedCodes.Any(x => x.Code.Id == code.Id) ? "acceptedCode" : "notAcceptedCode" %>">
                <%= code.Danger %>
            </span>
            <% 
            }
        }

        // ������� ���� ��������� �������� �����, ���� ������� ��� ���� � �������
        if (Model.ActiveTaskState.Task.Codes.Any(x => x.IsBonus))
        { %>
        (�: 
            <% 
            foreach (Code code in Model.ActiveTaskState.Task.Codes)
            { 
                if (code.IsBonus)
                { %>
            <span class="<%= Model.ActiveTaskState.AcceptedCodes.Any(x => x.Code.Id == code.Id) ? "acceptedBonusCode" : "notAcceptedCode" %>">
                <%= code.Danger %>
            </span>
                <% 
                } %>       
            <% 
            } %>       
        )
        <% 
        } %>       
        </p>

        <% 
        // ������� �������� ���� 
        if (Model.ActiveTaskState.AcceptedCodes.Count > 0)
        { %>
            <p>�������� ����:  
            <% 
            foreach (AcceptedCode acceptedCode in Model.ActiveTaskState.AcceptedCodes)
            { %>
                <span class="<%= acceptedCode.Code.IsBonus ? "acceptedBonusCode" : "acceptedCode" %>">
                    <%= acceptedCode.Code.Name %>
                </span>
            <% 
            } %>       
            </p>
        <% 
        } %>       
<!--
        <% 
        // ������� ������������ �������� ���� 
        if (Model.ActiveTaskState.AcceptedBadCodes.Count > 0)
        { %>
            <p> ������������ �������� ����:  
            <% 
            foreach (AcceptedBadCode ccceptedBadCode in Model.ActiveTaskState.AcceptedBadCodes)
            { %>
                <span class="acceptedBadCode">
                    <%= Html.Encode(ccceptedBadCode.Name)%>
                </span>
            <% 
            } %>       
            </p>
        <% 
        } %>       
-->
        <% 
        // ��������� ������� ����, ���� ��� ������� ���������
        if (Model.ActiveTaskState.AcceptedBadCodes.Count > 0)
        { %>
            <div style="color:Red">
                <%= Html.Encode(Model.ErrorMessage) %>
            </div>
            <div style="color:Red">
                �������� �������: <%= Game.BadCodesLimit - Model.ActiveTaskState.AcceptedBadCodes.Count %>
            </div>
            <div class="note">��� ���������� ������� ����� ������������ ���� �����, � ������� � ������ ������ ��������� ����� ������������������!</div>
        <% 
        } %>
        

        <% 
        // ��������� ������� ����, ���� ��� ������� ���������
        if (Model.ActiveTaskState.AcceptedBadCodes.Count < Game.BadCodesLimit)
        {
            // ���� �������� �����
            using (Html.BeginForm<TeamGameboardController>(c => c.SubmitCodes(Model.ActiveTaskState.Id, null), FormMethod.Post))
            { %>
            <%= Html.AntiForgeryToken()%>
            <div>
                <%= Html.TextBox("Codes")%>
                <%= Html.SubmitButton("btnSubmit", "���������")%>
            </div>
        <% 
            }
        } %>


        <% 
        // ������� ���������, ��� ��� �������� ���� ������� � ����� ������� � ���������� �������
        if (Model.ActiveTaskState.AcceptedCodes.Count(x => !x.Code.IsBonus) == Model.ActiveTaskState.Task.Codes.Count(x => !x.IsBonus))
        { %>
            <div style="font-weight:bold">��������!</div>
            <div style="color:Yellow">
            �� ����� ��� �������� ����! 
            ���� �� �� ������� ���������� ����� �������� �����, �� ������� �� ������ "��������� �������" ��� ��������� ������ �������.
            </div>
            <% 
            using (Html.BeginForm<TeamGameboardController>(c => c.NextTask(Model.ActiveTaskState.Id), FormMethod.Post)) 
            { %>
                <%= Html.AntiForgeryToken() %>
                <div>
                    <%= Html.SubmitButton("btnNextTask", "��������� �������", new Dictionary<string, object> {{"onclick", "return confirm('Are you sure?');"}})%>
                </div>
            <% 
            }
        } %>


        <% 
        if (Model.TeamGameState.Game.BonusTasks.AvailableNowTasks().Count() > 0)
        { %>
        
        <h2>�������� �������</h2>
        
        <ul>
            <%
            foreach (BonusTask task in Model.TeamGameState.Game.BonusTasks.AvailableNowTasks())
            { 
                if (!task.IsIndividual)
                {%>
                <li>
                    <div style="font-weight:bold">
                        <b>�������</b>
                        <span class="note">[��������: <%= task.StartTime %>, ������������� ��: <%= task.FinishTime %>]</span>
                    </div>
                    <div><%= BBCode.ConvertToHtml(task.TaskText)%></div>
                </li>
                <%
                } %>
            <%
            } %>
        </ul>            
        <% 
        } %>

        <h2>����������</h2>
        <ul>
            <li>���������: <%= Html.Encode(Model.TeamGameState.AcceptedTasks.Count(x => x.State == (int)TeamTaskStateFlag.Success)) %></li>
            <li>�����������: <%= Html.Encode(Model.TeamGameState.AcceptedTasks.Count(x => x.State == (int)TeamTaskStateFlag.Overtime)) %></li>
            <li>�����: <%= Html.Encode(Model.TeamGameState.AcceptedTasks.Count(x => x.State == (int)TeamTaskStateFlag.Canceled)) %></li>
            <li>������������������: <%= Html.Encode(Model.TeamGameState.AcceptedTasks.Count(x => x.State == (int)TeamTaskStateFlag.Cheat)) %></li>
            <p></p>
            <li>�������� ����: <%= Html.Encode(Model.TeamGameState.AcceptedTasks.BonusCodesCount())%></li>
        </ul>


        <p></p>
        <h2>���� �������</h2>
        <%
        // ������ ��� ����� �������. �������� ������ ��� �������� �������, ����� ������ ���������
        if (((User)User).Role.IsTeamLeader && Model.TeamGameState.ActiveTaskState.AcceptedTips.Count > 1 &&
            Model.ActiveTaskState.AcceptedCodes.Count(x => !x.Code.IsBonus) != Model.ActiveTaskState.Task.Codes.Count(x => !x.IsBonus)) 
        { %>
            <% 
            using (Html.BeginForm<TeamGameboardController>(c => c.SkipTask(Model.ActiveTaskState.Id), FormMethod.Post)) 
            { %>
                <%= Html.AntiForgeryToken() %>
                <div><%= Html.SubmitButton("btnSkipTask", "����� �������", new Dictionary<string, object> {{"onclick", "return confirm('�� ������� ��� ������ ����� �������?');"}})%></div>
            <% 
            } 
        }  else { %>
            <div>����� ������� ����� ������ �������� ������� � ������ ����� ������ ��������� � ������ ���� �� ����� ���� �����.</div>
        <% 
        } %>
    <% 
    } %>
  </div>
</body>
</html>
