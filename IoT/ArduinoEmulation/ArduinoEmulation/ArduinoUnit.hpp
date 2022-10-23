#pragma once

#include <string>
#include <vector>

#include "NMEA0183.h"
#include "ConfirmServiceCompletionApiModel.hpp"

#include "GPSModule.hpp"
#include "GPRSModule.hpp"

namespace ArduinoEmulation {

    using namespace std;

    class ArduinoUnit {

        int IoTCode;
        string password;
        string serviceURL;

        string jwtString;
        string ownerName;

        /* external emulated devices */
        GPSModule* GPS;
        GPRSModule* GPRS;

        class EEPROMModule;
        EEPROMModule* ROM;

    public:
        ArduinoUnit(GPSModule* gps, GPRSModule* gprs, string pathToROM);

        void init();

        class EEPROMModule {
            string pathToEEPROM;
        public:
            EEPROMModule(string path);

            string getStringVar(string name);

            int getIntegerVar(string name);

            static string filePath(string directoryPath, string fileName);
        };

    public:
        void authorize();

    private:
        string getJWTStringFromAuthorizeRequest(int iotCode, string pass);
        static string getJsonObjectByLoginApiModel(int iotCode, string pass);

    public:
        void attachIoTToUser();

    private:
        string getJWTStringFromAttachRequest(string jwtStr);
        static string getOwnerNameFromJWT(string jwtStr);

        static vector<string> split(string str, string delim);

    public:
        void confirmServiceCompletion();

    private:
        NMEA0183 getGeolocationFromGPS();
        static NMEA0183 getNMEAByString(string nmeaString);
        static ConfirmServiceCompletionApiModel getApiModelByNMEA(NMEA0183 nmea);
        static string getJsonObjectByConfirmApiModel(ConfirmServiceCompletionApiModel model);

        bool sendGeolocationToServer(string postObj, string jwtString);
    };
}