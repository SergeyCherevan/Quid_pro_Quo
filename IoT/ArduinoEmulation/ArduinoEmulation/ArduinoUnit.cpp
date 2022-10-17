#pragma once

#include "ArduinoUnit.hpp"

#include <iostream>
#include <fstream>
#include <stdio.h>

#include <nlohmann/json.hpp>

namespace ArduinoEmulation {

    using namespace std;

    // constructor
    ArduinoUnit::ArduinoUnit(GPSModule* gps, GPRSModule* gprs, string pathToROM) {
        cout << "Constructor ArduinoUnit() run...\n";

        GPS = gps;
        GPRS = gprs;

        string pathToEEPROM = pathToROM;
        ROM = new EEPROMModule(pathToEEPROM);

        cout << "My path to EEPROM: \"" << pathToEEPROM << "\"\n\n";
    }

    void ArduinoUnit::init() {
        cout << "Method init() run...\n";

        IoTCode = getIoTCodeFromEEPROM();
        password = getPasswordFromEEPROM();
        serviceURL = getServiceURLFromEEPROM();

        cout << "My IoT code: " << IoTCode << "\n";
        cout << "My password: \"" << password << "\"\n";
        cout << "My service URL: \"" << serviceURL << "\"\n\n";
    }

    ArduinoUnit::EEPROMModule::EEPROMModule(string path) : pathToEEPROM(path) { }

    string ArduinoUnit::EEPROMModule::getStringVar(string name) {
        ifstream file(filePath(pathToEEPROM, name));

        string result;
        copy(
            istream_iterator<char>(file),
            istream_iterator<char>(),
            insert_iterator<string>(result, result.begin())
        );

        file.close();
        return result;
    }

    int ArduinoUnit::EEPROMModule::getIntegerVar(string name) {
        ifstream file(filePath(pathToEEPROM, name));

        int number;
        file >> number;

        file.close();
        return number;
    }

    string ArduinoUnit::EEPROMModule::filePath(string directoryPath, string fileName) {
        char pathSeparator = '\0';

        if (directoryPath.find('/') != string::npos) {
            pathSeparator = '/';
        }

        if (directoryPath.find('\\') != string::npos) {
            pathSeparator = '\\';
        }

        if (pathSeparator == '\0') {
            throw exception("There are not any path separators in directory path");
        }

        return directoryPath + pathSeparator + fileName;
    }

    int ArduinoUnit::getIoTCodeFromEEPROM() {
        return ROM->getIntegerVar("IoTCode");
    }
    string ArduinoUnit::getPasswordFromEEPROM() {
        return ROM->getStringVar("password");
    }
    string ArduinoUnit::getServiceURLFromEEPROM() {
        return ROM->getStringVar("serviceURL");
    }

    void ArduinoUnit::authorize() {
        cout << "Method authorize() run...\n";

        jwtString = getJWTStringFromAuthorizeRequest(IoTCode, password);
        ownerName = getOwnerNameFromJWT(jwtString);

        cout << "My JWT-string: \"" << jwtString << "\"\n";
        cout << "My owner name: " << ownerName << "\n\n";
    }

    string ArduinoUnit::getJWTStringFromAuthorizeRequest(int iotCode, string pass) {
        using namespace nlohmann;

        string postObj = getJsonObjectByLoginApiModel(iotCode, pass);
        string response = GPRS->httpsPost(serviceURL + "/api/IoT/login/", postObj, "");
        json respObj = json::parse(response);

        return respObj["jwtString"];
    }

    string ArduinoUnit::getJsonObjectByLoginApiModel(int iotCode, string pass) {
        return string() + "{ " +
            "\"ioTCode\": " + to_string(iotCode) + ", " +
            "\"password\": \"" + pass + "\" }";
    }

    void ArduinoUnit::attachIoTToUser() {
        cout << "Method attachIoTToUser() run...\n";

        jwtString = getJWTStringFromAttachRequest(jwtString);
        ownerName = getOwnerNameFromJWT(jwtString);

        cout << "My JWT-string: \"" << jwtString << "\"\n";
        cout << "My owner name: " << ownerName << "\n\n";
    }

    string ArduinoUnit::getJWTStringFromAttachRequest(string jwtStr) {
        using namespace nlohmann;

        string response = GPRS->httpsPost(serviceURL + "/api/IoT/attach/", "{ }", jwtStr);
        json respObj = json::parse(response);

        return respObj["jwtString"];
    }

    string ArduinoUnit::getOwnerNameFromJWT(string jwtStr) {
        using namespace nlohmann;

        string bodyStr = split(jwtStr, ".")[1];
        string jsonStr = b64_decode(bodyStr);
        json jsonObj = json::parse(jsonStr);

        return jsonObj["OwnerName"];
    }

    string ArduinoUnit::b64_decode(string encoded_string) {
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
    bool ArduinoUnit::is_b64(unsigned char c) {
        return (isalnum(c) || (c == '+') || (c == '/'));
    }
    vector<string> ArduinoUnit::split(string str, string delim) {
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