$(function () {
    const $sidebar = $('#cart-sidebar');
    if (!$sidebar.length) return;

    const restaurantId = $sidebar.data('restaurant-id');
    const token = $('input[name="__RequestVerificationToken"]').first().val();

    function loadCart() {
        $.get('/Cart/Sidebar', { restaurantId: restaurantId })
            .done(html => $sidebar.html(html));
    }

    function send(action, dishId) {
        $.ajax({
            url: '/Cart/' + action,
            type: 'POST',
            data: { dishId: dishId, restaurantId: restaurantId },
            headers: { 'RequestVerificationToken': token }
        })
            .done(html => $sidebar.html(html))
            .fail(() => alert('Не удалось обновить корзину'));
    }

    $(document).on('click', '.js-cart-add', function () {
        send('Add', $(this).data('dish-id'));
    });
    $(document).on('click', '.js-cart-decrease', function () {
        send('Decrease', $(this).data('dish-id'));
    });
    $(document).on('click', '.js-cart-remove', function () {
        send('Remove', $(this).data('dish-id'));
    });

    loadCart();
});

