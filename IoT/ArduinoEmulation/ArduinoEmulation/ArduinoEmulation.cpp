#pragma once

#define CURL_STATICLIB

#include <iostream>
#include "ArduinoUnit.cpp"

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

    GPSModule gpsModule = GPSModule();
    GPRSModule gprsModule = GPRSModule();

    ArduinoUnit arduino(&gpsModule, &gprsModule, string(buff));
    arduino.init();

    delete[] buff;


    string command = "";
    while (command != "end") {
        cout << "\n\nYour command ( authorize / attach / confirm ): ";
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