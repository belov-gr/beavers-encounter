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
        <BeforeLoad Handler="gdTeamsState.el.mask('Загрузка...', 'x-mask-loading');" />
        <LoadException Handler="gdTeamsState.el.unmask();" />
        <Load Handler="gdTeamsState.el.unmask();" />
    </Listeners>
</ext:Store>

<ext:DesktopWindow
    ID="winGameControl" 
    runat="server" 
    Title="Управление игрой" 
    Icon="Joystick"              
    Width="800"
    Height="480"
    PageX="200" 
    PageY="10"
    AutoLoad="true">
    <Body>
        <ext:BorderLayout runat="server">
            <North>
                <ext:Panel ID="gameStatePanel" runat="server" Height="114" Border="false" Collapsible="true" Title="Панель управления" TitleCollapse="false">
                    <Body>
                        <ext:TableLayout ID="TableLayout1" runat="server" Columns="3">
                            <ext:Cell RowSpan="2">
                                <ext:Panel ID="gameStateControlPanel" runat="server" Width="220" Height="114" Border="false">
                                    <Body>
                                        <ext:FormLayout ID="gameStateFormLayout" runat="server" LabelWidth="100">
                                            <ext:Anchor Horizontal="100%">
                                                <ext:RadioGroup ID="rgGameState" runat="server" FieldLabel="Состояние игры" ColumnsNumber="1">
                                                    <Items>
                                                        <ext:Radio ID="radioPlanned" runat="server" BoxLabel="Планирование"/>
                                                        <ext:Radio ID="radioStartup" runat="server" BoxLabel="Подготовка"/>
                                                        <ext:Radio ID="radioStarted" runat="server" BoxLabel="Запущена"/>
                                                        <ext:Radio ID="radioFinished" runat="server" BoxLabel="Завершена"/>
                                                        <ext:Radio ID="radioCloused" runat="server" BoxLabel="Закрыта"/>
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
                                        <ext:Button ID="btnDownloadTelemetry" runat="server" Text="Скачать телеметрию" Icon="Television"></ext:Button>
                                    </Body>
                                </ext:Panel>
                            </ext:Cell>
                            <ext:Cell>
                                <ext:Panel ID="Panel3" runat="server" Height="25" Border="false">
                                    <Body>
                                        <ext:Button ID="btnShowGameRerults" runat="server" Text="Показать результаты" Icon="StarGold"></ext:Button>
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
                <ext:GridPanel ID="gdTeamsState" runat="server" StoreID="dsTeamsState" DisableSelection="true" Border="false" Title="Панель состояния команд">
                    <ColumnModel>
                        <Columns>
                            <ext:Column ColumnID="Name" DataIndex="Name" Header="Название<br/>команды">
                                <Renderer Handler="return '<b>'+record.data['Name']" /> 
                            </ext:Column>
                            <ext:Column ColumnID="Task" DataIndex="Task" Header="Выполняемое<br/>задание" Sortable="false"></ext:Column>
                            <ext:Column ColumnID="Time" DataIndex="Time" Header="Время<br/>выполнения" Width="75">
                                <Renderer Format="Substr" FormatArgs="3,8" />
                            </ext:Column>
                            <ext:Column ColumnID="Accpt" DataIndex="Accpt" Header="Заданий<br/>получено" Width="60"></ext:Column>
                            <ext:Column ColumnID="Success" DataIndex="Success" Header="Заданий<br/>выполнено" Width="60"></ext:Column>
                            <ext:Column ColumnID="Overtime" DataIndex="Overtime" Header="Заданий<br/>просрочено" Width="60"></ext:Column>
                            <ext:Column ColumnID="Canceled" DataIndex="Canceled" Header="Заданий<br/>слито" Width="60"></ext:Column>
                            <ext:Column ColumnID="Cheat" DataIndex="Cheat" Header="Заданий<br/>забанено" Width="60"></ext:Column>
                            <ext:Column ColumnID="Tips" DataIndex="Tips" Header="Выдано<br/>подсказок" Width="60" Sortable="false"></ext:Column>
                            <ext:Column ColumnID="CodesTotal" DataIndex="CodesTotal" Header="Кодов<br/>необходимо" Width="60" Sortable="false"></ext:Column>
                            <ext:Column ColumnID="CodesAccpt" DataIndex="CodesAccpt" Header="Кодов<br/>получено" Width="60" Sortable="false"></ext:Column>
                        </Columns>
                    </ColumnModel>
                    <TopBar>
                        <ext:Toolbar>
                            <Items>
                                <ext:ToolbarButton ID="btnStartRefresh" runat="server" Text="Запустить автообновление" Icon="ControlPlayBlue" Disabled="true">
                                    <Listeners>
                                        <Click Handler="el.disable();#{gameTaskManager}.startTask(gameTaskManager.tasks[0]);#{btnStopRefresh}.enable()"/>
                                    </Listeners>
                                </ext:ToolbarButton>
                                <ext:ToolbarButton ID="btnStopRefresh" runat="server" Text="Остановить автообновление" Icon="ControlStopBlue">
                                    <Listeners>
                                        <Click Handler="el.disable();#{gameTaskManager}.stopTask(gameTaskManager.tasks[0]);#{btnStartRefresh}.enable()"/>
                                    </Listeners>
                                </ext:ToolbarButton>
                                <ext:ComboBox ID="cbRefreshInterval" runat="server" Editable="false">
                                    <Items>
                                        <ext:ListItem Value="5000" Text="5 секунд" />
                                        <ext:ListItem Value="10000" Text="10 секунд" />
                                        <ext:ListItem Value="15000" Text="15 секунд" />
                                        <ext:ListItem Value="30000" Text="30 секунд" />
                                        <ext:ListItem Value="60000" Text="1 минута" />
                                        <ext:ListItem Value="300000" Text="5 минут" />
                                        <ext:ListItem Value="600000" Text="10 минут" />
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