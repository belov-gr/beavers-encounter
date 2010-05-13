<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Register Assembly="Coolite.Ext.Web" Namespace="Coolite.Ext.Web" TagPrefix="ext" %>
<ext:FitLayout ID="TipsFitLayout" runat="server">
    <ext:GridPanel 
        ID="TipsGrid" 
        runat="server" 
        StoreID="dsTips" 
        DisableSelection="true" 
        Border="false"
        EnableColumnHide="false"
        HideHeaders="true">
        <TopBar>
            <ext:Toolbar ID="Toolbar1" runat="server">
                <Items>
                    <ext:Button ID="btnAddTip" runat="server" Text="Добавить подсказку" Icon="Add">
                        <Listeners>
                            <Click Fn="onClickAddTip" />
                        </Listeners>
                    </ext:Button>
                </Items>
            </ext:Toolbar>
        </TopBar>
        <ColumnModel>
            <Columns>
                <ext:ImageCommandColumn ColumnID="cmd" Sortable="false" Width="64">
                    <Commands>
                        <ext:ImageCommand CommandName="Edit" Icon="Pencil">
                            <ToolTip Text="Изменить" />
                        </ext:ImageCommand>
                        <ext:ImageCommand CommandName="Delete" Icon="Cross">
                            <ToolTip Text="Удалить" />
                        </ext:ImageCommand>
                    </Commands>
                </ext:ImageCommandColumn>
                <ext:Column ColumnID="Title" DataIndex="SuspendTime" Header="Тип" Sortable="false" Width="400">
                    <Renderer Fn="tipRecordTitleTmpl"/>
                </ext:Column>
                <ext:Column ColumnID="SuspendTime" DataIndex="SuspendTime" Header="Время" Sortable="false" Width="80">
                    <Renderer Handler="return '' + record.data.SuspendTime + ' минут'" />
                </ext:Column>
            </Columns>
        </ColumnModel>
        <View>
            <ext:GridView ID="GridView1" ForceFit="true" EnableRowBody="true" runat="server">
                <GetRowClass Handler="rowParams.body = '<p>' + parseBBCode(record.data.Name) + '</p>'; return 'x-grid3-row-expanded';" />
            </ext:GridView>
        </View>
        <Listeners>
            <Command Fn="tipsGridCommand" />
        </Listeners>
    </ext:GridPanel>
</ext:FitLayout>

