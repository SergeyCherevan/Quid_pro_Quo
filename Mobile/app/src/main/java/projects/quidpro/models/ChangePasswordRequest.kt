package projects.quidpro.models

import com.google.gson.annotations.SerializedName

data class ChangePasswordRequest(
    @SerializedName("OldPassword")
    val OldPassword: String,
    @SerializedName("NewPassword")
    val NewPassword: String
)
