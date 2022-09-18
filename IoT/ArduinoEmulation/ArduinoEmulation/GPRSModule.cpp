#pragma once

#include <iostream>
#include <stdio.h>

#include <curl/curl.h>

namespace ArduinoEmulation {

    using namespace std;

    class GPRSModule {

    public:
        string httpsPost(string url, string jsonObject, string jwt) {
            cout << "Method httpsPost() run...\n";

            CURLcode ret;
            CURL* hnd;
            struct curl_slist* slist1;

            cout << "My URL: \'" << url << "\'\n";
            cout << "My JSON-object: " << jsonObject << "\n";
            cout << "My JWT-string: \'" << jwt << "\'\n\n";

            slist1 = NULL;
            slist1 = curl_slist_append(slist1, "Content-Type: application/json");
            slist1 = curl_slist_append(slist1, ("Authorization: Bearer "+ jwt).c_str());

            hnd = curl_easy_init();
            curl_easy_setopt(hnd, CURLOPT_URL, url.c_str());
            curl_easy_setopt(hnd, CURLOPT_NOPROGRESS, 1L);
            curl_easy_setopt(hnd, CURLOPT_POSTFIELDS, jsonObject.c_str());
            curl_easy_setopt(hnd, CURLOPT_USERAGENT, "curl/7.38.0");
            curl_easy_setopt(hnd, CURLOPT_HTTPHEADER, slist1);
            curl_easy_setopt(hnd, CURLOPT_MAXREDIRS, 50L);
            curl_easy_setopt(hnd, CURLOPT_CUSTOMREQUEST, "POST");
            curl_easy_setopt(hnd, CURLOPT_TCP_KEEPALIVE, 1L);

            ret = curl_easy_perform(hnd);

            curl_easy_cleanup(hnd);
            hnd = NULL;
            curl_slist_free_all(slist1);
            slist1 = NULL;

            return "Ok";
        }
    };
}