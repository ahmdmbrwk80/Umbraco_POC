//$(function () {
//    var loc = window.location.href; // returns the full URL
//    if (/en/.test(loc)) {
//        $('body').addClass('en');
//    }
//    else if(/ar/.test(loc)) {
//        $('body').addClass('ar');
//    }
//});


//$(function () {
//    var loc = window.location.href; // returns the full URL
//    if (/home-page/.test(loc)) {
//        $('body').addClass('home');
//    }
//});



///* Home sections */
//$('#header').load('header.html');
//$('#homeSlider').load('home-slider.html');
//$('#footer').load('footer.html');


$("button").each(function () {
    var o = $(this).attr('data-target');
    var c = o.replaceAll(' ', '');
    $(this).attr('data-target', c);
})

$(".tab-pane,.collapse[data-parent='#accordion']").each(function () {
    var h = $(this).attr('id');
    var j = h.replaceAll(' ', '');
    $(this).attr('id', j);
})

//$('.nav-tabs button:first-child').addClass('active');
//$('.tab-content .tab-pane:first-child').addClass('active show');










/* gallery */

$(document).ready(function () {
    var bigimage = $("#big");
    var thumbs = $("#thumbs");
    //var totalslides = 10;
    var syncedSecondary = true;

    bigimage
        .owlCarousel({
            items: 1,
            slideSpeed: 2000,
            nav: true,
            // autoplay: true,
            dots: false,
            loop: true,
            responsiveRefreshRate: 200,
            navText: [
                '<span class="icon-arrow-left2"></span>',
                '<span class="icon-arrow-right2"></span>'
            ]
        })
        .on("changed.owl.carousel", syncPosition);

    thumbs
        .on("initialized.owl.carousel", function () {
            thumbs
                .find(".owl-item")
                .eq(0)
                .addClass("current");
        })
        .owlCarousel({
            items: 4,
            dots: true,
            nav: false,
            smartSpeed: 200,
            slideSpeed: 500,
            slideBy: 4,
            responsiveRefreshRate: 100
        })
        .on("changed.owl.carousel", syncPosition2);

    function syncPosition(el) {
        //if loop is set to false, then you have to uncomment the next line
        //var current = el.item.index;

        //to disable loop, comment this block
        console.log(el);
        var count = el.item.count - 1;
        var current = Math.round(el.item.index - el.item.count / 2 - 0.5);

        if (current < 0) {
            current = count;
        }
        if (current > count) {
            current = 0;
        }
        //to this
        thumbs
            .find(".owl-item")
            .removeClass("current")
            .eq(current)
            .addClass("current");
        var onscreen = thumbs.find(".owl-item.active").length - 1;
        console.log(onscreen)
        var start = thumbs
            .find(".owl-item.active")
            .first()
            .index();
        var end = thumbs
            .find(".owl-item.active")
            .last()
            .index();
        console.log(end);
        if (current > end) {
            thumbs.data("owl.carousel").to(current, 100, true);
        }
        if (current < start) {
            thumbs.data("owl.carousel").to(current - onscreen, 100, true);
        }
    }

    function syncPosition2(el) {
        if (syncedSecondary) {
            var number = el.item.index;
            bigimage.data("owl.carousel").to(number, 100, true);
        }
    }

    thumbs.on("click", ".owl-item", function (e) {
        e.preventDefault();
        var number = $(this).index();
        bigimage.data("owl.carousel").to(number, 300, true);
    });
});

/* End of Gallery */