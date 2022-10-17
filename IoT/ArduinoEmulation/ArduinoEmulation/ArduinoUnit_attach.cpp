#pragma once

#include "ArduinoUnit.hpp"

#include <iostream>

#include <nlohmann/json.hpp>

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
}