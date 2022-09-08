package projects.quidpro.models

import android.app.Activity
import android.graphics.Bitmap
import androidx.core.content.res.ResourcesCompat
import androidx.core.graphics.drawable.toBitmap
import projects.quidpro.Helpers.FileController
import projects.quidpro.R


class RecommendationSystemParameters {
    var recommendationId = 0
    var recommendationImagesDownloaded = false
    var recommendationImage: Bitmap = Bitmap.createBitmap(100, 100, Bitmap.Config.ARGB_8888)

    constructor(_recommendationId: Int) {
        recommendationId = _recommendationId
    }

    constructor(_recommendationId: Int, _recommendationImagesDownloaded: Boolean, _imageFileName: String, activity: Activity) {
        recommendationId = _recommendationId
        recommendationImagesDownloaded = _recommendationImagesDownloaded

        try {
            if(_imageFileName.isNotEmpty())
                FileController.getAvatar(_imageFileName, recommendationObj = this)
            else {
                recommendationImage = ResourcesCompat.getDrawable(activity.resources, R.drawable.ic_person_first, activity.theme)?.toBitmap()!!
                recommendationImagesDownloaded = true
            }
        } catch (e: Exception) {
            recommendationImagesDownloaded = false
        }

    }
}