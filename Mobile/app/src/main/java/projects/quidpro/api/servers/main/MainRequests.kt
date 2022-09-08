package projects.quidpro.api.servers.main

import com.google.gson.JsonObject
import okhttp3.MultipartBody
import okhttp3.RequestBody
import okhttp3.ResponseBody
import projects.quidpro.models.*
import projects.quidpro.models.signalRMessageModels.MessagingCardApiModel
import retrofit2.Call
import retrofit2.http.*
import java.io.File
import java.time.LocalDateTime
import java.util.*
import kotlin.collections.ArrayList

interface MainRequests {
    @POST("Account/login")
    fun login(@Body body: LoginRequest): Call<LoginResponce>

    @POST("Account/registration")
    fun registration(@Body body: RegisterRequest): Call<RegisterResponce>

    /*@GET("Post/getByFilter")
    fun getAllTickets(@Query("model") getByFilter: AllTicketsRequest, @Header("Authorization") auth: String): Call<AllTicketsResponce>*/

    @GET("Post/getByFilter")
    fun getAllTickets(@Query("keywords") keywords: String,
                      @Query("date") date: String,
                      @Query("geomarker") geomarker: String,
                      @Query("pageNumber") pageNumber: Int,
                      @Query("pageSize") pageSize: Int,
                      @Header("Authorization") auth: String): Call<AllTicketsResponce>

    @GET("User/getByFilter")
    fun getAllRecommendations(@Query("keywords") keywords: String,
                      @Query("pageNumber") pageNumber: Int,
                      @Query("pageSize") pageSize: Int,
                      @Header("Authorization") auth: String): Call<AllRecommendationsResponce>

    @GET("File/image/{fileName}")
    fun getImage(@Path("fileName") fileName: String): Call<ResponseBody>

    @GET("File/avatar/{fileName}")
    fun getAvatar(@Path("fileName") fileName: String): Call<ResponseBody>

    @GET("User/getAvatarByName/{userName}")
    fun getAvatarByUserName(@Path("userName") userName: String): Call<ResponseBody>

    @POST("Post/publish")
    fun publishTicket(@Body publishTicketRequestBodyModel: RequestBody, @Header("Authorization") auth: String): Call<JsonObject>

    @GET("Account/currentUser")
    fun getCurrentUserProfile(@Header("Authorization") auth: String): Call<UserProfileInfoResponce>

    @Multipart
    @PUT("Account/edit")
    fun editProfile(@Part userName: MultipartBody.Part,
                    @Part userBiography: MultipartBody.Part,
                    @Part userAvatar: MultipartBody.Part,
                    @Header("Authorization") auth: String): Call<UserProfileInfoResponce>

    @PUT("Account/changePassword")
    fun userChangePassword(@Body changePass: ChangePasswordRequest,
                           @Header("Authorization") auth: String): Call<JsonObject>

    @Multipart
    @POST("messenger/sendMessage")
    fun sendNewMessage(@Part text: MultipartBody.Part, @Part destinationName: MultipartBody.Part,
                    @Part imageFile: MultipartBody.Part? = null,
                    @Part file: MultipartBody.Part? = null,
                    @Header("Authorization") auth: String): Call<MessagingCardApiModel>

    /*@POST("messenger/sendMessage")
    fun sendNewMessage(text: String = "",
                       destinationName: String = "",
                       @Header("Authorization") auth: String): Call<MessagingCardApiModel>*/

    /*@PUT("Users/edit")
    fun editProfile(@Header("Authorization") auth: String, @Body body: UserSettings): Call<JsonObject>

    @DELETE("Apiaries/delete/{id}")
    fun deleteApiaries(@Path("id") id: Int, @Header("Authorization") auth: String): Call<JsonObject>*/
}