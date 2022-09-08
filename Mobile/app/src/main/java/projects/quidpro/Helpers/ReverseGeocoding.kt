package projects.quidpro.Helpers

import android.graphics.BitmapFactory
import android.util.Log
import android.widget.TextView
import com.google.gson.JsonObject
import projects.quidpro.api.servers.geo.GeoClientBuilder
import projects.quidpro.api.servers.geo.GeoRequests
import projects.quidpro.models.TicketSystemParameters
import projects.quidpro.storage.Storage
import retrofit2.*
import java.lang.Exception

class ReverseGeocoding {
    companion object {
        /*fun GetInfoAboutTicketCity(latitude: String, longitude: String, localityLanguage: String, textViewTicketCity: TextView): String {
            var city = ""

            val request = GeoClientBuilder.buildClient(GeoRequests::class.java)
            val response = request.getReverseGeocode(latitude, longitude, localityLanguage)

            response.enqueue(object: Callback<JsonObject> {
                override fun onResponse(call: Call<JsonObject>, response: Response<JsonObject>) {
                    if (response.isSuccessful) {
                        val data = response.body()!!
                        city = data.get("city").asString
                        try {
                            textViewTicketCity.text = city
                        } catch (e: Exception){}
                        *//*var location = data.getAsJsonObject("location")

                        var country = location.getAsJsonObject("country").get("name").asString
                        var region = location.getAsJsonObject("region").get("name").asString
                        var city = location.get("city").asString*//*

                        //Storage.Session_Geolocation = city.plus(", ").plus(region).plus(", ").plus(country)
                        //Storage.Session_Geolocation = city.plus(", ").plus(country)
                    }
                }

                override fun onFailure(call: Call<JsonObject>, t: Throwable) {
                    Log.println(Log.ASSERT, "Info City", "Fail to get info about city of ticket")
                }
            })
            return city
        }*/

        fun getInfoAboutTicketCity(latitude: String, longitude: String, localityLanguage: String, ticketObj: TicketSystemParameters) {

            val request = GeoClientBuilder.buildClient(GeoRequests::class.java)
            val response = request.getReverseGeocodeServerSide(latitude, longitude, localityLanguage, Storage.reverseGeocodeApiKey)

            response.enqueue(object: Callback<JsonObject> {
                override fun onResponse(call: Call<JsonObject>, response: Response<JsonObject>) {
                    if (response.isSuccessful) {
                        val data = response.body()!!
                        try {
                            ticketObj.ticketCity = data.get("city").asString
                            ticketObj.ticketCitiesDownloaded = true
                        } catch (e: Exception){}
                        /*var location = data.getAsJsonObject("location")

                        var country = location.getAsJsonObject("country").get("name").asString
                        var region = location.getAsJsonObject("region").get("name").asString
                        var city = location.get("city").asString*/

                        //Storage.Session_Geolocation = city.plus(", ").plus(region).plus(", ").plus(country)
                        //Storage.Session_Geolocation = city.plus(", ").plus(country)
                    }
                }

                override fun onFailure(call: Call<JsonObject>, t: Throwable) {
                    Log.println(Log.ASSERT, "Info City", "Fail to get info about city of ticket")
                }
            })
        }

        fun getInfoAboutTicketCityForGeoFilter(latitude: String, longitude: String, localityLanguage: String) {

            val request = GeoClientBuilder.buildClient(GeoRequests::class.java)
            val response = request.getReverseGeocodeServerSide(latitude, longitude, localityLanguage, Storage.reverseGeocodeApiKey)

            response.enqueue(object: Callback<JsonObject> {
                override fun onResponse(call: Call<JsonObject>, response: Response<JsonObject>) {
                    if (response.isSuccessful) {
                        val data = response.body()!!
                        try {

                            if(data.get("continent").asString != "")
                                Storage.ticketsSearch.cityByGeocode = data.get("continent").asString
                            if(data.get("countryName").asString != "")
                                Storage.ticketsSearch.cityByGeocode = data.get("countryName").asString
                            if(data.get("principalSubdivision").asString != "")
                                Storage.ticketsSearch.cityByGeocode = data.get("principalSubdivision").asString
                            if(data.get("locality").asString != "")
                                Storage.ticketsSearch.cityByGeocode = data.get("locality").asString
                            if(data.get("city").asString != "")
                                Storage.ticketsSearch.cityByGeocode = data.get("city").asString
                            if(data.get("city").asString != "" && data.get("locality").asString != "")
                                Storage.ticketsSearch.cityByGeocode = data.get("city").asString.plus(", ").plus(data.get("locality").asString)

                            Storage.ticketsSearch.cityByGeocodeIsDownloaded = true

                        } catch (e: Exception){}
                        /*var location = data.getAsJsonObject("location")

                        var country = location.getAsJsonObject("country").get("name").asString
                        var region = location.getAsJsonObject("region").get("name").asString
                        var city = location.get("city").asString*/

                        //Storage.Session_Geolocation = city.plus(", ").plus(region).plus(", ").plus(country)
                        //Storage.Session_Geolocation = city.plus(", ").plus(country)
                    }
                }

                override fun onFailure(call: Call<JsonObject>, t: Throwable) {
                    Log.println(Log.ASSERT, "Info City", "Fail to get info about city of ticket")
                }
            })
        }

        fun getInfoAboutTicketCityForGeoCreateTicket(latitude: String, longitude: String, localityLanguage: String) {

            val request = GeoClientBuilder.buildClient(GeoRequests::class.java)
            val response = request.getReverseGeocodeServerSide(latitude, longitude, localityLanguage, Storage.reverseGeocodeApiKey)

            response.enqueue(object: Callback<JsonObject> {
                override fun onResponse(call: Call<JsonObject>, response: Response<JsonObject>) {
                    if (response.isSuccessful) {
                        val data = response.body()!!
                        try {

                            if(data.get("continent").asString != "")
                                Storage.ticketsCreate.cityByGeocode = data.get("continent").asString
                            if(data.get("countryName").asString != "")
                                Storage.ticketsCreate.cityByGeocode = data.get("countryName").asString
                            if(data.get("principalSubdivision").asString != "")
                                Storage.ticketsCreate.cityByGeocode = data.get("principalSubdivision").asString
                            if(data.get("locality").asString != "")
                                Storage.ticketsCreate.cityByGeocode = data.get("locality").asString
                            if(data.get("city").asString != "")
                                Storage.ticketsCreate.cityByGeocode = data.get("city").asString
                            if(data.get("city").asString != "" && data.get("locality").asString != "")
                                Storage.ticketsCreate.cityByGeocode = data.get("city").asString.plus(", ").plus(data.get("locality").asString)

                            Storage.ticketsCreate.cityByGeocodeIsDownloaded = true

                        } catch (e: Exception){}

                    }
                }

                override fun onFailure(call: Call<JsonObject>, t: Throwable) {
                    Log.println(Log.ASSERT, "Info City", "Fail to get info about city of ticket")
                }
            })
        }
    }
}