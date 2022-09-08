package projects.quidpro.adapters

import android.annotation.SuppressLint
import android.app.Dialog
import android.content.Context
import android.graphics.Color
import android.graphics.Typeface
import android.graphics.drawable.ColorDrawable
import android.opengl.Visibility
import android.view.ActionProvider
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.*
import androidx.cardview.widget.CardView
import androidx.core.content.ContextCompat
import androidx.core.view.isVisible
import projects.quidpro.R
import projects.quidpro.models.DialogSystemParameters
import projects.quidpro.models.RecommendationResponce
import projects.quidpro.storage.Storage
import java.lang.Exception

class AllDialogsAdapter(var mCtx: Context, private val resources: Int, private val items: ArrayList<DialogSystemParameters>, val fragmentContext: Context): ArrayAdapter<DialogSystemParameters>(mCtx, resources, items) {

    @SuppressLint("ViewHolder")
    override fun getView(position: Int, convertView: View?, parent: ViewGroup): View {

        val layoutInflater: LayoutInflater = LayoutInflater.from(mCtx)
        val view: View = layoutInflater.inflate(resources, null)

        // Dialog-User Avatar IMAGE
        val imageViewDialogProfilePhoto: ImageView = view.findViewById(R.id.imageViewDialogProfilePhoto)

        // Dialog-User Avatar CARD
        val cardDialogProfilePhoto: CardView = view.findViewById(R.id.cardDialogProfilePhoto)

        // Dialog-User username
        val textViewDialogUserName: TextView = view.findViewById(R.id.textViewDialogUserName)

        // Dialog-User last message
        val textViewDialogLastMessage: TextView = view.findViewById(R.id.textViewDialogLastMessage)

        // Dialog-User last message data
        val textViewDialogLastMessageData: TextView = view.findViewById(R.id.textViewDialogLastMessageData)

        // Dialog-User count new messages CARD
        val cardDialogNewMessageCount: CardView = view.findViewById(R.id.cardDialogNewMessageCount)

        // Dialog-User count new messages TextView
        val textViewDialogNewMessageCount: TextView = view.findViewById(R.id.textViewDialogNewMessageCount)

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

        /*buttonCommentsOfTicket.setOnClickListener {
            var intent = Intent (mCtx, ActivityNotificationScreen::class.java)
            if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.P) {
                intent.addFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
            }
            intent.putExtra("apiary_id", items[position].Id)
            mCtx.startActivity(intent)
        }*/

        val mItem = items[position]

        if(mItem.dialogUsername.isNotEmpty() && mItem.dialogUsernameDownloaded)
            textViewDialogUserName.text = mItem.dialogUsername

        if(mItem.dialogLastMessageDateDownloaded)
            textViewDialogLastMessage.text = mItem.dialogLastMessage

        if(mItem.dialogImagesDownloaded) {
            try {
                imageViewDialogProfilePhoto.setImageBitmap(mItem.dialogImage)
            } catch (e: Exception) {
                imageViewDialogProfilePhoto.setImageResource(R.drawable.ic_person_first)
            }
        } else imageViewDialogProfilePhoto.setImageResource(R.drawable.ic_person_first)

        if(mItem.dialogLastMessageDate.isNotEmpty() && mItem.dialogLastMessageDateDownloaded)
            textViewDialogLastMessageData.text = mItem.dialogLastMessageDate
        else textViewDialogLastMessageData.text = ""

        if(mItem.dialogNewMessagesCount > 0 && mItem.dialogNewMessagesCountDownloaded) {
            cardDialogNewMessageCount.visibility = View.VISIBLE
            textViewDialogNewMessageCount.text = mItem.dialogNewMessagesCount.toString()
            textViewDialogUserName.typeface = Typeface.DEFAULT_BOLD
            textViewDialogLastMessage.typeface = Typeface.DEFAULT_BOLD
            textViewDialogLastMessage.setTextColor(Color.BLACK)
        } else if(mItem.dialogNewMessagesCount == 0 && mItem.dialogNewMessagesCountDownloaded) {
            cardDialogNewMessageCount.visibility = View.INVISIBLE
            textViewDialogUserName.typeface = Typeface.DEFAULT
            textViewDialogLastMessage.typeface = Typeface.DEFAULT
            textViewDialogLastMessage.setTextColor(ContextCompat.getColor(fragmentContext, R.color.buttonPassive))
        }

        return view
    }

}