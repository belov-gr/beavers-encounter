/// <reference path="../vswd-ext_2.2.js" />
function createTask(response, result, el, type, action, extraParams){
    taskFormPanel.form.reset();
    dsCodes.removeAll();
    dsTips.removeAll();
    dsTask.removeAll();
    dsTask.addRecord(result.data[0]);
    taskLoaded(dsTask, result.data, null, true)
}

function taskLoaded(store, records, p, isNew){
    tabTask.enable();
    var isNewTask = isNew || false;
    if (records.length > 0){
        taskFormPanel.form.loadRecord(records[0]);

        //Попутно загружаем данные в dsTips и dsCodes
        var task = isNewTask ? dsTask.getAt(0).data : dsTask.getAt(0).json;
        dsTips.loadData({data: task.Tips, totalCount: task.Tips.length})
        dsCodes.loadData({data: task.Codes, totalCount: task.Codes.length})
        tabTask.setTitle(task.Name || "Новое задание");
    }
    initTaskUI(isNewTask);
    tabTask.el.unmask();
}

function initTaskUI(isNew){
    taskFormPanel.body.dom.__RequestVerificationToken.value = Ext.get('dummyForm').dom.__RequestVerificationToken.value;
    taskSaveBtn.setDisabled(true);
    taskDeleteBtn.setDisabled(isNew);
    NotAfterTasksBtn.setVisible(!isNew);
    NotOneTimeTasksBtn.setVisible(!isNew);

    if (!codesAccPanel.collapsed) {
        btnSaveCodes.setDisabled(!isNew);
    }
    tipsAccPanel.setVisible(!isNew);
    codesAccPanel.setVisible(!isNew);
}

function taskSaveFailureHandler(form, action){
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

function taskSaveSuccessHandler(form, action) {
    if (action.result && action.result.extraParams && action.result.extraParams.newID) {
        dsTask.getAt(0).id = action.result.extraParams.newID;
        GameTree.getNodeById('tasks').appendChild({leaf:true,text:action.result.extraParams.name,id: action.result.extraParams.newID});
        if (dsTask.getAt(0).newRecord) {
            delete dsTask.getAt(0).newRecord;
        }
    }
    else {
        Ext.MessageBox.alert('Success', 'Task has been saved!');
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

function successDeleteTask(response, result, el, type, action, extraParams){
    taskFormPanel.form.reset();
    var node = GameTree.getNodeById(extraParams.id);
    var selectNode = node.previousSibling || node.nextSibling || GameTree.getNodeById('root');
    selectNode.select();
    node.remove()
    onClickTreeNode(selectNode, null);
}

function getTaskID(){
    return (dsTask.getCount()>0 && !dsTask.getAt(0).newRecord) ? dsTask.getAt(0).id : ''
}

function taskChange(field, newValue, oldValue){
    taskSaveBtn.enable();
    if (!taskSaveBtn.disabled){
        animateButton(taskSaveBtn.btnEl);
    }
}

/* Обработчики для свойства "Не после" */
function ContainsIdInTask_NotAfterTasksField(id){
    var str = '[' + Task_NotAfterTasks.getValue() + ']';
    var arr = eval(str);
    for(var i=0; i < arr.length; i++){
        if (arr[i] == id){
            return true;
        }
    }
    return false;
}
function FillMenuNotAfterTasks(){
    var taskId = getTaskID();
    var nodes = GameTree.getNodeById('tasks').childNodes;
    NotAfterTasksBtn.menu.removeAll();
    for (var i=0; i < nodes.length; i++){
        if (taskId != nodes[i].id){
            NotAfterTasksBtn.menu.addMenuItem(new Ext.menu.CheckItem({text: nodes[i].text, id: nodes[i].id, checked: ContainsIdInTask_NotAfterTasksField(nodes[i].id)}));
        }
    }
}
function NotAfterTasksBtnClick(menu, item){
    Task_NotAfterTasks.setValue('');
    NotAfterTasksLabel.setValue('');
    for (var i=0; i < menu.items.items.length; i++){
        if (menu.items.items[i].checked){
            NotAfterTasksLabel.setValue(NotAfterTasksLabel.getValue() + (NotAfterTasksLabel.getValue() == '' ? '' : ', ') + menu.items.items[i].text);
            Task_NotAfterTasks.setValue(Task_NotAfterTasks.getValue() + ',' + menu.items.items[i].id);
        }
    }
    taskChange(null, null, null);
}
function ConvertNotAfterTasks(v, record){
    var value = '';
    for(var i=0; i < v.length; i++){
        value = value + ',' + v[i].Id;
    }
    return value;
}
function ConvertNotAfterTasksLabel(v, record){
    var value = '';
    for(var i=0; i < record.NotAfterTasks.length; i++){
        value = value + (value == '' ? '' : ', ') + record.NotAfterTasks[i].Name;
    }
    return value;
}
/* Обработчики для свойства "Не вместе" */
function ContainsIdInTask_NotOneTimeTasksField(id){
    var str = '[' + Task_NotOneTimeTasks.getValue() + ']';
    var arr = eval(str);
    for(var i=0; i < arr.length; i++){
        if (arr[i] == id){
            return true;
        }
    }
    return false;
}
function FillMenuNotOneTimeTasks(){
    var taskId = getTaskID();
    var nodes = GameTree.getNodeById('tasks').childNodes;
    NotOneTimeTasksBtn.menu.removeAll();
    for (var i=0; i < nodes.length; i++){
        if (taskId != nodes[i].id){
            NotOneTimeTasksBtn.menu.addMenuItem(new Ext.menu.CheckItem({text: nodes[i].text, id: nodes[i].id, checked: ContainsIdInTask_NotOneTimeTasksField(nodes[i].id)}));
        }
    }
}
function NotOneTimeTasksBtnClick(menu, item){
    Task_NotOneTimeTasks.setValue('');
    NotOneTimeTasksLabel.setValue('');
    for (var i=0; i < menu.items.items.length; i++){
        if (menu.items.items[i].checked){
            NotOneTimeTasksLabel.setValue(NotOneTimeTasksLabel.getValue() + (NotOneTimeTasksLabel.getValue() == '' ? '' : ', ') + menu.items.items[i].text);
            Task_NotOneTimeTasks.setValue(Task_NotOneTimeTasks.getValue() + ',' + menu.items.items[i].id);
        }
    }
    taskChange(null, null, null);
}
function ConvertNotOneTimeTasks(v, record){
    var value = '';
    for(var i=0; i < v.length; i++){
        value = value + ',' + v[i].Id;
    }
    return value;
}
function ConvertNotOneTimeTasksLabel(v, record){
    var value = '';
    for(var i=0; i < record.NotOneTimeTasks.length; i++){
        value = value + (value == '' ? '' : ', ') + record.NotOneTimeTasks[i].Name;
    }
    return value;
}
