package projects.quidpro.Helpers

import android.app.Activity
import okhttp3.MediaType.Companion.toMediaTypeOrNull
import okhttp3.MultipartBody
import okhttp3.RequestBody
import okhttp3.RequestBody.Companion.asRequestBody
import java.io.File

class MultiPartHelper {
    companion object {

        fun createMultiPartString(partName: String, text: String): MultipartBody.Part {
            return MultipartBody.Part.createFormData(partName, text)
        }

        fun createMultiPartMediaType(partName: String, fileMedia: File): MultipartBody.Part {
            val requestBody: RequestBody = fileMedia.asRequestBody("multipart/form-data".toMediaTypeOrNull())
            return MultipartBody.Part.createFormData(partName, fileMedia.name, requestBody)
        }

        // --- EXAMPLE OF RETROFIT REQUEST //

        // Create MultipartBody.Part object for each part-element - createMultiPartString() OR createMultiPartMediaType()

        /*    @Multipart
              @POST("Post/publish")
                      fun publishTicket(@Part title: MultipartBody.Part,
                          @Part description: MultipartBody.Part,
                          @Part geomarker: MultipartBody.Part,
                          @Part() dates: ArrayList<MultipartBody.Part>,
                          @Part() files: ArrayList<MultipartBody.Part>,
                          @Header("Authorization") auth: String): Call<JsonObject>    */

    }
}