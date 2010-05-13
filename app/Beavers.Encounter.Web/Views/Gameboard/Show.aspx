<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<Game>" %>
<%@ Register Assembly="Coolite.Ext.Web" Namespace="Coolite.Ext.Web" TagPrefix="ext" %>
<%@ Register src="~/Views/Gameboard/GameEditorWindow.ascx" tagname="GameEditorWindow" tagprefix="uc" %>
<%@ Register src="~/Views/Gameboard/TipEditorWindow.ascx" tagname="TipEditorWindow" tagprefix="uc" %>
<%@ Register src="~/Views/Gameboard/GameControlWindow.ascx" tagname="GameControlWindow" tagprefix="uc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Show</title>
    <meta http-equiv="Content-Type" content="text/html; windows-1251" />
    <link href="/Content/Gameboard.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="/Scripts/Gameboard/Utils.js"></script>
    <script type="text/javascript" src="/Scripts/Gameboard/GameTree.js"></script>
    <script type="text/javascript" src="/Scripts/Gameboard/Game.js"></script>
    <script type="text/javascript" src="/Scripts/Gameboard/Task.js"></script>
    <script type="text/javascript" src="/Scripts/Gameboard/Tips.js"></script>
    <script type="text/javascript" src="/Scripts/Gameboard/Bonus.js"></script>
    <script type="text/javascript" src="/Scripts/Gameboard/GameState.js"></script>
    <script type="text/javascript" src="/Scripts/bbcode.js"></script>
