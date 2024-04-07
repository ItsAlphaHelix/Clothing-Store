
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


function addProductToBag(productId, page, sorting, selectedProducts, selectedPrice, selectedSizes, searchBy) {

    var activeSizeElement = $('.mg-size-variant-outer ul li.active')[0];
    var sizeName = " ";
    var quantity = $('.mg-cart-quantity')[0].value;

    if (activeSizeElement) {
        sizeName = activeSizeElement.innerText;
    }

    var antiForgeryToken = $('input[name="__RequestVerificationToken"]').val();

    $.ajax({
        type: "POST",
        url: '/Bags/AddProductToBag',
        headers: {
            'RequestVerificationToken': antiForgeryToken
        },
        data:
        {
            productId,
            sizeName,
            quantity,
        },
        success: function () {

            if (window.location.href.includes("ProductDetails")) {
                return window.location.href = ' ';
            }
            else if (window.location.href.includes("Bag")) {
                return window.location.href = ' ';
            }

            var action = getCurrentAction();
            var controller = getCurrentController(action);

            var resultSelectedProducts = selectedProducts !== '' ? `&selectedProducts=${selectedProducts}` : ' ';
            var resultSelectedPrice = selectedPrice !== '' ? `&selectedPrice=${selectedPrice}` : ' ';
            var resultSelectedSizes = selectedSizes !== '' ? `&selectedSizes=${selectedSizes}` : ' ';
            var resultSearchBy = searchBy !== ' ' && searchBy != undefined ? `&searchBy=${searchBy}` : ' ';

            window.location.href = `/${controller}/${action}?page=${page}&sorting=${sorting}${resultSelectedProducts}${resultSelectedPrice}${resultSelectedSizes}${resultSearchBy}`;

        },
        error: function (xhr, status, error) {
            console.log(xhr.responseText)
            //var jsonResponse = JSON.parse(xhr.responseText);
            //var errorMessage = jsonResponse;

            //if (errorMessage.sizeError) {
            //    displayErrorMessage(errorMessage.sizeError, 'size-error-message');
            //} if (errorMessage.quantityError) {
            //    displayErrorMessage(errorMessage.quantityError, 'quantity-error-message');
            //    clearErrorMessage('size-error-message')
            //}
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

function getCurrentAction() {
    var currentLocation = window.location.href;
    var action = ' ';

    if (currentLocation.includes("AllMenProducts")) {
        action = "AllMenProducts";
    }
    else if (currentLocation.includes("AllWomenProducts")) {
        action = "AllWomenProducts";
    }
    else if (currentLocation.includes("ProductsByQuery")) {
        action = "ProductsByQuery";
    }
    else {
        action = "All";
    }
    return action;
}

function getCurrentController(action) {
    var controller = ' ';

    if (action === 'ProductsByQuery') {
        controller = 'Search'
    } else {
        controller = 'Products';
    }

    return controller;
}