package projects.quidpro.models

import com.google.gson.annotations.SerializedName

data class TicketResponce(
    @SerializedName("id")
    val Id: Int,
    @SerializedName("title")
    val Title: String,
    @SerializedName("text")
    val Text: String,
    @SerializedName("imageFileNames")
    val ImageFileNames: String,
    @SerializedName("authorName")
    val AuthorName: String,
    @SerializedName("postedAt")
    val PostedAt: String,
    @SerializedName("isActual")
    val IsActual: Boolean,
    @SerializedName("performServiceOnDatesList")
    val PerformServiceOnDatesList: ArrayList<String>,
    /*@SerializedName("performServiceInPlace")
    val PerformServiceInPlace: String*/
    @SerializedName("performServiceInPlaceLat")
    val PerformServiceInPlaceLat: Double,
    @SerializedName("performServiceInPlaceLng")
    val PerformServiceInPlaceLng: Double,
    @SerializedName("performServiceInPlaceZoom")
    val PerformServiceInPlaceZoom: Double
)
