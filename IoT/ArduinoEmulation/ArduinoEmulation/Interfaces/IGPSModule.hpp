#pragma once

#include <string>

namespace ArduinoEmulation {

    using namespace std;

    class IGPSModule {

    public:
        virtual string getNMEA0183Sring() = 0;
    };
}