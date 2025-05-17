function initializeRegister(urls) {
    var usernameTimer = null;
    var emailTimer = null;

    function checkUsername(username) {
        $.ajax({
            url: urls.checkUsername,
            type: "POST",
            data: { username: username },
            success: function (isUnique) {
                var help = $("#usernameHelp");
                if (isUnique) {
                    help.text("Bu kullanıcı adı kullanılabilir.")
                        .removeClass("text-danger")
                        .addClass("text-success");
                } else {
                    help.text("Bu kullanıcı adı zaten kullanımda.")
                        .removeClass("text-success")
                        .addClass("text-danger");
                }
            }
        });
    }

    function checkEmail(email) {
        $.ajax({
            url: urls.checkEmail,
            type: "POST",
            data: { email: email },
            success: function (isUnique) {
                var help = $("#emailHelp");
                if (isUnique) {
                    help.text("Bu e-posta adresi kullanılabilir.")
                        .removeClass("text-danger")
                        .addClass("text-success");
                } else {
                    help.text("Bu e-posta adresi zaten kullanımda.")
                        .removeClass("text-success")
                        .addClass("text-danger");
                }
            }
        });
    }

    function handleRegister(e) {
        e.preventDefault();
        var formData = {
            username: $("#username").val(),
            email: $("#email").val(),
            firstName: $("#firstName").val(),
            lastName: $("#lastName").val(),
            password: $("#password").val()
        };

        $.ajax({
            url: urls.register,
            type: "POST",
            data: formData,
            success: function (response) {
                if (response.success) {
                    alert(response.message);
                    window.location.href = urls.login;
                } else {
                    alert(response.message);
                }
            },
            error: function () {
                alert("Bir hata oluştu!");
            }
        });
    }

    $("#username").on("input", function () {
        clearTimeout(usernameTimer);
        var username = $(this).val();
        if (username.length > 2) {
            usernameTimer = setTimeout(function () {
                checkUsername(username);
            }, 500);
        }
    });

    $("#email").on("input", function () {
        clearTimeout(emailTimer);
        var email = $(this).val();
        if (email.length > 5 && email.includes("@")) {
            emailTimer = setTimeout(function () {
                checkEmail(email);
            }, 500);
        }
    });

    $("#registerForm").on("submit", handleRegister);
} 