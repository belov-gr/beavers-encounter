<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="Beavers.Encounter.Core"%>
<%@ Register Assembly="Coolite.Ext.Web" Namespace="Coolite.Ext.Web" TagPrefix="ext" %>
<script runat="server">
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Ext.IsAjaxRequest)
        {
            this.Game_GameDate.Text = ((Game) ViewData.Model).GameDate.ToString();
            this.Game_GameDateDate.SelectedDate = ((Game)ViewData.Model).GameDate.Date;
            this.Game_GameDateTime.SelectedTime = ((Game)ViewData.Model).GameDate.TimeOfDay;
        }
    }
</script>

<ext:FitLayout ID="gameFitLayout1" runat="server">
    <ext:Panel ID="gamePanelTopBars" runat="server" Border="false">
        <TopBar>
            <ext:Toolbar ID="gameToolbar" runat="server">
                <Items>
                    <ext:ToolbarButton ID="gameSaveBtn" runat="server" Text="���������" Icon="Disk">
                        <Listeners>
                            <Click Handler="#{gameFormPanel}.form.submit({waitMsg:'����������...', params:{id: getGameID()}, success: gameSaveSuccessHandler, failure: gameSaveFailureHandler});" />
                        </Listeners>
                    </ext:ToolbarButton>
                </Items>
            </ext:Toolbar>
        </TopBar>
        <Body>
            <ext:FitLayout ID="gameFitLayout2" runat="server">
                <ext:FormPanel ID="gameFormPanel" runat="server" Border="false" BodyStyle="padding:5px" Height="0" Width="0" Url="/Gameboard/SaveGame/">
                    <Body>
                        <ext:FormLayout ID="FormLayout1" runat="server" LabelWidth="156">
                            <ext:Anchor>
                                <ext:TextField ID="Game_Id" runat="server" DataIndex="Id" Hidden="true" />
                            </ext:Anchor>
                            <ext:Anchor Horizontal="100%">
                                <ext:TextField ID="Game_Name" DataIndex="Name" FieldLabel="��������" Width="250" AllowBlank="false" EmptyText="������� �������� ����" MaxLengthText="250"/>
                            </ext:Anchor>
                            <ext:Anchor>
                                <ext:TextField ID="Game_GameDate" runat="server" Hidden="true" DataIndex="GameDate" FieldLabel="����" Width="180" AllowBlank="true">
                                </ext:TextField>
                            </ext:Anchor>
                            <ext:Anchor>
                                <ext:MultiField ID="MultiField1" FieldLabel="���� � ����� ����������" runat="server">
                                    <Fields>
                                        <ext:DateField ID="Game_GameDateDate" runat="server" DataIndex="GameDate" Width="90" AllowBlank="false">
                                            <Listeners>
                                                <Change Handler="Game_GameDate.setRawValue(Game_GameDateDate.value + ' ' + Game_GameDateTime.value);" />
                                            </Listeners>
                                        </ext:DateField>
                                        <ext:TimeField ID="Game_GameDateTime" runat="server" DataIndex="GameTime" Width="80" AllowBlank="false">
                                            <Listeners>
                                                <Change Handler="Game_GameDate.setRawValue(Game_GameDateDate.value + ' ' + Game_GameDateTime.value);" />
                                            </Listeners>
                                        </ext:TimeField>
                                    </Fields>
                                </ext:MultiField>
                            </ext:Anchor>
                            <ext:Anchor>
                                <ext:ComboBox 
                                    ID="Game_TotalTime" 
                                    DataIndex="TotalTime" 
                                    FieldLabel="����������������� ����" 
                                    Editable="true"
                                    AllowNegative="false" 
                                    AllowDecimals="false"   
                                    EmptyText="����� � �������" 
                                    AllowBlank="false"
                                    Width="173">
                                    <Items>
                                        <ext:ListItem Value="10" Text="10 �����" />
                                        <ext:ListItem Value="20" Text="20 �����" />
                                        <ext:ListItem Value="30" Text="30 �����" />
                                        <ext:ListItem Value="60" Text="1 ���" />
                                        <ext:ListItem Value="120" Text="2 ����" />
                                        <ext:ListItem Value="180" Text="3 ����" />
                                        <ext:ListItem Value="240" Text="4 ����" />
                                        <ext:ListItem Value="300" Text="5 �����" />
                                        <ext:ListItem Value="360" Text="6 �����" />
                                        <ext:ListItem Value="420" Text="7 �����" />
                                        <ext:ListItem Value="480" Text="8 �����" />
                                        <ext:ListItem Value="540" Text="9 �����" />
                                    </Items>
                                </ext:ComboBox>
                            </ext:Anchor>
                            <ext:Anchor>
                                <ext:ComboBox 
                                    ID="Game_TimePerTask" 
                                    DataIndex="TimePerTask" 
                                    FieldLabel="����� �� �������" 
                                    Editable="true"
                                    AllowNegative="false" 
                                    AllowDecimals="false"   
                                    EmptyText="����� � �������" 
                                    AllowBlank="false"
                                    Width="173">
                                    <Items>
                                        <ext:ListItem Value="1" Text="1 ������" />
                                        <ext:ListItem Value="2" Text="2 ������" />
                                        <ext:ListItem Value="3" Text="3 ������" />
                                        <ext:ListItem Value="5" Text="5 �����" />
                                        <ext:ListItem Value="10" Text="10 �����" />
                                        <ext:ListItem Value="15" Text="15 �����" />
                                        <ext:ListItem Value="20" Text="20 �����" />
                                        <ext:ListItem Value="25" Text="25 �����" />
                                        <ext:ListItem Value="30" Text="30 �����" />
                                        <ext:ListItem Value="40" Text="40 �����" />
                                        <ext:ListItem Value="50" Text="50 �����" />
                                        <ext:ListItem Value="60" Text="1 ���" />
                                        <ext:ListItem Value="90" Text="1 ��� 30 �����" />
                                        <ext:ListItem Value="120" Text="2 ����" />
                                    </Items>
                                </ext:ComboBox>
                            </ext:Anchor>
                            <ext:Anchor>
                                <ext:ComboBox 
                                    ID="Game_TimePerTip" 
                                    DataIndex="TimePerTip" 
                                    FieldLabel="����� �� ���������" 
                                    Editable="true"
                                    AllowNegative="false" 
                                    AllowDecimals="false"   
                                    EmptyText="����� � �������" 
                                    AllowBlank="false"
                                    Width="173">
                                    <Items>
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
                                    </Items>
                                </ext:ComboBox>
                            </ext:Anchor>
                            <ext:Anchor>
                                <ext:TextField ID="Game_PrefixMainCode" DataIndex="PrefixMainCode" FieldLabel="������� ��������� ����" AllowBlank="false" MaxLength="6" Width="173"/>
                            </ext:Anchor>
                            <ext:Anchor>
                                <ext:TextField ID="Game_PrefixBonusCode" DataIndex="PrefixBonusCode" FieldLabel="������� ��������� ����" AllowBlank="true" MaxLength="6" Width="173"/>
                            </ext:Anchor>
                            <ext:Anchor>
                                <ext:Panel ID="pnlDescription" runat="server" Border="false" BodyStyle="background: transparent;">
                                    <Body>
                                        <ext:FormLayout ID="FormLayout2" runat="server" LabelAlign="Top" StyleSpec="background: transparent">
                                            <ext:Anchor Horizontal="100%">
                                                <ext:TextArea ID="Game_Description" runat="server" DataIndex="Description" FieldLabel="�������� ����" Height="185"></ext:TextArea>
                                            </ext:Anchor>
                                        </ext:FormLayout>
                                    </Body>
                                </ext:Panel>
                            </ext:Anchor>
                        </ext:FormLayout>
                    </Body>
                </ext:FormPanel>
            </ext:FitLayout>
            <ext:ToolTip runat="server" Target="Game_Name" Html="�������� ����"/>
            <ext:ToolTip runat="server" Target="MultiField1" Html="���� ���������� ����"/>
            <ext:ToolTip runat="server" Target="Game_PrefixMainCode" Html="��������, 14DR"/>
            <ext:ToolTip runat="server" Target="Game_PrefixBonusCode" Html="��������, 14B"/>
        </Body>
    </ext:Panel>
</ext:FitLayout>
