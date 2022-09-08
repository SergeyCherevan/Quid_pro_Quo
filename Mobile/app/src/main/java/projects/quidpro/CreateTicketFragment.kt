package projects.quidpro

import android.app.Activity
import android.app.DatePickerDialog
import android.app.Dialog
import android.app.TimePickerDialog
import android.content.Intent
import android.graphics.Color
import android.graphics.drawable.ColorDrawable
import android.graphics.drawable.Drawable
import android.media.ExifInterface
import android.net.Uri
import android.os.Build
import android.os.Bundle
import android.provider.MediaStore
import android.view.Gravity
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.*
import androidx.activity.result.contract.ActivityResultContracts
import androidx.annotation.RequiresApi
import androidx.appcompat.app.AppCompatActivity
import androidx.appcompat.content.res.AppCompatResources
import androidx.cardview.widget.CardView
import androidx.core.content.ContextCompat
import androidx.core.net.toUri
import androidx.core.view.children
import androidx.fragment.app.Fragment
import com.google.android.gms.maps.CameraUpdateFactory
import com.google.android.gms.maps.GoogleMap
import com.google.android.gms.maps.OnMapReadyCallback
import com.google.android.gms.maps.SupportMapFragment
import com.google.android.gms.maps.model.LatLng
import com.google.android.gms.maps.model.Marker
import com.google.android.gms.maps.model.MarkerOptions
import com.google.gson.JsonObject
import okhttp3.MediaType.Companion.toMediaTypeOrNull
import okhttp3.MultipartBody
import okhttp3.RequestBody
import okhttp3.RequestBody.Companion.asRequestBody
import okhttp3.internal.format
import projects.quidpro.Helpers.FileController
import projects.quidpro.Helpers.PermissionHelper.Companion.verifyStoragePermissions
import projects.quidpro.activities.BottomMenuActivity
import projects.quidpro.activities.GeolocationCreateTicketActivity
import projects.quidpro.activities.GeolocationFilterActivity
import projects.quidpro.api.servers.main.MainClientBuilder
import projects.quidpro.api.servers.main.MainRequests
import projects.quidpro.databinding.FragmentCreateTicketBinding
import projects.quidpro.storage.Storage
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response
import java.io.File
import java.io.InputStream
import java.text.Format
import java.time.LocalDateTime
import java.time.format.DateTimeFormatter
import java.util.*


// TODO: Rename parameter arguments, choose names that match
// the fragment initialization parameters, e.g. ARG_ITEM_NUMBER
private const val ARG_PARAM1 = "param1"
private const val ARG_PARAM2 = "param2"

/**
 * A simple [Fragment] subclass.
 * Use the [CreateTicketFragment.newInstance] factory method to
 * create an instance of this fragment.
 */
class CreateTicketFragment : Fragment() {

    private var _binding: FragmentCreateTicketBinding? = null
    private val binding get() = _binding!!
    private var dialogBoxFullPhotoView: Dialog? = null
    private var dialogBoxCreatedTickedSuccessfully: Dialog? = null

    lateinit var mapFragmentPreView: SupportMapFragment
    lateinit var googleMapPreView: GoogleMap
    private lateinit var markerPreView: Marker
    private lateinit var currentPositionPreView: LatLng

    private var calendar: Calendar = Calendar.getInstance()

    @RequiresApi(Build.VERSION_CODES.N)
    private val getCommentMedia = registerForActivityResult(ActivityResultContracts.StartActivityForResult()) { result ->
        if (result.resultCode == Activity.RESULT_OK) {
            val uri = result.data?.data
            loadAndManipulationNewPhoto(uri!!)
        }
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        arguments?.let {

        }
    }

    @RequiresApi(Build.VERSION_CODES.N)
    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        // Inflate the layout for this fragment
        _binding = FragmentCreateTicketBinding.inflate(inflater, container, false)

        val actionBar = (activity as AppCompatActivity).supportActionBar
        actionBar!!.setLogo(R.drawable.ic_shuffle_tracks_actionbar_icon)
        actionBar.title = "  Create New Ticket"
        actionBar.setDisplayUseLogoEnabled(true)
        actionBar.setDisplayShowHomeEnabled(true)

