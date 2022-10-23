#pragma once

#include "GPSModule.hpp"

#include <iostream>
#include <fstream>

#include "ArduinoUnit.hpp"

namespace ArduinoEmulation {

    using namespace std;

    GPSModule::GPSModule(string path) : pathToGPS(path) { }

    string GPSModule::getNMEA0183Sring() {
        ifstream file(ArduinoUnit::EEPROMModule::filePath(pathToGPS, "GPSInput"));

        string result;
        copy(
            istream_iterator<char>(file),
            istream_iterator<char>(),
            insert_iterator<string>(result, result.begin())
        );

        file.close();
        return result;
    }
}