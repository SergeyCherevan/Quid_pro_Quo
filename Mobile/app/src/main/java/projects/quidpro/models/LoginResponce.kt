package projects.quidpro.models

import com.google.gson.annotations.SerializedName

data class LoginResponce(
/*    @SerializedName("id")
    val Id: Int,
    @SerializedName("username")
    val UserName: String,
    @SerializedName("avatarfilename")
    val AvatarFileName: String,
    @SerializedName("biographi")
    val Biography: String,
    @SerializedName("role")
    val Role: String,*/
    @SerializedName("jwtString")
    val JwtString: String
)
