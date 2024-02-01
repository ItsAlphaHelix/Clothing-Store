$(document).ready(function(){
    
    /*
     * Product Slider
     */
    
    $('.mg-product-slider').slick({
        slidesToShow: 1,
        slidesToScroll: 1,
        arrows: false,
        fade: true,
        asNavFor: '.mg-product-slider-thumb',
        accessibility: false
    });
    $('.mg-product-slider-thumb').slick({
        slidesToShow: 5,
        slidesToScroll: 1,
        asNavFor: '.mg-product-slider',
        dots: false,
        centerMode: false,
        focusOnSelect: true
    });
    $(document).on("click", ".mg-color-variant", function(event){
        event.preventDefault();
        $(".mg-color-variant").removeClass("mg-active");
        $(this).addClass("mg-active");
    });
    
    /*
     * Review section
     */
    
    $(".mg-write-review-toggle-btn").click(function(){
        $(".mg-write-review-pannel-outer").slideToggle();
    });
    
    /*
     * Recommended products
     */
    $('.mg-you-may-like-product-carousal').slick({
        dots: false,
        infinite: false,
        speed: 300,
        slidesToShow: 4,
        slidesToScroll: 1,
        responsive: [
            {
                breakpoint: 600,
                settings: {
                    slidesToShow: 2,
                    slidesToScroll: 2
                }
            },
            {
                breakpoint: 480,
                settings: {
                    slidesToShow: 1,
                    slidesToScroll: 1
                }
            }
        ]
    });
});


