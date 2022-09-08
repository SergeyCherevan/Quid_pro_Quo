package projects.quidpro.models

class TicketsSearchParameters {
    var keywords = ""
    var date = ""
    var geomarker = ""
    var pageNumber = 0
    var pageSize = 0

    var cityByGeocode = ""
    var cityByGeocodeIsDownloaded = false
}