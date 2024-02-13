function getAuthPartialView(event) {
    event.preventDefault();
    $.ajax({
        url: '/Accounts/GetAuthPartialView',
        success: function (data) {
            console.log(data);
            $('#account-modal').find('.modal-dialog').html(data); // Update the modal body with the fetched HTML
            $('#account-modal').modal('show');

            // Getting again these three scripts. The reason for doing this is because when the AJAX request starts, it didn't load the scripts I want, and I should explicitly load them in the request.
            $.getScript("https://code.jquery.com/jquery-3.6.0.min.js", function () {
                $.getScript("https://cdnjs.cloudflare.com/ajax/libs/jquery-validate/1.19.3/jquery.validate.min.js", function () {

                    $.getScript("https://cdnjs.cloudflare.com/ajax/libs/jquery-validation-unobtrusive/3.2.12/jquery.validate.unobtrusive.min.js");

                });
            });
        },
        error: function (xhr, status, error) {
            console.log("Error:", xhr.responseText);
        }
    });
}

// Register request
$(document).on('submit', '#register', function (event) {
    event.preventDefault();
    var data = $("#register").serialize();
    $.ajax({
        type: "POST",
        url: '/Accounts/Register',
        data: data,
        success: function (response) {
            console.log("Successfully registered!");
            $('#mg-signin-tab').click();
        },
        error: function (xhr, status, error) {
            var response = JSON.parse(xhr.responseText);

            var form = $('#register');
            form.find('.text-danger').each(function () {
                $(this).text('');
            });

            form.find("span[data-valmsg-for='Email']").text(response[0]).show();
        }
    });
});

// Login request
$(document).on('submit', '#login', function (event) {
    event.preventDefault();
    var data = $("#login").serialize();
    $.ajax({
        type: "POST",
        url: '/Accounts/Login',
        data: data,
        success: function (response) {
            console.log("Successfully logged in!");
            
            $('#account').attr('id', 'logout');
            if (window.location.pathname !== '/') {
                $('#logout img').attr('src', '/images/logout.png');
            } else {
                $('#logout img').attr('src', '/images/white-logout.png');
            }

            $('#account-modal').hide();
            $('.modal-backdrop').hide();
            $('body').css('overflow', '');

            localStorage.setItem('User', JSON.stringify({ fullName: response.fullName }));

        },
        error: function (xhr, status, error) {
            var response = JSON.parse(xhr.responseText);

            var form = $('#login');
            form.find('.text-danger').each(function () {
                $(this).text('');
            });

            form.find("span[data-valmsg-for='Email']").text(response[0]).show();
            form.find("span[data-valmsg-for='Password']").text(response[0]).show();
        }
    });

});

//Logout the user 
$(document).on('click', '#logout', () => {
    $.ajax({
        type: "GET",
        url: '/Accounts/Logout',
        success: function (response) {
            console.log("Successfully logouted!");

            $('#logout').attr('onclick', "getAuthPartialView(event);");
            $('#logout').attr('data-bs-toggle', "modal");
            $('#logout').attr('data-bs-target', "#account-modal");

            if (window.location.pathname !== '/') {
                $('#logout img').attr('src', '/images/svgs/profile.svg');
            } else {
                $('#logout img').attr('src', '/images/svgs/account-white.svg');
            }

        }
    });
});

//Redirection to SignUp
$(document).on('click', '.mg-signup-trigger', () => {
    $('#mg-signup-tab').click();
});