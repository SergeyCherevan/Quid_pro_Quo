#pragma once

#include "ArduinoUnit.hpp"

#include <iostream>

namespace ArduinoEmulation {

    using namespace std;

    // constructor
    ArduinoUnit::ArduinoUnit(GPSModule* gps, GPRSModule* gprs, string pathToROM) {
        cout << "Constructor ArduinoUnit() run...\n";

        GPS = gps;
        GPRS = gprs;

        string pathToEEPROM = pathToROM;
        ROM = new EEPROMModule(pathToEEPROM);

        cout << "My path to EEPROM: \"" << pathToEEPROM << "\"\n\n";
    }

    void ArduinoUnit::init() {
        cout << "Method init() run...\n";

        IoTCode = getIoTCodeFromEEPROM();
        password = getPasswordFromEEPROM();
        serviceURL = getServiceURLFromEEPROM();

        cout << "My IoT code: " << IoTCode << "\n";
        cout << "My password: \"" << password << "\"\n";
        cout << "My service URL: \"" << serviceURL << "\"\n\n";
    }

    int ArduinoUnit::getIoTCodeFromEEPROM() {
        return ROM->getIntegerVar("IoTCode");
    }
    string ArduinoUnit::getPasswordFromEEPROM() {
        return ROM->getStringVar("password");
    }
    string ArduinoUnit::getServiceURLFromEEPROM() {
        return ROM->getStringVar("serviceURL");
    }
}