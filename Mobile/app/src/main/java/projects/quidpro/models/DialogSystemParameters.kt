package projects.quidpro.models

import android.app.Activity
import android.graphics.Bitmap
import androidx.core.content.res.ResourcesCompat
import androidx.core.graphics.drawable.toBitmap
import projects.quidpro.Helpers.FileController
import projects.quidpro.R

class DialogSystemParameters {
    var dialogId = 0

    var dialogImagesDownloaded = false
    var dialogImage: Bitmap = Bitmap.createBitmap(100, 100, Bitmap.Config.ARGB_8888)

    var dialogUsernameDownloaded = false
    var dialogUsername: String = ""

    var dialogLastMessageDownloaded = false
    var dialogLastMessage: String = ""

    var dialogLastMessageDateDownloaded = false
    var dialogLastMessageDate: String = ""

    var dialogNewMessagesCountDownloaded = false
    var dialogNewMessagesCount: Int = 0

    constructor() {

    }

    constructor(_dialogId: Int = 0, _dialogImagesDownloaded: Boolean,
                _dialogUsernameDownloaded: Boolean, _dialogUsername: String = "",
                _dialogLastMessageDownloaded: Boolean, _dialogLastMessage: String = "",
                _dialogLastMessageDateDownloaded: Boolean, _dialogLastMessageDate: String = "",
                _dialogNewMessagesCountDownloaded: Boolean, _dialogNewMessagesCount: Int = 0,
                activity: Activity) {

        dialogId = _dialogId

        dialogImagesDownloaded = _dialogImagesDownloaded

        dialogUsernameDownloaded = _dialogUsernameDownloaded
        dialogUsername = _dialogUsername

        dialogLastMessageDownloaded = _dialogLastMessageDownloaded
        dialogLastMessage = _dialogLastMessage

        dialogLastMessageDateDownloaded = _dialogLastMessageDateDownloaded
        dialogLastMessageDate = _dialogLastMessageDate

        dialogNewMessagesCountDownloaded = _dialogNewMessagesCountDownloaded
        dialogNewMessagesCount = _dialogNewMessagesCount

        try {

            if(dialogUsername.isNotEmpty() && dialogUsernameDownloaded)
                FileController.getAvatar(dialogUsername, dialogObj = this)
            else {
                dialogImage = ResourcesCompat.getDrawable(activity.resources, R.drawable.ic_person_first, activity.theme)?.toBitmap()!!
                dialogImagesDownloaded = true
            }

        } catch (e: Exception) {
            dialogImagesDownloaded = false
        }

    }
}