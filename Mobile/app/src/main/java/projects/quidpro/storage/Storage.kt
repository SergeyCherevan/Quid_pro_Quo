package projects.quidpro.storage

import android.net.Uri
import projects.quidpro.models.*
import projects.quidpro.models.signalRMessageModels.MessagingApiModel

class Storage {
    companion object {
        var LoginedAccount = Account()
        var LoginedAccountClaims = AccountClaims()
        var userAvatar = UserAvatarSettings()
        var ticketSystemParametersList = ArrayList<TicketSystemParameters>()
        var recommendationSystemParametersList = ArrayList<RecommendationSystemParameters>()
        var dialogSystemParameters = ArrayList<DialogSystemParameters>()
        var currentUpdatedDialog = ""
        var ticketsSearch = TicketsSearchParameters()
        var ticketsCreate = TicketCreateParameters()

        var selectedMessagesDialogSystemParameter: selectedMessagesDialogSystemParameters = selectedMessagesDialogSystemParameters()

        const val reverseGeocodeApiKey: String = "bdc_affe7921198f459cab54967b5b9d15f3"
    }
}