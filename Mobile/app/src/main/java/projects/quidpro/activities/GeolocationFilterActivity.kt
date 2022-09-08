package projects.quidpro.activities

import android.os.Bundle
import android.os.CountDownTimer
import android.view.View
import android.widget.Toast
import androidx.appcompat.app.AppCompatActivity
import com.google.android.gms.maps.CameraUpdateFactory
import com.google.android.gms.maps.GoogleMap
import com.google.android.gms.maps.GoogleMap.OnMapClickListener
import com.google.android.gms.maps.OnMapReadyCallback
import com.google.android.gms.maps.SupportMapFragment
import com.google.android.gms.maps.model.LatLng
import com.google.android.gms.maps.model.Marker
import com.google.android.gms.maps.model.MarkerOptions
import projects.quidpro.Helpers.ReverseGeocoding
import projects.quidpro.R
import projects.quidpro.databinding.ActivityGeolocationFilterBinding
import projects.quidpro.storage.Storage


class GeolocationFilterActivity : AppCompatActivity() {

    lateinit var binding: ActivityGeolocationFilterBinding
    lateinit var mapFragment: SupportMapFragment
    lateinit var googleMap: GoogleMap

    private lateinit var marker: Marker
    private lateinit var currentPosition: LatLng

    private var timer: CountDownTimer? = null

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        binding = ActivityGeolocationFilterBinding.inflate(layoutInflater)
        setContentView(binding.root)

        val actionBar = supportActionBar
        actionBar!!.setLogo(R.drawable.ic_shuffle_tracks_actionbar_icon)
        actionBar.title = "  Geolocation"
        actionBar.setDisplayUseLogoEnabled(true)
        actionBar.setDisplayShowHomeEnabled(true)
        actionBar.setDisplayHomeAsUpEnabled(true)

        binding.apply {
            mapFragment = supportFragmentManager.findFragmentById(R.id.mapViewFiltersTickets) as SupportMapFragment
            mapFragment.getMapAsync(OnMapReadyCallback {
                googleMap = it

                if(Storage.ticketsSearch.geomarker == "") {
                    /*currentPosition = LatLng(49.990400, 36.232781)
                    marker = googleMap.addMarker(MarkerOptions().position(currentPosition).title("Default location"))!!
                    googleMap.animateCamera(CameraUpdateFactory.newLatLngZoom(marker.position, 12f))
                    binding.textViewCityText.text = "Kharkiv"
                    Storage.ticketsSearch.geomarker = currentPosition.latitude.toString().plus(";").plus(currentPosition.longitude.toString())*/
                    googleMap.animateCamera(CameraUpdateFactory.newLatLngZoom(LatLng(49.029251, 32.229536), 6f))
                    binding.imageButtonSubmitGeolocation.isEnabled = false
                    binding.cardGeolocationFilterInfo.visibility = View.INVISIBLE
                } else {
                    currentPosition = LatLng(Storage.ticketsSearch.geomarker.split(",")[0].toDouble(),
                        Storage.ticketsSearch.geomarker.split(",")[1].toDouble())
                    getCityAndCreateMarkerByClick()
                }

                googleMap.setOnMapClickListener(OnMapClickListener { latLng ->
                    binding.imageButtonSubmitGeolocation.isEnabled = true
                    binding.cardGeolocationFilterInfo.visibility = View.VISIBLE

                    Storage.ticketsSearch.geomarker = latLng.latitude.toString().plus(",").plus(latLng.longitude.toString())

                    currentPosition = latLng
                    getCityAndCreateMarkerByClick()
                })

            })

            binding.imageButtonSubmitGeolocation.setOnClickListener {
                onBackPressed()
            }
        }
    }

    private fun getCityAndCreateMarkerByClick() {
        try {

            ReverseGeocoding.getInfoAboutTicketCityForGeoFilter(currentPosition.latitude.toString(), currentPosition.longitude.toString(), "en")

           /* if(marker != null) {
                marker.remove()
            }*/
                googleMap.clear()
                marker = googleMap.addMarker(MarkerOptions().position(currentPosition).title(currentPosition.toString()))!!
                googleMap.animateCamera(CameraUpdateFactory.newLatLngZoom(marker.position, 11f))

            startCheckDownloadCityByGeocode()

        } catch (e: Exception) {

        }
    }

    private fun startCheckDownloadCityByGeocode() {
        startCountDownTimer()
    }

    private fun stopCheckDownloadCityByGeocode() {
        timer?.cancel()
    }

    private fun startCountDownTimer(){
        timer?.cancel()
        timer = object: CountDownTimer(100, 100) {
            override fun onTick(millisUntilFinished: Long) {
                //Toast.makeText(_binding!!.root.context,"+++", Toast.LENGTH_SHORT).show()
            }

            override fun onFinish() {
                try {
                    if(Storage.ticketsSearch.cityByGeocodeIsDownloaded) {
                        if(marker != null && currentPosition != null) {
                            marker.title = Storage.ticketsSearch.cityByGeocode
                            binding.textViewCityText.text = Storage.ticketsSearch.cityByGeocode
                            Storage.ticketsSearch.cityByGeocodeIsDownloaded = false
                        }
                        return
                    }
                    startCheckDownloadCityByGeocode()
                } catch (e: Exception){
                }
            }
        }.start()
    }

    override fun onSupportNavigateUp(): Boolean {
        onBackPressed()
        return true
    }
}