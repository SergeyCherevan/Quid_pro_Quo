package projects.quidpro

import android.app.Activity
import android.content.Intent
import android.graphics.Bitmap
import android.graphics.BitmapFactory
import android.net.Uri
import android.os.Bundle
import android.provider.MediaStore
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Toast
import androidx.activity.result.contract.ActivityResultContracts
import androidx.appcompat.app.AppCompatActivity
import androidx.fragment.app.Fragment
import okhttp3.ResponseBody
import projects.quidpro.Helpers.FileController
import projects.quidpro.Helpers.MultiPartHelper
import projects.quidpro.Helpers.PermissionHelper
import projects.quidpro.Helpers.SignalRMessengerHub
import projects.quidpro.activities.ChangePasswordActivity
import projects.quidpro.activities.LoginActivity
import projects.quidpro.activities.ProfileSettingsActivity
import projects.quidpro.api.servers.main.MainClientBuilder
import projects.quidpro.api.servers.main.MainRequests
import projects.quidpro.databinding.FragmentProfileBinding
import projects.quidpro.models.*
import projects.quidpro.storage.Storage
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response
import java.io.ByteArrayOutputStream
import java.io.File
import java.io.FileOutputStream
import java.io.InputStream


class ProfileFragment : Fragment() {

    private var _binding: FragmentProfileBinding? = null
    private val binding get() = _binding!!

    private val getCommentMedia = registerForActivityResult(ActivityResultContracts.StartActivityForResult()) { result ->
        if (result.resultCode == Activity.RESULT_OK) {
            val uri = result.data?.data
            updateUserAvatar(uri!!)
        }
    }

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
        _binding = FragmentProfileBinding.inflate(inflater, container, false)

        val actionBar = (activity as AppCompatActivity).supportActionBar
        actionBar!!.setLogo(R.drawable.ic_shuffle_tracks_actionbar_icon)
        actionBar.title = "  Profile"
        actionBar.setDisplayUseLogoEnabled(true)
        actionBar.setDisplayShowHomeEnabled(true)

        try {
            loadUserProfileInfo()
        } catch (e: java.lang.Exception) { }

        binding.apply {

            binding.cardProfilePhoto.setOnLongClickListener {
                openNewPhoto()
                return@setOnLongClickListener true
            }

            binding.cardProfilePhotoEditButton.setOnClickListener {
                openNewPhoto()
            }

            binding.cardProfileMyProfile.setOnClickListener {
                val intent = Intent(requireContext(), ProfileSettingsActivity::class.java)
                requireActivity().startActivity(intent)
            }

            binding.cardProfileChangePassword.setOnClickListener {
                val intent = Intent(requireContext(), ChangePasswordActivity::class.java)
                requireActivity().startActivity(intent)
            }

            binding.cardProfileLogout.setOnClickListener {

                Storage.LoginedAccount = Account()
                Storage.LoginedAccountClaims = AccountClaims()
                Storage.userAvatar = UserAvatarSettings()
                Storage.ticketSystemParametersList.clear()
                Storage.ticketsSearch = TicketsSearchParameters()
                Storage.ticketsCreate = TicketCreateParameters()
                Storage.dialogSystemParameters.clear()
                Storage.selectedMessagesDialogSystemParameter = selectedMessagesDialogSystemParameters()

                SignalRMessengerHub.stopConnectionHub()

                val intent = Intent(requireContext(), LoginActivity::class.java)
                requireActivity().startActivity(intent)
                requireActivity().finish()
            }

        }

