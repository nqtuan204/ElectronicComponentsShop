// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

//const { param } = require("jquery");

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

var items = [];
var favProductIds = [];

function getItems() {
    if (document.cookie.includes('token')) {
        fetch(`/Cart/GetItems`).then(re => re.json()).then(data => {
            items = data;
            renderCart(items);
        });
        getFavList();
    }
};

function getFavList() {
    fetch(`/User/GetFavouriteProductIds`).then(re => re.json()).then(data => { favProductIds = data; setFavList(); });
}

function setFavList() {
    console.log('set FavList to' + favProductIds.length);
    document.getElementById('wish-list').innerHTML = favProductIds.length;
}

function addToFavList(productId) {
    if (!favProductIds.includes(productId)) {
        fetch(`/User/AddToFavourites/${productId}`).then(re => re.json()).then(data => {
            favProductIds.push(productId); setFavList();
        }).catch(re => window.location.href = '/User/Login');
    }
    else {
        fetch(`/User/RemoveFromFavourites/${productId}`).then(re => re.json()).then(data => {
            favProductIds = favProductIds.filter(i => i != productId); setFavList();
        }).catch(re => window.location.href = '/User/Login');
    }
}

var renderCart = (items) => {
    if (items.length > 0) {
        let total = 0;
        total = items.map(item => parseInt(item.quantity)).reduce((sum = 0, quantity) => sum + quantity);
        document.getElementById('cart-item-quantity').innerHTML = total;
        document.getElementById('cart-buttons').style.display = 'block';
        var cartItems = document.getElementById('cart-item-list');
        cartItems.innerHTML = '';
        for (let item of items) {
            cartItems.innerHTML += `<div class="product-widget" id="cart-item-${item.productId}">
    <div class="product-img">
        <img src="${item.productThumbnailURL}" alt="" />
    </div>
    <div class="product-body">
        <h3 class="product-name"><a href="${item.productURL}">${item.productName}</a></h3>
        <h4 class="product-price"><span class="qty">${item.quantity}x</span>${formattedPrice(item.price)}</h4>
    </div>
    <button class="delete" onclick="removeItem(${item.productId})"><i class="fa fa-close"></i></button>
</div>`
        }

        let cartTotal = document.getElementById('cart-total');
        cartTotal.innerHTML = `<small>Số sản phẩm: ${totalQuantity(items)}</small>
                                    <h5>SUBTOTAL: ${amount(items)}</h5>`
        if (window.location.href.includes('Cart') || window.location.href.includes('cart'))
            renderCartTable();
    }
}

function totalQuantity(items) {
    return items.map(item => parseInt(item.quantity)).reduce((sum = 0, quantity) => sum + quantity);
}

function amount(items) {
    return formattedPrice(items.map(item => item.quantity * item.price).reduce((sum = 0, el) => sum + el));
}

function clearCart() {
    items = [];
    var cartItems = document.getElementById('cart-item-list');
    cartItems.innerHTML = '';
    let cartTotal = document.getElementById('cart-total');
    cartTotal.innerHTML = 'Không có sản phẩm nào.';
    document.getElementById('cart-buttons').style.display = 'none';
    fetch('/Cart/Clear');
}

function removeItem(productId) {
    if (items.length == 1)
        clearCart();
    else {
        document.getElementById(`cart-item-${productId}`).remove();
        items = items.filter(item => item.productId != productId);
        renderCart(items);
        fetch(`/Cart/RemoveAll/${productId}`);
    }
    if (window.location.href.includes('Cart') || window.location.href.includes('cart'))
        renderCartTable();
}

function addToCart(productId, quantity) {
    fetch(`/Cart/AddItem/${productId}?quantity=${quantity}`).then(re => re.json()).then(data => {
        items = data;
        renderCart(items);
    }).catch(re => window.location.href = '/User/Login');
}

function addMultiple(productId) {
    let quantity = parseInt(document.getElementById('product-details-quantity').value);
    console.log(`add ${quantity} SP ${productId}`);
    addToCart(productId, quantity);
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
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

//
var temp_items = [];

function clearCartTable() {
    document.getElementById('cart-table-footer').innerHTML = '';
    document.getElementById('cart-table').innerHTML = 'Không có sản phẩm nào';
}

function renderCartTable() {
    clearCartTable();
    var cartTable = document.getElementById('cart-table');
    if (items.length > 0) {
        temp_items = items.map(el => el);
        setTempAmount(temp_items);
        cartTable.innerHTML = '';
        for (item of temp_items) {
            cartTable.innerHTML += `<div class="row" id="cart-table-line-${item.productId}">
                        <div class="col-xs-2">
                            <img class="img-responsive" src="${item.productThumbnailURL}">
                        </div>
						<div class="col-xs-4">
                            <h4 class="product-name"><strong>${item.productName}</strong ></h4 >
                        </div>
                        <div class="col-xs-6">
                            <div class="col-xs-6 text-right">
                                <h6><strong>${formattedPrice(item.price)} <span class="text-muted">x</span></strong></h6>
                            </div>
                            <div class="col-xs-4">
                                <input type="number" class="form-control input-sm" value="${item.quantity}" min="1" onchange="updateCartTable(${item.productId})" id="cart-table-input-${item.productId}">
                            </div>
                            <div class="col-xs-2">
                                <button type="button" class="btn btn-link btn-xs" onclick="removeLine(${item.productId})">
                                    <span class="glyphicon glyphicon-trash"> </span>
                                </button>
                            </div>
                        </div>
                    </div><hr id="margin-line-${item.productId}">`;
        }
        cartTable.innerHTML += `<div class="row">
						<div class="text-center">
							<div class="col-xs-9">
								<h6 class="text-right"> </h6>
							</div>
							<div class="col-xs-3">
								<button type="button" class="btn btn-default btn-sm btn-block" onclick="updateCart()">
									Áp dụng
								</button>
							</div>
						</div>
					</div>`;
    }
}

function setTempAmount(temp_items) {
    document.getElementById('cart-table-footer').innerHTML = `<div class="row text-center">
                        <div class="col-xs-9">
                            <h4 class="text-right">Tổng cộng <strong>${amount(temp_items)}</strong></h4>
                        </div>
                        <div class="col-xs-3">
                            <button type="button" class="btn btn-success btn-block" onclick="window.location.href='/Checkout'">
                                Checkout
                            </button>
                        </div>
                    </div>`
}

function removeLine(productId) {
    temp_items = temp_items.filter(i => i.productId != productId);
    if (temp_items.length == 0)
        clearCartTable();
    else {
        document.getElementById(`cart-table-line-${productId}`).remove();
        document.getElementById(`margin-line-${productId}`).remove();
        setTempAmount(temp_items);
    }
}

function updateCartTable(productId) {
    let item = temp_items.filter(i => i.productId == productId)[0];
    item.quantity = document.getElementById(`cart-table-input-${productId}`).value;
    setTempAmount(temp_items);
}

function updateCart() {
    items = temp_items.map(x => x);
    if (items.length == 0) {
        clearCart();
    }
    else {
        renderCart(items);
        var cart = {
            items: temp_items
        };
        fetch('/Cart/Update', {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(cart)
        }).then(re => console.log(re)).catch(re => console.log(re));
    };
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
