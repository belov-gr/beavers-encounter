<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Register Assembly="Coolite.Ext.Web" Namespace="Coolite.Ext.Web" TagPrefix="ext" %>
<ext:FitLayout ID="codesFitLayout" runat="server">
    <ext:GridPanel 
        ID="codesGrid" 
        runat="server" 
        StoreID="dsCodes" 
        DisableSelection="true" 
        Border="false"
        EnableColumnHide="false"
        ClicksToEdit="1" 
        >
        <TopBar>
            <ext:Toolbar runat="server">
                <Items>
                    <ext:ToolbarButton ID="btnAddCode" runat="server" Text="Добавить код" Icon="Add">
                        <Listeners>
                            <Click Handler="#{codesGrid}.insertRecord(0, {});#{codesGrid}.getView().focusRow(0);#{codesGrid}.startEditing(1, 0);" />
                        </Listeners>
                    </ext:ToolbarButton>
                    <ext:ToolbarButton ID="btnSaveCodes" runat="server" Text="Сохранить" Icon="Disk" Disabled="true">
                        <Listeners>
                            <Click Handler="#{dsCodes}.save();#{btnSaveCodes}.setDisabled(true);" />
                        </Listeners>
                    </ext:ToolbarButton>
                </Items>
            </ext:Toolbar>
        </TopBar>
        <ColumnModel>
            <Columns>
                <ext:ImageCommandColumn ColumnID="cmd" Sortable="false" Width="34">
                    <Commands>
                        <ext:ImageCommand CommandName="Delete" Icon="Cross">
                            <ToolTip Text="Удалить код" />
                        </ext:ImageCommand>
                    </Commands>
                </ext:ImageCommandColumn>
                <ext:Column ColumnID="Name" DataIndex="Name" Header="Код" Width="100">
                    <Editor>
                        <ext:NumberField AllowBlank="false" AllowDecimals="false" AllowNegative="false" MaxLengthText="4"></ext:NumberField>
                    </Editor>
                </ext:Column>
                <ext:Column ColumnID="Danger" DataIndex="Danger" Header="КО" Width="100">
                    <Editor>
                        <ext:TextField AllowBlank="false" MaxLengthText="50"></ext:TextField>
                    </Editor>
                </ext:Column>
                <ext:CheckColumn ColumnID="IsBonus" DataIndex="IsBonus" Header="Бонус?" Width="60" Editable="true">
                </ext:CheckColumn>
            </Columns>
        </ColumnModel>
        <Listeners>
            <AfterEdit Handler="#{btnSaveCodes}.enable();animateButton(#{btnSaveCodes}.btnEl);" />
            <Command Handler="#{codesGrid}.deleteRecord(record);#{btnSaveCodes}.enable();animateButton(#{btnSaveCodes}.btnEl);" />
        </Listeners>
    </ext:GridPanel>
</ext:FitLayout>

