function getAuthPartialView(event) {
    $.ajax({
        url: '/Accounts/GetAuthPartialView',
        success: function (data) {
            $('#account-modal').find('.modal-dialog').html(data);
            $('#account-modal').modal('show');
        },
        error: function (xhr, status, error) {
            console.log("Error:", xhr.responseText);
        }
    });
}

$(document).on('click', '.mg-signup-trigger', () => {
    $('#mg-signup-tab').click();
});