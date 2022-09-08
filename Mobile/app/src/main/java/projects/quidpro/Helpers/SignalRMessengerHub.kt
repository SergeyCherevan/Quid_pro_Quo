package projects.quidpro.Helpers

import android.app.Activity
import android.content.Context
import android.widget.Adapter
import android.widget.GridView
import android.widget.Toast
import com.google.gson.reflect.TypeToken
import com.microsoft.signalr.HubConnection
import com.microsoft.signalr.HubConnectionBuilder
import com.microsoft.signalr.HubConnectionState
import projects.quidpro.Helpers.ParseTime.Companion.parseForDialog
import projects.quidpro.api.servers.main.MainClientBuilder.apiUrl
import projects.quidpro.models.DialogSystemParameters
import projects.quidpro.models.RecommendationSystemParameters
import projects.quidpro.models.selectedMessagesDialogSystemParameters
import projects.quidpro.models.signalRMessageModels.MessageApiModel
import projects.quidpro.models.signalRMessageModels.MessagingApiModel
import projects.quidpro.models.signalRMessageModels.MessagingCardApiModel
import projects.quidpro.storage.Storage

class SignalRMessengerHub {
    companion object {
        private var hubConnection: HubConnection = HubConnectionBuilder.create("${apiUrl}/messenger")
            .withHeader("Authorization", "Bearer " + Storage.LoginedAccount.jwtString)
            .build()

        private fun rebuildHubConnection() {
            hubConnection = HubConnectionBuilder.create("${apiUrl}/messenger")
                .withHeader("Authorization", "Bearer " + Storage.LoginedAccount.jwtString)
                .build()
        }

        fun getHubContext(): HubConnection {
            return hubConnection
        }

        fun startConnectionHub(){
            if(!checkConnectionState()) {
                rebuildHubConnection()
                hubConnection.start().blockingAwait()
            }
        }

        fun removeAllHubHandlers() {
            hubConnection.remove("GetMessagingCardsResponse")
            hubConnection.remove("GetMessagingResponse")
            hubConnection.remove("MessagesIsViewedResponse")
            //hubConnection.remove("MessagingsIsUpdated")
        }

        fun removeUpdateMessagesListener() {
            hubConnection.remove("MessagingsIsUpdated")
        }

        fun stopConnectionHub(){
            if(checkConnectionState()) {
                removeAllHubHandlers()
                hubConnection.stop().blockingAwait()
            }
        }

        private fun checkConnectionState() : Boolean {
            return hubConnection.connectionState == HubConnectionState.CONNECTED
        }

        fun getMessagingCardsResponse(activity: Activity) {
            val listType = object : TypeToken<ArrayList<MessagingCardApiModel?>?>() {}.type
            hubConnection.on<ArrayList<MessagingCardApiModel>>("GetMessagingCardsResponse", { item ->

                val listOfItems: ArrayList<MessagingCardApiModel> = item
                listOfItems.forEach { item ->
                    Storage.dialogSystemParameters.add(
                        DialogSystemParameters(
                            _dialogImagesDownloaded = false,
                            _dialogUsernameDownloaded = true,
                            _dialogUsername = item.userName,
                            _dialogLastMessageDownloaded = false, // set false!
                            _dialogLastMessageDateDownloaded = false, // set false!
                            _dialogNewMessagesCountDownloaded = true,
                            _dialogNewMessagesCount = item.countOfNotViewedMessages,
                            activity = activity)
                    )
                }

                Storage.dialogSystemParameters.forEach { dialog ->
                    /*if(dialog.dialogNewMessagesCount > 0) {
                        getNewMessagesRequest(dialog.dialogUsername)
                    }
                    else {
                        getMessagingRequestForDialogs(dialog.dialogUsername)
                    }*/
                    getMessagingRequestForDialogs(dialog.dialogUsername)
                }

            }, listType)
        }



        fun getNewMessagesResponse() {
            val listType = object : TypeToken<ArrayList<MessageApiModel?>?>() {}.type
            hubConnection.on<ArrayList<MessageApiModel>>("GetNewMessagesResponse", { item ->

                val listOfItems: ArrayList<MessageApiModel> = item
                if(listOfItems.isEmpty())
                    return@on

                Storage.dialogSystemParameters.find { dialog -> dialog.dialogUsername == Storage.currentUpdatedDialog}?.dialogLastMessage = listOfItems[listOfItems.size - 1].text
                Storage.dialogSystemParameters.find { dialog -> dialog.dialogUsername == Storage.currentUpdatedDialog}?.dialogLastMessageDownloaded = true

                val date = parseForDialog(listOfItems[listOfItems.size - 1].postedAt)
                Storage.dialogSystemParameters.find { dialog -> dialog.dialogUsername == Storage.currentUpdatedDialog}?.dialogLastMessageDate = date
                Storage.dialogSystemParameters.find { dialog -> dialog.dialogUsername == Storage.currentUpdatedDialog}?.dialogLastMessageDateDownloaded = true

            }, listType)
        }

        fun getMessagingResponseForDialogs() {
            val listType = object : TypeToken<MessagingApiModel?>() {}.type
            hubConnection.on<MessagingApiModel>("GetMessagingResponse", { item ->

                if(item.messagesList.isEmpty())
                    return@on

                Storage.dialogSystemParameters.find { dialog -> dialog.dialogUsername == item.user2Name}?.dialogLastMessage = item.messagesList[item.messagesList.size - 1].text
                Storage.dialogSystemParameters.find { dialog -> dialog.dialogUsername == item.user2Name}?.dialogLastMessageDownloaded = true

                val date = parseForDialog(item.messagesList[item.messagesList.size - 1].postedAt)
                Storage.dialogSystemParameters.find { dialog -> dialog.dialogUsername == item.user2Name}?.dialogLastMessageDate = date
                Storage.dialogSystemParameters.find { dialog -> dialog.dialogUsername == item.user2Name}?.dialogLastMessageDateDownloaded = true

            }, listType)
        }



        fun getMessagingResponseForDialogMessages(messagesDialogParameter: selectedMessagesDialogSystemParameters, listAllDialogMessages: GridView) {
            val listType = object : TypeToken<MessagingApiModel?>() {}.type
            hubConnection.on<MessagingApiModel>("GetMessagingResponse", { item ->

                if(item.messagesList.isEmpty())
                    return@on

                messagesDialogParameter.selectedMessagesDialog.user1Name = item.user1Name
                messagesDialogParameter.selectedMessagesDialog.user2Name = item.user2Name
                item.messagesList.forEach {
                        msg -> messagesDialogParameter.selectedMessagesDialog.messagesList.add(msg)
                }
                //messagesDialogParameter.selectedMessagesDialog.messagesList = item.messagesList
                //messagesDialogParameter.selectedMessagesDialog.messagesList.addAll(item.messagesList)

                messagesDialogParameter.selectedMessagesDialogDownloaded = true

            }, listType)
        }

        fun getMessagingCardsRequest() {
            hubConnection.send("GetMessagingCardsRequest")
        }

        fun getNewMessagesRequest(companionName: String) {
            hubConnection.send("GetNewMessagesRequest", companionName)
        }

        private fun getMessagingRequestForDialogs(companionName: String) {
            hubConnection.send("GetMessagingRequest", companionName)
        }

        fun getMessagingRequestForDialogMessages(companionName: String) {
            hubConnection.send("GetMessagingRequest", companionName)
        }

        fun messagesIsViewedRequest(companionName: String, messageIDs: ArrayList<Int>) {
            hubConnection.send("MessagesIsViewedRequest", companionName, messageIDs)
        }
    }
}