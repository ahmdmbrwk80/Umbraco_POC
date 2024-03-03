$('.owl-carousel-home').owlCarousel({
    loop: true,
    margin: 10,
    nav: false,
    responsive: {
        0: {
            items: 1
        },
        600: {
            items: 1
        },
        1000: {
            items: 1
        }
    }
});


$(document).on('click', '.dropdown-menu', function (e) {
    e.stopPropagation();
});


$(function () {
    //caches a jQuery object containing the header element
    var header = $(".onScrollF");
    $(window).scroll(function () {
        var scroll = $(window).scrollTop();

        if (scroll >= 175) {
            header.addClass("fixed");
        } else {
            header.removeClass("fixed");
        }
    });
});




$('.owl-feeds').owlCarousel({
    loop: true,
    margin: 20,
    nav: false,
    dots: true,
    stagePadding: 30,
    responsive: {
        0: {
            items: 1
        },
        600: {
            items: 2
        },
        1000: {
            items: 4
        }
    }
})



