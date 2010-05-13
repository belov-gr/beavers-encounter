/// <reference path="../vswd-ext_2.2.js" />
function createBonus(response, result, el, type, action, extraParams){
    bonusFormPanel.form.reset();
    dsBonus.removeAll();
    dsBonus.addRecord(result.data[0]);
    bonusLoaded(dsBonus, result.data, null, true)
}

function bonusLoaded(store, records, p, isNew){
    tabBonus.enable();
    tabBonus.doLayout();
    var isNewBonus = isNew || false;
    if (records.length > 0){
        bonusFormPanel.form.loadRecord(records[0]);
        var bonus = isNewBonus ? dsBonus.getAt(0).data : dsBonus.getAt(0).json;
        tabBonus.setTitle(bonus.Name || "Новое бонусное задание");
        
        // Дата и время начала
        Bonus_StartTime.setValue(bonus.StartTime);
        Bonus_StartTimeDate.setValue(bonus.StartTime.substr(0, 10));
        Bonus_StartTimeTime.setValue(bonus.StartTime.substr(11, 5));
        
        // Дата и время окончания
        Bonus_FinishTime.setValue(bonus.FinishTime);
        Bonus_FinishTimeDate.setValue(bonus.FinishTime.substr(0, 10));
        Bonus_FinishTimeTime.setValue(bonus.FinishTime.substr(11, 5));
        
        Bonus_TaskText.setValue(bonus.TaskText);
        Bonus_Id.setValue(bonus.Id);
    }
    initBonusUI(isNewBonus);
    tabBonus.el.unmask();
}

function initBonusUI(isNew){
    bonusSaveBtn.setDisabled(true);
    bonusDeleteBtn.setDisabled(isNew);
    setBonusControlsNames();
    Bonus__RequestVerificationToken.setValue(Ext.get('dummyForm').dom.__RequestVerificationToken.value);
    //bonusFormPanel.body.dom.__RequestVerificationToken.value = Ext.get('dummyForm').dom.__RequestVerificationToken.value;
}

function bonusSaveFailureHandler(form, action){
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

function bonusSaveSuccessHandler(form, action) {
    if (action.result && action.result.extraParams && action.result.extraParams.newID) {
        dsBonus.getAt(0).id = action.result.extraParams.newID;
        GameTree.getNodeById('bonuses').appendChild({leaf:true,text:action.result.extraParams.name,id: action.result.extraParams.newID});
        if (dsBonus.getAt(0).newRecord) {
            delete dsBonus.getAt(0).newRecord;
        }
    }
    else {
        Ext.MessageBox.alert('Success', 'BonusTask has been saved!');
    }

    if (action.options.params.setNew) {
        DetailsForm.form.reset();
        dsCustomer.removeAll();

        var rec = new dsCustomer.recordType();
        rec.newRecord = true;
        dsCustomer.add(rec);
        initTaskUI(true);
    }
    else {
        initTaskUI(false);
    }
}

function successDeleteBonus(response, result, el, type, action, extraParams){
    bonusFormPanel.form.reset();
    var node = GameTree.getNodeById(extraParams.id);
    var selectNode = node.previousSibling || node.nextSibling || GameTree.getNodeById('root');
    selectNode.select();
    node.remove()
    onClickTreeNode(selectNode, null);
}

function getBonusID(){
    return (dsBonus.getCount()>0 && !dsBonus.getAt(0).newRecord) ? dsBonus.getAt(0).id : ''
}

function bonusChange(field, newValue, oldValue){
    bonusSaveBtn.enable();
    if (!bonusSaveBtn.disabled){
        animateButton(bonusSaveBtn.btnEl);
    }
}

function setBonusControlsNames() {
    Bonus_Id.el.dom.name = 'BonusTask.Id'
    Bonus_Name.el.dom.name = 'BonusTask.Name'
    Bonus_StartTime.el.dom.name = 'BonusTask.StartTime'
    Bonus_FinishTime.el.dom.name = 'BonusTask.FinishTime'
    Bonus_IsIndividual.el.dom.name = 'BonusTask.IsIndividual'
    Bonus_TaskText.el.dom.name = 'BonusTask.TaskText'
    Bonus__RequestVerificationToken.el.dom.name = '__RequestVerificationToken'
}
