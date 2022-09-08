package projects.quidpro.models

import com.google.gson.annotations.SerializedName

data class UserProfileInfoResponce(
    @SerializedName("id")
    val Id: Int,
    @SerializedName("userName")
    val UserName: String,
    @SerializedName("avatarFileName")
    val AvatarFileName: String,
    @SerializedName("biographi")
    val Biography: String,
    @SerializedName("role")
    val Role: String
)
