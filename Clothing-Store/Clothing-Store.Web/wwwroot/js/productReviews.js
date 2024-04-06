function bindRazorProperties(productId, userFullName, userProfileImageUrl) {

    $(document).on('click', '#post-review', function (event) {
        event.preventDefault();

        var data = $(".mg-form").serialize() +
            "&productId=" + productId +
            "&userFullName=" + userFullName +
            "&userProfileImageUrl=" + userProfileImageUrl;

        console.log(data)
        $.ajax({
            type: 'POST',
            url: '/Products/PostProductReview',
            contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
            data: data,
            success: function (result) {
                console.log(result)
                var newReviewHtml = `
    <div class="mg-customer-review-panel pt-5">
        <div class="row align-items-center">
            <div class="col-9">
                <div class="mg-customer-review-user-details-outer">
                    <div class="mg-customer-review-user-detail-image">
                        <img src="${result.userProfileImageUrl == null ? '/images/pngs/profile.png' : result.userProfileImageUrl}" /> <!-- Consider dynamically setting this based on user data -->
                    </div>
                    <div class="mg-customer-review-user-details">
                        <div class="mg-customer-username">${DOMPurify.sanitize(result.userFullName)}</div>
                        <div class="mg-rating-outer">
                            <div class="mg-ratings-full">
                                <i class="fa fa-star" aria-hidden="true"></i>
                                <i class="fa fa-star" aria-hidden="true"></i>
                                <i class="fa fa-star" aria-hidden="true"></i>
                                <i class="fa fa-star" aria-hidden="true"></i>
                                <i class="fa fa-star" aria-hidden="true"></i>

                                <span class="mg-ratings" style="width: ${result.rating * 20}%;">
                                    <i class="fa fa-star" aria-hidden="true"></i>
                                    <i class="fa fa-star" aria-hidden="true"></i>
                                    <i class="fa fa-star" aria-hidden="true"></i>
                                    <i class="fa fa-star" aria-hidden="true"></i>
                                    <i class="fa fa-star" aria-hidden="true"></i>
                                </span>
                            </div>

                            <div class="mg-ratings-full">
                                <!-- Dynamically generate rating stars based on the review rating -->
                            </div>
                        </div>
                        <div class="mg-rating-count">( ${DOMPurify.sanitize(result.rating.toFixed(1))} )</div>
                    </div>
                </div>
            </div>
            <div class="col-3 text-end">
                <div class="mg-user-review-date">преди 0 секунди</div>
            </div>
        </div>
        <div class="mg-customer-review-text mt-3">
            <p>${DOMPurify.sanitize(result.message)}</p>
        </div>
    </div>
    `;

                $('.mg-customers-reviews-outer').append(newReviewHtml);
                $("#message").val('');
                $(".mg-review-star-rating-form-inner input[type='radio'][name='Rating']").prop('checked', false);
            },
            error: function (xhr, status, error) {

                console.log(xhr.responseText);
                var response = JSON.parse(xhr.responseText);

                var form = $('#form-review');
                form.find('.text-danger').each(function () {
                    $(this).text('');
                });

                form.find("span[data-valmsg-for='Message']").text(response[0]).show();
            }
        });
    })

    $(document).on('click', '.mg-see-more-reviews-btn', function (event) {
        event.preventDefault();
        var nextPageNumber = $(this).data('pageNumber');
       // var productId = '@Model.Id';

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

    $(document).on('click', '#write-review', function (event) {
        var currentUrl = window.location.href;
        var baseUrl = window.location.origin;
        var relativeUrl = currentUrl.replace(baseUrl, '');
        var returnUrl = encodeURIComponent(relativeUrl);

        $.ajax({
            type: "GET",
            url: '/Products/IsUserLogin',
            success: function (response) {
                if (!response.isLoggedIn) {
                    $('.mg-write-review-pannel-outer').hide();
                    window.location.href = `/Identity/Account/Login?returnUrl=${returnUrl}`;
                }
            },
            error: function (xhr, status, error) {
                console.log(xhr.response.Text)
            }
        });

    });
}