#pragma once

#include "ArduinoUnit.hpp"

#include <iostream>

#include <nlohmann/json.hpp>

namespace ArduinoEmulation {

    using namespace std;

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
}