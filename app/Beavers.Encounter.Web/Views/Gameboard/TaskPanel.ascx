<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Register Assembly="Coolite.Ext.Web" Namespace="Coolite.Ext.Web" TagPrefix="ext" %>
<%@ Register src="~/Views/Gameboard/TipsPanel.ascx" tagname="TipsPanel" tagprefix="uc" %>
<%@ Register src="~/Views/Gameboard/CodesPanel.ascx" tagname="CodesPanel" tagprefix="uc" %>
<ext:FitLayout ID="taskFitLayoutTopBars" runat="server">
<ext:Panel runat="server" Border="false">
<Body>
<ext:Accordion ID="taskAccordion" runat="server" Animate="true"  >
    <ext:Panel ID="taskPanelTopBars" runat="server" Border="false" Title="�������� �������" Icon="Report">
        <TopBar>
            <ext:Toolbar ID="taskToolbar" runat="server">
                <Items>
                    <ext:ToolbarButton ID="taskSaveBtn" runat="server" Text="���������" Icon="Disk">
                        <Listeners>
                            <Click Handler="#{taskFormPanel}.form.submit({waitMsg:'����������...', params:{id: getTaskID()}, success: taskSaveSuccessHandler, failure: taskSaveFailureHandler});" />
                        </Listeners>
                    </ext:ToolbarButton>
                    <ext:ToolbarButton ID="taskNewBtn" runat="server" Text="����� �������" Icon="Add">
                        <AjaxEvents>
                            <Click Url="/Gameboard/CreateTask/"
                                CleanRequest="false"
                                Method="POST"
                                IsUpload="false"
                                Failure="Ext.Msg.show({title:'������',msg: result.errorMessage + ' ' + result.responseText,buttons: Ext.Msg.OK,icon: Ext.Msg.ERROR});"
                                Success="createTask(response, result, el, type, action, extraParams);"
                                FormID="taskFormPanel">
                            </Click>
                        </AjaxEvents>
                    </ext:ToolbarButton>
                    <ext:ToolbarButton ID="taskDeleteBtn" runat="server" Text="�������" Icon="Cross">
                        <AjaxEvents>
                            <Click Url="/Gameboard/DeleteTask/"
                                CleanRequest="true"
                                Method="POST"
                                IsUpload="false"
                                Failure="Ext.Msg.show({title:'������',msg: result.errorMessage + ' ' + result.responseText,buttons: Ext.Msg.OK,icon: Ext.Msg.ERROR});"
                                Success="successDeleteTask(response, result, el, type, action, extraParams);"
                                FormID="taskFormPanel">
                                <Confirmation Message="������� �������?" Title="��������" ConfirmRequest="true" />
                                <ExtraParams>
                                    <ext:Parameter Name="id" Value="Task_Id.value" Mode="Raw"></ext:Parameter>
                                </ExtraParams>
                            </Click>
                        </AjaxEvents>
                    </ext:ToolbarButton>
                </Items>
            </ext:Toolbar>
        </TopBar>
        <Body>
            <ext:FitLayout ID="taskFitLayout1" runat="server">
                <ext:FormPanel ID="taskFormPanel" runat="server" Border="false" BodyStyle="padding:5px" Url="/Gameboard/SaveTask/">
                    <Listeners>
                        <Show Handler="taskFormPanel.body.dom.__RequestVerificationToken.value = Ext.get('dummyForm').dom.__RequestVerificationToken.value;" />
                    </Listeners>
                    <Body>
                        <ext:FormLayout ID="taskFormLayout1" runat="server" LabelWidth="156">
                            <ext:Anchor>
                                <ext:TextField ID="__RequestVerificationToken" runat="server" Hidden="true" Text="123"></ext:TextField>
                            </ext:Anchor>
                            <ext:Anchor>
                                <ext:TextField ID="Task_Id" runat="server" DataIndex="Id" Hidden="true" />
                            </ext:Anchor>
                            <ext:Anchor Horizontal="100%">
                                <ext:TextField ID="Task_Name" runat="server" DataIndex="Name" FieldLabel="������� ��������" Width="250" AllowBlank="false" EmptyText="������� ������� ��������">
                                    <Listeners>
                                        <Change Fn="taskChange" />
                                    </Listeners>
                                </ext:TextField>
                            </ext:Anchor>
                            <ext:Anchor>
                                <ext:ComboBox ID="Task_TaskType" runat="server" DataIndex="TaskType" FieldLabel="��� �������" AllowBlank="false" Width="200">
                                    <Items>
                                        <ext:ListItem Value="Classic" Text="������������ �������" />
                                        <ext:ListItem Value="NeedForSpeed" Text="������� � ����������" />
                                        <ext:ListItem Value="RussianRoulette" Text="������� � ������� ���������" />
                                    </Items>
                                    <Listeners>
                                        <Change Fn="taskChange" />
                                    </Listeners>
                                </ext:ComboBox>
                            </ext:Anchor>
                            <ext:Anchor>
                                <ext:NumberField ID="Task_Priority" runat="server" DataIndex="Priority" FieldLabel="��������� �������" AllowDecimals="false" AllowNegative="true">
                                    <Listeners>
                                        <Change Fn="taskChange" />
                                    </Listeners>
                                </ext:NumberField>
                            </ext:Anchor>
                            <ext:Anchor>
                                <ext:Checkbox ID="Task_StreetChallendge" runat="server" DataIndex="StreetChallendge" FieldLabel="Street Challenge">
                                    <Listeners>
                                        <Change Fn="taskChange" />
                                    </Listeners>
                                </ext:Checkbox>
                            </ext:Anchor>
                            <ext:Anchor>
                                <ext:Checkbox ID="Task_Agents" runat="server" DataIndex="Agents" FieldLabel="������� � ��������" >
                                    <Listeners>
                                        <Change Fn="taskChange" />
                                    </Listeners>
                                </ext:Checkbox>
                            </ext:Anchor>
                            <ext:Anchor>
                                <ext:Checkbox ID="Task_Locked" runat="server" DataIndex="Locked" FieldLabel="������� ��������������">
                                    <Listeners>
                                        <Change Fn="taskChange" />
                                    </Listeners>
                                </ext:Checkbox>
                            </ext:Anchor>
                            <ext:Anchor Horizontal="100%">
                                <ext:Panel ID="pnlTaskProp2" runat="server" Border="false" BodyStyle="background: transparent;">
                                    <Body>
                                        <ext:FormLayout ID="taskFormLayout2" runat="server" LabelAlign="Top" StyleSpec="background: transparent">
                                            <ext:Anchor Horizontal="100%">
                                                <ext:MultiField FieldLabel="�� �����">
                                                    <Fields>
                                                        <ext:TextField ID="Task_NotAfterTasks" runat="server" Hidden="true" DataIndex="NotAfterTasks" />
                                                        <ext:Button ID="NotAfterTasksBtn" runat="server" Text="��������">
                                                            <Menu>
                                                                <ext:Menu MinWidth="180">
                                                                    <Listeners>
                                                                        <BeforeShow Fn="FillMenuNotAfterTasks" />
                                                                        <Click Fn="NotAfterTasksBtnClick" />
                                                                    </Listeners>
                                                                </ext:Menu>
                                                            </Menu>
                                                        </ext:Button>
                                                        <ext:TextField ID="NotAfterTasksLabel" runat="server" DataIndex="NotAfterTasksLabel" ReadOnly="true" Width="300" StyleSpec="border:0px solid #B5B8C8;background: transparent;"/>
                                                    </Fields>
                                                </ext:MultiField>
                                            </ext:Anchor>
                                            <ext:Anchor Horizontal="100%">
                                                <ext:MultiField FieldLabel="�� ������">
                                                    <Fields>
                                                        <ext:TextField ID="Task_NotOneTimeTasks" runat="server" Hidden="true" DataIndex="NotOneTimeTasks" />
                                                        <ext:Button ID="NotOneTimeTasksBtn" runat="server" Text="��������">
                                                            <Menu>
                                                                <ext:Menu MinWidth="180">
                                                                    <Listeners>
                                                                        <BeforeShow Fn="FillMenuNotOneTimeTasks" />
                                                                        <Click Fn="NotOneTimeTasksBtnClick" />
                                                                    </Listeners>
                                                                </ext:Menu>
                                                            </Menu>
                                                        </ext:Button>
                                                        <ext:TextField ID="NotOneTimeTasksLabel" runat="server" DataIndex="NotOneTimeTasksLabel" ReadOnly="true" Width="300" StyleSpec="border:0px solid #B5B8C8;background: transparent;"/>
                                                    </Fields>
                                                </ext:MultiField>
                                            </ext:Anchor>
                                        </ext:FormLayout>
                                    </Body>
                                </ext:Panel>
                            </ext:Anchor>
                        </ext:FormLayout>
                    </Body>
                </ext:FormPanel>
            </ext:FitLayout>
            <ext:ToolTip runat="server" Target="Task_Name" Html="� �������� ���� ������� �� ������ ��� ��������, ������� �������� ������� �������� ������ ������� ����. ��������, �������."></ext:ToolTip>
            <ext:ToolTip runat="server" Target="Task_StreetChallendge" Html="������ ������� ���������, ��� ������� ����� ������ ���� �������� ��� ������ ������� � ������ ����."/>
            <ext:ToolTip runat="server" Target="Task_Agents" Html="������� ������������ ��� ������������� ������� ���, ����� ������� � �������� ����������� ������������� ������ ����� ��������. ��������, ������� � +500."/>
            <ext:ToolTip runat="server" Target="Task_Locked" Html="���� ���������� ������ �������, �� ������� �� ����� ���������� ��������. ���� ������� ����� �������������/������� � �������� ����."/>
            <ext:ToolTip runat="server" Target="Task_TaskType" Html="0 - ������������ �������, 1 - ������� � ����������, 2 - ������� � ������� ���������."/>
            <ext:ToolTip runat="server" Target="Task_Priority" Title="��������� �������" AutoHide="false" Closable="true" Draggable="true" Html="��������� ����� ���� ������������� ��� �������������. ��������� ������ 100 ��������� ������� �������� �������� ��� �������, ��� ���� ������������ ������ ������� ������������ ������ ��������� 3-4 �������. ��� ���������� 150 ������������ ������� ������������ ����� ��������� 4-5 ������. ������������� ��������� ��������� ����������� ������ ������� ��������."/>
        </Body> 
    </ext:Panel>
    <ext:Panel ID="tipsAccPanel" runat="server" Title="����� ������� � ���������" Border="false" Icon="Note">
        <Body>
            <uc:TipsPanel ID="tipsPenel" runat="server" />
        </Body>
    </ext:Panel>
    <ext:Panel ID="codesAccPanel" runat="server" Title="����" Border="false" Icon="Pill">
        <Body>
            <uc:CodesPanel ID="codesPanel" runat="server" />
        </Body>
    </ext:Panel>
</ext:Accordion>
</Body>
</ext:Panel>
</ext:FitLayout>
