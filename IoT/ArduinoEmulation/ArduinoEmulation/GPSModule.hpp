#pragma once

#include <string>

namespace ArduinoEmulation {

    using namespace std;

    class GPSModule {

    public:
        string getNMEA0183Sring();
    };
}