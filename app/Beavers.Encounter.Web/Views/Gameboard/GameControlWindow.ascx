<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Game>" %>
<%@ Register Assembly="Coolite.Ext.Web" Namespace="Coolite.Ext.Web" TagPrefix="ext" %>

<ext:Store ID="dsTeamsState" runat="server" AutoLoad="false">
    <Proxy>
        <ext:HttpProxy Url="/Gameboard/GetTeamsState/"/>
    </Proxy>
    <Reader>
        <ext:JsonReader ReaderID="Id" Root="data" TotalProperty="totalCount">
            <Fields>
                <ext:RecordField Name="Id"/>
                <ext:RecordField Name="Name"/>
                <ext:RecordField Name="Task"/>
                <ext:RecordField Name="Time"/>
                <ext:RecordField Name="Accpt"/>
                <ext:RecordField Name="Success"/>
                <ext:RecordField Name="Overtime"/>
                <ext:RecordField Name="Canceled"/>
                <ext:RecordField Name="Cheat"/>
                <ext:RecordField Name="Tips"/>
                <ext:RecordField Name="CodesTotal"/>
                <ext:RecordField Name="CodesAccpt"/>
            </Fields>
        </ext:JsonReader>        
    </Reader>
    <BaseParams>
        <ext:Parameter Name="id" Value="gameId.value" Mode="Raw"/>
    </BaseParams>
    <Listeners>
        <BeforeLoad Handler="gdTeamsState.el.mask('��������...', 'x-mask-loading');" />
        <LoadException Handler="gdTeamsState.el.unmask();" />
        <Load Handler="gdTeamsState.el.unmask();" />
    </Listeners>
</ext:Store>

