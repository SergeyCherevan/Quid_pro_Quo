#pragma once

#include "ArduinoUnit.hpp"

#include <iostream>

namespace ArduinoEmulation {

    using namespace std;

    void ArduinoUnit::confirmServiceCompletion() {
        cout << "Method sendGeolocationToServer() run...\n";

        NMEA0183 nmeaGeoloc = getGeolocationFromGPS();
        ConfirmServiceCompletionApiModel apiModel = getApiModelByNMEA(nmeaGeoloc);
        string postObj = getJsonObjectByConfirmApiModel(apiModel);

        cout << "My JSON-object for send to server: " << postObj << "\n\n";

        bool success = sendGeolocationToServer(postObj, jwtString);

        cout << "\n\n";
    }

    NMEA0183 ArduinoUnit::getGeolocationFromGPS() {
        string nmeaString = GPS->getNMEA0183Sring();

        cout << "My nmea string: " << nmeaString << "\n";

        return getNMEAByString(nmeaString);
    }

    NMEA0183 ArduinoUnit::getNMEAByString(string nmeaString) {
        NMEA0183 nmea;
        nmea.latitude = 4914.08;
        nmea.p_hemisphere = 'N';
        nmea.longitude = 2823.97;
        nmea.j_hemisphere = 'E';
        nmea.date = 100722;
        nmea.utc_time = 065200.00;

        return nmea;
    }

    ConfirmServiceCompletionApiModel ArduinoUnit::getApiModelByNMEA(NMEA0183 nmea) {
        ConfirmServiceCompletionApiModel apiModel;

        apiModel.latitude = int(nmea.latitude / 100);
        apiModel.latitude = apiModel.latitude + (nmea.latitude / 100 - apiModel.latitude) * 100 / 60;
        if (nmea.p_hemisphere == 'S') {
            apiModel.latitude = -apiModel.latitude;
        }

        apiModel.longitude = int(nmea.longitude / 100);
        apiModel.longitude = apiModel.longitude + (nmea.longitude / 100 - apiModel.longitude) * 100 / 60;
        if (nmea.j_hemisphere == 'W') {
            apiModel.longitude = -apiModel.longitude;
        }

        apiModel.dateTime = "2022-07-10T06:52:00";

        return apiModel;
    }

    string ArduinoUnit::getJsonObjectByConfirmApiModel(ConfirmServiceCompletionApiModel model) {
        return string() + "{ " +
            "\"latitude\": " + to_string(model.latitude) + ", " +
            "\"longitude\": " + to_string(model.longitude) + ", " +
            "\"dateTime\": \"" + model.dateTime + "\" }";
    }

    bool ArduinoUnit::sendGeolocationToServer(string postObj, string jwtString) {
        string response = GPRS->httpsPost(serviceURL + "/api/IoT/confirmServiceCompletion/", postObj, jwtString);

        if (!response.empty()) {
            return true;
        }
        else {
            return false;
        }
    }
}