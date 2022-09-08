package projects.quidpro.models

import com.google.gson.annotations.SerializedName

data class RecommendationResponce(
    @SerializedName("id")
    val id: Int,
    @SerializedName("userName")
    val userName: String,
    @SerializedName("avatarFileName")
    val avatarFileName: String,
    @SerializedName("biographi")
    val biographi: String,
    @SerializedName("role")
    val role: String
)
