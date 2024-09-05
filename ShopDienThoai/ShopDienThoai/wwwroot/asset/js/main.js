import {carousel_banner} from './source.js';
carousel_banner();

import {slide_img_product} from './source.js';
slide_img_product();


//import {slider_row} from './source.js';
//slider_row('js_dienthoai', 'js_maytinhbang', 'js_dongho', 'js_phukien', 'js_laptop');

$(document).ready(function () {
    // Khi nhấn vào nút close
    $(".close").on("click", function () {
        $("#myModal").hide();
    });

    // Khi nhấn ra ngoài modal container
    $(window).on("click", function (event) {
        if ($(event.target).is("#myModal")) {
            $("#myModal").hide();
        }
    });
});


$(document).on('click', '.btn_wishlist', function () {
    var iconTag = $(this).find('i');
    if (iconTag.hasClass('fa-heart')) {
        if (iconTag.hasClass('fa-regular')) {
            iconTag.removeClass('fa-regular').addClass('fa-solid');
        } else {
            iconTag.removeClass('fa-solid').addClass('fa-regular');
        }
    }
})



$(document).ready(function () {
    // wishlist
    $.ajax({
        url: '/shopping/GetProductIdInWishList', // Thay đổi theo tên controller và action
        type: 'get',
        data: {},
        success: function (rs) {
            var productidlist = rs.productidlist;
            $('.btn_wishlist').each(function () {

                var productId = parseInt($(this).attr('product-id'));
                var iconTag = $(this).find('i');

                

                if (productidlist.includes(productId)) {
                    // Sản phẩm đã có trong danh sách yêu thích
                    iconTag.removeClass('fa-regular fa-heart').addClass('fa-solid fa-heart');
                } else {
                    // Sản phẩm chưa có trong danh sách yêu thích
                    iconTag.removeClass('fa-solid fa-heart').addClass('fa-regular fa-heart');
                }
            });
        }
    });



    var listWishListid = []
    function updateWishList() {
        return new Promise((resolve, reject) => {
            $.ajax({
                url: '/shopping/getWishListId',
                type: 'get',
                success: function (rs) {
                    listWishListid = rs.wishlistid;
                    resolve(listWishListid);
                },
                error: function () {
                    reject('Có lỗi xảy ra khi lấy danh sách yêu thích.');
                }
            });
        });
    }
    $(document).on('click', '.check_all', function () {
        if ($(this).is(':checked')) {
            updateWishList()
                .then(function () {
                    
                })
                .catch(function (error) {
                    console.error(error);
                });
        } else {
            listWishListid = []
        }
    });
    $(document).on('click', '.btn_remove', function () {
        $.ajax({
            url: '/shopping/removeallfromwishlist',
            type: 'get',
            success: function (rs) {
                
            }
        });
    });
})

