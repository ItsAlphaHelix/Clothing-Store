function callChangeOrder(sorting, searchBy) {

    var selectedProducts = getSelectedProducts();
    var selectedSizes = getSelectedSizes();
    var selectedPrice = localStorage.getItem("selectedPrice");

    var resultSelectedProducts = selectedProducts.length != 0 ? `&selectedProducts=${selectedProducts.join(',')}` : " ";
    var resultSelectedPrice = selectedPrice != " " ? `&selectedPrice=${selectedPrice}` : " ";
    var resultSelectedSizes = selectedSizes.length != 0 ? `&selectedSizes=${selectedSizes.join(',')}` : " ";
    var resultSearchBy = selectedSizes.length != 0 ? `&selectedSizes=${selectedSizes.join(',')}` : " ";
    var resultSearchBy = searchBy != " " && searchBy != undefined ? `&searchBy=${searchBy}` : " ";

    var action = getCurrentAction();
    var controller = getCurrentController(action);

    window.location.href = `/${controller}/${action}?page=1&sorting=${sorting}${resultSelectedProducts}${resultSelectedPrice}${resultSelectedSizes}${resultSearchBy}`
}

function callOnChangeSizes(element, sorting, searchBy) {
    var checkboxValue = element.getAttribute('value');
    var selectedSizes = getSelectedSizes();

    if (selectedSizes.includes(checkboxValue)) {

        selectedSizes = selectedSizes.filter(size => size !== checkboxValue);
    } else {

        selectedSizes.push(checkboxValue);
    }

    setSelectedSizes(selectedSizes);

    var selectedProducts = getSelectedProducts();
    var selectedSizes = getSelectedSizes();
    var selectedPrice = localStorage.getItem("selectedPrice");

    var resultSelectedProducts = selectedProducts.length != 0 ? `&selectedProducts=${selectedProducts.join(',')}` : " ";
    var resultSelectedPrice = selectedPrice != " " ? `&selectedPrice=${selectedPrice}` : " ";
    var resultSelectedSizes = selectedSizes.length != 0 ? `&selectedSizes=${selectedSizes.join(',')}` : " ";
    var resultSearchBy = searchBy != " " && searchBy != undefined ? `&searchBy=${searchBy}` : " ";

    var action = getCurrentAction();
    var controller = getCurrentController(action);

    window.location.href = `/${controller}/${action}?page=1&sorting=${sorting}${resultSelectedProducts}${resultSelectedPrice}${resultSelectedSizes}${resultSearchBy}`
}

function getSelectedSizes() {
    var storedSizes = localStorage.getItem('selectedSizes');
    return storedSizes ? storedSizes.split(',') : [];
}

function setSelectedSizes(selectedSizes) {
    localStorage.setItem('selectedSizes', selectedSizes.join(','));
}

function callOnChangePrices(element, sorting, searchBy) {

    if (localStorage.getItem("selectedPrice") != " ") {
        localStorage.setItem('selectedPrice', " ");
    }
    else {

        var checkboxValue = element.getAttribute('value');
        localStorage.setItem("selectedPrice", checkboxValue)
    }


    var selectedProducts = getSelectedProducts();
    var selectedSizes = getSelectedSizes();
    var selectedPrice = localStorage.getItem("selectedPrice");

    var resultSelectedProducts = selectedProducts.length != 0 ? `&selectedProducts=${selectedProducts.join(',')}` : " ";
    var resultSelectedPrice = selectedPrice != " " ? `&selectedPrice=${selectedPrice}` : " ";
    var resultSelectedSizes = selectedSizes.length != 0 ? `&selectedSizes=${selectedSizes.join(',')}` : " ";
    var resultSearchBy = searchBy != " " && searchBy != undefined ? `&searchBy=${searchBy}` : " ";

    var action = getCurrentAction();
    var controller = getCurrentController(action);

    window.location.href = `/${controller}/${action}?page=1&sorting=${sorting}${resultSelectedProducts}${resultSelectedPrice}${resultSelectedSizes}${resultSearchBy}`
}

function getSelectedProducts() {
    var storedProducts = localStorage.getItem('selectedProducts');
    return storedProducts ? storedProducts.split(',') : [];
}

function setSelectedProducts(selectedProducts) {
    localStorage.setItem('selectedProducts', selectedProducts.join(','));
}

function callChangeProducts(element, sorting, searchBy) {

    var checkboxValue = element.getAttribute('value');
    var selectedProducts = getSelectedProducts();

    if (selectedProducts.includes(checkboxValue)) {

        selectedProducts = selectedProducts.filter(product => product !== checkboxValue);
    } else {

        selectedProducts.push(checkboxValue);
    }

    setSelectedProducts(selectedProducts);

    var selectedSizes = getSelectedSizes();
    var selectedPrice = localStorage.getItem("selectedPrice");

    var resultSelectedProducts = selectedProducts.length != 0 ? `&selectedProducts=${selectedProducts.join(',')}` : " ";
    var resultSelectedPrice = selectedPrice != " " ? `&selectedPrice=${selectedPrice}` : " ";
    var resultSelectedSizes = selectedSizes.length != 0 ? `&selectedSizes=${selectedSizes.join(',')}` : " ";
    var resultSearchBy = searchBy != " " && searchBy != undefined ? `&searchBy=${searchBy}` : " ";

    var action = getCurrentAction();
    var controller = getCurrentController(action);

    window.location.href = `/${controller}/${action}?page=1&sorting=${sorting}${resultSelectedProducts}${resultSelectedPrice}${resultSelectedSizes}${resultSearchBy}`

}


document.addEventListener('DOMContentLoaded', function () {
    var selectedProducts = getSelectedProducts();
    selectedProducts.forEach(function (product) {
        var anchorProduct = document.querySelector('a[value="' + product + '"]');
        if (anchorProduct) {
            anchorProduct.parentNode.classList.add('active');
        }
    });

    var selectedSizes = getSelectedSizes();

    selectedSizes.forEach(function (size) {
        var anchorSize = document.querySelector('a[value="' + size + '"]');
        if (anchorSize) {
            anchorSize.parentNode.classList.add('active');
        }
    });

    var price = localStorage.getItem("selectedPrice");
    var anchorPrice = document.querySelector('a[value="' + price + '"]');

    if (anchorPrice) {
        anchorPrice.parentNode.classList.add('active');
    }
});

function getCurrentAction() {
    var currentLocation = window.location.href;
    var action = " ";

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
    var controller = " ";

    if (action === "ProductsByQuery") {
        controller = "Search"
    } else {
        controller = "Products"
    }

    return controller;
}