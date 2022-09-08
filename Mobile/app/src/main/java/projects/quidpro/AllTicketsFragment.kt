package projects.quidpro

import android.content.Intent
import android.graphics.PorterDuff
import android.graphics.PorterDuffColorFilter
import android.graphics.drawable.Drawable
import android.os.Bundle
import android.os.CountDownTimer
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.SearchView
import android.widget.Toast
import androidx.appcompat.app.AppCompatActivity
import androidx.appcompat.content.res.AppCompatResources
import androidx.core.content.ContextCompat
import androidx.core.content.ContextCompat.getColor
import androidx.fragment.app.Fragment
import projects.quidpro.activities.FiltersActivity
import projects.quidpro.adapters.AllTicketsAdapter
import projects.quidpro.api.servers.main.MainClientBuilder
import projects.quidpro.api.servers.main.MainRequests
import projects.quidpro.databinding.FragmentAllTicketsBinding
import projects.quidpro.models.AllTicketsResponce
import projects.quidpro.models.TicketSystemParameters
import projects.quidpro.storage.Storage
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response

// TODO: Rename parameter arguments, choose names that match
// the fragment initialization parameters, e.g. ARG_ITEM_NUMBER
private const val ARG_PARAM1 = "param1"
private const val ARG_PARAM2 = "param2"

/**
 * A simple [Fragment] subclass.
 * Use the [AllTicketsFragment.newInstance] factory method to
 * create an instance of this fragment.
 */
class AllTicketsFragment : Fragment() {

    private var _binding: FragmentAllTicketsBinding? = null
    private val binding get() = _binding!!
    private var timer: CountDownTimer? = null

