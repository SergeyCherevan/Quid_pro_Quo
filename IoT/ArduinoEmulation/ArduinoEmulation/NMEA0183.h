#pragma once

namespace ArduinoEmulation {

    struct NMEA0183 {
        // $ or !
        char dollar_or_exclamation;
        // GP - GPS, GL - GLONASS, GN - GPS + GLONASS, GA - GALILEO, BD or GB - BeiDou
        char source[2];
        // RMC, GGA, GLL, THS, ZDA etc.
        char format[3];

        // time by UTC: hhmmss.sss
        double utc_time;
        // A - data is reliable, V - is not
        char data_is_reliable;
        // latitude: GGMM.MM, GG - degrees, MM.MM - minutes
        double latitude;
        // N - North Hemisphere, S - Southern Hemisphere
        char p_hemisphere;
        // longitude: gggmm.mm, GG - degrees, MM.MM - minutes
        double longitude;
        // E- Eastern Hemisphere, W - Western Hemisphere
        char j_hemisphere;
        // horizontal velocity component in knots: v.v
        double horizontal_speed;
        // heading (путевой угол) in degrees: bbb.b
        double heading;
        // date by UTC: ddmmyy, dd - day, mm - month, yy - year
        unsigned int date;
        // magnetic declination in degrees: xx.x 
        double magnetic_declination;
        // magnetic declination direction: E - subtract from true course, W - add to true course
        char magnetic_declination_direction;
        // mode indicator: A - standalone, D - differential, E - approximation, N - unreliable data
        char mode_indicator;

        // *
        char asterisk;
        // checksum of properties
        short int checksum;
        // <CR> byte = 0x0D
        char cr;
        // <LF> byte = 0x0A
        char lf;
    };
}