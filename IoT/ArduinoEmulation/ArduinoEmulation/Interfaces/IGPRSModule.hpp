#pragma once

#include <string>

namespace ArduinoEmulation {

    using namespace std;

    class IGPRSModule {

    public:
        virtual string httpsPost(string url, string postObj, string jwt) = 0;
    };
}