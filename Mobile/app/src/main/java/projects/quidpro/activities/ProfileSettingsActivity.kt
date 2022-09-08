package projects.quidpro.activities

import android.content.ContentResolver
import android.content.Intent
import android.content.res.Resources
import android.graphics.Bitmap
import android.graphics.BitmapFactory
import android.net.Uri
import android.os.Bundle
import android.widget.Toast
import androidx.appcompat.app.AppCompatActivity
import androidx.core.content.ContextCompat
import projects.quidpro.Helpers.FileController
import projects.quidpro.Helpers.MultiPartHelper
import projects.quidpro.R
import projects.quidpro.api.servers.main.MainClientBuilder
import projects.quidpro.api.servers.main.MainRequests
import projects.quidpro.databinding.ActivityProfileSettingsBinding
import projects.quidpro.models.AllTicketsResponce
import projects.quidpro.models.LoginRequest
import projects.quidpro.models.LoginResponce
import projects.quidpro.models.UserProfileInfoResponce
import projects.quidpro.storage.Storage
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response
import java.io.File


class ProfileSettingsActivity : AppCompatActivity() {

    private val context = this
    lateinit var binding: ActivityProfileSettingsBinding

    private var userName: String = ""
    private var userBiography: String = ""

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        binding = ActivityProfileSettingsBinding.inflate(layoutInflater)
        setContentView(binding.root)

        val actionBar = supportActionBar
        actionBar!!.setLogo(R.drawable.ic_shuffle_tracks_actionbar_icon)
        actionBar.title = "  Profile settings"
        actionBar.setDisplayUseLogoEnabled(true)
        actionBar.setDisplayShowHomeEnabled(true)
        actionBar.setDisplayHomeAsUpEnabled(true)

        binding.apply {

            deActivateButtonSaveChanges()
            loadProfileData()
            activateButtonSaveChanges()

            binding.buttonProfileSettingsSaveChanges.setOnClickListener {

                deActivateButtonSaveChanges()

                userName = binding.editTextProfileSettingsUserName.text.toString()
                userBiography = binding.editTextProfileSettingsUserBiography.text.toString()

                if(userName.isEmpty()) {
                    Toast.makeText(context, "Name is empty.", Toast.LENGTH_LONG).show()
                    activateButtonSaveChanges()
                    return@setOnClickListener
                }
                if(userBiography.isEmpty()) {
                    Toast.makeText(context, "Biography is empty.", Toast.LENGTH_LONG).show()
                    activateButtonSaveChanges()
                    return@setOnClickListener
                }

                sendSaveChangesRequest(userName, userBiography)

            }

        }

    }

    private fun activateButtonSaveChanges() {
        binding.buttonProfileSettingsSaveChanges.isEnabled = true
        binding.buttonProfileSettingsSaveChanges.setBackgroundColor(ContextCompat.getColor(context, R.color.main_project_color_two))

        binding.editTextProfileSettingsUserName.isEnabled = true
        binding.editTextProfileSettingsUserBiography.isEnabled = true
    }

    private fun deActivateButtonSaveChanges() {
        binding.buttonProfileSettingsSaveChanges.isEnabled = false
        binding.buttonProfileSettingsSaveChanges.setBackgroundColor(ContextCompat.getColor(context, R.color.buttonPassive))

        binding.editTextProfileSettingsUserName.isEnabled = false
        binding.editTextProfileSettingsUserBiography.isEnabled = false
    }

    private fun loadProfileData() {
        userName = Storage.LoginedAccount.userName
        userBiography = Storage.LoginedAccount.biography

        binding.editTextProfileSettingsUserName.setText(userName)
        binding.editTextProfileSettingsUserBiography.setText(userBiography)
    }

    private fun sendSaveChangesRequest(_userName: String, _userBiography: String) {

        val partUserName = MultiPartHelper.createMultiPartString("UserName", _userName)
        val partUserBiography = MultiPartHelper.createMultiPartString("Biographi", _userBiography)
        val partUserAvatar = MultiPartHelper.createMultiPartMediaType("AvatarFile", Storage.userAvatar.avatarAsFile ?: File(""))

        val request = MainClientBuilder.buildClient(MainRequests::class.java)
        val response = request.editProfile(partUserName, partUserBiography, partUserAvatar, "Bearer ${Storage.LoginedAccount.jwtString}")
        response.enqueue(object: Callback<UserProfileInfoResponce> {
            override fun onResponse(
                call: Call<UserProfileInfoResponce>,
                response: Response<UserProfileInfoResponce>
            ) {
                if (response.isSuccessful) {
                    val data = response.body()!!

                    Storage.LoginedAccount.avatarFileName = data.AvatarFileName
                    Storage.LoginedAccount.userName = data.UserName
                    Storage.LoginedAccount.biography = data.Biography

                    Storage.LoginedAccountClaims.userName = data.UserName

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

            override fun onFailure(call: Call<UserProfileInfoResponce>, t: Throwable) {
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
       /* val intent = Intent(context, BottomMenuActivity::class.java)
        startActivity(intent)*/
        return true
    }
}