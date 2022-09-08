package projects.quidpro.adapters

import android.annotation.SuppressLint
import android.content.Context
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.*
import projects.quidpro.Helpers.ParseTime
import projects.quidpro.R
import projects.quidpro.models.signalRMessageModels.MessageApiModel

class AllDialogMessagesAdapter(private var mCtx: Context, private val resources: Int, private val items: ArrayList<MessageApiModel>, private val userName: String, private val userCompanionName: String): ArrayAdapter<MessageApiModel>(mCtx, resources, items) {

    @SuppressLint("ViewHolder")
    override fun getView(position: Int, convertView: View?, parent: ViewGroup): View {

        val layoutInflater: LayoutInflater = LayoutInflater.from(mCtx)
        val view: View = layoutInflater.inflate(resources, null) // set 'parent' [2 parameter]
        val message = items[position]

        /*cardDialogProfilePhoto.setOnLongClickListener {
            try {

                // Full Photo View Dialog Box Settings
                val dialogBoxFullPhotoView = Dialog(fragmentContext)
                dialogBoxFullPhotoView.setContentView(R.layout.create_ticket_fullphoto_custom_dialogbox)
                dialogBoxFullPhotoView.window?.setBackgroundDrawable(ColorDrawable(Color.TRANSPARENT))

                val imageViewFullPhotoView: ImageView? = dialogBoxFullPhotoView.findViewById<ImageView>(
                    R.id.imageViewFullPhotoView)
                imageViewFullPhotoView?.setImageDrawable(imageViewDialogProfilePhoto.drawable)

                val imageButtonCloseFullPhotoView: Button? = dialogBoxFullPhotoView.findViewById<Button>(
                    R.id.imageButtonCloseFullPhotoView)
                imageButtonCloseFullPhotoView?.setOnClickListener {
                    dialogBoxFullPhotoView.dismiss()
                }

                dialogBoxFullPhotoView.show()

            } catch (e: Exception) {
                Toast.makeText(fragmentContext, "Error open user avatar.", Toast.LENGTH_SHORT).show()
            }

            return@setOnLongClickListener true
        }*/

        if(message.authorName == userCompanionName)
            addNewMessageFrom(message.text, message.postedAt, layoutInflater, (view as ViewGroup))
        else if (message.authorName == userName)
            addNewMessageTo(message.text, message.postedAt, layoutInflater, (view as ViewGroup), message.notViewed)

        return view
    }

    private fun addNewMessageFrom(messageText: String, messageTime: String, layoutInflater: LayoutInflater, root: ViewGroup) {

        val newMessageFrom: View = layoutInflater.inflate(R.layout.item_row_message_from, root, false) // change to false

        newMessageFrom.findViewById<TextView>(R.id.textViewNewMessageFromText).text = messageText
        newMessageFrom.findViewById<TextView>(R.id.textViewNewMessageFromTimeText).text = ParseTime.parseForMessage(messageTime)

        /*addedPhotoView.findViewById<Button>(R.id.imageButtonCreateTicketClearPhoto).setOnClickListener {
            val parentLLSecondView: LinearLayout = (it.parent.parent as LinearLayout)
            val parentCardView: CardView = (parentLLSecondView.parent as CardView)
            val parentRLView: RelativeLayout = (parentCardView.parent as RelativeLayout)
            val parentLinearLayoutView: LinearLayout = (parentRLView.parent as LinearLayout)
            val indexOfClickedCardView: Int = parentLinearLayoutView.indexOfChild(parentRLView)
            binding.listNewPhotos.removeViewAt(indexOfClickedCardView)
            Storage.ticketsCreate.photosAsFileList.removeAt(indexOfClickedCardView)
        }

        val clickedPhoto = addedPhotoView.findViewById<ImageView>(R.id.imageViewCreateTicketNewPhoto).drawable
        addedPhotoView.findViewById<ImageView>(R.id.imageViewCreateTicketNewPhoto).setOnClickListener {
            openFullPhotoViewDialogBox(clickedPhoto)
        }*/

        root.addView(newMessageFrom)
    }

    private fun addNewMessageTo(messageText: String, messageTime: String, layoutInflater: LayoutInflater, root: ViewGroup, msgNotViewedStatus: Boolean) {

        val newMessageTo: View = layoutInflater.inflate(R.layout.item_row_message_to, root, false) // change to false

        newMessageTo.findViewById<TextView>(R.id.textViewNewMessageToText).text = messageText
        newMessageTo.findViewById<TextView>(R.id.textViewNewMessageToTimeText).text = ParseTime.parseForMessage(messageTime)
        if(msgNotViewedStatus)
            newMessageTo.findViewById<ImageView>(R.id.imageViewNewMessageSendStatus).setImageResource(R.drawable.ic_double_check_one_unconfirmed)
        else if(!msgNotViewedStatus)
            newMessageTo.findViewById<ImageView>(R.id.imageViewNewMessageSendStatus).setImageResource(R.drawable.ic_double_check_one_confirmed)

        /*addedPhotoView.findViewById<Button>(R.id.imageButtonCreateTicketClearPhoto).setOnClickListener {
            val parentLLSecondView: LinearLayout = (it.parent.parent as LinearLayout)
            val parentCardView: CardView = (parentLLSecondView.parent as CardView)
            val parentRLView: RelativeLayout = (parentCardView.parent as RelativeLayout)
            val parentLinearLayoutView: LinearLayout = (parentRLView.parent as LinearLayout)
            val indexOfClickedCardView: Int = parentLinearLayoutView.indexOfChild(parentRLView)
            binding.listNewPhotos.removeViewAt(indexOfClickedCardView)
            Storage.ticketsCreate.photosAsFileList.removeAt(indexOfClickedCardView)
        }

        val clickedPhoto = addedPhotoView.findViewById<ImageView>(R.id.imageViewCreateTicketNewPhoto).drawable
        addedPhotoView.findViewById<ImageView>(R.id.imageViewCreateTicketNewPhoto).setOnClickListener {
            openFullPhotoViewDialogBox(clickedPhoto)
        }*/

        root.addView(newMessageTo)
    }

}