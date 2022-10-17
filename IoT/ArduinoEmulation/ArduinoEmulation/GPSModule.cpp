#pragma once

#include "GPSModule.hpp"

#include <iostream>
#include <fstream>

namespace ArduinoEmulation {

    using namespace std;

    string GPSModule::getNMEA0183Sring() {
        return "$GPRMC,065200.00,A,4914.08,N,2823.97,E,,,100722,,,A*4E\r\n";
    }
}