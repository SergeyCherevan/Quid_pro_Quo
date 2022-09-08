package projects.quidpro.activities

import android.app.Activity
import android.content.Intent
import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.os.CountDownTimer
import android.view.View
import android.widget.GridView
import android.widget.Toast
import com.google.gson.reflect.TypeToken
import projects.quidpro.Helpers.MultiPartHelper
import projects.quidpro.Helpers.SignalRMessengerHub
import projects.quidpro.R
import projects.quidpro.adapters.AllDialogMessagesAdapter
import projects.quidpro.adapters.AllDialogsAdapter
import projects.quidpro.api.servers.main.MainClientBuilder
import projects.quidpro.api.servers.main.MainRequests
import projects.quidpro.databinding.ActivityFiltersBinding
import projects.quidpro.databinding.ActivityMessageDialogBinding
import projects.quidpro.models.*
import projects.quidpro.models.signalRMessageModels.MessageApiModel
import projects.quidpro.models.signalRMessageModels.MessagesReadedResponceModel
import projects.quidpro.models.signalRMessageModels.MessagingApiModel
import projects.quidpro.models.signalRMessageModels.MessagingCardApiModel
import projects.quidpro.storage.Storage
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response

class MessageDialogActivity : AppCompatActivity() {

    private val context = this
    lateinit var binding: ActivityMessageDialogBinding
    private var companionUserName: String = "?"
    private var timer: CountDownTimer? = null

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        binding = ActivityMessageDialogBinding.inflate(layoutInflater)
        setContentView(binding.root)

        val actionBar = supportActionBar
        actionBar!!.setLogo(R.drawable.ic_shuffle_tracks_actionbar_icon)
        actionBar.title = "  Messages"
        actionBar.setDisplayUseLogoEnabled(true)
        actionBar.setDisplayShowHomeEnabled(true)
        actionBar.setDisplayHomeAsUpEnabled(true)

