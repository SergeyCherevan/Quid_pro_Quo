package projects.quidpro.Helpers

import android.content.Context
import android.content.res.Resources
import android.graphics.Bitmap
import android.graphics.BitmapFactory
import android.graphics.drawable.Drawable
import android.net.Uri
import android.provider.DocumentsContract
import android.provider.MediaStore
import android.util.Log
import android.widget.ImageView
import android.widget.TextView
import androidx.core.graphics.drawable.DrawableCompat
import com.google.gson.JsonObject
import okhttp3.RequestBody
import okhttp3.ResponseBody
import okio.ByteString.Companion.readByteString
import projects.quidpro.R
import projects.quidpro.api.servers.geo.GeoClientBuilder
import projects.quidpro.api.servers.geo.GeoRequests
import projects.quidpro.api.servers.main.MainClientBuilder
import projects.quidpro.api.servers.main.MainRequests
import projects.quidpro.models.DialogSystemParameters
import projects.quidpro.models.RecommendationSystemParameters
import projects.quidpro.models.TicketSystemParameters
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response
import java.io.File
import java.lang.Exception

class FileController {
    companion object {
        fun getImage(fileName: String, ticketObj: TicketSystemParameters) {

            val request = MainClientBuilder.buildClient(MainRequests::class.java)
            val response = request.getImage(fileName)

            response.enqueue(object: Callback<ResponseBody> {
                override fun onResponse(call: Call<ResponseBody>, response: Response<ResponseBody>) {
                    if (response.isSuccessful) {
                        val data = response.body()!!
                        try {
                            ticketObj.ticketImage = BitmapFactory.decodeStream(data.byteStream())
                            ticketObj.ticketImagesDownloaded = true
                        } catch (e: Exception) {}
                    }
                }

                override fun onFailure(call: Call<ResponseBody>, t: Throwable) {
                    Log.println(Log.ASSERT, "Get Image", "Fail to get image of ticket")
                }
            })
        }

        fun getAvatar(fileName: String, recommendationObj: RecommendationSystemParameters) {

            val request = MainClientBuilder.buildClient(MainRequests::class.java)
            val response = request.getAvatar(fileName)

            response.enqueue(object: Callback<ResponseBody> {
                override fun onResponse(call: Call<ResponseBody>, response: Response<ResponseBody>) {
                    if (response.isSuccessful) {
                        val data = response.body()!!
                        try {
                            recommendationObj.recommendationImage = BitmapFactory.decodeStream(data.byteStream())
                            recommendationObj.recommendationImagesDownloaded = true
                        } catch (e: Exception) {}
                    }
                }

                override fun onFailure(call: Call<ResponseBody>, t: Throwable) {
                    Log.println(Log.ASSERT, "Get Avatar", "Fail to get avatar of user")
                }
            })
        }

        fun getAvatar(userName: String, dialogObj: DialogSystemParameters) {

            val request = MainClientBuilder.buildClient(MainRequests::class.java)
            val response = request.getAvatarByUserName(userName)

            response.enqueue(object: Callback<ResponseBody> {
                override fun onResponse(call: Call<ResponseBody>, response: Response<ResponseBody>) {
                    if (response.isSuccessful) {
                        val data = response.body()!!
                        try {
                            dialogObj.dialogImage = BitmapFactory.decodeStream(data.byteStream())
                            dialogObj.dialogImagesDownloaded = true
                        } catch (e: Exception) {}
                    }
                }

                override fun onFailure(call: Call<ResponseBody>, t: Throwable) {
                    Log.println(Log.ASSERT, "Get Avatar", "Fail to get avatar of user")
                }
            })
        }

        //val mediaType = "multipart/form-data".toMediaTypeOrNull()

        /*fun getPartBodyFromUri(context: Context, uri: Uri): MultipartBody.Part {
            val realPath = getPathFromURI(context, uri)
            val fileImage = createFile(realPath)
            val requestBody = createRequestBody(fileImage)
            return createPart(fileImage, requestBody)
        }*/

        fun getFileFromUri(context: Context, uri: Uri): File {
            val realPath = getPathFromURI(context, uri)
            //val fileImage = createFile(realPath)
            //val requestBody = createRequestBody(fileImage)
            //return createPart(fileImage, requestBody)
            return createFile(realPath)
        }

        private fun createFile(realPath: String): File {
            return File(realPath)
        }

        /*private fun createRequestBody(file: File): RequestBody {
            return file.asRequestBody(mediaType)
        }

        private fun createPart(file: File, requestBody: RequestBody): MultipartBody.Part {
            return MultipartBody.Part.createFormData("imageFile", file.name, requestBody)
        }*/

        private fun getPathFromURI(context: Context, uri: Uri): String {
            var realPath = String()
            uri.path?.let { path ->

                val databaseUri: Uri
                val selection: String?
                val selectionArgs: Array<String>?

                if (path.contains("/document/image:")) { // files selected from "Documents"
                    databaseUri = MediaStore.Images.Media.EXTERNAL_CONTENT_URI
                    selection = "_id=?"
                    selectionArgs = arrayOf(DocumentsContract.getDocumentId(uri).split(":")[1])
                } else { // files selected from all other sources, especially on Samsung devices
                    databaseUri = uri
                    selection = null
                    selectionArgs = null
                }

                try {
                    val column = "_data"
                    val projection = arrayOf(column)
                    val cursor = context.contentResolver.query(
                        databaseUri,
                        projection,
                        selection,
                        selectionArgs,
                        null
                    )
                    cursor?.let {
                        if (it.moveToFirst()) {
                            val columnIndex = cursor.getColumnIndexOrThrow(column)
                            realPath = cursor.getString(columnIndex)
                        }
                        cursor.close()
                    }
                } catch (e: Exception) {
                    println(e)
                }
            }
            return realPath
        }
    }
}