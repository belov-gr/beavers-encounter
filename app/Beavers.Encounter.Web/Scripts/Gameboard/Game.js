function gameLoaded(store, records)
{
    if (records.length > 0) {
        gameFormPanel.form.loadRecord(records[0]);
    }
    initGameUI(false);
    tabGame.el.unmask();
}

function initGameUI(isNew) {
    //btnDelete.setDisabled(isNew);
    //tabOrders.setDisabled(isNew);
    //CustomerID.setDisabled(!isNew);
}

function getGameID(){
    return (dsGame.getCount()>0 && !dsGame.getAt(0).newRecord) ? dsGame.getAt(0).id : ''
}

function gameSaveFailureHandler(form, action){
    var msg = '';
    if (action.failureType == "client" || (action.result && action.result.errors && action.result.errors.length > 0)) {
        msg = "Проверьте корректность значений полей!";
    } else if (action.result && action.result.extraParams.msg) {
        msg = action.result.extraParams.msg;
    } else if (action.response) {
        msg = action.response.responseText;
    }

    Ext.Msg.show({
        title: 'Ошибка сохранения',
        msg: msg,
        buttons: Ext.Msg.OK,
        icon: Ext.Msg.ERROR
    });
}

function gameSaveSuccessHandler(form, action) {
    Ext.MessageBox.alert('Success', 'Свойства игры сохранены успешно!');
    initGameUI(false);
}

function gameSetControlsNames()
{
    Game_Id.el.dom.name='Game.Id'
    Game_Name.el.dom.name='Game.Name'
    Game_GameDate.el.dom.name='Game.GameDate'
    Game_TotalTime.hiddenField.name='Game.TotalTime'
    Game_TimePerTask.hiddenField.name='Game.TimePerTask'
    Game_TimePerTip.hiddenField.name='Game.TimePerTip'
    Game_Description.el.dom.name='Game.Description'
    Game_PrefixMainCode.el.dom.name='Game.PrefixMainCode'
    Game_PrefixBonusCode.el.dom.name='Game.PrefixBonusCode'
}

