package projects.quidpro.models

import okhttp3.MultipartBody
import java.io.File
import java.time.LocalDateTime

class TicketCreateParameters {
    var title = ""
    var description = ""
    var geomarker = ""
    var photosAsFileList: ArrayList<File> = ArrayList()
    var datesList: ArrayList<String> = ArrayList()

    var cityByGeocode = ""
    var cityByGeocodeIsDownloaded = false
}