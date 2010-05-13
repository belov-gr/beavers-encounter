/// <reference path="../vswd-ext_2.2.js" />
var activeTaskId = 0;
var activeBonusId = 0;

function onClickTreeNode(node, e){
    if (node.id == 'root'){
        tabGame.show();
        return;
    }
    if (parseInt(node.id, 10) != NaN && node.parentNode.id == 'tasks'){
        activeTaskId = node.id;
        tabTask.show();
        dsTask.reload();
    }
    if (parseInt(node.id, 10) != NaN && node.parentNode.id == 'bonuses'){
        activeBonusId = node.id;
        tabBonus.show();
        dsBonus.reload();
    }
}

function onShowTreeContextMenu(node, e){
    TreeContextMenu.items.get('tcmAdd').setVisible(node.id == 'tasks' || node.id == 'bonuses');
    TreeContextMenu.TargetNode = node;
    TreeContextMenu.show(node.ui.anchor);
}

function onClickTreeContextMenu(item, e){
    item.parentMenu.hide();
    if (item.id == 'tcmAdd'){
        if (item.parentMenu.TargetNode.id == 'tasks'){
            tabTask.show();
            var params=arguments;
            Coolite.AjaxEvent.confirmRequest({
                formProxyArg: "taskFormPanel",
                method: "POST",
                url: "/Gameboard/CreateTask/",
                userFailure: function(response, result, el, type, action, extraParams){Ext.Msg.show({title:'Ошибка',msg: result.errorMessage + ' ' + result.responseText,buttons: Ext.Msg.OK,icon: Ext.Msg.ERROR});},
                userSuccess: function(response, result, el, type, action, extraParams){createTask(response, result, el, type, action, extraParams);}
                ,control:item});
        } 
        else if (item.parentMenu.TargetNode.id == 'bonuses'){
            tabBonus.show();
            var params=arguments;
            Coolite.AjaxEvent.confirmRequest({
                formProxyArg: "bonusFormPanel",
                method: "POST",
                url: "/Gameboard/CreateBonus/",
                userFailure: function(response, result, el, type, action, extraParams){Ext.Msg.show({title:'Ошибка',msg: result.errorMessage + ' ' + result.responseText,buttons: Ext.Msg.OK,icon: Ext.Msg.ERROR});},
                userSuccess: function(response, result, el, type, action, extraParams){createBonus(response, result, el, type, action, extraParams);}
                ,control:item});
        }
    }
    if (item.id == 'tcmRefresh'){
        if (item.parentMenu.TargetNode.isRoot || item.parentMenu.TargetNode.leaf) {
            return;
        } else {
            item.parentMenu.TargetNode.reload();
        }
    }
}
