package projects.quidpro.Helpers

import android.annotation.SuppressLint
import android.widget.TextView
import java.text.DateFormat
import java.text.SimpleDateFormat
import java.util.*

class ParseTime {
    companion object {
        @SuppressLint("SimpleDateFormat")
        fun parse(time: String): String {
            var parsed_time = ""
            try {
                val jud = SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ms").parse(time)
                //val month: String = DateFormat.getDateInstance(SimpleDateFormat.LONG, Locale("ru")).format(jud)
                //val time15: String = DateFormat.getDateTimeInstance(DateFormat.SHORT, DateFormat.SHORT, Locale.forLanguageTag("ru")).format(jud)
                //val time15: String = DateFormat.getDateInstance(DateFormat.LONG, Locale.forLanguageTag("en")).format(jud)
                val time15: String = DateFormat.getDateTimeInstance(DateFormat.LONG, DateFormat.SHORT, Locale.forLanguageTag("en")).format(jud)
                parsed_time = time15.split(" ")[0].plus(" ").plus(time15.split(" ")[1]).plus(" ").plus(time15.split(" ")[3])
            } catch (e: Exception) {
                parsed_time = "---"
            }
            return parsed_time
        }

        @SuppressLint("SimpleDateFormat")
        fun parseForDialog(time: String): String {
            var parsedTime = ""
            try {
                val jud = SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ms").parse(time)
                //val time15: String = DateFormat.getDateInstance(SimpleDateFormat.LONG, Locale("ru")).format(jud)
                val time15: String = DateFormat.getDateInstance(SimpleDateFormat.SHORT, Locale("ru")).format(jud)
                //val time15: String = DateFormat.getDateTimeInstance(DateFormat.SHORT, DateFormat.SHORT, Locale.forLanguageTag("ru")).format(jud)
                //val time15: String = DateFormat.getDateInstance(DateFormat.LONG, Locale.forLanguageTag("en")).format(jud)
                //val time15: String = DateFormat.getDateTimeInstance(DateFormat.LONG, DateFormat.SHORT, Locale.forLanguageTag("en")).format(jud)
                //parsed_time = time15.split(" ")[0].plus(" ").plus(time15.split(" ")[1]).plus(" ").plus(time15.split(" ")[3])
                parsedTime = time15
            } catch (e: Exception) {
                parsedTime = "---"
            }
            return parsedTime
        }

        @SuppressLint("SimpleDateFormat")
        fun parseForMessage(time: String): String {
            var parsedTime = ""
            try {
                val jud = SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ms").parse(time)
                val msgTime: String = DateFormat.getTimeInstance(SimpleDateFormat.SHORT, Locale("ru")).format(jud)
                parsedTime = msgTime
            } catch (e: Exception) {
                parsedTime = "---"
            }
            return parsedTime
        }
    }
}