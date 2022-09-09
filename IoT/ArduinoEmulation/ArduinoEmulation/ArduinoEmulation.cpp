#pragma once

#define CURL_STATICLIB

#include <iostream>
#include "ArduinoUnit.cpp"

using namespace std;
using namespace ArduinoEmulation;

int main(void)
{
    GPSModule gpsModule = GPSModule();
    GPRSModule gprsModule = GPRSModule();

    ArduinoUnit arduino(&gpsModule, &gprsModule);
    arduino.init();

    string command = "";
    while (command != "end") {
        cout << "Your command: ";
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