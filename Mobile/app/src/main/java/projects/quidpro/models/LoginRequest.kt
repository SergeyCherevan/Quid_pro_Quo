package projects.quidpro.models

import com.google.gson.annotations.SerializedName

data class LoginRequest(
    @SerializedName("username")
    val UserName: String,
    @SerializedName("password")
    val Password: String
)
