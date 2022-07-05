{
    let avatarBlock = document.getElementsByClassName("avatar-with-button")[0];
    let avatarInput = document.getElementById("avatar-input");
    avatarBlock.addEventListener('click', avatarBlockClick);

    function avatarBlockClick() {
        avatarInput.click();
    }

    avatarInput.addEventListener('change', showUploadAvatar);

    function showUploadAvatar() {
        let file = this.files[0];
        let imageOnForm = document.getElementsByClassName("avatar-on-profile")[0];

        if (!file.type.startsWith('image/')) {
            imageOnForm.src = null;
            avatarInput.value = null;
            return;
        }

        let reader = new FileReader();
        reader.onload = function (e) {
            imageOnForm.src = e.target.result;
        }
        reader.readAsDataURL(file);
    }
}
