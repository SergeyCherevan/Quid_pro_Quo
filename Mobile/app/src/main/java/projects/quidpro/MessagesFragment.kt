package projects.quidpro

import android.content.Intent
import android.os.Bundle
import android.os.CountDownTimer
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.AdapterView
import androidx.appcompat.app.AppCompatActivity
import com.google.gson.reflect.TypeToken
import projects.quidpro.Helpers.SignalRMessengerHub
import projects.quidpro.activities.MessageDialogActivity
import projects.quidpro.adapters.AllDialogsAdapter
import projects.quidpro.databinding.FragmentMessagesBinding
import projects.quidpro.models.signalRMessageModels.MessagingCardApiModel
import projects.quidpro.storage.Storage

class MessagesFragment : Fragment() {

    private var _binding: FragmentMessagesBinding? = null
    private val binding get() = _binding!!
    private var timer: CountDownTimer? = null

    private var firstStart = true

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        arguments?.let {
        }
    }

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        // Inflate the layout for this fragment
        _binding = FragmentMessagesBinding.inflate(inflater, container, false)

        val actionBar = (activity as AppCompatActivity).supportActionBar
        actionBar!!.setLogo(R.drawable.ic_shuffle_tracks_actionbar_icon)
        actionBar.title = "  Messages"
        actionBar.setDisplayUseLogoEnabled(true)
        actionBar.setDisplayShowHomeEnabled(true)

        binding.apply {

            getAllDialogs()
            updateAllDialogs()
        }

        return binding.root
    }

    private fun getAllDialogs() {

        val context = requireActivity().applicationContext

        // Start Connection to Hub
        SignalRMessengerHub.startConnectionHub()

        // clear storage and set adapter content and start timer
        Storage.dialogSystemParameters.clear()
        binding.listAllMessages.adapter = AllDialogsAdapter(context, R.layout.message_dialog_item_row, Storage.dialogSystemParameters, requireContext())
        startCountDownTimer()

        // set onclick open dialog messages activity
        binding.listAllMessages.setOnItemClickListener { parent: AdapterView<*>, view: View, position: Int, id: Long ->
            val companionUserName: String = Storage.dialogSystemParameters[position].dialogUsername
            val intent = Intent (context, MessageDialogActivity::class.java)
            intent.putExtra("COMPANION_USER_NAME", companionUserName)
            requireActivity().startActivity(intent)
        }

        // Get last messages and lastmessages-date of all dialogs
        SignalRMessengerHub.getMessagingResponseForDialogs()
        SignalRMessengerHub.getNewMessagesResponse()

        // Get all dialogs cards
        SignalRMessengerHub.getMessagingCardsResponse(requireActivity())
        SignalRMessengerHub.getMessagingCardsRequest()
    }

    private fun updateAllDialogs() {
        //SignalRMessengerHub.removeAllHubHandlers()
        SignalRMessengerHub.getHubContext().remove("GetNewMessagesResponse")
        SignalRMessengerHub.getNewMessagesResponse()
        SignalRMessengerHub.removeUpdateMessagesListener()
        val listType = object : TypeToken<ArrayList<MessagingCardApiModel?>?>() {}.type
        SignalRMessengerHub.getHubContext().on<ArrayList<MessagingCardApiModel>>("MessagingsIsUpdated", { item ->

            val listOfItems: ArrayList<MessagingCardApiModel> = item
            listOfItems.forEach { item ->

                val dialogNewMessagesCount = Storage.dialogSystemParameters.find { dialog -> dialog.dialogUsername == item.userName}?.dialogNewMessagesCount ?: 0
                if(item.countOfNotViewedMessages > dialogNewMessagesCount) {
                    Storage.dialogSystemParameters.find { dialog -> dialog.dialogUsername == item.userName}?.dialogNewMessagesCount = item.countOfNotViewedMessages
                    Storage.dialogSystemParameters.find { dialog -> dialog.dialogUsername == item.userName}?.dialogNewMessagesCountDownloaded = true
                    Storage.dialogSystemParameters.find { dialog -> dialog.dialogUsername == item.userName}?.dialogLastMessageDownloaded = false
                    Storage.dialogSystemParameters.find { dialog -> dialog.dialogUsername == item.userName}?.dialogLastMessageDateDownloaded = false

                    Storage.currentUpdatedDialog = item.userName

                    SignalRMessengerHub.getNewMessagesRequest(item.userName)

                    stopCheckDownloadDialogsComponents()
                    startCheckDownloadDialogsComponents()
                }

            }

        }, listType)
    }

    override fun onDestroyView() {
        super.onDestroyView()
        _binding = null
        Storage.dialogSystemParameters.clear()
        // Stop Connection to Hub (or delete just all handlers???)
        //SignalRMessengerHub.stopConnectionHub()
        SignalRMessengerHub.getHubContext().remove("GetNewMessagesResponse")
        SignalRMessengerHub.removeAllHubHandlers()
    }

    override fun onResume() {
        super.onResume()
        if(firstStart) {
            firstStart = false
            return
        }
        getAllDialogs()
        updateAllDialogs()
    }

    private fun startCheckDownloadDialogsComponents() {
        timer?.start()
    }

    private fun stopCheckDownloadDialogsComponents() {
        timer?.cancel()
    }

    private fun startCountDownTimer(){
        timer = object: CountDownTimer(800, 400) {
            override fun onTick(millisUntilFinished: Long) {
                //Toast.makeText(_binding!!.root.context,"+++", Toast.LENGTH_SHORT).show()
            }

            override fun onFinish() {
                var dialogCount: Int = 0
                try {
                    for(dialog in Storage.dialogSystemParameters) {
                        if(!dialog.dialogImagesDownloaded ||
                            !dialog.dialogUsernameDownloaded ||
                            !dialog.dialogLastMessageDownloaded ||
                            !dialog.dialogLastMessageDateDownloaded ||
                            !dialog.dialogNewMessagesCountDownloaded) {
                            break
                        }

                        dialogCount++

                        if(dialogCount == Storage.dialogSystemParameters.size) {
                            binding.listAllMessages.invalidateViews()
                            return
                        }
                    }
                } catch (e: Exception){
                }
                startCheckDownloadDialogsComponents()
            }
        }.start()
    }
}