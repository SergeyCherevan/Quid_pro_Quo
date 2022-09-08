package projects.quidpro.api.servers.geo

import com.google.gson.JsonObject
import projects.quidpro.models.AllTicketsResponce
import retrofit2.Call
import retrofit2.http.GET
import retrofit2.http.Header
import retrofit2.http.Path
import retrofit2.http.Query

interface GeoRequests {
    //reverse-geocode-client?latitude={latitude}&longitude={longitude}&localityLanguage={localityLanguage}
    @GET("reverse-geocode-client")
    fun getReverseGeocode(@Query("latitude") latitude: String = "50.515944", @Query("longitude") longitude: String = "30.235055", @Query("localityLanguage") localityLanguage: String = "en"): Call<JsonObject>

    @GET("reverse-geocode")
    fun getReverseGeocodeServerSide(@Query("latitude") latitude: String = "50.515944", @Query("longitude") longitude: String = "30.235055", @Query("localityLanguage") localityLanguage: String = "en", @Query("key") key: String = ""): Call<JsonObject>
}