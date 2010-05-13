<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Register Assembly="Coolite.Ext.Web" Namespace="Coolite.Ext.Web" TagPrefix="ext" %>
<ext:FitLayout ID="bonusFitLayoutTopBars" runat="server">
<ext:Panel runat="server" Border="false">
    <TopBar>
        <ext:Toolbar ID="bonusToolbar" runat="server">
            <Items>
                <ext:ToolbarButton ID="bonusSaveBtn" runat="server" Text="Сохранить" Icon="Disk">
                    <Listeners>
                        <Click Handler="#{bonusFormPanel}.form.submit({waitMsg:'Сохранение...', params:{id: getBonusID()}, success: bonusSaveSuccessHandler, failure: bonusSaveFailureHandler});" />
                    </Listeners>
                </ext:ToolbarButton>
                <ext:ToolbarButton ID="bonusNewBtn" runat="server" Text="Новый бонус" Icon="Add">
                    <AjaxEvents>
                        <Click Url="/Gameboard/CreateBonus/"
                            CleanRequest="false"
                            Method="POST"
                            IsUpload="false"
                            Failure="Ext.Msg.show({title:'Ошибка',msg: result.errorMessage + ' ' + result.responseText,buttons: Ext.Msg.OK,icon: Ext.Msg.ERROR});"
                            Success="createBonus(response, result, el, type, action, extraParams);"
                            FormID="bonusFormPanel">
                        </Click>
                    </AjaxEvents>
                </ext:ToolbarButton>
                <ext:ToolbarButton ID="bonusDeleteBtn" runat="server" Text="Удалить" Icon="Cross">
                    <AjaxEvents>
                        <Click Url="/Gameboard/DeleteBonus/"
                            CleanRequest="true"
                            Method="POST"
                            IsUpload="false"
                            Failure="Ext.Msg.show({title:'Ошибка',msg: result.errorMessage + ' ' + result.responseText,buttons: Ext.Msg.OK,icon: Ext.Msg.ERROR});"
                            Success="successDeleteBonus(response, result, el, type, action, extraParams);"
                            FormID="bonusFormPanel">
                            <Confirmation Message="Удальть бонусное задание?" Title="Внимание" ConfirmRequest="true" />
                            <ExtraParams>
                                <ext:Parameter Name="id" Value="Bonus_Id.value" Mode="Raw"></ext:Parameter>
                            </ExtraParams>
                        </Click>
                    </AjaxEvents>
                </ext:ToolbarButton>
            </Items>
        </ext:Toolbar>
    </TopBar>
    <Body>
        <ext:FitLayout ID="bonusFitLayout1" runat="server">
            <ext:FormPanel ID="bonusFormPanel" runat="server" Border="false" BodyStyle="padding:5px" Url="/Gameboard/SaveBonus/">
                <Body>
                    <ext:FormLayout ID="bonusFormLayout1" runat="server" LabelWidth="156">
                        <ext:Anchor>
                            <ext:TextField ID="Bonus__RequestVerificationToken" runat="server" Hidden="true" Text="123"></ext:TextField>
                        </ext:Anchor>
                        <ext:Anchor>
                            <ext:TextField ID="Bonus_Id" runat="server" DataIndex="Id" Hidden="true" />
                        </ext:Anchor>
                        <ext:Anchor Horizontal="100%">
                            <ext:TextField ID="Bonus_Name" runat="server" DataIndex="Name" FieldLabel="Кодовое название" Width="250" AllowBlank="false" EmptyText="Введите кодовое название">
                                <Listeners>
                                    <Change Fn="bonusChange" />
                                </Listeners>
                            </ext:TextField>
                        </ext:Anchor>
                        <ext:Anchor>
                            <ext:TextField ID="Bonus_StartTime" runat="server" Hidden="true" DataIndex="StartTime"/>
                        </ext:Anchor>
                        <ext:Anchor>
                            <ext:MultiField ID="Bonus_StartTimeComplex" FieldLabel="Время выдачи" runat="server">
                                <Fields>
                                    <ext:DateField ID="Bonus_StartTimeDate" runat="server" DataIndex="StartTimeDate" Width="90" AllowBlank="false">
                                        <Listeners>
                                            <Change Handler="Bonus_StartTime.setRawValue(Bonus_StartTimeDate.value + ' ' + Bonus_StartTimeTime.value);" />
                                        </Listeners>
                                    </ext:DateField>
                                    <ext:TimeField ID="Bonus_StartTimeTime" runat="server" DataIndex="StartTimeTime" Width="80" AllowBlank="false">
                                        <Listeners>
                                            <Change Handler="Bonus_StartTime.setRawValue(Bonus_StartTimeDate.value + ' ' + Bonus_StartTimeTime.value);" />
                                        </Listeners>
                                    </ext:TimeField>
                                </Fields>
                            </ext:MultiField>
                        </ext:Anchor>
                        <ext:Anchor>
                            <ext:TextField ID="Bonus_FinishTime" runat="server" Hidden="true" DataIndex="FinishTime"/>
                        </ext:Anchor>
                        <ext:Anchor>
                            <ext:MultiField ID="Bonus_FinishTimeComplex" FieldLabel="Время окончания" runat="server">
                                <Fields>
                                    <ext:DateField ID="Bonus_FinishTimeDate" runat="server" DataIndex="FinishTimeDate" Width="90" AllowBlank="false">
                                        <Listeners>
                                            <Change Handler="Bonus_FinishTime.setRawValue(Bonus_FinishTimeDate.value + ' ' + Bonus_FinishTimeTime.value);" />
                                        </Listeners>
                                    </ext:DateField>
                                    <ext:TimeField ID="Bonus_FinishTimeTime" runat="server" DataIndex="FinishTimeTime" Width="80" AllowBlank="false">
                                        <Listeners>
                                            <Change Handler="Bonus_FinishTime.setRawValue(Bonus_FinishTimeDate.value + ' ' + Bonus_FinishTimeTime.value);" />
                                        </Listeners>
                                    </ext:TimeField>
                                </Fields>
                            </ext:MultiField>
                        </ext:Anchor>
                        <ext:Anchor>
                            <ext:Checkbox ID="Bonus_IsIndividual" runat="server" DataIndex="IsIndividual" FieldLabel="Индивидуальное задание">
                                <Listeners>
                                    <Change Fn="bonusChange" />
                                </Listeners>
                            </ext:Checkbox>
                        </ext:Anchor>
                        <ext:Anchor>
                            <ext:Panel ID="pnlBonusText" runat="server" Border="false" BodyStyle="background: transparent;">
                                <Body>
                                    <ext:FormLayout ID="bonusFormLayout2" runat="server" LabelAlign="Top" StyleSpec="background: transparent">
                                        <ext:Anchor Horizontal="100%">
                                            <ext:TextArea ID="Bonus_TaskText" runat="server" DataIndex="TaskText" FieldLabel="Формулировка задания" Height="200"/>
                                        </ext:Anchor>
                                    </ext:FormLayout>
                                </Body>
                            </ext:Panel>
                        </ext:Anchor>
                    </ext:FormLayout>
                </Body>            
            </ext:FormPanel>
        </ext:FitLayout>
    </Body>
</ext:Panel>
</ext:FitLayout>

