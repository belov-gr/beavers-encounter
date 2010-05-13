<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Game>" %>
<%@ Register Assembly="Coolite.Ext.Web" Namespace="Coolite.Ext.Web" TagPrefix="ext" %>
<%@ Register src="~/Views/Gameboard/GamePanel.ascx" tagname="GamePanel" tagprefix="uc" %>
<%@ Register src="~/Views/Gameboard/TaskPanel.ascx" tagname="TaskPanel" tagprefix="uc" %>
<%@ Register src="~/Views/Gameboard/BonusPanel.ascx" tagname="BonusPanel" tagprefix="uc" %>
<ext:DesktopWindow 
    ID="winGameEditor" 
    runat="server" 
    Title="Редактор игры" 
    Icon="ApplicationFormEdit"             
    Width="640"
    Height="480"
    PageX="25" 
    PageY="25"
    AutoLoad="true">
    <Body>
        <ext:Hidden ID="gameId" runat="server" Text='<%# ViewData.Model.Id %>' AutoDataBind="true" />
        <ext:BorderLayout ID="BorderLayout1" runat="server">
            <West Collapsible="true" Split="true">
                <ext:FormPanel ID="treeForm" runat="server" AutoScroll="true" Border="false">
                    <Body>
                        <ext:TreePanel 
                            ID="GameTree" 
                            runat="server" 
                            Width="200" 
                            AutoScroll="false"
                            Border="false"
                            DDGroup="grp2"
                            EnableDD="true">
                            <Root>
                                <ext:TreeNode NodeID="root" Text="Игра" Expanded="true">
                                    <Nodes>
                                        <ext:AsyncTreeNode NodeID="tasks" Text="Задания"/>
                                        <ext:AsyncTreeNode NodeID="bonuses" Text="Бонусы"/>
                                        <ext:AsyncTreeNode NodeID="teams" Text="Команды"/>
                                    </Nodes>
                                </ext:TreeNode>
                            </Root>
                            <AjaxEvents>
                                <BeforeLoad
                                    Url="/Gameboard/NodeLoad"
                                    CleanRequest="true"
                                    Method="POST"
                                    IsUpload="true"
                                    Failure="Ext.Msg.show({title:'Ошибка',msg: result.errorMessage + ' ' + result.responseText,buttons: Ext.Msg.OK,icon: Ext.Msg.ERROR});"
                                    Success="node.loadNodes(Ext.decode(response.responseText));">
                                    <ExtraParams>
                                        <ext:Parameter Name="gameId" Value="gameId.value" Mode="Raw"/>
                                        <ext:Parameter Name="nodeId" Value="node.id" Mode="Raw"/>
                                    </ExtraParams>
                                </BeforeLoad>
                            </AjaxEvents>
                            <Listeners>
                                <Click Fn="onClickTreeNode" />
                                <ContextMenu Fn="onShowTreeContextMenu" />
                            </Listeners>
                        </ext:TreePanel>
                    </Body>
                </ext:FormPanel>
            </West>
            <Center>
                <ext:TabPanel ID="tpMain" runat="server" ActiveTabIndex="0" Border="false" AnimScroll="true" AutoScroll="true" EnableTabScroll="true" >
                    <Tabs>
                        <ext:Tab ID="tabGame" runat="server" Title="Игра">
                            <Body>
                                <uc:GamePanel ID="GamePanel" runat="server" />
                            </Body>
                            <Listeners>
                                <Activate Handler="dsGame.reload()" />
                            </Listeners>
                        </ext:Tab>
                        <ext:Tab ID="tabTask" runat="server" Title="Задания" Disabled="true">
                            <Body>
                                <uc:TaskPanel ID="TaskPanel" runat="server" />
                            </Body>
                        </ext:Tab>
                        <ext:Tab ID="tabBonus" runat="server" Title="Бонусы" Disabled="true">
                            <Body>
                                <uc:BonusPanel ID="BonusPanel" runat="server" />
                            </Body>
                        </ext:Tab>
                    </Tabs>
                    <Plugins>
                        <ext:TabCloseMenu ID="TabCloseMenu1" runat="server"/>
                    </Plugins>
                </ext:TabPanel>
            </Center>
        </ext:BorderLayout>
    </Body>
    <Listeners>
        <Activate Fn="gameSetControlsNames" />
    </Listeners>
</ext:DesktopWindow>

<ext:Menu ID="TreeContextMenu" runat="server">
    <Items>
        <ext:MenuItem ID="tcmAdd" Text="Создать" Icon="Add">
            <Listeners>
                <Click Fn="onClickTreeContextMenu" />
            </Listeners>
        </ext:MenuItem>
        <ext:MenuItem ID="tcmRefresh" Text="Обновить" Icon="PageRefresh">
            <Listeners>
                <Click Fn="onClickTreeContextMenu" />
            </Listeners>
        </ext:MenuItem>
    </Items>
</ext:Menu>
