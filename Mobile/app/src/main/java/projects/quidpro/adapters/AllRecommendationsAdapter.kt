package projects.quidpro.adapters

import android.annotation.SuppressLint
import android.app.Dialog
import android.content.Context
import android.graphics.Color
import android.graphics.drawable.ColorDrawable
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.*
import androidx.cardview.widget.CardView
import projects.quidpro.Helpers.ParseTime
import projects.quidpro.R
import projects.quidpro.models.RecommendationResponce
import projects.quidpro.models.TicketResponce
import projects.quidpro.storage.Storage
import java.lang.Exception

class AllRecommendationsAdapter(var mCtx: Context, private val resources: Int, private val items: List<RecommendationResponce>, val fragmentContext: Context): ArrayAdapter<RecommendationResponce>(mCtx, resources, items) {
    @SuppressLint("ViewHolder")
    override fun getView(position: Int, convertView: View?, parent: ViewGroup): View {

        val layoutInflater: LayoutInflater = LayoutInflater.from(mCtx)
        val view: View = layoutInflater.inflate(resources, null)

        // User id
        val textViewRecommendationId: TextView = view.findViewById(R.id.textViewRecommendationId)

        // User Avatar IMAGE
        val imageViewRecommendationProfilePhoto: ImageView = view.findViewById(R.id.imageViewRecommendationProfilePhoto)

        // User Avatar Image CARD
        val cardRecommendationProfilePhoto: CardView = view.findViewById(R.id.cardRecommendationProfilePhoto)

        // User id
        val textViewRecommendationUserName: TextView = view.findViewById(R.id.textViewRecommendationUserName)

        // User id
        val textViewRecommendationUserBiography: TextView = view.findViewById(R.id.textViewRecommendationUserBiography)

        // User Like IMAGE
        val recommendationsUserLike: ImageView = view.findViewById(R.id.recommendationsUserLike)

        // User Dislike IMAGE
        val recommendationsUserDislike: ImageView = view.findViewById(R.id.recommendationsUserDislike)

        recommendationsUserLike.setImageResource(R.drawable.ic_like_off)
        recommendationsUserDislike.setImageResource(R.drawable.ic_dislike_off)

        recommendationsUserLike.setOnClickListener {
            recommendationsUserLike.setImageResource(R.drawable.ic_like_on)
            recommendationsUserDislike.setImageResource(R.drawable.ic_dislike_off)
        }

        recommendationsUserDislike.setOnClickListener {
            recommendationsUserDislike.setImageResource(R.drawable.ic_dislike_on)
            recommendationsUserLike.setImageResource(R.drawable.ic_like_off)
        }

        cardRecommendationProfilePhoto.setOnLongClickListener {
            try {

                // Full Photo View Dialog Box Settings
                val dialogBoxFullPhotoView = Dialog(fragmentContext)
                dialogBoxFullPhotoView.setContentView(R.layout.create_ticket_fullphoto_custom_dialogbox)
                dialogBoxFullPhotoView.window?.setBackgroundDrawable(ColorDrawable(Color.TRANSPARENT))

                val imageViewFullPhotoView: ImageView? = dialogBoxFullPhotoView.findViewById<ImageView>(R.id.imageViewFullPhotoView)
                imageViewFullPhotoView?.setImageDrawable(imageViewRecommendationProfilePhoto.drawable)

                val imageButtonCloseFullPhotoView: Button? = dialogBoxFullPhotoView.findViewById<Button>(R.id.imageButtonCloseFullPhotoView)
                imageButtonCloseFullPhotoView?.setOnClickListener {
                    dialogBoxFullPhotoView.dismiss()
                }

                dialogBoxFullPhotoView.show()

            } catch (e: Exception) {
                Toast.makeText(fragmentContext, "Error open user avatar.", Toast.LENGTH_SHORT).show()
            }

            return@setOnLongClickListener true
        }

        /*buttonCommentsOfTicket.setOnClickListener {
            var intent = Intent (mCtx, ActivityNotificationScreen::class.java)
            if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.P) {
                intent.addFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
            }
            intent.putExtra("apiary_id", items[position].Id)
            mCtx.startActivity(intent)
        }*/

        val mItem = items[position]

        textViewRecommendationId.text = mItem.id.toString()

        try {
            if(Storage.recommendationSystemParametersList.find { it.recommendationId == mItem.id}?.recommendationImagesDownloaded == true)
                imageViewRecommendationProfilePhoto.setImageBitmap(Storage.recommendationSystemParametersList.find { it.recommendationId == mItem.id }?.recommendationImage)
            else
                imageViewRecommendationProfilePhoto.setImageResource(R.drawable.ic_person_first)
        } catch (e: Exception) {
            imageViewRecommendationProfilePhoto.setImageResource(R.drawable.ic_person_first)
        }

        textViewRecommendationUserName.text = mItem.userName
        textViewRecommendationUserBiography.text = mItem.biographi

        return view
    }
}