        binding.apply {

            mapFragmentPreView = childFragmentManager.findFragmentById(R.id.mapPreViewCreateTickets) as SupportMapFragment
            mapFragmentPreView.getMapAsync(OnMapReadyCallback {
                googleMapPreView = it

                googleMapPreView.setOnMapClickListener(GoogleMap.OnMapClickListener { latLng ->
                    openGeolocationActivity()
                })

            })

            binding.buttonSelectGeolocationCreateTicket.setOnClickListener {
                openGeolocationActivity()
            }

            binding.cardCreateTicketAddPhotoPreInfo.setOnClickListener {
                openNewPhoto()
            }

            binding.imageButtonCreateTicketAddPhoto.setOnClickListener {
                openNewPhoto()
            }

            binding.cardDateCreateTicketPreInfo.setOnClickListener {
                if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
                    openCalendar()
                }
            }

            binding.imageButtonAddNewDate.setOnClickListener {
                if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
                    openCalendar()
                }
            }

            binding.buttonSendRequestPublishTicket.setOnClickListener {

                binding.buttonSendRequestPublishTicket.isEnabled = false

                Storage.ticketsCreate.title = binding.editTextCreateTicketTitle.text.toString()
                Storage.ticketsCreate.description = binding.editTextCreateTicketDescription.text.toString()

                if(Storage.ticketsCreate.title.isEmpty()) {
                    Toast.makeText(requireContext(), "Title is empty.", Toast.LENGTH_LONG).show()
                    binding.buttonSendRequestPublishTicket.isEnabled = true
                    return@setOnClickListener
                }
                if(Storage.ticketsCreate.description.isEmpty()) {
                    Toast.makeText(requireContext(), "Description is empty.", Toast.LENGTH_LONG).show()
                    binding.buttonSendRequestPublishTicket.isEnabled = true
                    return@setOnClickListener
                }
                if(Storage.ticketsCreate.geomarker.isEmpty()) {
                    Toast.makeText(requireContext(), "Geolocation is empty.", Toast.LENGTH_LONG).show()
                    binding.buttonSendRequestPublishTicket.isEnabled = true
                    return@setOnClickListener
                }
                if(Storage.ticketsCreate.photosAsFileList.isEmpty()) {
                    Toast.makeText(requireContext(), "Photo is empty.", Toast.LENGTH_LONG).show()
                    binding.buttonSendRequestPublishTicket.isEnabled = true
                    return@setOnClickListener
                }
                if(Storage.ticketsCreate.datesList.isEmpty()) {
                    Toast.makeText(requireContext(), "Date and time is empty.", Toast.LENGTH_LONG).show()
                    binding.buttonSendRequestPublishTicket.isEnabled = true
                    return@setOnClickListener
                }

                // Create RequestBody
                val requestBodyCreateTicket = MultipartBody.Builder().setType(MultipartBody.FORM).apply {
                    addFormDataPart("Title", Storage.ticketsCreate.title)
                    addFormDataPart("Text", Storage.ticketsCreate.description)
                    addFormDataPart("PerformServiceInPlace", Storage.ticketsCreate.geomarker)
                    // my files are List<ByteArray>, okhttp has a few utility methods like .toRequestBody for various types like below
                    Storage.ticketsCreate.photosAsFileList.forEach { file ->
                        addFormDataPart("ImageFiles", file.name, file.asRequestBody("multipart/form-data".toMediaTypeOrNull()))
                    }
                    Storage.ticketsCreate.datesList.forEach { date ->
                        addFormDataPart("PerformServiceOnDatesList", date)
                    }
                }.build()

                val request = MainClientBuilder.buildClient(MainRequests::class.java)
                val response = request.publishTicket(requestBodyCreateTicket, "Bearer ${Storage.LoginedAccount.jwtString}")

                response.enqueue(object: Callback<JsonObject> {
                    @RequiresApi(Build.VERSION_CODES.O)
                    override fun onResponse(
                        call: Call<JsonObject>,
                        response: Response<JsonObject>
                    ) {
                        if (response.isSuccessful) {
                            val data = response.body()!!

                            //Toast.makeText(requireContext(), "Created", Toast.LENGTH_LONG).show()

                            openCreatedTickedSuccessfullyDialogBox()

                        } else {
                            when(response.code()) {
                                404 -> {
                                    Toast.makeText(context, "Not found. 404", Toast.LENGTH_SHORT).show()
                                    binding.buttonSendRequestPublishTicket.isEnabled = true
                                }
                                400 -> {
                                    Toast.makeText(context, "Error. 400", Toast.LENGTH_SHORT).show()
                                    binding.buttonSendRequestPublishTicket.isEnabled = true
                                }
                                else -> {
                                    Toast.makeText(context, "Error", Toast.LENGTH_SHORT).show()
                                    binding.buttonSendRequestPublishTicket.isEnabled = true
                                }
                            }
                        }
                    }

                    override fun onFailure(call: Call<JsonObject>, t: Throwable) {
                        Toast.makeText(context, "Fatal Error", Toast.LENGTH_SHORT).show()
                        binding.buttonSendRequestPublishTicket.isEnabled = true
                    }
                })

            }

        }

        return binding.root
    }

    private fun updateTicketSavedStatus() {
        updateStatusMap()
    }

    private fun updateStatusMap() {
        if(Storage.ticketsCreate.geomarker != "") {
            currentPositionPreView = LatLng(
                Storage.ticketsCreate.geomarker.split(",")[0].toDouble(),
                Storage.ticketsCreate.geomarker.split(",")[1].toDouble())

            googleMapPreView.clear()
            markerPreView = googleMapPreView.addMarker(MarkerOptions().position(currentPositionPreView).title(currentPositionPreView.toString()))!!
            googleMapPreView.animateCamera(CameraUpdateFactory.newLatLngZoom(markerPreView.position, 11f))
        } else {
            googleMapPreView.clear()
            googleMapPreView.animateCamera(CameraUpdateFactory.newLatLngZoom(LatLng(49.989772, 36.229503), 5f))
        }
    }

    private fun openGeolocationActivity() {
        val intent = Intent(context, GeolocationCreateTicketActivity::class.java)
        startActivity(intent)
    }

    @RequiresApi(Build.VERSION_CODES.O)
    private fun openCalendar() {

        // Initialize month array
        val months = arrayOf("01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12")

        // Initialize days array
        val days = arrayOf("01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13",
            "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24",
            "25", "26", "27", "28", "29", "30", "31")

        // Initialize hours array
        val hours = arrayOf("00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13",
            "14", "15", "16", "17", "18", "19", "20", "21", "22", "23")

        // Initialize minutes array
        val minutes = arrayOf("00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13",
            "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30",
            "31", "32", "33", "34", "35", "36", "37", "38", "39", "40", "41", "42", "43", "44", "45", "46", "47",
            "48", "49", "50", "51", "52", "53", "54", "55", "56", "57", "58", "59")

        // Initialize calendar variables
        val year = calendar.get(Calendar.YEAR)
        val month = calendar.get(Calendar.MONTH)
        val day = calendar.get(Calendar.DAY_OF_MONTH)
        val hour = calendar.get(Calendar.HOUR)
        val minute = calendar.get(Calendar.MINUTE)

        // Initialize and open DatePickerDialog
        val dpd = DatePickerDialog(requireContext(), R.style.DateTimePickerDialogTheme, DatePickerDialog.OnDateSetListener { view, _year, _monthOfYear, _dayOfMonth ->

            // Create temp variable of date
            var tempDate: String = ""

            // Set selected date in calendar
            try {
                calendar.set(Calendar.YEAR, _year)
                calendar.set(Calendar.MONTH, _monthOfYear)
                calendar.set(Calendar.DAY_OF_MONTH, _dayOfMonth)
            } catch (e: Exception) { }

            // Save date to temp variable
            tempDate = days[_dayOfMonth - 1].toString().plus(".").plus(months[_monthOfYear]).plus(".").plus(_year.toString())

                // Initialize and open TimePickerDialog
                val tpd = TimePickerDialog(requireContext(), R.style.DateTimePickerDialogTheme, TimePickerDialog.OnTimeSetListener { view, _hourOfDay, _minute ->

                    // Set selected time in calendar
                    try {
                        calendar.set(Calendar.HOUR, _hourOfDay)
                        calendar.set(Calendar.MINUTE, _minute)
                    } catch (e: Exception) { }

                    // Save date with time to temp variable
                    val tempDateTime: String = tempDate.plus(" ").plus(hours[_hourOfDay]).plus(":").plus(minutes[_minute]).plus(":").plus("00")

                    // Check date on 'already exist status'
                    if(Storage.ticketsCreate.datesList.contains(tempDateTime)) {
                        Toast.makeText(requireContext(), "This date already exist.", Toast.LENGTH_SHORT).show()
                        return@OnTimeSetListener
                    }

                    // Save date to storage
                    Storage.ticketsCreate.datesList.add(tempDateTime)

                    val dateTimeForView = "Date: ".plus(tempDateTime.split(" ")[0]).plus(" Time: ").plus(tempDateTime.split(" ")[1])

                    // Add selected date to view
                    addNewDateToView(createNewTextView(dateTimeForView), generateImageViewCloseButtonDate())

                }, hour, minute, true)
                tpd.show()

        }, year, month, day)
        dpd.show()

    }

    @RequiresApi(Build.VERSION_CODES.N)
    private fun openNewPhoto() {

        // get Permissions to open Gallery
        verifyStoragePermissions(requireActivity())

        // open Gallery
        val gallery = Intent(Intent.ACTION_PICK, MediaStore.Images.Media.INTERNAL_CONTENT_URI)
        getCommentMedia.launch(gallery)

    }

    @RequiresApi(Build.VERSION_CODES.N)
    private fun loadAndManipulationNewPhoto(uri: Uri) {

        val file: File = FileController.getFileFromUri(requireContext(), uri = uri)
        Storage.ticketsCreate.photosAsFileList.add(file)

        // GET METADATA FROM IMAGE
        val inputStream: InputStream = requireContext().contentResolver.openInputStream(uri)!!

        val exif = ExifInterface(inputStream)
        val width = exif.getAttribute(ExifInterface.TAG_IMAGE_WIDTH) ?: "0"
        val height = exif.getAttribute(ExifInterface.TAG_IMAGE_LENGTH) ?: "0"
        val size = exif.getAttribute(ExifInterface.TAG_MAKE) ?: "Internet"

        // GENERATE FULL RESOLUTION '0x0'
        val resolution = generatePhotoResolution(width, height)

        // CREATE AND ADD PHOTO TO VIEW
        addNewPhotoToView(uri, resolution, size.toString())

    }


    private fun generatePhotoResolution(width: String, height: String): String {
        return width.plus("x").plus(height)
    }

    private fun addNewPhotoToView(uri: Uri, fileResolution: String, fileSize: String) {
        val addedPhotoView: View = layoutInflater.inflate(R.layout.add_photo_create_ticket_row, binding.listNewPhotos, false)

        addedPhotoView.findViewById<ImageView>(R.id.imageViewCreateTicketNewPhoto).setImageURI(uri)
        addedPhotoView.findViewById<TextView>(R.id.textViewCreateTicketPhotoResolution).text = fileResolution
        addedPhotoView.findViewById<TextView>(R.id.textViewCreateTicketPhotoSize).text = fileSize

        addedPhotoView.findViewById<Button>(R.id.imageButtonCreateTicketClearPhoto).setOnClickListener {
            val parentLLSecondView: LinearLayout = (it.parent.parent as LinearLayout)
            val parentCardView: CardView = (parentLLSecondView.parent as CardView)
            val parentRLView: RelativeLayout = (parentCardView.parent as RelativeLayout)
            val parentLinearLayoutView: LinearLayout = (parentRLView.parent as LinearLayout)
            val indexOfClickedCardView: Int = parentLinearLayoutView.indexOfChild(parentRLView)
            binding.listNewPhotos.removeViewAt(indexOfClickedCardView)
            Storage.ticketsCreate.photosAsFileList.removeAt(indexOfClickedCardView)
        }

        val clickedPhoto = addedPhotoView.findViewById<ImageView>(R.id.imageViewCreateTicketNewPhoto).drawable
        addedPhotoView.findViewById<ImageView>(R.id.imageViewCreateTicketNewPhoto).setOnClickListener {
            openFullPhotoViewDialogBox(clickedPhoto)
        }

        binding.listNewPhotos.addView(addedPhotoView)
    }

    private fun openFullPhotoViewDialogBox(clickedPhoto: Drawable) {

        try {
            // Full Photo View Dialog Box Settings
            dialogBoxFullPhotoView = Dialog(requireContext())
            dialogBoxFullPhotoView?.setContentView(R.layout.create_ticket_fullphoto_custom_dialogbox)
            dialogBoxFullPhotoView?.window?.setBackgroundDrawable(ColorDrawable(Color.TRANSPARENT))

        } catch (e: Exception) {

        }

        val imageViewFullPhotoView: ImageView? = dialogBoxFullPhotoView?.findViewById<ImageView>(R.id.imageViewFullPhotoView)
        imageViewFullPhotoView?.setImageDrawable(clickedPhoto)

        val imageButtonCloseFullPhotoView: Button? = dialogBoxFullPhotoView?.findViewById<Button>(R.id.imageButtonCloseFullPhotoView)
        imageButtonCloseFullPhotoView?.setOnClickListener {
            dialogBoxFullPhotoView?.dismiss()
        }

        dialogBoxFullPhotoView?.show()
    }

    private fun generateImageViewCloseButtonDate(): ImageView {
        val imageButtonCloseView = ImageView(requireContext())
        val params = LinearLayout.LayoutParams(LinearLayout.LayoutParams.WRAP_CONTENT, LinearLayout.LayoutParams.WRAP_CONTENT)
        params.setMargins(800, 0,0,0)
        imageButtonCloseView.layoutParams = params
        imageButtonCloseView.setImageResource(R.drawable.ic_close_circle_first)
        imageButtonCloseView.scaleType = ImageView.ScaleType.CENTER_INSIDE
        imageButtonCloseView.setOnClickListener{
            val parentCardView: CardView = (it.parent as CardView)
            val parentLinearLayoutView: LinearLayout = (parentCardView.parent as LinearLayout)
            val indexOfClickedCardView: Int = parentLinearLayoutView.indexOfChild(parentCardView)
            binding.listNewDates.removeViewAt(indexOfClickedCardView)
            Storage.ticketsCreate.datesList.removeAt(indexOfClickedCardView)
        }
        return imageButtonCloseView
    }

    private fun createNewTextView(date: String): TextView {
        val textView = TextView(requireContext())
        val params = LinearLayout.LayoutParams(LinearLayout.LayoutParams.WRAP_CONTENT, LinearLayout.LayoutParams.WRAP_CONTENT)
        textView.layoutParams = params
        textView.text = date
        textView.textSize = 20F
        textView.textAlignment = View.TEXT_ALIGNMENT_TEXT_START
        textView.setTextColor(ContextCompat.getColor(requireContext(), R.color.buttonPassive))
        return textView
    }

    private fun addNewDateToView(dateText: TextView, imgButton: ImageView) {
        // Initialize a new CardView instance
        val cardView = CardView(requireContext())

        // Initialize a new LayoutParams instance, CardView width and height
        val layoutParams = LinearLayout.LayoutParams(
            LinearLayout.LayoutParams.WRAP_CONTENT, // CardView width
            LinearLayout.LayoutParams.WRAP_CONTENT // CardView height
        )

        // Set margins for card view
        layoutParams.setMargins(10, 10, 10, 10)

        layoutParams.gravity = Gravity.CENTER_HORIZONTAL

        // Set the card view layout params
        cardView.layoutParams = layoutParams

        // Set the card view corner radius
        cardView.radius = 50F

        // Set the card view content padding
        cardView.setContentPadding(30,10,20,10)

        // Set the card view background color
        //cardView.setCardBackgroundColor(Color.LTGRAY)
        cardView.setCardBackgroundColor(Color.WHITE)

        // Set card view elevation
        cardView.cardElevation = 10F

        // Set card view maximum elevation
        cardView.maxCardElevation = 12F

        // Set a click listener for card view
        cardView.setOnClickListener(View.OnClickListener {
            try {
                Toast.makeText(requireContext(), (cardView.children.first() as TextView).text, Toast.LENGTH_SHORT).show()
            } catch (e: Exception) { }
        }
        )

        cardView.visibility = View.VISIBLE

        // Add an TextView and ImageView to the CardView
        cardView.addView(dateText)
        cardView.addView(imgButton)

        // Finally, add the CardView in root layout
        binding.listNewDates.addView(cardView)

    }

    @RequiresApi(Build.VERSION_CODES.O)
    private fun openCreatedTickedSuccessfullyDialogBox() {

        try {
            // Ticked View Dialog Box Settings
            dialogBoxCreatedTickedSuccessfully = Dialog(requireContext())
            dialogBoxCreatedTickedSuccessfully?.setContentView(R.layout.successfull_created_ticked)
            dialogBoxCreatedTickedSuccessfully?.window?.setBackgroundDrawable(ColorDrawable(Color.TRANSPARENT))

        } catch (e: Exception) {

        }

        val imageButtonCloseCreatedTickedSuccessfullDialogBox: Button? = dialogBoxCreatedTickedSuccessfully?.findViewById<Button>(R.id.imageButtonCloseCreatedTickedSuccessfullDialogBox)
        imageButtonCloseCreatedTickedSuccessfullDialogBox?.setOnClickListener {

            clearStorage()

            dialogBoxCreatedTickedSuccessfully?.dismiss()

            binding.buttonSendRequestPublishTicket.isEnabled = true

            val intent = Intent(context, BottomMenuActivity::class.java)
            startActivity(intent)
        }

        dialogBoxCreatedTickedSuccessfully?.show()
    }

    private fun clearStorage() {
        Storage.ticketsCreate.datesList.clear()
        Storage.ticketsCreate.photosAsFileList.clear()
        Storage.ticketsCreate.description = ""
        Storage.ticketsCreate.title = ""
        Storage.ticketsCreate.cityByGeocodeIsDownloaded = false
        Storage.ticketsCreate.cityByGeocode = ""
    }

    override fun onResume() {
        super.onResume()
        updateTicketSavedStatus()
    }

    override fun onDestroyView() {
        super.onDestroyView()
        clearStorage()
        _binding = null
    }

}