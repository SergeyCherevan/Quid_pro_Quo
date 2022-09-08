package projects.quidpro.adapters

import android.annotation.SuppressLint
import android.content.Context
import android.content.Intent
import android.os.Build
import android.provider.SyncStateContract
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.*
import projects.quidpro.Helpers.FileController
import projects.quidpro.Helpers.ParseTime
import projects.quidpro.Helpers.ReverseGeocoding
import projects.quidpro.R
import projects.quidpro.models.TicketResponce
import projects.quidpro.models.TicketSystemParameters
import projects.quidpro.storage.Storage
import java.lang.Exception

class AllTicketsAdapter(var mCtx: Context, private val resources: Int, private val items: List<TicketResponce>): ArrayAdapter<TicketResponce>(mCtx, resources, items) {
    @SuppressLint("ViewHolder")
    override fun getView(position: Int, convertView: View?, parent: ViewGroup): View {

        val layoutInflater: LayoutInflater = LayoutInflater.from(mCtx)
        val view: View = layoutInflater.inflate(resources, null)

        // TICKET IMAGE
        val imageViewTiketImg: ImageView = view.findViewById(R.id.imageViewTiketImg)

        val textViewTicketTitle: TextView = view.findViewById(R.id.textViewTicketTitle)
        val textViewCityOfTicket: TextView = view.findViewById(R.id.textViewCityOfTicket)
        val textViewTimeOfTicket: TextView = view.findViewById(R.id.textViewTimeOfTicket)

        val buttonCommentsOfTicket: Button = view.findViewById(R.id.buttonCommentsOfTicket)
        val imageButtonStatusOfTicket: ImageButton = view.findViewById(R.id.imageButtonStatusOfTicket)

        /*buttonCommentsOfTicket.setOnClickListener {
            var intent = Intent (mCtx, ActivityNotificationScreen::class.java)
            if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.P) {
                intent.addFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
            }
            intent.putExtra("apiary_id", items[position].Id)
            mCtx.startActivity(intent)
        }*/

        val mItem = items[position]

        try {
            if(Storage.ticketSystemParametersList.find { it.ticketId == mItem.Id }?.ticketImagesDownloaded == true)
                imageViewTiketImg.setImageBitmap(Storage.ticketSystemParametersList.find { it.ticketId == mItem.Id }?.ticketImage)
            else
                imageViewTiketImg.setImageResource(R.drawable.default_ticket_img)
        } catch (e: Exception) {
            imageViewTiketImg.setImageResource(R.drawable.default_ticket_img)
        }

        textViewTicketTitle.text = mItem.Title

        try {
            if(Storage.ticketSystemParametersList.find { it.ticketId == mItem.Id }?.ticketCitiesDownloaded == true)
                textViewCityOfTicket.text = Storage.ticketSystemParametersList.find { it.ticketId == mItem.Id }?.ticketCity
            else
                textViewCityOfTicket.setText(R.string.default_ticket_city)
        } catch (e: Exception) {
            textViewCityOfTicket.setText(R.string.default_ticket_city)
        }

        textViewTimeOfTicket.text = ParseTime.parse(mItem.PostedAt)

        // COUNT OF COMMENTS IN TICKET
        //buttonCommentsOfTicket.text = mItem.CountOfBeehives.toString()

        if(!mItem.IsActual) imageButtonStatusOfTicket.setImageResource(R.drawable.ic_emoji_second) else imageButtonStatusOfTicket.setImageResource(R.drawable.ic_emoji_first)

        return view
    }
}