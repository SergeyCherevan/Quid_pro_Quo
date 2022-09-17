#pragma once

#define CURL_STATICLIB

#include <iostream>
#include "ArduinoUnit.cpp"

using namespace std;
using namespace ArduinoEmulation;

int main(int argc, char* argv[])
{
    GPSModule gpsModule = GPSModule();
    GPRSModule gprsModule = GPRSModule();

    ArduinoUnit arduino(&gpsModule, &gprsModule, string(argv[0]));
    arduino.init();

    string command = "";
    while (command != "end") {
        cout << "Your command (authorize/attach/confirm): ";
        cin >> command;

        if (command == "authorize") {
            arduino.authorize();
        }
        else if (command == "attach") {
            arduino.attachIoTToUser();
        }
        else if (command == "confirm") {
            arduino.confirmServiceCompletion();
        }
    }

    return 0;
}