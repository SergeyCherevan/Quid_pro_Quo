#pragma once

#include <iostream>

#include "ArduinoUnit.hpp"
#include "GPSModule.hpp"
#include "GPRSModule.hpp"

int main(int argc, char* argv[]) {
    char* buff; {
        const int BUFFSIZE = 200;
        buff = new char[BUFFSIZE];
        int strLen = (char*)((int)strrchr(argv[0], '\\') + (int)strrchr(argv[0], '/')) - argv[0];
        strncpy_s(buff, BUFFSIZE * sizeof(char), argv[0], strLen);
        buff[strLen] = '\0';
    }

    using namespace std;
    using namespace ArduinoEmulation;

    GPSModule gpsModule = GPSModule(string(buff));
    GPRSModule gprsModule = GPRSModule();

    IArduinoUnit& arduino = (IArduinoUnit&)ArduinoUnit(&gpsModule, &gprsModule, string(buff));
    arduino.init();

    delete[] buff;


    string command = "";
    while (command != "end") {
        cout << "\n\nYour command ( authorize / attach / confirm / end ): ";
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