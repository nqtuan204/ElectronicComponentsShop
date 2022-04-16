// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function formattedPrice(price) {
    let i = 0;
    priceString = price.toString();
    while (priceString.length - (i + 3) >= 1) {
        i += 3;
        priceString = priceString.slice(0, priceString.length - i) + '.' + priceString.slice(priceString.length - i);
        i++;
    }
    return priceString + 'đ';
}


var getCart = () => {
    console.log('get cart!');
    var cart = {
        items: []
    }

    fetch(`/Cart/Get`).then(re => re.json()).then(data => {
        console.log(data);
        cart = data;
        let total = 0;
        total = cart.items.map(item => item.quantity).reduce((sum = 0, quantity) => sum + quantity);
        document.getElementById('cart-item-quantity').innerHTML = total;
        renderCart(cart.items);
    });
};


var renderCart = (items) => {
    var cartItems = document.getElementById('cart-details');
    cartItems.innerHTML = '';
    for (let item of items) {
        cartItems.innerHTML += `<div class="product-widget">
    <div class="product-img">
        <img src="${item.productThumbnailURL}" alt="" />
    </div>
    <div class="product-body">
        <h3 class="product-name"><a href="${item.productURL}">${item.productName}</a></h3>
        <h4 class="product-price"><span class="qty">${item.quantity}x</span>${formattedPrice(item.price)}</h4>
    </div>
    <button class="delete"><i class="fa fa-close"></i></button>
</div>`
    }
}


// Mẫu html CartItem
/*<div class="product-widget">
    <div class="product-img">
        <img src="" alt="" />
    </div>
    <div class="product-body">
        <h3 class="product-name"><a href="#">product name goes here</a></h3>
        <h4 class="product-price"><span class="qty">1x</span>$980.00</h4>
    </div>
    <button class="delete"><i class="fa fa-close"></i></button>
</div>*/