// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).on('click', '.btn-add-to-cart', function () {
    const productId = $(this).data('code');

    $.ajax({
        url: '/Cart/AddToCart',
        method: 'POST',
        data: ({ productId: productId, quantity: 1 }),
        success: function (count) {
            if (count < 0) {
                showToaster('Error', 'Add to cart fail ')
            }
           
            showToaster('Success', 'Add to cart succesful')
        }
    });
});