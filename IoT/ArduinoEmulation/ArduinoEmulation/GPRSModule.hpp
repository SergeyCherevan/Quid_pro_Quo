#pragma once

#include "Interfaces/IGPRSModule.hpp"

#include <string>

namespace ArduinoEmulation {

    using namespace std;

    class GPRSModule : public IGPRSModule {

    public:
        string httpsPost(string url, string postObj, string jwt);

    private:
        static size_t getResponseToString(char* content, size_t size, size_t nmeber, string* my_str);
    };
}