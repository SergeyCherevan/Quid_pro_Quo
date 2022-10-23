#pragma once

#include "ArduinoUnit.hpp"

#include <iostream>

namespace ArduinoEmulation {

    using namespace std;

    // constructor
    ArduinoUnit::ArduinoUnit(IGPSModule* gps, IGPRSModule* gprs, string pathToROM) {
        cout << "Constructor ArduinoUnit() run...\n";

        GPS = gps;
        GPRS = gprs;

        string pathToEEPROM = pathToROM;
        ROM = new EEPROMModule(pathToEEPROM);

        cout << "My path to EEPROM: \"" << pathToEEPROM << "\"\n\n";
    }

    void ArduinoUnit::init() {
        cout << "Method init() run...\n";

        IoTCode = ROM->getIntegerVar("IoTCode");
        password = ROM->getStringVar("password");
        serviceURL = ROM->getStringVar("serviceURL");

        cout << "My IoT code: " << IoTCode << "\n";
        cout << "My password: \"" << password << "\"\n";
        cout << "My service URL: \"" << serviceURL << "\"\n\n";
    }
}