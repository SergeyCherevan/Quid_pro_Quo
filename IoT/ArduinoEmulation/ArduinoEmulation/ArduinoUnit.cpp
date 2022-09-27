#pragma once

#include <iostream>
#include <stdio.h>

#include <nlohmann/json.hpp>

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
            pathToEEPROM = pathToROM.substr(0, pathToROM.find_last_of('\\'));

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
            return 987654321;
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

            cout << "My JWT-string: \"" << jwtString << "\"\n";
            cout << "My owner name: " << ownerName << "\n\n";
        }

    private:
        string getJWTStringFromAuthorizeRequest(int iotCode, string pass) {
            using namespace nlohmann;

            string postObj = getJsonObjectByLoginApiModel(iotCode, pass);
            string response = GPRS->httpsPost(serviceURL + "/api/IoT/login/", postObj, "");
            json respObj = json::parse(response);
            
            return respObj["jwtString"];
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
        string getJWTStringFromAttachRequest(string jwtStr) {
            using namespace nlohmann;

            string postObj = getJsonObjectByLoginApiModel(IoTCode, password);
            string response = GPRS->httpsPost(serviceURL + "/api/IoT/login/", postObj, "");
            json respObj = json::parse(response);

            return respObj["jwtString"];
        }
        static string getOwnerNameFromJWT(string jwtStr) {
            using namespace nlohmann;

            string bodyStr = split(jwtStr, ".")[1];
            string jsonStr = b64_decode(bodyStr);
            json jsonObj = json::parse(jsonStr);

            return jsonObj["OwnerName"];
        }

        static string b64_decode(string encoded_string) {
            string base64_chars =
                "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
                "abcdefghijklmnopqrstuvwxyz"
                "0123456789+/";

            int in_len = encoded_string.size();
            int i = 0;
            int j = 0;
            int in_ = 0;
            unsigned char char_array_4[4], char_array_3[3];
            string ret;

            while (in_len-- && (encoded_string[in_] != '=') && is_b64(encoded_string[in_])) {
                char_array_4[i++] = encoded_string[in_]; in_++;
                if (i == 4) {
                    for (i = 0; i < 4; i++)
                        char_array_4[i] = base64_chars.find(char_array_4[i]);

                    char_array_3[0] = (char_array_4[0] << 2) + ((char_array_4[1] & 0x30) >> 4);
                    char_array_3[1] = ((char_array_4[1] & 0xf) << 4) + ((char_array_4[2] & 0x3c) >> 2);
                    char_array_3[2] = ((char_array_4[2] & 0x3) << 6) + char_array_4[3];

                    for (i = 0; (i < 3); i++)
                        ret += char_array_3[i];
                    i = 0;
                }
            }

            if (i) {
                for (j = i; j < 4; j++)
                    char_array_4[j] = 0;

                for (j = 0; j < 4; j++)
                    char_array_4[j] = base64_chars.find(char_array_4[j]);

                char_array_3[0] = (char_array_4[0] << 2) + ((char_array_4[1] & 0x30) >> 4);
                char_array_3[1] = ((char_array_4[1] & 0xf) << 4) + ((char_array_4[2] & 0x3c) >> 2);
                char_array_3[2] = ((char_array_4[2] & 0x3) << 6) + char_array_4[3];

                for (j = 0; (j < i - 1); j++) ret += char_array_3[j];
            }

            return ret;
        }
        static bool is_b64(unsigned char c) {
            return (isalnum(c) || (c == '+') || (c == '/'));
        }
        static vector<string> split(string str, string delim) {
            vector<string> result;
            size_t pos = 0;
            string substr;

            while ((pos = str.find(delim)) != string::npos) {
                substr = str.substr(0, pos);
                result.push_back(substr);
                str.erase(0, pos + delim.length());
            }

            return result;
        }

    public:
        void confirmServiceCompletion() {
            cout << "Method sendGeolocationToServer() run...\n";

            NMEA0183 nmeaGeoloc = getGeolocationFromGPS();
            ConfirmServiceCompletionApiModel apiModel = getApiModelByNMEA(nmeaGeoloc);
            string postObj = getJsonObjectByConfirmApiModel(apiModel);

            cout << "My JSON-object for send to server: " << postObj << "\n\n";

            bool success = sendGeolocationToServer(postObj, jwtString);

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

        bool sendGeolocationToServer(string postObj, string jwtString) {
            string response = GPRS->httpsPost(serviceURL + "/api/IoT/confirmServiceCompletion/", postObj, jwtString);

            if (!response.empty()) {
                return true;
            }
            else {
                return false;
            }
        }
    };
}