 $(document).ready(function () {
     
     /*
      * Filter checkbox
      */
     
    $(".mg-filter-checkbox ul li").click(function () {
        $(this).toggleClass("active");
    });
    $(".mg-filter-nav-btn").click(function (event) {
        event.preventDefault();
        $(".mg-filter-outer").addClass("mg-filter-open");
    });
    $(".mg-filter-nav-close-btn").click(function (event) {
        event.preventDefault();
        $(".mg-filter-outer").removeClass("mg-filter-open");
    });
    $(".mg-sidenav-overlay").click(function (event) {
        event.preventDefault();
        $(".mg-filter-outer").removeClass("mg-filter-open");
    });
    
    /*
     * Sorting Bar
     */
    
    $(".mg-html-sorting-bar").click(function () {
        $(".mg-html-sorting-options-pannel").toggle();
    });
});