<ext:DesktopWindow
    ID="winGameControl" 
    runat="server" 
    Title="���������� �����" 
    Icon="Joystick"              
    Width="800"
    Height="480"
    PageX="200" 
    PageY="10"
    AutoLoad="true">
    <Body>
        <ext:BorderLayout runat="server">
            <North>
                <ext:Panel ID="gameStatePanel" runat="server" Height="114" Border="false" Collapsible="true" Title="������ ����������" TitleCollapse="false">
                    <Body>
                        <ext:TableLayout ID="TableLayout1" runat="server" Columns="3">
                            <ext:Cell RowSpan="2">
                                <ext:Panel ID="gameStateControlPanel" runat="server" Width="220" Height="114" Border="false">
                                    <Body>
                                        <ext:FormLayout ID="gameStateFormLayout" runat="server" LabelWidth="100">
                                            <ext:Anchor Horizontal="100%">
                                                <ext:RadioGroup ID="rgGameState" runat="server" FieldLabel="��������� ����" ColumnsNumber="1">
                                                    <Items>
                                                        <ext:Radio ID="radioPlanned" runat="server" BoxLabel="������������"/>
                                                        <ext:Radio ID="radioStartup" runat="server" BoxLabel="����������"/>
                                                        <ext:Radio ID="radioStarted" runat="server" BoxLabel="��������"/>
                                                        <ext:Radio ID="radioFinished" runat="server" BoxLabel="���������"/>
                                                        <ext:Radio ID="radioCloused" runat="server" BoxLabel="�������"/>
                                                    </Items>
                                                    <Listeners>
                                                        <Change Fn="onChangeGameState" />
                                                    </Listeners>
                                                </ext:RadioGroup>
                                            </ext:Anchor>
                                        </ext:FormLayout>
                                    </Body>
                                </ext:Panel>
                            </ext:Cell>
                            <ext:Cell>
                                <ext:Panel ID="Panel2" runat="server" Height="25" Border="false">
                                    <Body>
                                        <ext:Button ID="btnDownloadTelemetry" runat="server" Text="������� ����������" Icon="Television"></ext:Button>
                                    </Body>
                                </ext:Panel>
                            </ext:Cell>
                            <ext:Cell>
                                <ext:Panel ID="Panel3" runat="server" Height="25" Border="false">
                                    <Body>
                                        <ext:Button ID="btnShowGameRerults" runat="server" Text="�������� ����������" Icon="StarGold"></ext:Button>
                                    </Body>
                                </ext:Panel>
                            </ext:Cell>
                            <ext:Cell ColSpan="2">
                                <ext:Panel ID="pnlGameStateInfo" runat="server" Height="64" Border="false" BodyStyle="color: White;">
                                </ext:Panel>
                            </ext:Cell>
                        </ext:TableLayout>
                    </Body>
                </ext:Panel>
            </North>
            <Center>
                <ext:GridPanel ID="gdTeamsState" runat="server" StoreID="dsTeamsState" DisableSelection="true" Border="false" Title="������ ��������� ������">
                    <ColumnModel>
                        <Columns>
                            <ext:Column ColumnID="Name" DataIndex="Name" Header="��������<br/>�������">
                                <Renderer Handler="return '<b>'+record.data['Name']" /> 
                            </ext:Column>
                            <ext:Column ColumnID="Task" DataIndex="Task" Header="�����������<br/>�������" Sortable="false"></ext:Column>
                            <ext:Column ColumnID="Time" DataIndex="Time" Header="�����<br/>����������" Width="75">
                                <Renderer Format="Substr" FormatArgs="3,8" />
                            </ext:Column>
                            <ext:Column ColumnID="Accpt" DataIndex="Accpt" Header="�������<br/>��������" Width="60"></ext:Column>
                            <ext:Column ColumnID="Success" DataIndex="Success" Header="�������<br/>���������" Width="60"></ext:Column>
                            <ext:Column ColumnID="Overtime" DataIndex="Overtime" Header="�������<br/>����������" Width="60"></ext:Column>
                            <ext:Column ColumnID="Canceled" DataIndex="Canceled" Header="�������<br/>�����" Width="60"></ext:Column>
                            <ext:Column ColumnID="Cheat" DataIndex="Cheat" Header="�������<br/>��������" Width="60"></ext:Column>
                            <ext:Column ColumnID="Tips" DataIndex="Tips" Header="������<br/>���������" Width="60" Sortable="false"></ext:Column>
                            <ext:Column ColumnID="CodesTotal" DataIndex="CodesTotal" Header="�����<br/>����������" Width="60" Sortable="false"></ext:Column>
                            <ext:Column ColumnID="CodesAccpt" DataIndex="CodesAccpt" Header="�����<br/>��������" Width="60" Sortable="false"></ext:Column>
                        </Columns>
                    </ColumnModel>
                    <TopBar>
                        <ext:Toolbar>
                            <Items>
                                <ext:ToolbarButton ID="btnStartRefresh" runat="server" Text="��������� ��������������" Icon="ControlPlayBlue" Disabled="true">
                                    <Listeners>
                                        <Click Handler="el.disable();#{gameTaskManager}.startTask(gameTaskManager.tasks[0]);#{btnStopRefresh}.enable()"/>
                                    </Listeners>
                                </ext:ToolbarButton>
                                <ext:ToolbarButton ID="btnStopRefresh" runat="server" Text="���������� ��������������" Icon="ControlStopBlue">
                                    <Listeners>
                                        <Click Handler="el.disable();#{gameTaskManager}.stopTask(gameTaskManager.tasks[0]);#{btnStartRefresh}.enable()"/>
                                    </Listeners>
                                </ext:ToolbarButton>
                                <ext:ComboBox ID="cbRefreshInterval" runat="server" Editable="false">
                                    <Items>
                                        <ext:ListItem Value="5000" Text="5 ������" />
                                        <ext:ListItem Value="10000" Text="10 ������" />
                                        <ext:ListItem Value="15000" Text="15 ������" />
                                        <ext:ListItem Value="30000" Text="30 ������" />
                                        <ext:ListItem Value="60000" Text="1 ������" />
                                        <ext:ListItem Value="300000" Text="5 �����" />
                                        <ext:ListItem Value="600000" Text="10 �����" />
                                    </Items>
                                    <SelectedItem Value="5000" />
                                    <Listeners>
                                        <Select Handler="#{gameTaskManager}.tasks[0].interval = record.data.value;" />
                                    </Listeners>
                                </ext:ComboBox>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                </ext:GridPanel>
            </Center>
            <South>
                <ext:Label Text="South"></ext:Label>
            </South>
        </ext:BorderLayout>    
    </Body>
    <Listeners>
        <Show Handler="#{gameTaskManager}.startAll();" />
        <Hide Handler="#{gameTaskManager}.stopAll();" />
    </Listeners>
</ext:DesktopWindow>

<ext:TaskManager ID="gameTaskManager" runat="server">
    <Tasks>
        <ext:Task 
            TaskID="refreshTeamsState" 
            AutoRun="false" 
            Interval="5000">
            <Listeners>
                <Update Handler="dsTeamsState.reload();" />
            </Listeners>
        </ext:Task>
        <ext:Task 
            TaskID="refreshGameState" 
            AutoRun="false" 
            Interval="5000">
            <Listeners>
                <Update Fn="onGameStateUpdate" />
            </Listeners>
        </ext:Task>
    </Tasks>
</ext:TaskManager>