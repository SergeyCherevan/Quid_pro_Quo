package projects.quidpro.models

import com.google.gson.annotations.SerializedName

data class RegisterRequest(
    @SerializedName("username")
    val UserName: String,
    @SerializedName("password")
    val Password: String
)
