<%@ Control Language="C#" Inherits="Beavers.Encounter.Web.Views.ViewUserControl<Task>" %>
<%@ Import Namespace="Beavers.Encounter.Common"%>

	<h2>����� ������� � ���������</h2>

<div class="registeredTeams">
    <ul>
    <%
    int indx = 0;
    foreach (Tip tip in Model.Tips)
    {%>
        <li>
        <div style="font-weight:bold">
            <%= tip.SuspendTime == 0 ? "�������:" : String.Format("��������� �{0}. ����� ���������: ����� {1} ���.", indx, tip.SuspendTime)%> 
        </div>
        <div>
        <%= BBCode.ConvertToHtml(tip.Name) %>
        </div>
        <%= Html.ActionLink<TipsController>(c => c.Edit(tip.Id), "��������")%>           
		
		<% 
        if (tip.SuspendTime > 0)
        {
            using (Html.BeginForm<TipsController>(c => c.Delete(tip.Id)))
            { %>
            <%= Html.AntiForgeryToken()%>
		    <input type="submit" value="�������" onclick="return confirm('Are you sure?');" />
        <% 
            }
        } %>
        </li>
        <p />
    <%
        indx++;
    } %>
    </ul>
    <p><%= Html.ActionLink<TipsController>(c => c.Create(Model.Id), "�������� ���������")%></p>
</div>


