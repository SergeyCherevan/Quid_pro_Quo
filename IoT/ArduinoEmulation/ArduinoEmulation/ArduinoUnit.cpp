#pragma once

#include <iostream>
#include <stdio.h>

#include <curl/curl.h>

#include "NMEA0183.h"
#include "ConfirmServiceCompletionApiModel.hpp"

#include "GPSModule.cpp"
#include "GPRSModule.cpp"

using namespace std;

namespace ArduinoEmulation {

    class ArduinoUnit {

        int IoTCode;
        string password;

        string jwtString;
        string ownerName;

        /* external emulated devices */
        GPSModule* GPS;
        GPRSModule* GPRS;

    public:
        ArduinoUnit(GPSModule* gps, GPRSModule* gprs) {
            GPS = gps;
            GPRS = gprs;
        }

        void init() {
            cout << "Method init() run...\n";

            IoTCode = getIoTCodeFromEEPROM();
            password = getPasswordFromEEPROM();

            cout << "My IoT code: " << IoTCode << "\n";
            cout << "My password: \"" << password << "\"\n\n";
        }

    private:
        int getIoTCodeFromEEPROM() {
            return 123456789;
        }
        string getPasswordFromEEPROM() {
            return "1234";
        }

    public:
        void authorize() {
            cout << "Method authorize() run...\n";

            jwtString = getJWTStringFromAuthorizeRequest(IoTCode, password);
            ownerName = getOwnerNameFromJWT(jwtString);

            cout << "My JWT-string: \'" << jwtString << "\'\n";
            cout << "My owner name: " << ownerName << "\n\n";
        }

    private:
        static string getJWTStringFromAuthorizeRequest(int iotCode, string pass) {
            return "My JWT-string";
        }

    public:
        void attachIoTToUser() {
            cout << "Method attachIoTToUser() run...\n";

            jwtString = getJWTStringFromAttachRequest(jwtString);
            ownerName = getOwnerNameFromJWT(jwtString);

            cout << "My JWT-string: \'" << jwtString << "\'\n";
            cout << "My owner name: " << ownerName << "\n\n";
        }

    private:
        static string getJWTStringFromAttachRequest(string jwtString) {
            return "My JWT-string";
        }
        static string getOwnerNameFromJWT(string jwtStr) {
            return "OwnerName";
        }

    public:
        void confirmServiceCompletion() {
            cout << "Method sendGeolocationToServer() run...\n";

            NMEA0183 nmeaGeoloc = getGeolocationFromGPS();
            ConfirmServiceCompletionApiModel apiModel = getApiModelByNMEA(nmeaGeoloc);
            string jsonObject = getJsonObjectByConfirmApiModel(apiModel);

            cout << "My json object for send to server: " << jsonObject << "\n\n";

            bool success = sendGeolocationToServer(jsonObject, jwtString);

            cout << "\n\n";
        }

    private:
        NMEA0183 getGeolocationFromGPS() {
            string nmeaString = GPS->getNMEA0183Sring();

            cout << "My nmea string: " << nmeaString << "\n";

            return getNMEAByString(nmeaString);
        }

        static NMEA0183 getNMEAByString(string nmeaString) {
            NMEA0183 nmea;
            nmea.latitude = 4914.08;
            nmea.p_hemisphere = 'N';
            nmea.longitude = 2823.97;
            nmea.j_hemisphere = 'E';
            nmea.date = 100722;
            nmea.utc_time = 065200.00;

            return nmea;
        }

        static ConfirmServiceCompletionApiModel getApiModelByNMEA(NMEA0183 nmea) {
            ConfirmServiceCompletionApiModel apiModel;

            apiModel.latitude = int(nmea.latitude / 100);
            apiModel.latitude = apiModel.latitude + (nmea.latitude/100 - apiModel.latitude) * 100 / 60;
            if (nmea.p_hemisphere == 'S') {
                apiModel.latitude = -apiModel.latitude;
            }

            apiModel.longitude = int(nmea.longitude / 100);
            apiModel.longitude = apiModel.longitude + (nmea.longitude / 100 - apiModel.longitude) * 100 / 60;
            if (nmea.j_hemisphere == 'W') {
                apiModel.longitude = -apiModel.longitude;
            }

            apiModel.dateTime = "2022-07-10T06:52:00";
            /*apiModel.dateTime = string() +
                "20" + to_string(nmea.date % 100) + "-" +
                       to_string(nmea.date % 10000 / 100) + "-" +
                       to_string(nmea.date / 10000) + "T" + 
                       to_string(int(nmea.utc_time / 10000)) + ":" +
                       to_string(int(fmod(nmea.utc_time, 10000) / 100)) + ":" + 
                       to_string(int(fmod(nmea.utc_time, 100))) + ".00Z";*/

            return apiModel;
        }

        static string getJsonObjectByConfirmApiModel(ConfirmServiceCompletionApiModel model) {
            return string() + "{ " +
                "\"latitude\": " + to_string(model.latitude) + ", " +
                "\"longitude\": " + to_string(model.longitude) + ", " +
                "\"dateTime\": \"" + model.dateTime + "\" }";
        }

        bool sendGeolocationToServer(string jsonObject, string jwtString) {
            string response = GPRS->httpsPost("https://localhost:5001/api/IoT/confirmServiceCompletion/", jsonObject, jwtString);

            if (!response.empty()) {
                return true;
            }
            else {
                return false;
            }
        }
    };
}