package projects.quidpro.models

import android.app.Activity
import android.content.Context
import android.content.res.Resources
import android.graphics.Bitmap
import android.graphics.BitmapFactory
import android.graphics.drawable.Drawable
import androidx.core.content.res.ResourcesCompat
import androidx.core.graphics.drawable.toBitmap
import projects.quidpro.Helpers.FileController
import projects.quidpro.Helpers.ReverseGeocoding
import projects.quidpro.R
import java.lang.Exception

class TicketSystemParameters {
    var ticketId = 0
    var ticketImagesDownloaded = false
    var ticketCitiesDownloaded = false
    var ticketImage: Bitmap = Bitmap.createBitmap(100, 100, Bitmap.Config.ARGB_8888)
    var ticketCity = "City of country"

    constructor(_ticketId: Int) {
        ticketId = _ticketId
    }

    constructor(_ticketId: Int, _ticketImagesDownloaded: Boolean, _ticketCitiesDownloaded: Boolean, _imageFileNames: String, _PerformServiceInPlaceLat: Double, _PerformServiceInPlaceLng: Double, activity: Activity) {
        ticketId = _ticketId
        ticketImagesDownloaded = _ticketImagesDownloaded
        ticketCitiesDownloaded = _ticketCitiesDownloaded

        try {
            if(_imageFileNames.isNotEmpty())
                FileController.getImage(_imageFileNames.split(";")[0], ticketObj = this)
            else {
                ticketImage = ResourcesCompat.getDrawable(activity.resources, R.drawable.default_ticket_img, activity.theme)?.toBitmap()!!
                ticketImagesDownloaded = true
            }
        } catch (e: Exception) {
            ticketImagesDownloaded = false
        }

        try {
            if(_PerformServiceInPlaceLat.toString().isNotEmpty() && _PerformServiceInPlaceLng.toString().isNotEmpty())
                ReverseGeocoding.getInfoAboutTicketCity(_PerformServiceInPlaceLat.toString(), _PerformServiceInPlaceLng.toString(), "en", ticketObj = this)
            else {
                ticketCity = "City of country"
                ticketImagesDownloaded = true
            }
        } catch (e: Exception) {
            ticketCitiesDownloaded = false
        }

        // нижнее закоментить , верхнее раскоментить - и все!

        // ticketCitiesDownloaded = true
    }


}