        return binding.root
    }

    private fun loadUserProfileInfo() {

        val request = MainClientBuilder.buildClient(MainRequests::class.java)
        val response = request.getCurrentUserProfile("Bearer " + Storage.LoginedAccount.jwtString)

        response.enqueue(object: Callback<UserProfileInfoResponce> {
            override fun onResponse(call: Call<UserProfileInfoResponce>, response: Response<UserProfileInfoResponce>) {
                if (response.isSuccessful) {
                    val data = response.body()!!
                    try {
                        Storage.LoginedAccount.id = data.Id
                        Storage.LoginedAccount.userName = data.UserName ?: ""
                        Storage.LoginedAccount.biography = data.Biography ?: "My biography.."
                        Storage.LoginedAccount.avatarFileName = data.AvatarFileName ?: ""
                        Storage.LoginedAccount.role = data.Role ?: ""

                        loadDefaultProfileAvatar()

                        if(Storage.LoginedAccount.avatarFileName.isNotEmpty())
                            loadUserAvatar()

                    } catch (e: Exception) {
                        Toast.makeText(requireContext(), "Get Profile Info Fail.", Toast.LENGTH_LONG).show()
                    }
                }
            }

            override fun onFailure(call: Call<UserProfileInfoResponce>, t: Throwable) {
                Toast.makeText(requireContext(), "Fatal error to get profile info.", Toast.LENGTH_LONG).show()
            }
        })

    }

    private fun loadDefaultProfileAvatar() {
        binding.imageViewProfilePhoto.setImageResource(R.drawable.default_avatar)
        val bitmap = BitmapFactory.decodeResource(requireContext().resources, R.drawable.default_avatar)
        if(Storage.LoginedAccount.avatarFileName == "")
            Storage.LoginedAccount.avatarFileName = "default_avatar.png"
        Storage.userAvatar.avatarAsFile = convertBitMapToFile(bitmap, Storage.LoginedAccount.avatarFileName)
    }

    private fun loadUserAvatar() {

        val request = MainClientBuilder.buildClient(MainRequests::class.java)
        val response = request.getAvatar(Storage.LoginedAccount.avatarFileName)

        response.enqueue(object: Callback<ResponseBody> {
            override fun onResponse(call: Call<ResponseBody>, response: Response<ResponseBody>) {
                if (response.isSuccessful) {
                    val data = response.body()!!
                    val inputStream: InputStream = data.byteStream()
                    val byteArray: ByteArray = inputStream.readBytes()
                    try {

                        val bitmap = BitmapFactory.decodeByteArray(byteArray, 0, byteArray.size)
                        binding.imageViewProfilePhoto.setImageBitmap(bitmap)

                        Storage.userAvatar.avatarAsFile = convertBitMapToFile(bitmap, Storage.LoginedAccount.avatarFileName)

                    } catch (e: Exception) {
                        loadDefaultProfileAvatar()
                    }
                }
            }

            override fun onFailure(call: Call<ResponseBody>, t: Throwable) {
                Toast.makeText(requireContext(), "Get Image Fail to get image of UserAvatar", Toast.LENGTH_LONG).show()
                loadDefaultProfileAvatar()
            }
        })

    }

    private fun convertBitMapToFile(bitmap: Bitmap, fileName: String): File {
        //create a file to write bitmap data
        val f = File(requireContext().cacheDir, fileName)
        f.createNewFile()

        //Convert bitmap to byte array
        val bos = ByteArrayOutputStream();
        bitmap.compress(Bitmap.CompressFormat.PNG, 0 /*ignored for PNG*/, bos)
        val bitMapData: ByteArray = bos.toByteArray()

        //write the bytes in file
        val fos = FileOutputStream(f)
        fos.write(bitMapData)
        fos.flush()
        fos.close()

        return f
    }

    private fun openNewPhoto() {

        // get Permissions to open Gallery
        PermissionHelper.verifyStoragePermissions(requireActivity())

        // open Gallery
        val gallery = Intent(Intent.ACTION_PICK, MediaStore.Images.Media.INTERNAL_CONTENT_URI)
        getCommentMedia.launch(gallery)

    }

    private fun updateUserAvatar(uri: Uri) {

        val file: File = FileController.getFileFromUri(requireContext(), uri = uri)
        Storage.userAvatar.avatarAsUri = uri
        Storage.userAvatar.avatarAsFile = file
        Storage.userAvatar.avatarName = file.name

        sendSaveChangesRequest(uri)

    }

    private fun sendSaveChangesRequest(uri: Uri) {

        val partUserName = MultiPartHelper.createMultiPartString("UserName", Storage.LoginedAccount.userName)
        val partUserBiography = MultiPartHelper.createMultiPartString("Biographi", Storage.LoginedAccount.biography)
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

                    Toast.makeText(requireContext(), "Avatar updated successfully.", Toast.LENGTH_LONG).show()

                    binding.imageViewProfilePhoto.setImageURI(uri)

                    Storage.LoginedAccount.avatarFileName = data.AvatarFileName

                } else {
                    when(response.code()) {
                        404 -> {
                            Toast.makeText(requireContext(), "Avatar update error 404.", Toast.LENGTH_LONG).show()
                        }
                        400 -> {
                            Toast.makeText(requireContext(), "Avatar update error 400.", Toast.LENGTH_LONG).show()
                        }
                        else -> {
                            Toast.makeText(requireContext(), "Avatar update error.", Toast.LENGTH_LONG).show()
                        }
                    }
                }
            }

            override fun onFailure(call: Call<UserProfileInfoResponce>, t: Throwable) {
                Toast.makeText(requireContext(), "AVATAR UPDATE FATAL ERROR.", Toast.LENGTH_LONG).show()
            }
        })

    }

}