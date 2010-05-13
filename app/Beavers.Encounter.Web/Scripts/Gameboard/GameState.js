/// <reference path="../vswd-ext_2.2.js" />
var inAjaxUpdate = false;

function onChangeGameState(sender,el)
{
    if (el.checked && !inAjaxUpdate) {
        gameTaskManager.stopTask(gameTaskManager.tasks[1]);
        switch (el.id) {
        case "radioPlanned":
            // reset game
            radioStartup.setDisabled(false);
            radioStarted.setDisabled(true);
            radioFinished.setDisabled(true);
            radioCloused.setDisabled(true);
	        break
        case "radioStartup":
            // Startup game
            radioStarted.setDisabled(false);
            radioFinished.setDisabled(true);
            radioCloused.setDisabled(true);
	        break
        case "radioStarted":
            // Start game
            radioStartup.setDisabled(true);
            radioFinished.setDisabled(false);
            radioCloused.setDisabled(true);
	        break
        case "radioFinished":
            // Stop game
            radioStartup.setDisabled(true);
            radioStarted.setDisabled(true);
            radioCloused.setDisabled(false);
	        break
        case "radioCloused":
            // Clouse game
            radioStartup.setDisabled(true);
            radioStarted.setDisabled(true);
            radioFinished.setDisabled(true);
	        break
        default: 
            return;
        }
        
        Coolite.AjaxEvent.confirmRequest({
            method: "POST",
            url: "/Gameboard/SetGameState?id=" + gameId.value + '&state=' + el.id.substr(5, 10),
            userFailure: function(response, result, el, type, action, extraParams){
                Ext.Msg.show({title:'Ошибка',msg: result.errorMessage + ' ' + result.responseText,buttons: Ext.Msg.OK,icon: Ext.Msg.ERROR});
                gameTaskManager.startTask(gameTaskManager.tasks[1]);
            },
            userSuccess: function(response, result, el, type, action, extraParams){
                console.log('Состояние изменено!');
                dsTeamsState.reload();
                gameTaskManager.startTask(gameTaskManager.tasks[1]);
            }
            ,control:radioStartup});
    }
    else if (inAjaxUpdate) {
        inAjaxUpdate = false;
    }
}

function updateGameStateUpdate(data)
{
    var rb = Ext.getCmp('radio' + data.result);
    if (!rb.checked) {
        inAjaxUpdate = true;
        radioPlanned.setDisabled(false);
        radioStartup.setDisabled(false);
        radioStarted.setDisabled(false);
        radioFinished.setDisabled(false);
        radioCloused.setDisabled(false);

        radioPlanned.setValue(false);
        radioStartup.setValue(false);
        radioStarted.setValue(false);
        radioFinished.setValue(false);
        radioCloused.setValue(false);

        rb.setDisabled(false);
        rb.suspendEvents();
        rb.setValue(true);
        rb.resumeEvents();

        switch (data.result){
        case "Planned":
            radioStartup.setDisabled(false);
            radioStarted.setDisabled(true);
            radioFinished.setDisabled(true);
            radioCloused.setDisabled(true);
	        break
        case "Startup":
            radioStarted.setDisabled(false);
            radioFinished.setDisabled(true);
            radioCloused.setDisabled(true);
	        break
        case "Started":
            radioStartup.setDisabled(true);
            radioFinished.setDisabled(false);
            radioCloused.setDisabled(true);
	        break
        case "Finished":
            radioStartup.setDisabled(true);
            radioStarted.setDisabled(true);
            radioCloused.setDisabled(false);
	        break
        case "Cloused":
            radioStartup.setDisabled(true);
            radioStarted.setDisabled(true);
            radioFinished.setDisabled(true);
	        break
        default: 
            return;
        }
    }
}

function onGameStateUpdate()
{
    Coolite.AjaxEvent.confirmRequest({
        method: "POST",
        url: "/Gameboard/GetGameState?id=" + gameId.value,
        userFailure: function(response, result, el, type, action, extraParams){
            Ext.Msg.show({title:'Ошибка',msg: result.errorMessage + ' ' + result.responseText,buttons: Ext.Msg.OK,icon: Ext.Msg.ERROR});
        },
        userSuccess: function(response, result, el, type, action, extraParams){
            updateGameStateUpdate(result);
        }
        ,control:radioStartup});
}
