#pragma once

#include "ArduinoUnit.hpp"

#include <fstream>

namespace ArduinoEmulation {

    using namespace std;

    ArduinoUnit::EEPROMModule::EEPROMModule(string path) : pathToEEPROM(path) { }

    string ArduinoUnit::EEPROMModule::getStringVar(string name) {
        ifstream file(filePath(pathToEEPROM, name));

        string result;
        copy(
            istream_iterator<char>(file),
            istream_iterator<char>(),
            insert_iterator<string>(result, result.begin())
        );

        file.close();
        return result;
    }

    int ArduinoUnit::EEPROMModule::getIntegerVar(string name) {
        ifstream file(filePath(pathToEEPROM, name));

        int number;
        file >> number;

        file.close();
        return number;
    }

    string ArduinoUnit::EEPROMModule::filePath(string directoryPath, string fileName) {
        char pathSeparator = '\0';

        if (directoryPath.find('/') != string::npos) {
            pathSeparator = '/';
        }

        if (directoryPath.find('\\') != string::npos) {
            pathSeparator = '\\';
        }

        if (pathSeparator == '\0') {
            throw exception("There are not any path separators in directory path");
        }

        return directoryPath + pathSeparator + fileName;
    }
}