#pragma once

#include "ArduinoUnit.hpp"

#include <iostream>

#include <nlohmann/json.hpp>
#include <base64.cpp>

namespace ArduinoEmulation {

    using namespace std;

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
        string jsonStr = Base64_decode(bodyStr);
        json jsonObj = json::parse(jsonStr);

        return jsonObj["OwnerName"];
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
}