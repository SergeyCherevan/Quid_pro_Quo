package projects.quidpro.activities

import android.content.Intent
import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.widget.Toast
import projects.quidpro.R
import projects.quidpro.api.servers.main.MainClientBuilder
import projects.quidpro.api.servers.main.MainRequests
import projects.quidpro.databinding.ActivityLoginBinding
import projects.quidpro.databinding.ActivityRegisterBinding
import projects.quidpro.models.LoginRequest
import projects.quidpro.models.LoginResponce
import projects.quidpro.models.RegisterRequest
import projects.quidpro.models.RegisterResponce
import projects.quidpro.storage.Storage
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response

class RegisterActivity : AppCompatActivity() {

    lateinit var binding: ActivityRegisterBinding
    private val context = this

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        binding = ActivityRegisterBinding.inflate(layoutInflater)
        setContentView(binding.root)

        val actionBar = supportActionBar
        actionBar!!.setLogo(R.drawable.ic_shuffle_tracks_actionbar_icon)
        actionBar.title = "Registration"
        actionBar.setDisplayUseLogoEnabled(true)
        actionBar.setDisplayShowHomeEnabled(true)
        actionBar.setDisplayHomeAsUpEnabled(true)

        binding.apply {
            binding.textViewLoginLink.setOnClickListener {
                val intent = Intent(context, LoginActivity::class.java)
                startActivity(intent)
            }

            binding.buttonRegister.setOnClickListener {
                binding.buttonRegister.isEnabled = false
                if(binding.editTextUserName.text.toString() == "" || binding.editTextUserPassword.text.toString() == "") {
                    Toast.makeText(context, "One of the fields is empty.", Toast.LENGTH_SHORT).show()
                    binding.buttonRegister.isEnabled = true
                } else {
                    val mail = binding.editTextUserName.text.toString()
                    val pass = binding.editTextUserPassword.text.toString()

                    val requests = MainClientBuilder.buildClient(MainRequests::class.java)
                    val response = requests.registration(RegisterRequest(mail, pass))

                    response.enqueue(object: Callback<RegisterResponce> {
                        override fun onResponse(call: Call<RegisterResponce>, response: Response<RegisterResponce>) {
                            if (response.isSuccessful) {

                                Toast.makeText(context, "Registration successful.", Toast.LENGTH_SHORT).show()

                                val data = response.body()!!
                                Storage.LoginedAccount.id = data.Id
                                Storage.LoginedAccount.userName = data.UserName
                                if(data.AvatarFileName != null) Storage.LoginedAccount.avatarFileName = data.AvatarFileName
                                Storage.LoginedAccount.biography = data.Biography
                                Storage.LoginedAccount.role = data.Role


                                /// START LOGIN REQUEST ///

                                val requestsLogin = MainClientBuilder.buildClient(MainRequests::class.java)
                                val responseLogin = requestsLogin.login(LoginRequest(mail, pass))

                                responseLogin.enqueue(object: Callback<LoginResponce> {
                                    override fun onResponse(call: Call<LoginResponce>, response: Response<LoginResponce>) {
                                        if (response.isSuccessful) {

                                            val dataLogin = response.body()!!
                                            Storage.LoginedAccount.jwtString = dataLogin.JwtString

                                            binding.buttonRegister.isEnabled = true

                                            val intent = Intent(context, BottomMenuActivity::class.java)
                                            startActivity(intent)
                                        } else {
                                            when(response.code()) {
                                                404 -> {
                                                    Toast.makeText(context, "The user with this login or password does not exist.", Toast.LENGTH_SHORT).show()
                                                    binding.buttonRegister.isEnabled = true
                                                    val intent = Intent(context, LoginActivity::class.java)
                                                    startActivity(intent)
                                                }
                                                else -> {
                                                    Toast.makeText(context, "An error occurred during authorization.", Toast.LENGTH_SHORT).show()
                                                    binding.buttonRegister.isEnabled = true
                                                    val intent = Intent(context, LoginActivity::class.java)
                                                    startActivity(intent)
                                                }
                                            }
                                        }
                                    }

                                    override fun onFailure(call: Call<LoginResponce>, t: Throwable) {
                                        Toast.makeText(context, "Application Error.\nSend Request Error.", Toast.LENGTH_SHORT).show()
                                        binding.buttonRegister.isEnabled = true
                                        val intent = Intent(context, LoginActivity::class.java)
                                        startActivity(intent)
                                    }
                                })

                                /// END LOGIN REQUEST ///



                            } else {
                                when(response.code()) {
                                    404 -> {
                                        Toast.makeText(context, "The user with this login or password does not exist.", Toast.LENGTH_SHORT).show()
                                        binding.buttonRegister.isEnabled = true
                                    }
                                    else -> {
                                        Toast.makeText(context, "An error occurred during registration.", Toast.LENGTH_SHORT).show()
                                        binding.buttonRegister.isEnabled = true
                                    }
                                }
                            }
                        }

                        override fun onFailure(call: Call<RegisterResponce>, t: Throwable) {
                            Toast.makeText(context, "Application Error.\nSend Request Error.", Toast.LENGTH_SHORT).show()
                            binding.buttonRegister.isEnabled = true
                        }
                    })
                }
            }
        }
    }

    override fun onSupportNavigateUp(): Boolean {
        onBackPressed()
        return true
    }
}