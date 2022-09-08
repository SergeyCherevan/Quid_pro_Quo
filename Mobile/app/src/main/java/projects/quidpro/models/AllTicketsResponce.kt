package projects.quidpro.models

import com.google.gson.annotations.SerializedName

data class AllTicketsResponce(
    @SerializedName("posts")
    val Tickets: ArrayList<TicketResponce>,
    @SerializedName("postsCount")
    val TicketsCount: Int
)
