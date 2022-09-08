package projects.quidpro.models

import android.net.Uri
import java.io.File

class UserAvatarSettings {
    var avatarAsUri: Uri? = null
    var avatarAsFile: File = File("")
    var avatarName = ""
}