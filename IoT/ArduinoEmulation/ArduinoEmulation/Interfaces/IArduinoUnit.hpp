#pragma once

namespace ArduinoEmulation {

    using namespace std;

    class IArduinoUnit {

    public:
        virtual void init() = 0;
        virtual void authorize() = 0;
        virtual void attachIoTToUser() = 0;
        virtual void confirmServiceCompletion() = 0;
    };
}