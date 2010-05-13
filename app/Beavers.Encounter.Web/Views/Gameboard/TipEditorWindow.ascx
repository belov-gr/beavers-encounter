<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Register Assembly="Coolite.Ext.Web" Namespace="Coolite.Ext.Web" TagPrefix="ext" %>
<ext:DesktopWindow 
    ID="winTipEditor"
    runat="server"
    Title="����� �������/���������"
    Icon="World"
    Width="400"
    Height="360"
    Modal="true"
    AutoLoad="false"
    Closable="false"
    Maximizable="false">
    <Body>
        <ext:FitLayout ID="tipFitLayout" runat="server">
            <ext:FormPanel ID="tipFormPanel" runat="server" Border="false" Url="/Gameboard/SaveTip" BodyStyle="padding:5px">
                <BaseParams>
                    <ext:Parameter Name="taskId" Value="getTaskID()" Mode="Raw"/>
                </BaseParams>
                <Body>
                    <ext:FormLayout ID="tipFormLayout" runat="server" LabelAlign="Top">
                        <ext:Anchor>
                            <ext:TextField ID="Tip_RequestVerificationToken" runat="server" Hidden="true" Text="123"></ext:TextField>
                        </ext:Anchor>
                        <ext:Anchor>
                            <ext:TextField ID="Tip_Id" runat="server" DataIndex="Id" Hidden="true"></ext:TextField>
                        </ext:Anchor>
                        <ext:Anchor Horizontal="100%">
                            <ext:TextArea ID="Tip_Name" runat="server" DataIndex="Name" FieldLabel="����� �������/���������" Height="200"></ext:TextArea>
                        </ext:Anchor>
                        <ext:Anchor>
                            <ext:ComboBox 
                                ID="Tip_SuspendTime" 
                                DataIndex="SuspendTime" 
                                FieldLabel="�����" 
                                Editable="true"
                                AllowNegative="false" 
                                AllowDecimals="false"   
                                EmptyText="����� � �������" 
                                AllowBlank="false"
                                Width="150">
                                <Items>
                                    <ext:ListItem Value="0" Text="0 �����" />
                                    <ext:ListItem Value="1" Text="1 ������" />
                                    <ext:ListItem Value="2" Text="2 ������" />
                                    <ext:ListItem Value="3" Text="3 ������" />
                                    <ext:ListItem Value="5" Text="5 �����" />
                                    <ext:ListItem Value="10" Text="10 �����" />
                                    <ext:ListItem Value="15" Text="15 �����" />
                                    <ext:ListItem Value="20" Text="20 �����" />
                                    <ext:ListItem Value="25" Text="25 �����" />
                                    <ext:ListItem Value="30" Text="30 �����" />
                                    <ext:ListItem Value="35" Text="35 �����" />
                                    <ext:ListItem Value="40" Text="40 �����" />
                                    <ext:ListItem Value="45" Text="45 �����" />
                                    <ext:ListItem Value="50" Text="50 �����" />
                                    <ext:ListItem Value="55" Text="55 �����" />
                                    <ext:ListItem Value="60" Text="1 ���" />
                                    <ext:ListItem Value="70" Text="1 ��� 10 �����" />
                                    <ext:ListItem Value="80" Text="1 ��� 20 �����" />
                                    <ext:ListItem Value="90" Text="1 ��� 30 �����" />
                                    <ext:ListItem Value="100" Text="1 ��� 40 �����" />
                                </Items>
                            </ext:ComboBox>
                        </ext:Anchor>
                    </ext:FormLayout>
                </Body>
                <Buttons>
                    <ext:Button ID="tipSaveBtn" runat="server" Text="���������" Icon="Disk">
                        <Listeners>
                            <Click Handler="#{tipFormPanel}.form.submit({waitMsg:'����������...', params:{id: getTipID()}, success: tipSaveSuccessHandler, failure: tipSaveFailureHandler});" />
                        </Listeners>
                    </ext:Button>
                    <ext:Button ID="tipCancelBtn" runat="server" Text="������" Icon="Cancel">
                        <Listeners>
                            <Click Fn="onCloseTipWindow" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
            </ext:FormPanel>
        </ext:FitLayout>
    </Body>
    <Listeners>
        <Show Handler="tipFormPanel.body.dom.__RequestVerificationToken.value = Ext.get('dummyForm').dom.__RequestVerificationToken.value;" />
        <Activate Fn="tipSetControlsNames" />
    </Listeners>
    <ToolTips>
        <ext:ToolTip runat="server" Target="Tip_Name" Width="420" AutoHide="false" Closable="true" Draggable="true" >
            <AutoLoad Url="/Content/Help/bbCode.html"/>
        </ext:ToolTip>
    </ToolTips>
</ext:DesktopWindow>
    
