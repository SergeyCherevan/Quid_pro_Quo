#pragma once

#include "Interfaces/IGPSModule.hpp"

#include <string>

namespace ArduinoEmulation {

    using namespace std;

    class GPSModule : public IGPSModule {
        string pathToGPS;
    public:
        GPSModule(string path);

        string getNMEA0183Sring();
    };
}