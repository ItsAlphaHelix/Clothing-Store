$(document).on('click', '.mg-see-more-reviews-btn', function (event) {
    event.preventDefault();
    var nextPageNumber = $(this).data('pageNumber');
    var productId = '@Model.Id';

    $.ajax({
        type: "GET",
        url: '/Products/ProductDetails',
        data: { id: productId, pageNumber: nextPageNumber },
        success: function (response) {
            $('.mg-customers-reviews-outer').append(response);
            $('.mg-see-more-reviews-btn').data('pageNumber', nextPageNumber + 1)
        },
        error: function (xhr, status, error) {
            console.log('An error occurred while loading the next page of movies: ' + error);
        }
    });
});

$(document).on('click', '.mg-size-variant-outer ul li', function (event) {
    event.preventDefault();
    $('.mg-size-variant-outer ul li').removeClass('active');

    $(this).addClass('active');
    clearErrorMessage('size-error-message')
});

function decrementQuantity() {
    var quantityInput = $('.mg-cart-quantity')[0];
    if (quantityInput.value > 1) {
        quantityInput.stepDown();
        clearErrorMessage('quantity-error-message');
    } else {
        displayErrorMessage("quantity-error-message");
    }
}

function incrementQuantity() {
    var quantityInput = $('.mg-cart-quantity')[0];
    quantityInput.stepUp();
    clearErrorMessage('quantity-error-message');
}

function addProductToBag(productId) {

    var activeSizeElement = $('.mg-size-variant-outer ul li.active')[0];
    var sizeName = " ";
    var quantity = $('.mg-cart-quantity')[0].value;

    if (activeSizeElement) {
        sizeName = activeSizeElement.innerText;
    }

    $.ajax({
        type: "POST",
        url: '/ShoppingBags/AddProductToBag',
        data:
        {
            productId,
            sizeName,
            quantity
        },
        success: function (response) {

            console.log("Everything is okey.")
            window.location.href = "/ShoppingBags/All";

        },
        error: function (xhr, status, error) {
            var jsonResponse = JSON.parse(xhr.responseText);
            var errorMessage = jsonResponse;

            if (errorMessage.sizeError) {
                displayErrorMessage(errorMessage.sizeError, 'size-error-message');
            } if (errorMessage.quantityError) {
                displayErrorMessage(errorMessage.quantityError, 'quantity-error-message');
                clearErrorMessage('size-error-message')
            }
        }
    });
}
function displayErrorMessage(message, elementId) {
    var errorMessageElement = $('#' + elementId);
    errorMessageElement.text(message);
}

function clearErrorMessage(elementId) {
    var errorMessageElement = $('#' + elementId);
    errorMessageElement.text('');
}
