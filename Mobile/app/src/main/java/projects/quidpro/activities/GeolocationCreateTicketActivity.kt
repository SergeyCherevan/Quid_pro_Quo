package projects.quidpro.activities

import android.content.Intent
import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.os.CountDownTimer
import android.view.View
import com.google.android.gms.maps.CameraUpdateFactory
import com.google.android.gms.maps.GoogleMap
import com.google.android.gms.maps.OnMapReadyCallback
import com.google.android.gms.maps.SupportMapFragment
import com.google.android.gms.maps.model.LatLng
import com.google.android.gms.maps.model.Marker
import com.google.android.gms.maps.model.MarkerOptions
import projects.quidpro.Helpers.ReverseGeocoding
import projects.quidpro.R
import projects.quidpro.databinding.ActivityGeolocationCreateTicketBinding
import projects.quidpro.databinding.ActivityGeolocationFilterBinding
import projects.quidpro.storage.Storage

class GeolocationCreateTicketActivity : AppCompatActivity() {

    lateinit var binding: ActivityGeolocationCreateTicketBinding

    lateinit var mapFragment: SupportMapFragment
    lateinit var googleMap: GoogleMap

    private lateinit var marker: Marker
    private lateinit var currentPosition: LatLng
    private var zoom: Float = 11F

    private var timer: CountDownTimer? = null

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        binding = ActivityGeolocationCreateTicketBinding.inflate(layoutInflater)
        setContentView(binding.root)

        val actionBar = supportActionBar
        actionBar!!.setLogo(R.drawable.ic_shuffle_tracks_actionbar_icon)
        actionBar.title = "  Geolocation"
        actionBar.setDisplayUseLogoEnabled(true)
        actionBar.setDisplayShowHomeEnabled(true)
        actionBar.setDisplayHomeAsUpEnabled(true)

        binding.apply {
            mapFragment = supportFragmentManager.findFragmentById(R.id.mapViewCreateTicketTickets) as SupportMapFragment
            mapFragment.getMapAsync(OnMapReadyCallback {
                googleMap = it

                if(Storage.ticketsCreate.geomarker == "") {
                    googleMap.animateCamera(CameraUpdateFactory.newLatLngZoom(LatLng(49.029251, 32.229536), 6f))
                    binding.imageButtonCreateTicketSubmitGeolocation.isEnabled = false
                    binding.cardGeolocationCreateTicketInfo.visibility = View.INVISIBLE
                } else {
                    currentPosition = LatLng(
                        Storage.ticketsCreate.geomarker.split(",")[0].toDouble(),
                        Storage.ticketsCreate.geomarker.split(",")[1].toDouble())
                    zoom = Storage.ticketsCreate.geomarker.split(",")[2].toFloat()

                    getCityAndCreateMarkerByClick()
                }

                googleMap.setOnMapClickListener(GoogleMap.OnMapClickListener { latLng ->
                    binding.imageButtonCreateTicketSubmitGeolocation.isEnabled = true
                    binding.cardGeolocationCreateTicketInfo.visibility = View.VISIBLE

                    zoom = googleMap.cameraPosition.zoom

                    Storage.ticketsCreate.geomarker =
                        latLng.latitude.toString().plus(",").plus(latLng.longitude.toString().plus(",").plus(zoom.toInt().toString()))

                    currentPosition = latLng

                    getCityAndCreateMarkerByClick()
                })

            })

            binding.imageButtonCreateTicketSubmitGeolocation.setOnClickListener {
                onBackPressed()
            }
        }
    }

    private fun getCityAndCreateMarkerByClick() {
        try {

            ReverseGeocoding.getInfoAboutTicketCityForGeoCreateTicket(currentPosition.latitude.toString(), currentPosition.longitude.toString(), "en")

            /* if(marker != null) {
                 marker.remove()
             }*/

            googleMap.clear()
            marker = googleMap.addMarker(MarkerOptions().position(currentPosition).title(currentPosition.toString()))!!
            googleMap.animateCamera(CameraUpdateFactory.newLatLngZoom(marker.position, zoom))

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
                    if(Storage.ticketsCreate.cityByGeocodeIsDownloaded) {
                        if(marker != null && currentPosition != null) {
                            marker.title = Storage.ticketsCreate.cityByGeocode
                            binding.textViewCreateTicketCityText.text = Storage.ticketsCreate.cityByGeocode
                            Storage.ticketsCreate.cityByGeocodeIsDownloaded = false
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