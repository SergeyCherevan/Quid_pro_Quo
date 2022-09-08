package projects.quidpro

import android.os.Bundle
import android.os.CountDownTimer
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.SearchView
import android.widget.Toast
import androidx.appcompat.app.AppCompatActivity
import projects.quidpro.adapters.AllRecommendationsAdapter
import projects.quidpro.adapters.AllTicketsAdapter
import projects.quidpro.api.servers.main.MainClientBuilder
import projects.quidpro.api.servers.main.MainRequests
import projects.quidpro.databinding.FragmentAllTicketsBinding
import projects.quidpro.databinding.FragmentSuggestionsBinding
import projects.quidpro.models.AllRecommendationsResponce
import projects.quidpro.models.AllTicketsResponce
import projects.quidpro.models.RecommendationSystemParameters
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
 * Use the [SuggestionsFragment.newInstance] factory method to
 * create an instance of this fragment.
 */
class SuggestionsFragment : Fragment() {

    private var _binding: FragmentSuggestionsBinding? = null
    private val binding get() = _binding!!
    private var timer: CountDownTimer? = null

    private  var keywords: String = ""

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        arguments?.let {

        }
    }

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        // Inflate the layout for this fragment
        _binding = FragmentSuggestionsBinding.inflate(inflater, container, false)

        val actionBar = (activity as AppCompatActivity).supportActionBar
        actionBar!!.setLogo(R.drawable.ic_shuffle_tracks_actionbar_icon)
        actionBar.title = "  Users"
        actionBar.setDisplayUseLogoEnabled(true)
        actionBar.setDisplayShowHomeEnabled(true)

        binding.apply {

            binding.searchRecommendationView.setOnQueryTextListener(object: SearchView.OnQueryTextListener {
                override fun onQueryTextSubmit(query: String?): Boolean {
                    binding.searchRecommendationView.clearFocus()
                    keywords = query!!
                    getAllRecommendations(keywords = query)
                    return false
                }

                override fun onQueryTextChange(newText: String?): Boolean {
                    if(newText == "") {
                        //binding.searchTicketsView.isIconified = true;
                        //binding.searchTicketsView.clearFocus()
                        keywords = newText
                        getAllRecommendations(keywords = newText)
                    }
                    return false
                }
            })

            // Get all recommendations(users) start

            getAllRecommendations()

            // Get all recommendations(users) end

        }

        return binding.root
    }

    private fun getAllRecommendations(keywords: String = "") {

        val context = requireActivity().applicationContext

        val request = MainClientBuilder.buildClient(MainRequests::class.java)
        val response = request.getAllRecommendations(keywords, pageNumber = 0, pageSize = 999, "Bearer ${Storage.LoginedAccount.jwtString}")
        response.enqueue(object: Callback<AllRecommendationsResponce> {
            override fun onResponse(
                call: Call<AllRecommendationsResponce>,
                response: Response<AllRecommendationsResponce>
            ) {
                if (response.isSuccessful) {
                    val data = response.body()!!

                    val list = data.Recommendations.toList()
                    Storage.recommendationSystemParametersList.clear()

                    for(recommendation in list) {
                        Storage.recommendationSystemParametersList.add(
                        RecommendationSystemParameters(recommendation.id, false, recommendation.avatarFileName ?: "", requireActivity())
                        )
                    }

                    binding.listAllRecommendations.adapter = AllRecommendationsAdapter(context, R.layout.recomendation_item_row, list, requireContext())
                    startCheckDownloadRecommendationsComponents()

                    /*binding.apiariesList.setOnItemClickListener { parent: AdapterView<*>, view: View, position: Int, id: Long ->
                        Storage.Selected_apiary_id = list[position].id
                        val intent = Intent (context, ActivityBeehivesScreen::class.java)
                        requireActivity().startActivity(intent)
                    }*/

                } else {
                    when(response.code()) {
                        404 -> {
                            Toast.makeText(context, "Error 404.", Toast.LENGTH_SHORT).show()
                        }
                        400 -> {
                            Toast.makeText(context, "Error 400.", Toast.LENGTH_SHORT).show()
                        }
                        else -> {
                            Toast.makeText(context, "Error.", Toast.LENGTH_SHORT).show()
                        }
                    }
                }
            }

            override fun onFailure(call: Call<AllRecommendationsResponce>, t: Throwable) {
                Toast.makeText(context, "Fatal Error.", Toast.LENGTH_SHORT).show()
            }
        })
    }

    override fun onDestroyView() {
        super.onDestroyView()
        _binding = null
    }

    private fun startCheckDownloadRecommendationsComponents() {
        startCountDownTimer()
    }

    private fun stopCheckDownloadRecommendationsComponents() {
        timer?.cancel()
    }

    private fun startCountDownTimer(){
        timer?.cancel()
        timer = object: CountDownTimer(100, 100) {
            override fun onTick(millisUntilFinished: Long) {
                //Toast.makeText(_binding!!.root.context,"+++", Toast.LENGTH_SHORT).show()
            }

            override fun onFinish() {
                var userCount: Int = 0
                try {
                    for(recommendation in Storage.recommendationSystemParametersList) {
                        if(!recommendation.recommendationImagesDownloaded) {
                            break
                        }

                        userCount++

                        if(userCount == Storage.recommendationSystemParametersList.size) {
                            binding.listAllRecommendations.invalidateViews()
                            return
                        }
                    }
                } catch (e: Exception){
                }
                startCheckDownloadRecommendationsComponents()
            }
        }.start()
    }

}