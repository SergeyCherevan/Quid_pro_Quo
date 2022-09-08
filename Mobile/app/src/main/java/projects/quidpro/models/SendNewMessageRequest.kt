package projects.quidpro.models

import com.google.gson.annotations.SerializedName

data class SendNewMessageRequest(
    @SerializedName("text")
    val text: String,
    @SerializedName("destinationName")
    val destinationName: String,
    @SerializedName("imageFile")
    val imageFile: String?,
    @SerializedName("file")
    val file: String?
)
