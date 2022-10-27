#pragma once

#include "ArduinoUnit.hpp"

#include <iostream>
#include <sstream>

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

        NMEA0183 nmea = getNMEAByString(nmeaString);

        return nmea;
    }

    NMEA0183 ArduinoUnit::getNMEAByString(string nmeaString) {
        NMEA0183 nmea;

        stringstream strStrm;
        strStrm << nmeaString;

        char strBuf[21];

        nmea.dollar_or_exclamation = strStrm.get();
        strStrm.read(nmea.source, 2);
        strStrm.read(nmea.format, 3);

        if (strStrm.get() != ',') {
            throw exception("Error of format input geolocation data: string must be delimited by ','");
        }

        strStrm.getline(strBuf, 21, ',');
        if (strlen(strBuf) > 0) {
            nmea.utc_time = atof(strBuf);
        }
        else {
            nmea.utc_time = 0;
        }

        strStrm.getline(strBuf, 2, ',');
        if (strlen(strBuf) > 0) {
            nmea.data_is_reliable = strBuf[0];
        }
        else {
            nmea.data_is_reliable = '\0';
        }

        strStrm.getline(strBuf, 21, ',');
        if (strlen(strBuf) > 0) {
            nmea.latitude = atof(strBuf);
        }
        else {
            nmea.latitude = 0;
        }

        strStrm.getline(strBuf, 2, ',');
        if (strlen(strBuf) > 0) {
            nmea.p_hemisphere = strBuf[0];
        }
        else {
            nmea.p_hemisphere = '\0';
        }

        strStrm.getline(strBuf, 21, ',');
        if (strlen(strBuf) > 0) {
            nmea.longitude = atof(strBuf);
        }

        strStrm.getline(strBuf, 2, ',');
        if (strlen(strBuf) > 0) {
            nmea.j_hemisphere = strBuf[0];
        }
        else {
            nmea.j_hemisphere = '\0';
        }

        strStrm.getline(strBuf, 21, ',');
        if (strlen(strBuf) > 0) {
            nmea.horizontal_speed = atof(strBuf);
        }
        else {
            nmea.horizontal_speed = 0;
        }

        strStrm.getline(strBuf, 21, ',');
        if (strlen(strBuf) > 0) {
            nmea.heading = atof(strBuf);
        }
        else {
            nmea.heading = 0;
        }

        strStrm.getline(strBuf, 21, ',');
        if (strlen(strBuf) > 0) {
            nmea.date = atoi(strBuf);
        }
        else {
            nmea.date = 0;
        }

        strStrm.getline(strBuf, 21, ',');
        if (strlen(strBuf) > 0) {
            nmea.magnetic_declination = atof(strBuf);
        }
        else {
            nmea.magnetic_declination = 0;
        }

        strStrm.getline(strBuf, 2, ',');
        if (strlen(strBuf) > 0) {
            nmea.magnetic_declination_direction = strBuf[0];
        }
        else {
            nmea.magnetic_declination_direction = '\0';
        }

        if (strStrm.peek() != '*') {
            strStrm.getline(strBuf, 2, '*');
            if (strlen(strBuf) > 0) {
                nmea.mode_indicator = strBuf[0];
            }
            else {
                nmea.mode_indicator = '\0';
            }
        }

        if (strlen(strBuf) > 1) {
            throw exception("Error of format input geolocation data: string must be ended by '*'");
        }

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

        char strBuf[21];

        sprintf_s(strBuf, "%02u", nmea.date % 100);
        apiModel.dateTime = string("20") + strBuf;

        sprintf_s(strBuf, "%02u", nmea.date % 10000 / 100);
        apiModel.dateTime += string("-") + strBuf;

        sprintf_s(strBuf, "%02u", int(nmea.date / 10000));
        apiModel.dateTime += string("-") + strBuf;

        sprintf_s(strBuf, "%02u", int(nmea.utc_time / 10000));
        apiModel.dateTime += string("T") + strBuf;

        sprintf_s(strBuf, "%02u", int(nmea.utc_time / 100) % 100);
        apiModel.dateTime += string(":") + strBuf;

        sprintf_s(strBuf, "%06.3f", nmea.utc_time - int(nmea.utc_time / 100) * 100);
        apiModel.dateTime += string(":") + strBuf;

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