    //private var firstFragmentVisit = true

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        arguments?.let {
        }
    }

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        // inflate the layout and bind to the _binding
        _binding = FragmentAllTicketsBinding.inflate(inflater, container, false)

        val actionBar = (activity as AppCompatActivity).supportActionBar
        actionBar!!.setLogo(R.drawable.ic_shuffle_tracks_actionbar_icon)
        actionBar.title = "  Home"
        actionBar.setDisplayUseLogoEnabled(true)
        actionBar.setDisplayShowHomeEnabled(true)

        if(Storage.ticketsSearch.geomarker == "" && Storage.ticketsSearch.date == "") {
            binding.buttonFiltersOpen.setBackgroundColor(getColor(requireContext(), R.color.buttonPassive))
            val mDrawable: Drawable? = AppCompatResources.getDrawable(requireContext(), R.drawable.ic_options_second);
            //mDrawable?.colorFilter = PorterDuffColorFilter(0xffff00, PorterDuff.Mode.MULTIPLY)
            binding.buttonFiltersOpen.setCompoundDrawablesRelativeWithIntrinsicBounds(null, null, mDrawable, null)
            binding.buttonFiltersOpen.setTextColor(getColor(requireContext(), R.color.buttonPassiveTextColor))
        } else {
            binding.buttonFiltersOpen.setBackgroundColor(ContextCompat.getColor(requireContext(), R.color.main_project_color_one))
            val mDrawable: Drawable? = AppCompatResources.getDrawable(requireContext(), R.drawable.ic_options_second_white);
            //mDrawable?.colorFilter = PorterDuffColorFilter(0xffff00, PorterDuff.Mode.MULTIPLY)
            binding.buttonFiltersOpen.setCompoundDrawablesRelativeWithIntrinsicBounds(null, null, mDrawable, null)
            binding.buttonFiltersOpen.setTextColor(getColor(requireContext(), R.color.buttonActiveTextColor))
        }

        binding.searchTicketsView.setQuery(Storage.ticketsSearch.keywords, false)
        binding.searchTicketsView.isSubmitButtonEnabled = true
        binding.searchTicketsView.setOnQueryTextListener(object: SearchView.OnQueryTextListener {
            override fun onQueryTextSubmit(query: String?): Boolean {
                binding.searchTicketsView.clearFocus()
                Storage.ticketsSearch.keywords = query!!
                getAllTickets(keywords = query)
                return false
            }

            override fun onQueryTextChange(newText: String?): Boolean {
                if(newText == "") {
                    //binding.searchTicketsView.isIconified = true;
                    //binding.searchTicketsView.clearFocus()
                    Storage.ticketsSearch.keywords = newText
                    getAllTickets(keywords = newText)
                }
                return false
            }
        })

        binding.buttonFiltersOpen.setOnClickListener {
            val intent = Intent (context, FiltersActivity::class.java)
            requireActivity().startActivity(intent)
        }

        // Get all tickets start

        getAllTickets()

        // Get all tickets end

        return binding.root
    }

    private fun getAllTickets(keywords: String = Storage.ticketsSearch.keywords,
                              date: String = Storage.ticketsSearch.date,
                              geomarker: String = Storage.ticketsSearch.geomarker,
                              pageNumber: Int = Storage.ticketsSearch.pageNumber,
                              pageSize: Int = Storage.ticketsSearch.pageSize) {
        /// REQUEST FOR GET ALL TICKETS

        val context = requireActivity().applicationContext

        val request = MainClientBuilder.buildClient(MainRequests::class.java)
        //val response = request.getAllTickets(keywords, pageNumber, pageSize, "Bearer ${Storage.LoginedAccount.jwtString}")

        //val searchTickets = AllTicketsRequest(keywords, date, geomarker, pageNumber, pageSize)

        val response = request.getAllTickets(keywords, date, geomarker, pageNumber, pageSize, "Bearer ${Storage.LoginedAccount.jwtString}")
        response.enqueue(object: Callback<AllTicketsResponce> {
            override fun onResponse(
                call: Call<AllTicketsResponce>,
                response: Response<AllTicketsResponce>
            ) {
                if (response.isSuccessful) {
                    val data = response.body()!!

                    val list = data.Tickets.toList()
                    Storage.ticketSystemParametersList.clear()

                    for(ticket in list) {
                        Storage.ticketSystemParametersList.add(TicketSystemParameters(ticket.Id,
                            _ticketImagesDownloaded = false,
                            _ticketCitiesDownloaded = false,
                            _imageFileNames = ticket.ImageFileNames,
                            _PerformServiceInPlaceLat = ticket.PerformServiceInPlaceLat,
                            _PerformServiceInPlaceLng = ticket.PerformServiceInPlaceLng,
                            requireActivity()
                        ))
                    }

                    binding.listAllTickets.adapter = AllTicketsAdapter(context, R.layout.ticket_item_row, list)

                    startCheckDownloadTicketComponents()
                    /*binding.apiariesList.setOnItemClickListener { parent: AdapterView<*>, view: View, position: Int, id: Long ->
                        Storage.Selected_apiary_id = list[position].id
                        val intent = Intent (context, ActivityBeehivesScreen::class.java)
                        requireActivity().startActivity(intent)
                    }*/

                } else {
                    when(response.code()) {
                        404 -> {
                            Toast.makeText(context, "Пасік не знайдено", Toast.LENGTH_SHORT).show()
                        }
                        400 -> {
                            Toast.makeText(context, "Помилка при підрахунку праметрів пасік 400", Toast.LENGTH_SHORT).show()
                        }
                        else -> {
                            Toast.makeText(context, "Сталася помилка при виведенні пасік", Toast.LENGTH_SHORT).show()
                        }
                    }
                }
            }

            override fun onFailure(call: Call<AllTicketsResponce>, t: Throwable) {
                Toast.makeText(context, "Сталася помилка при виведенні пасік", Toast.LENGTH_SHORT).show()
            }
        })

        /// REQUEST FOR GET ALL TICKETS
    }

    override fun onDestroyView() {
        super.onDestroyView()
        Storage.ticketSystemParametersList.clear()
        _binding = null
    }

    override fun onResume() {
        super.onResume()
        /*if(!firstFragmentVisit)
            getAllTickets()
        firstFragmentVisit = false*/
    }

    private fun startCheckDownloadTicketComponents() {
        startCountDownTimer()
    }

    private fun stopCheckDownloadTicketComponents() {
        timer?.cancel()
    }

    private fun startCountDownTimer(){
        timer?.cancel()
        timer = object: CountDownTimer(100, 100) {
            override fun onTick(millisUntilFinished: Long) {
                //Toast.makeText(_binding!!.root.context,"+++", Toast.LENGTH_SHORT).show()
            }

            override fun onFinish() {
                var ticketCount: Int = 0
                try {
                    for(ticket in Storage.ticketSystemParametersList) {
                        if(!ticket.ticketImagesDownloaded || !ticket.ticketCitiesDownloaded) {
                            break
                        }

                        ticketCount++

                        if(ticketCount == Storage.ticketSystemParametersList.size) {
                            binding.listAllTickets.invalidateViews()
                            return
                        }
                    }
                } catch (e: Exception){
                }
                startCheckDownloadTicketComponents()
            }
        }.start()
    }

}