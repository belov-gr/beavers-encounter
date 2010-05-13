/// <reference path="../vswd-ext_2.2.js" />
function tipRecordTitleTmpl(value, metadata, record, rowIndex, colIndex, store) {
    if (record.data.SuspendTime == 0) {
        return "<b>Задание</b>";
    }
    if (record.data.SuspendTime == 30) {
        return "<b>Подсказка №1</b>";
    }
    if (record.data.SuspendTime == 60) {
        return "<b>Подсказка №2</b>";
    }
}

function deleteTip(tipId)
{
    Coolite.AjaxEvent.confirmRequest({
        formProxyArg: "taskFormPanel",
        method: "POST",
        url: "/Gameboard/DeleteTip?id=" + tipId,
        userFailure: function(response, result, el, type, action, extraParams) {
            Ext.Msg.show({
                title: 'Ошибка',
                msg: result.errorMessage + ' ' + result.responseText,
                buttons: Ext.Msg.OK, 
                icon: Ext.Msg.ERROR }); 
        },
        userSuccess: function(response, result, el, type, action, extraParams) {
            dsTask.reload();
        },
        control: TipsGrid,
        eventMask: "Удаление..."
    });
}

function tipsGridCommand(command,record,rowIndex,colIndex)
{
    if (command == "Delete"){
        Ext.Msg.show({
           title:'Предупреждение',
           msg: 'Удалить подсказку?',
           buttons: Ext.Msg.YESNO,
           animEl: 'elId',
           icon: Ext.MessageBox.QUESTION,
           fn: function(btn){ if (btn == 'yes'){ deleteTip(record.data.Id);}}
        });
    }
    if (command == "Edit") {
        winTipEditor.show();
        tipFormPanel.form.loadRecord(record);
    }
}

function tipSaveFailureHandler(form, action){
    var msg = '';
    if (action.failureType == "client" || (action.result && action.result.errors && action.result.errors.length > 0)) {
        msg = "Please check fields";
    } else if (action.result && action.result.extraParams.msg) {
        msg = action.result.extraParams.msg;
    } else if (action.response) {
        msg = action.response.responseText;
    }

    Ext.Msg.show({
        title: 'Save Error',
        msg: msg,
        buttons: Ext.Msg.OK,
        icon: Ext.Msg.ERROR
    });
}

function tipSaveSuccessHandler(form, action) {
    winTipEditor.hide();
    tipFormPanel.form.reset();
    dsTask.reload();
}

function getTipID(){
    return Tip_Id.getValue();
}

function onClickAddTip(btn, arguments) {
    var params = arguments;
    Coolite.AjaxEvent.confirmRequest({
        formProxyArg: "tipFormPanel",
        method: "POST",
        url: "/Gameboard/CreateTip?taskId=" + getTaskID(),
        userFailure: function(response, result, el, type, action, extraParams) { Ext.Msg.show({ title: 'Ошибка', msg: result.errorMessage + ' ' + result.responseText, buttons: Ext.Msg.OK, icon: Ext.Msg.ERROR }); },
        userSuccess: function(response, result, el, type, action, extraParams) { createTip(response, result, el, type, action, extraParams); },
        control: TipsGrid
    });
    tabTask.el.mask('Создание...', 'x-mask-loading');
}

function createTip(response, result, el, type, action, extraParams) {
    tipFormPanel.form.reset();
    dsTips.addRecord(result.data[0]);
    tabTask.el.unmask();
    winTipEditor.show();
    tipFormPanel.form.loadRecord(dsTips.getAt(dsTips.getCount() - 1));
}

function onCloseTipWindow(el) {
    if (Tip_Id.getValue() == 0) {
        dsTask.reload();
    }
    tipFormPanel.form.reset();
    winTipEditor.hide();
}

function tipSetControlsNames() {
    Tip_Id.el.dom.name = 'Tip.Id'
    Tip_Name.el.dom.name = 'Tip.Name'
    Tip_RequestVerificationToken.el.dom.name = '__RequestVerificationToken'
}
