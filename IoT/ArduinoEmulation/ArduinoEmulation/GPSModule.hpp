#pragma once

#include <string>

namespace ArduinoEmulation {

    using namespace std;

    class GPSModule {

        string pathToGPS;

    public:
        GPSModule(string path);

        string getNMEA0183Sring();
    };
}