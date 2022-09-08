package projects.quidpro.activities

import android.content.Intent
import android.os.Bundle
import android.widget.Toast
import androidx.appcompat.app.AppCompatActivity
import projects.quidpro.R
import projects.quidpro.api.servers.main.MainClientBuilder
import projects.quidpro.api.servers.main.MainRequests
import projects.quidpro.databinding.ActivityLoginBinding
import projects.quidpro.models.LoginRequest
import projects.quidpro.models.LoginResponce
import projects.quidpro.storage.Storage
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response


class LoginActivity : AppCompatActivity() {

    lateinit var binding: ActivityLoginBinding
    private val context = this

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        binding = ActivityLoginBinding.inflate(layoutInflater)
        setContentView(binding.root)

        val actionBar = supportActionBar
        actionBar!!.setLogo(R.drawable.ic_shuffle_tracks_actionbar_icon)
        actionBar.title = "  Login"
        actionBar.setDisplayUseLogoEnabled(true)
        actionBar.setDisplayShowHomeEnabled(true)

        binding.apply {
            binding.textViewRegisterLink.setOnClickListener {
                val intent = Intent(context, RegisterActivity::class.java)
                startActivity(intent)
            }

            binding.buttonLogin.setOnClickListener {
                binding.buttonLogin.isEnabled = false
                if(binding.editTextUserName.text.toString() == "" || binding.editTextUserPassword.text.toString() == "") {
                    Toast.makeText(context, "One of the fields is empty.", Toast.LENGTH_SHORT).show()
                    binding.buttonLogin.isEnabled = true
                } else {
                    val mail = binding.editTextUserName.text.toString()
                    val pass = binding.editTextUserPassword.text.toString()

                    val requests = MainClientBuilder.buildClient(MainRequests::class.java)
                    val response = requests.login(LoginRequest(mail, pass))

                    response.enqueue(object: Callback<LoginResponce> {
                        override fun onResponse(call: Call<LoginResponce>, response: Response<LoginResponce>) {
                            if (response.isSuccessful) {

                                val data = response.body()!!
                                Storage.LoginedAccount.jwtString = data.JwtString
                                Storage.LoginedAccount.userName = mail
                                Storage.LoginedAccountClaims.userName = mail
                                Storage.LoginedAccountClaims.userPass = pass

                                binding.buttonLogin.isEnabled = true

                                val intent = Intent(context, BottomMenuActivity::class.java)
                                startActivity(intent)
                            } else {
                                when(response.code()) {
                                    404 -> {
                                        Toast.makeText(context, "Not found.", Toast.LENGTH_SHORT).show()
                                        binding.buttonLogin.isEnabled = true
                                    }
                                    400 -> {
                                        Toast.makeText(context, "The user with this login or password does not exist.", Toast.LENGTH_SHORT).show()
                                        binding.buttonLogin.isEnabled = true
                                    }
                                    else -> {
                                        Toast.makeText(context, "An error occurred during authorization.", Toast.LENGTH_SHORT).show()
                                        binding.buttonLogin.isEnabled = true
                                    }
                                }
                            }
                        }

                        override fun onFailure(call: Call<LoginResponce>, t: Throwable) {
                            Toast.makeText(context, "Application Error.\nSend Request Error.", Toast.LENGTH_SHORT).show()
                            binding.buttonLogin.isEnabled = true
                        }
                    })
                }
            }
        }
    }
}