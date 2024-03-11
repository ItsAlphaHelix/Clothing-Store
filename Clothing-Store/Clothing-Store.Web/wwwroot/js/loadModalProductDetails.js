function loadProductDetails(productId) {
    $.ajax({
        url: '/Products/GetSmallDetails',
        data: { productId: productId },
        type: 'GET',
        success: function (data) {
            $.getScript('/js/mg-common.js', function () {
                $('#mg-quick-view-modal').find('.modal-dialog').html(data);
                $('#mg-quick-view-modal').modal('show');
            });
        },
        error: function () {
            alert('Error! Cannot retrieve product details.');
        }
    });
}
