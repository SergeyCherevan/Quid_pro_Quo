#pragma once

#include <iostream>
#include <stdio.h>

#include "NMEA0183.h"
#include "ConfirmServiceCompletionApiModel.hpp"

#include "GPSModule.cpp"
#include "GPRSModule.cpp"

namespace ArduinoEmulation {

    using namespace std;

    class ArduinoUnit {

        int IoTCode;
        string password;
        string serviceURL;

        string jwtString;
        string ownerName;

        /* external emulated devices */
        GPSModule* GPS;
        GPRSModule* GPRS;
        string pathToEEPROM;

    public:
        ArduinoUnit(GPSModule* gps, GPRSModule* gprs, string pathToROM) {
            cout << "Constructor ArduinoUnit() run...\n";

            GPS = gps;
            GPRS = gprs;
            pathToEEPROM = pathToROM;

            cout << "My path to EEPROM: \"" << pathToEEPROM << "\"\n\n";
        }

        void init() {
            cout << "Method init() run...\n";

            IoTCode = getIoTCodeFromEEPROM();
            password = getPasswordFromEEPROM();
            serviceURL = getServiceURLFromEEPROM();

            cout << "My IoT code: " << IoTCode << "\n";
            cout << "My password: \"" << password << "\"\n";
            cout << "My service URL: \"" << serviceURL << "\"\n\n";
        }

    private:
        int getIoTCodeFromEEPROM() {
            return 123456789;
        }
        string getPasswordFromEEPROM() {
            return "1234";
        }
        string getServiceURLFromEEPROM() {
            return "https://localhost:5001";
        }

    public:
        void authorize() {
            cout << "Method authorize() run...\n";

            jwtString = getJWTStringFromAuthorizeRequest(IoTCode, password);
            ownerName = getOwnerNameFromJWT(jwtString);

            cout << "\n\n"; 
            cout << "My JWT-string: \'" << jwtString << "\'\n";
            cout << "My owner name: " << ownerName << "\n\n";
        }

    private:
        string getJWTStringFromAuthorizeRequest(int iotCode, string pass) {
            string jsonObject = getJsonObjectByLoginApiModel(iotCode, pass);
            string response = GPRS->httpsPost(serviceURL + "/api/IoT/login/", jsonObject, "");
            
            return /*respObj["jwtString"];*/
"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.\
eyJJb1RDb2RlIjoiOTg3NjU0MzIxIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQXJkdWlub1VuaXQiLCJPd25lck5hbWUiOiJLYXRlIiwibmJmIjoxNjYzNDM0NzM0LCJleHAiOjE2Njk2NTU1MzQsImlzcyI6IlF1aWRfcHJvX1F1byIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3QifQ.\
K5jyU56jwfjy1zdV1VQdC9Uyiz7AZwMjlNhU8ycaz9M";
        }

        static string getJsonObjectByLoginApiModel(int iotCode, string pass) {
            return string() + "{ " +
                "\"ioTCode\": " + to_string(iotCode) + ", " +
                "\"password\": \"" + pass + "\" }";
        }

    public:
        void attachIoTToUser() {
            cout << "Method attachIoTToUser() run...\n";

            jwtString = getJWTStringFromAttachRequest(jwtString);
            ownerName = getOwnerNameFromJWT(jwtString);

            cout << "My JWT-string: \"" << jwtString << "\"\n";
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

            return apiModel;
        }

        static string getJsonObjectByConfirmApiModel(ConfirmServiceCompletionApiModel model) {
            return string() + "{ " +
                "\"latitude\": " + to_string(model.latitude) + ", " +
                "\"longitude\": " + to_string(model.longitude) + ", " +
                "\"dateTime\": \"" + model.dateTime + "\" }";
        }

        bool sendGeolocationToServer(string jsonObject, string jwtString) {
            string response = GPRS->httpsPost(serviceURL + "/api/IoT/confirmServiceCompletion/", jsonObject, jwtString);

            if (!response.empty()) {
                return true;
            }
            else {
                return false;
            }
        }
    };
}