        binding.apply {

            binding.imageButtonSendNewMessage.setOnClickListener {
                setMessagesReadedResponce()
                sendNewMessages()
            }

            companionUserName = intent.getStringExtra("COMPANION_USER_NAME") ?: "NULL"
            binding.textViewCompanionUserName.text = companionUserName

            //start check for updated msg
            SignalRMessengerHub.removeUpdateMessagesListener()
            updateAllDialogMessages()

            //get all msg
            getAllDialogMessages()

        }

    }

    private fun getAllDialogMessages() {

        // Start Connection to Hub
        SignalRMessengerHub.startConnectionHub()

        // clear storage and set adapter content
        Storage.selectedMessagesDialogSystemParameter.selectedMessagesDialog.messagesList.clear()
        binding.listAllDialogMessages.adapter = AllDialogMessagesAdapter(context,
            R.layout.item_row_message_template,
            Storage.selectedMessagesDialogSystemParameter.selectedMessagesDialog.messagesList,
            Storage.LoginedAccountClaims.userName, companionUserName)

        startCountDownTimer()

        // Get messages from server
        SignalRMessengerHub.getMessagingResponseForDialogMessages(Storage.selectedMessagesDialogSystemParameter, binding.listAllDialogMessages)
        SignalRMessengerHub.getMessagingRequestForDialogMessages(companionUserName)

    }

    private fun updateAllDialogMessages() {
        val listType = object : TypeToken<ArrayList<MessagingCardApiModel?>?>() {}.type
        SignalRMessengerHub.getHubContext().on<ArrayList<MessagingCardApiModel>>("MessagingsIsUpdated", { item ->

            val listOfItems: ArrayList<MessagingCardApiModel> = item
            listOfItems.forEach { item ->


                if(Storage.selectedMessagesDialogSystemParameter.selectedMessagesDialog.user2Name == item.userName &&
                        item.countOfNotViewedMessages > 0) {

                    stopCheckDownloadRecommendationsComponents()
                    startCheckDownloadDialogMessagesComponents()

                    // Start Connection to Hub
                    SignalRMessengerHub.startConnectionHub()

                    // Remove all previous handlers
                    SignalRMessengerHub.removeAllHubHandlers()

                    // clear storage and set adapter content
                    Storage.selectedMessagesDialogSystemParameter.selectedMessagesDialog.messagesList.clear()
                    Storage.selectedMessagesDialogSystemParameter.selectedMessagesDialogDownloaded = false

                    // Get messages from server
                    SignalRMessengerHub.getMessagingResponseForDialogMessages(Storage.selectedMessagesDialogSystemParameter, binding.listAllDialogMessages)
                    SignalRMessengerHub.getMessagingRequestForDialogMessages(companionUserName)


                }
            }

        }, listType)
    }

    private fun setMessagesReadedRequest() {
        val listMsg: ArrayList<Int> = ArrayList()
        Storage.selectedMessagesDialogSystemParameter.selectedMessagesDialog.messagesList.forEach { message ->
            if(message.notViewed && message.authorName != Storage.LoginedAccountClaims.userName)
                listMsg.add(message.id)
        }

        SignalRMessengerHub.messagesIsViewedRequest(Storage.selectedMessagesDialogSystemParameter.selectedMessagesDialog.user2Name,
        listMsg)
    }

    fun setMessagesReadedResponce() {
        val listType = object : TypeToken<MessagesReadedResponceModel?>() {}.type
        SignalRMessengerHub.getHubContext().on<MessagesReadedResponceModel>("MessagesIsViewedResponse", { item: MessagesReadedResponceModel ->

            if(item.listReadedMessages.isEmpty())
                return@on

            item.listReadedMessages.forEach { updatedMsg: Int ->
                Storage.selectedMessagesDialogSystemParameter.selectedMessagesDialog.messagesList.find { chatMsg: MessageApiModel ->
                    chatMsg.id == updatedMsg}?.notViewed = false
            }

        }, listType)
    }

    private fun sendNewMessages() {

        binding.imageButtonSendNewMessage.isEnabled = false

        val messageText = binding.editTextSendNewMessageText.text.toString()
        if(messageText == "") {
            binding.imageButtonSendNewMessage.isEnabled = true
            return
        }

        val dataMessage = SendNewMessageRequest(messageText, companionUserName, null, null)
        val partMessageText = MultiPartHelper.createMultiPartString("Text", dataMessage.text)
        val partDestinationName = MultiPartHelper.createMultiPartString("DestinationName", dataMessage.destinationName)

        val requests = MainClientBuilder.buildClient(MainRequests::class.java)
        val response = requests.sendNewMessage(partMessageText, partDestinationName, null, null, "Bearer " + Storage.LoginedAccount.jwtString)

        response.enqueue(object: Callback<MessagingCardApiModel> {
            override fun onResponse(call: Call<MessagingCardApiModel>, response: Response<MessagingCardApiModel>) {
                if (response.isSuccessful) {

                    val data = response.body()!!

                    // update listview messages
                    //updateAllDialogMessages()

                    binding.editTextSendNewMessageText.setText("")
                    binding.imageButtonSendNewMessage.isEnabled = true

                } else {
                    when(response.code()) {
                        401 -> {
                            Toast.makeText(context, "Unauthorized.", Toast.LENGTH_SHORT).show()
                            binding.imageButtonSendNewMessage.isEnabled = true
                        }
                        404 -> {
                            Toast.makeText(context, "Not found.", Toast.LENGTH_SHORT).show()
                            binding.imageButtonSendNewMessage.isEnabled = true
                        }
                        400 -> {
                            Toast.makeText(context, "Error 400.", Toast.LENGTH_SHORT).show()
                            binding.imageButtonSendNewMessage.isEnabled = true
                        }
                        else -> {
                            Toast.makeText(context, "An error occurred during send message.", Toast.LENGTH_SHORT).show()
                            binding.imageButtonSendNewMessage.isEnabled = true
                        }
                    }
                }
            }

            override fun onFailure(call: Call<MessagingCardApiModel>, t: Throwable) {
                Toast.makeText(context, "Application Error.\nSend Request Error.", Toast.LENGTH_SHORT).show()
                binding.imageButtonSendNewMessage.isEnabled = true
            }
        })

    }

    private fun startCheckDownloadDialogMessagesComponents() {
        timer?.start()
    }

    private fun stopCheckDownloadRecommendationsComponents() {
        timer?.cancel()
    }

    private fun startCountDownTimer(){
        timer = object: CountDownTimer(100, 100) {
            override fun onTick(millisUntilFinished: Long) {
                //Toast.makeText(_binding!!.root.context,"+++", Toast.LENGTH_SHORT).show()
            }

            override fun onFinish() {
                try {
                    if(Storage.selectedMessagesDialogSystemParameter.selectedMessagesDialogDownloaded) {
                        binding.listAllDialogMessages.invalidateViews()
                        setMessagesReadedRequest()
                        return
                    }
                } catch (e: Exception){
                }
                startCheckDownloadDialogMessagesComponents()
            }
        }.start()
    }

    override fun onSupportNavigateUp(): Boolean {
        onBackPressed()
        return true
    }

    override fun onPause() {
        super.onPause()
        SignalRMessengerHub.removeAllHubHandlers()
        SignalRMessengerHub.removeUpdateMessagesListener()
        Storage.selectedMessagesDialogSystemParameter.selectedMessagesDialogDownloaded = false
        Storage.selectedMessagesDialogSystemParameter.selectedMessagesDialog.messagesList.clear()
    }
}