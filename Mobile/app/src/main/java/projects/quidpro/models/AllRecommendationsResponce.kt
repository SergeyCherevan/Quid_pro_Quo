package projects.quidpro.models

import com.google.gson.annotations.SerializedName

data class AllRecommendationsResponce(
    @SerializedName("users")
    val Recommendations: ArrayList<RecommendationResponce>,
    @SerializedName("usersCount")
    val usersCount: Int
)
