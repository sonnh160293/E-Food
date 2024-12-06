(function () {

    function initial() {
        registerEvents();
    }

    function registerEvents() {
        // Use 'input' event to update total immediately when quantity changes
        $(document).on('input', '.txt-quantity', function () {
            const self = $(this);
            const parentDiv = self.closest('div').parent().closest('div');
            const price = parseInt(parentDiv.find('.txt-price').text().replaceAll('.', ''));
            const quantity = parseInt(self.val());

            if (isNaN(quantity) || quantity < 0) {
                return; // Skip if the quantity is invalid
            }

            const total = price * quantity;
            parentDiv.find('.txt-total').text(total.toString().replace(/\B(?=(\d{3})+(?!\d))/g, "."));

            updateTotalCart();
        });
    }

    function updateTotalCart() {
        let totalCart = 0;

        $('.txt-total').each(function () {
            const itemTotal = parseInt($(this).text().replaceAll('.', ''));
            if (!isNaN(itemTotal)) {
                totalCart += itemTotal;
            }
        });

        $('.txt-total-cart').text(totalCart.toString().replace(/\B(?=(\d{3})+(?!\d))/g, "."));
    }

    initial();

    document.addEventListener('DOMContentLoaded', function () {
        document.getElementById('btnSaveCart').addEventListener('click', function () {
            // Array to store product details
            let products = [];

            // Loop through each quantity input field
            document.querySelectorAll('.txt-quantity').forEach(function (input) {
                let quantity = parseInt(input.value) || 0;
                let productId = input.getAttribute('data-productId');

                // Find the corresponding hidden input field to get the id
                let idInput = input.closest('div').querySelector('input[type=hidden][name=id]');
                let id = idInput ? idInput.getAttribute('data-id') : null;

                if (quantity > 0) {
                    products.push({
                        id: id,            // Add the id to the object
                        productId: productId,
                        quantity: quantity
                    });
                }
            });

            $.ajax({
                url: 'Cart/Update',
                method: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(products),
                success: function (response) {
                    if (response) {
                        showToaster('Success', "Save cart successfull")
                    }
                }
            })
        });

       
    });

    $(document).ready(function () {
        $('.btn-delete').on('click', function (e) {
            e.preventDefault(); // Prevent the default action of the button

            // Get the product ID
            var productId = $(this).closest('div.row').find('input[name="quantity"]').data('productid');

            // Confirm the delete action
            if (confirm('Are you sure you want to remove this item from your cart?')) {
                $.ajax({
                    type: 'POST',
                    url: 'Cart/Delete', // Adjust the URL to your controller and action method
                    data: { productId: productId },
                    success: function (response) {
                        if (response) {
                            // Remove the cart item from the DOM
                            $(e.target).closest('.row').remove();
                            // Optionally, update the total cart amount
                            updateCartTotal();
                        } else {
                            alert('Failed to remove the item. Please try again.');
                        }
                    },
                    error: function () {
                        alert('An error occurred. Please try again.');
                    }
                });
            }
        });

        // Function to update the total cart amount
        function updateCartTotal() {
            var totalCart = 0;
            $('.txt-total').each(function () {
                totalCart += parseFloat($(this).text().replace(/,/g, ''));
            });
            $('#totalCart').text(totalCart.toLocaleString('vi-VN'));
        }
    });


    //total check
    $(document).ready(function () {
        $('.item-checkbox').change(function () {
            updateCartSummary();
        });

        function updateCartSummary() {
            let checkedItems = 0;
            let totalPrice = 0;

            $('.item-checkbox:checked').each(function () {
                checkedItems++;
                totalPrice += parseFloat($(this).data('price'));
            });

            $('#checkedItemsCount').text(checkedItems);
            $('#totalCheckedItems').text(checkedItems);
            $('#checkedTotalPrice').text(totalPrice.toLocaleString('vi-VN'));
        }
    });

    //submit checked item
    document.addEventListener("DOMContentLoaded", function () {
        const checkoutButton = document.querySelector(".btn-checkout");
        const cartForm = document.createElement("form");

        checkoutButton.addEventListener("click", function () {
            const checkedItems = document.querySelectorAll(".item-checkbox:checked");
            const ids = Array.from(checkedItems).map(item => item.closest('.row').querySelector('input[name="id"]').dataset.id);

            // Add checked item IDs to form as hidden inputs
            ids.forEach(id => {
                const hiddenInput = document.createElement("input");
                hiddenInput.type = "hidden";
                hiddenInput.name = "checkedItemIds";
                hiddenInput.value = id;
                cartForm.appendChild(hiddenInput);
            });

            cartForm.action = "/Order/Checkout";
            cartForm.method = "GET";
            document.body.appendChild(cartForm);
            cartForm.submit();
        });
    });


})();
