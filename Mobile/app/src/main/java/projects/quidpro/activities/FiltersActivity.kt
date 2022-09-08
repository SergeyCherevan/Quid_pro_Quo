package projects.quidpro.activities

import android.app.DatePickerDialog
import android.app.Dialog
import android.content.Intent
import android.graphics.Color
import android.graphics.drawable.ColorDrawable
import android.os.Bundle
import android.widget.Button
import android.widget.Toast
import androidx.appcompat.app.AppCompatActivity
import androidx.core.content.ContextCompat
import com.google.android.gms.maps.CameraUpdateFactory
import com.google.android.gms.maps.GoogleMap
import com.google.android.gms.maps.OnMapReadyCallback
import com.google.android.gms.maps.SupportMapFragment
import com.google.android.gms.maps.model.LatLng
import com.google.android.gms.maps.model.Marker
import com.google.android.gms.maps.model.MarkerOptions
import okhttp3.MultipartBody
import projects.quidpro.R
import projects.quidpro.api.servers.main.MainClientBuilder
import projects.quidpro.api.servers.main.MainRequests
import projects.quidpro.databinding.ActivityFiltersBinding
import projects.quidpro.models.AllTicketsResponce
import projects.quidpro.storage.Storage
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response
import java.text.DateFormat
import java.text.SimpleDateFormat
import java.util.*
import kotlin.time.Duration.Companion.days

class FiltersActivity : AppCompatActivity() {

    private val context = this
    lateinit var binding: ActivityFiltersBinding
    lateinit var mapFragment: SupportMapFragment
    lateinit var googleMap: GoogleMap

    private lateinit var marker: Marker
    private lateinit var currentPosition: LatLng

    private var ticketsCount = 0

    private var calendar: Calendar = Calendar.getInstance()

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        binding = ActivityFiltersBinding.inflate(layoutInflater)
        setContentView(binding.root)

        val actionBar = supportActionBar
        actionBar!!.setLogo(R.drawable.ic_shuffle_tracks_actionbar_icon)
        actionBar.title = "  Filters"
        actionBar.setDisplayUseLogoEnabled(true)
        actionBar.setDisplayShowHomeEnabled(true)
        actionBar.setDisplayHomeAsUpEnabled(true)

