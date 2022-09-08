package projects.quidpro.models

import com.google.gson.annotations.SerializedName

data class AllTicketsRequest(
    @SerializedName("keywords")
    val Keywords: String,
    @SerializedName("date")
    val Date: String,
    @SerializedName("geomarker")
    val Geomarker: String,
    @SerializedName("pageNumber")
    val PageNumber: Int,
    @SerializedName("pageSize")
    val pageSize: Int
)
