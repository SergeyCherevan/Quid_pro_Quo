package projects.quidpro.activities

import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.widget.Toast
import androidx.core.content.ContextCompat
import com.google.gson.JsonObject
import projects.quidpro.Helpers.MultiPartHelper
import projects.quidpro.R
import projects.quidpro.api.servers.main.MainClientBuilder
import projects.quidpro.api.servers.main.MainRequests
import projects.quidpro.databinding.ActivityChangePasswordBinding
import projects.quidpro.databinding.ActivityProfileSettingsBinding
import projects.quidpro.models.ChangePasswordRequest
import projects.quidpro.models.LoginRequest
import projects.quidpro.models.LoginResponce
import projects.quidpro.models.UserProfileInfoResponce
import projects.quidpro.storage.Storage
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response
import java.io.File

class ChangePasswordActivity : AppCompatActivity() {

    private val context = this
    lateinit var binding: ActivityChangePasswordBinding

    private var userOldPass: String = ""
    private var userNewPass: String = ""
    private var userRepeatPass: String = ""

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        binding = ActivityChangePasswordBinding.inflate(layoutInflater)
        setContentView(binding.root)

        val actionBar = supportActionBar
        actionBar!!.setLogo(R.drawable.ic_shuffle_tracks_actionbar_icon)
        actionBar.title = "  Change Password"
        actionBar.setDisplayUseLogoEnabled(true)
        actionBar.setDisplayShowHomeEnabled(true)
        actionBar.setDisplayHomeAsUpEnabled(true)

        binding.apply {

            binding.buttonChangeUserPassSaveChanges.setOnClickListener {

                deActivateButtonSaveChanges()

                userOldPass = binding.editTextChangeUserPassOldPass.text.toString()
                userNewPass = binding.editTextChangeUserPassNewPass.text.toString()
                userRepeatPass = binding.editTextChangeUserPassRepeatPass.text.toString()

                if(!userNewPass.equals(userRepeatPass)) {
                    Toast.makeText(context, "'New pass' and 'Repeat pass' not equal", Toast.LENGTH_LONG).show()
                    activateButtonSaveChanges()
                    return@setOnClickListener
                }

                sendSaveChangesRequest()

            }

        }
    }

    private fun activateButtonSaveChanges() {
        binding.buttonChangeUserPassSaveChanges.isEnabled = true
        binding.buttonChangeUserPassSaveChanges.setBackgroundColor(ContextCompat.getColor(context, R.color.main_project_color_two))

        binding.editTextChangeUserPassOldPass.isEnabled = true
        binding.editTextChangeUserPassNewPass.isEnabled = true
        binding.editTextChangeUserPassRepeatPass.isEnabled = true
    }

    private fun deActivateButtonSaveChanges() {
        binding.buttonChangeUserPassSaveChanges.isEnabled = false
        binding.buttonChangeUserPassSaveChanges.setBackgroundColor(ContextCompat.getColor(context, R.color.buttonPassive))

        binding.editTextChangeUserPassOldPass.isEnabled = false
        binding.editTextChangeUserPassNewPass.isEnabled = false
        binding.editTextChangeUserPassRepeatPass.isEnabled = true
    }

    private fun sendSaveChangesRequest() {

        val request = MainClientBuilder.buildClient(MainRequests::class.java)
        val response = request.userChangePassword(ChangePasswordRequest(userOldPass, userNewPass), "Bearer ${Storage.LoginedAccount.jwtString}")
        response.enqueue(object: Callback<JsonObject> {
            override fun onResponse(
                call: Call<JsonObject>,
                response: Response<JsonObject>
            ) {
                if (response.isSuccessful) {
                    val data = response.body()!!

                    Storage.LoginedAccountClaims.userPass = userNewPass

                    updateToken()

                } else {
                    when(response.code()) {
                        404 -> {
                            Toast.makeText(context, "Changes save error 404.", Toast.LENGTH_LONG).show()
                            activateButtonSaveChanges()
                        }
                        400 -> {
                            Toast.makeText(context, "Changes save error 400.", Toast.LENGTH_LONG).show()
                            activateButtonSaveChanges()
                        }
                        else -> {
                            Toast.makeText(context, "Changes save error.", Toast.LENGTH_LONG).show()
                            activateButtonSaveChanges()
                        }
                    }
                }
            }

            override fun onFailure(call: Call<JsonObject>, t: Throwable) {
                Toast.makeText(context, "CHANGES SAVE FATAL ERROR.", Toast.LENGTH_LONG).show()
                activateButtonSaveChanges()
            }
        })

    }

    private fun updateToken() {
        val requests = MainClientBuilder.buildClient(MainRequests::class.java)
        val response = requests.login(LoginRequest(Storage.LoginedAccountClaims.userName, Storage.LoginedAccountClaims.userPass))

        response.enqueue(object: Callback<LoginResponce> {
            override fun onResponse(call: Call<LoginResponce>, response: Response<LoginResponce>) {
                if (response.isSuccessful) {

                    val data = response.body()!!
                    Storage.LoginedAccount.jwtString = data.JwtString

                    Toast.makeText(context, "Changes saved successfully.", Toast.LENGTH_LONG).show()
                    activateButtonSaveChanges()


                } else {
                    when(response.code()) {
                        404 -> {
                            Toast.makeText(context, "(UpdateTokenMethod) Changes save error 404.", Toast.LENGTH_LONG).show()
                            activateButtonSaveChanges()
                        }
                        400 -> {
                            Toast.makeText(context, "(UpdateTokenMethod) Changes save error 400.", Toast.LENGTH_LONG).show()
                            activateButtonSaveChanges()
                        }
                        else -> {
                            Toast.makeText(context, "(UpdateTokenMethod) Changes save error.", Toast.LENGTH_LONG).show()
                            activateButtonSaveChanges()
                        }
                    }
                }
            }

            override fun onFailure(call: Call<LoginResponce>, t: Throwable) {
                Toast.makeText(context, "(UpdateTokenMethod) CHANGES SAVE FATAL ERROR.", Toast.LENGTH_LONG).show()
                activateButtonSaveChanges()
            }
        })
    }

    override fun onSupportNavigateUp(): Boolean {
        onBackPressed()
        return true
    }
}