        binding.apply {
            mapFragment = supportFragmentManager.findFragmentById(R.id.mapPreViewFiltersTickets) as SupportMapFragment
            mapFragment.getMapAsync(OnMapReadyCallback {
                googleMap = it

                updateTicketsListCount()
            })

            binding.cardGeolocationFilterPreInfo.setOnClickListener {
                val intent = Intent(context, GeolocationFilterActivity::class.java)
                startActivity(intent)
            }

            binding.imageButtonClearGeolocation.setOnClickListener {
                Storage.ticketsSearch.geomarker = ""
                Storage.ticketsSearch.cityByGeocode = ""
                Storage.ticketsSearch.cityByGeocodeIsDownloaded = false

                updateTicketsListCount()
            }

            binding.buttonShowFilteredTickets.setOnClickListener {
                //onBackPressed()
                val intent = Intent(context, BottomMenuActivity::class.java)
                startActivity(intent)
            }

            binding.buttonClearAllFilters.setOnClickListener {
                Storage.ticketsSearch.geomarker = ""
                Storage.ticketsSearch.cityByGeocode = ""
                Storage.ticketsSearch.date = ""
                Storage.ticketsSearch.cityByGeocodeIsDownloaded = false

                val intent = Intent(context, BottomMenuActivity::class.java)
                startActivity(intent)
            }

            binding.cardDateFilterPreInfo.setOnClickListener {
                openCalendar()
            }

            binding.imageViewDateFilterPreInfo.setOnClickListener {
                openCalendar()
            }

            binding.imageButtonClearDate.setOnClickListener {
                Storage.ticketsSearch.date = ""
                updateTicketsListCount()
            }
        }
    }

    private fun updateTicketsListCount() {
        updateStatusMap()
        updateStatusLablesGeocodeAndCity()
        updateStatusButtonClearGeolocation()

        updateStatusButtonClearDate()
        updateStatusLableDate()
        updateCalendar()

        updateTicketsCount()
    }

    private fun updateStatusMap() {
        if(Storage.ticketsSearch.geomarker != "") {
            currentPosition = LatLng(Storage.ticketsSearch.geomarker.split(",")[0].toDouble(),
                Storage.ticketsSearch.geomarker.split(",")[1].toDouble())

            googleMap.clear()
            marker = googleMap.addMarker(MarkerOptions().position(currentPosition).title(currentPosition.toString()))!!
            googleMap.animateCamera(CameraUpdateFactory.newLatLngZoom(marker.position, 11f))
        } else {
            googleMap.clear()
            googleMap.animateCamera(CameraUpdateFactory.newLatLngZoom(LatLng(49.989772, 36.229503), 5f))
        }
    }

    private fun updateStatusButtonClearGeolocation() {
        if(Storage.ticketsSearch.geomarker == "") {
            binding.imageButtonClearGeolocation.setBackgroundColor(ContextCompat.getColor(context, R.color.buttonPassive))
            binding.imageButtonClearGeolocation.isEnabled = false
        } else {
            binding.imageButtonClearGeolocation.setBackgroundColor(ContextCompat.getColor(context, R.color.buttonRedFilters))
            binding.imageButtonClearGeolocation.isEnabled = true
        }
    }

    private fun updateStatusLablesGeocodeAndCity() {
        binding.textViewCityText.text = Storage.ticketsSearch.cityByGeocode
        if(Storage.ticketsSearch.geomarker != "") {
            binding.textViewCoordinatesLatText.text = Storage.ticketsSearch.geomarker.split(",")[0]
            binding.textViewCoordinatesLongText.text = Storage.ticketsSearch.geomarker.split(",")[1]
        } else {
            binding.textViewCoordinatesLatText.text = ""
            binding.textViewCoordinatesLongText.text = ""
        }
    }

    private fun updateTicketsCount(keywords: String = Storage.ticketsSearch.keywords,
                                   date: String = Storage.ticketsSearch.date,
                                   geomarker: String = Storage.ticketsSearch.geomarker,
                                   pageNumber: Int = Storage.ticketsSearch.pageNumber,
                                   pageSize: Int = Storage.ticketsSearch.pageSize) {

        /// REQUEST FOR GET ALL TICKETS

        val request = MainClientBuilder.buildClient(MainRequests::class.java)
        val response = request.getAllTickets(keywords, date, geomarker, pageNumber, pageSize, "Bearer ${Storage.LoginedAccount.jwtString}")
        response.enqueue(object: Callback<AllTicketsResponce> {
            override fun onResponse(
                call: Call<AllTicketsResponce>,
                response: Response<AllTicketsResponce>
            ) {
                if (response.isSuccessful) {
                    val data = response.body()!!

                    ticketsCount = data.TicketsCount

                    val btnNewText: String = "Show".plus( " ").plus( ticketsCount.toString()).plus( " ").plus("tickets")
                    binding.buttonShowFilteredTickets.text = btnNewText

                } else {
                    when(response.code()) {
                        404 -> {
                            Toast.makeText(context, "Not found. 404", Toast.LENGTH_SHORT).show()
                        }
                        400 -> {
                            Toast.makeText(context, "Error. 400", Toast.LENGTH_SHORT).show()
                        }
                        else -> {
                            Toast.makeText(context, "Error", Toast.LENGTH_SHORT).show()
                        }
                    }
                }
            }

            override fun onFailure(call: Call<AllTicketsResponce>, t: Throwable) {
                Toast.makeText(context, "Error", Toast.LENGTH_SHORT).show()
            }
        })

        /// REQUEST FOR GET ALL TICKETS

    }

    private fun updateStatusButtonClearDate() {
        if(Storage.ticketsSearch.date == "") {
            binding.imageButtonClearDate.setBackgroundColor(ContextCompat.getColor(context, R.color.buttonPassive))
            binding.imageButtonClearDate.isEnabled = false
        } else {
            binding.imageButtonClearDate.setBackgroundColor(ContextCompat.getColor(context, R.color.buttonRedFilters))
            binding.imageButtonClearDate.isEnabled = true
        }
    }

    private fun updateStatusLableDate() {
        if(Storage.ticketsSearch.date != "") {
            val dateArr = Storage.ticketsSearch.date.split("-")
            val dateFromStorage = "Date: ".plus(dateArr[2]).plus(".").plus(dateArr[1]).plus(".").plus(dateArr[0])
            binding.textViewDateText.text = dateFromStorage
        } else {
            val dateFromStorage = "Date: "
            binding.textViewDateText.text = dateFromStorage
        }
    }

    private fun updateCalendar() {
        if(Storage.ticketsSearch.date != "") {
            try {
                calendar.set(Calendar.YEAR, Storage.ticketsSearch.date.split("-")[0].toInt())
                calendar.set(Calendar.MONTH, Storage.ticketsSearch.date.split("-")[1].toInt() - 1)
                calendar.set(Calendar.DAY_OF_MONTH, Storage.ticketsSearch.date.split("-")[2].toInt())
            } catch (e: Exception) { }

        }
    }

    private fun openCalendar() {

        // Initialize month array
        val months = arrayOf("01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12")

        // Initialize calendar variables
        // Init get variables
        val year = calendar.get(Calendar.YEAR)
        val month = calendar.get(Calendar.MONTH)
        val day = calendar.get(Calendar.DAY_OF_MONTH)

        // Initialize calendar
        val dpd = DatePickerDialog(context, DatePickerDialog.OnDateSetListener { view, _year, _monthOfYear, _dayOfMonth ->

            // Set selected date in calendar
            try {
                calendar.set(Calendar.YEAR, _year)
                calendar.set(Calendar.MONTH, _monthOfYear)
                calendar.set(Calendar.DAY_OF_MONTH, _dayOfMonth)
            } catch (e: Exception) { }

            // Display Selected date in text box
            val newCurrentFilterDateForButton: String = "Date: ".plus(_dayOfMonth.toString()).plus(".").plus(months[_monthOfYear]).plus(".").plus(_year.toString())
            binding.textViewDateText.text = newCurrentFilterDateForButton

            // Save date to storage
            val newCurrentFilterDateForStorage: String = _year.toString().plus("-").plus(months[_monthOfYear]).plus("-").plus(_dayOfMonth.toString())
            Storage.ticketsSearch.date = newCurrentFilterDateForStorage

            // Update count of tickets and all filters status
            updateTicketsListCount()

        }, year, month, day)

        dpd.show()

    }

    override fun onResume() {
        super.onResume()
        //updateTicketsListCount()
    }

    override fun onRestart() {
        super.onRestart()
        updateTicketsListCount()
    }

    override fun onSupportNavigateUp(): Boolean {
        //onBackPressed()
        val intent = Intent(context, BottomMenuActivity::class.java)
        startActivity(intent)
        return true
    }
}