</head>
<body>
    <form ID="dummyForm" runat="server">
        <%= Html.AntiForgeryToken()%>
    </form>
    
    <ext:ScriptManager ID="ScriptManager1" runat="server">
    </ext:ScriptManager>

    <ext:Store ID="dsGame" runat="server" ShowWarningOnFailure="true" AutoLoad="false">
        <Proxy>
            <ext:HttpProxy Url="/Gameboard/GetGame/"></ext:HttpProxy>
        </Proxy>
        <Reader>
            <ext:JsonReader ReaderID="Id" Root="data" TotalProperty="totalCount">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Name" />
                    <ext:RecordField Name="TotalTime" />
                    <ext:RecordField Name="TimePerTask" />
                    <ext:RecordField Name="TimePerTip" />
                    <ext:RecordField Name="Description" />
                    <ext:RecordField Name="PrefixMainCode" />
                    <ext:RecordField Name="PrefixBonusCode" />
                </Fields>
            </ext:JsonReader>
        </Reader>
        <BaseParams>
            <ext:Parameter Name="id" Value="gameId.value" Mode="Raw"></ext:Parameter>
        </BaseParams>
        <Listeners>
            <BeforeLoad Handler="tabGame.el.mask('Загрузка...', 'x-mask-loading');" />
            <LoadException Handler="tabGame.el.unmask();" />
            <Load Fn="gameLoaded" />
        </Listeners>
    </ext:Store>

    <ext:Store ID="dsTask" runat="server" ShowWarningOnFailure="true" AutoLoad="false" WarningOnDirty="false">
        <Proxy>
            <ext:HttpProxy Url="/Gameboard/GetTask/"></ext:HttpProxy>
        </Proxy>
        <Reader>
            <ext:JsonReader ReaderID="Id" Root="data" TotalProperty="totalCount">
                <Fields>
                    <ext:RecordField Name="Id"/>
                    <ext:RecordField Name="Name"/>
                    <ext:RecordField Name="StreetChallendge"/>
                    <ext:RecordField Name="Agents"/>
                    <ext:RecordField Name="Locked"/>
                    <ext:RecordField Name="TaskType"/>
                    <ext:RecordField Name="Priority"/>
                    <ext:RecordField Name="NotAfterTasks" >
                        <Convert Fn="ConvertNotAfterTasks"/>
                    </ext:RecordField>
                    <ext:RecordField Name="NotAfterTasksLabel">
                        <Convert Fn="ConvertNotAfterTasksLabel"/>
                    </ext:RecordField>
                    <ext:RecordField Name="NotOneTimeTasks" >
                        <Convert Fn="ConvertNotOneTimeTasks"/>
                    </ext:RecordField>
                    <ext:RecordField Name="NotOneTimeTasksLabel">
                        <Convert Fn="ConvertNotOneTimeTasksLabel"/>
                    </ext:RecordField>
                </Fields>
            </ext:JsonReader>
        </Reader>
        <BaseParams>
            <ext:Parameter Name="gameId" Value="gameId.value" Mode="Raw"></ext:Parameter>
            <ext:Parameter Name="taskId" Value="activeTaskId" Mode="Raw"></ext:Parameter>
        </BaseParams>
        <Listeners>
            <BeforeLoad Handler="tabTask.el.mask('Загрузка...', 'x-mask-loading');" />
            <LoadException Handler="tabTask.el.unmask();" />
            <Load Fn="taskLoaded" />
        </Listeners>
    </ext:Store>
    
    <ext:Store ID="dsTips" runat="server" ShowWarningOnFailure="true" AutoLoad="false" WarningOnDirty="false">
        <Reader>
            <ext:JsonReader ReaderID="Id" Root="data" TotalProperty="totalCount">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Name" />
                    <ext:RecordField Name="SuspendTime" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>

    <ext:Store ID="dsCodes" runat="server" ShowWarningOnFailure="true" AutoLoad="false" WarningOnDirty="false" RefreshAfterSaving="None" UseIdConfirmation="false">
        <UpdateProxy>
            <ext:HttpWriteProxy Url="/Gameboard/SaveCode" Method="POST"/>
        </UpdateProxy>
        <Reader>
            <ext:JsonReader ReaderID="Id" Root="data" TotalProperty="totalCount">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Name" />
                    <ext:RecordField Name="Danger" />
                    <ext:RecordField Name="IsBonus" Type="Boolean"/>
                </Fields>
            </ext:JsonReader>
        </Reader>
        <WriteBaseParams>
            <ext:Parameter Name="taskId" Value="getTaskID()" Mode="Raw"/>
        </WriteBaseParams>
    </ext:Store>

    <ext:Store ID="dsBonus" runat="server" ShowWarningOnFailure="true" AutoLoad="false" WarningOnDirty="false">
        <Proxy>
            <ext:HttpProxy Url="/Gameboard/GetBonus/"></ext:HttpProxy>
        </Proxy>
        <Reader>
            <ext:JsonReader ReaderID="Id" Root="data" TotalProperty="totalCount">
                <Fields>
                    <ext:RecordField Name="Id"/>
                    <ext:RecordField Name="Name"/>
                    <ext:RecordField Name="TaskText"/>
                    <ext:RecordField Name="StartTime"/>
                    <ext:RecordField Name="FinishTime"/>
                    <ext:RecordField Name="IsIndividual"/>
                </Fields>
            </ext:JsonReader>
        </Reader>
        <BaseParams>
            <ext:Parameter Name="gameId" Value="gameId.value" Mode="Raw"></ext:Parameter>
            <ext:Parameter Name="bonusId" Value="activeBonusId" Mode="Raw"></ext:Parameter>
        </BaseParams>
        <Listeners>
            <BeforeLoad Handler="tabBonus.el.mask('Загрузка...', 'x-mask-loading');" />
            <LoadException Handler="tabBonus.el.unmask();" />
            <Load Fn="bonusLoaded" />
        </Listeners>
    </ext:Store>

    <ext:Desktop ID="GameboardDesktop" runat="server" ShortcutTextColor="White">
        <Body>
            <div style="vertical-align:middle;">
                <span><img src="/Content/Images/logo_aspnetmvc2.png" /></span>
                <span><img src="/Content/Images/logo_sharparch-large.png" /></span>
                <span><img src="/Content/Images/logo_extJS.png" /></span>
                <span><img src="/Content/Images/logo_coolitestudio2.gif" /></span>
                <span><img src="/Content/Images/logo_nhibernate.gif" /></span>
                <span><img src="/Content/Images/logo_fluent.png" /></span>
                <span><img src="/Content/Images/logo_nunit.gif" /></span>
                <span><img src="/Content/Images/logo_rhinomocks.png" /></span>
                <span><img src="/Content/Images/logo_googlecode.png" /></span>
            </div>
        </Body>
        <StartMenu>
            <Items>
                <ext:MenuItem Text="Редактор игры" Icon="ApplicationFormEdit">
                    <Listeners>
                        <Click Handler="#{winGameEditor}.show();" />
                    </Listeners>
                </ext:MenuItem>
                <ext:MenuItem Text="Управление игрой" Icon="Joystick">
                    <Listeners>
                        <Click Handler="#{winGameControl}.show();" />
                    </Listeners>
                </ext:MenuItem>
                <ext:MenuItem Text="Старый редактор" Icon="ApplicationOsxTerminal">
                    <Listeners>
                        <Click Handler="#{winOldEditorBrowser}.show();" />
                    </Listeners>
                </ext:MenuItem>
            </Items>
        </StartMenu>
    </ext:Desktop>

    <uc:GameEditorWindow ID="gameEditorWindow" runat="server"/>
    <uc:TipEditorWindow ID="tipEditorWindow" runat="server"/>
    <uc:GameControlWindow ID="gameControlWindow" runat="server" />

    <ext:DesktopWindow 
        ID="winOldEditorBrowser" 
        runat="server" 
        Title="Старый редактор" 
        Icon="World"              
        Width="1000"
        Height="600"
        PageX="25" 
        PageY="25">
        <AutoLoad Url="/Games/Edit/" Mode="IFrame">
            <Params>
                <ext:Parameter Name="id" Value="18" Mode="Raw"/>
            </Params>
        </AutoLoad>
    </ext:DesktopWindow>    
</body>
</html>
