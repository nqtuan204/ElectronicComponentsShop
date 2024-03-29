﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

//const { param } = require("jquery");

// Write your JavaScript code.

getCartItems();
getFavList();
//SUMMARY CART//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
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

function cartItemquantity(items) {
    return items.map(item => parseInt(item.quantity)).reduce((sum = 0, quantity) => sum + quantity, 0);
}

function cartAmount(items) {
    return formattedPrice(items.map(item => parseInt(item.quantity) * parseInt(item.price)).reduce((sum, el) => sum + el, 0));
}

async function getCartItems() {
    let items = JSON.parse(window.sessionStorage.getItem('items'));
    if (items == null) {
        await fetch(`/Cart/GetItems`).then(re => re.json()).then(data => {
            items = data;
            if (items != null) {
                window.sessionStorage.setItem('items', JSON.stringify(items));
                renderSummaryCart(items);
            }
            else {
                items = [];
                window.sessionStorage.setItem('items', JSON.stringify(items));
            }
                
        }).catch(er => null);
        
    }
    else
        renderSummaryCart(items);
};

function renderSummaryCartItem(item) {
    let template =
        `<div class="product-widget" id="summary-cart-item-${item.productId}">
        <div class="product-img">
            <img src="${item.productThumbnailURL}" alt="" />
        </div>
        <div class="product-body">
            <h3 class="product-name"><a href="${item.URL}">${item.productName}</a ></h3 >
            <h4 class="product-price"><span class="qty">${item.quantity}x</span>${formattedPrice(item.price)}</h4>
        </div>
        <button class="delete" onclick="removeSummaryCartItem(${item.productId})"><i class="fa fa-close"></i></button>
    </div>`;
    return template;
}

function renderSummaryCartItems(items) {
    let list = '';
    for (let item of items)
        list += renderSummaryCartItem(item);
    return list;
}

function updateSummaryCartQuantity(items) {
    if (items.length == 0)
        document.getElementById('cart-item-quantity').innerHTML = 0;
    else
        document.getElementById('cart-item-quantity').innerHTML = cartItemquantity(items);
}

function renderSummaryCart(items) {
    let cart = document.getElementsByClassName('cart-dropdown')[0];
    if (items.length == 0) {
        updateSummaryCartQuantity(items);
        cart.innerHTML = document.getElementById('empty-summary-cart').innerHTML;
    }
    else {
        updateSummaryCartQuantity(items);
        cart.innerHTML =
            `<div class="cart-list" id="cart-item-list">
            ${renderSummaryCartItems(items)}
        </div>
        <div class="cart-summary">
			<small>${cartItemquantity(items)} sản phẩm</small>
			<h5>Tổng cộng: ${cartAmount(items)}</h5>
		</div>
        <div class="cart-btns" id="cart-buttons">
            <a href="/Cart">Xem giỏ hàng</a>
            <a href="/CheckOut">Thanh toán  <i class="fa fa-arrow-circle-right"></i></a>
        </div>`;
    }
    if (window.location.href.toLowerCase().includes('cart'))
        renderFullCart(items);
}

function removeSummaryCartItem(productId) {
    let items = JSON.parse(window.sessionStorage.getItem('items'));
    items = items.filter(i => i.productId != productId);
    renderSummaryCart(items);
    fetch(`/Cart/RemoveAll/${productId}`).then(re => window.sessionStorage.setItem('items', JSON.stringify(items))).catch(er => {
        window.sessionStorage.removeItem('items');
        window.location.href = '/User/Login';
    });
}

function addToCart(productId, quantity) {
    let items = JSON.parse(window.sessionStorage.getItem('items'));
    if (items != null) {
        fetch(`/Cart/AddItem/${productId}?quantity=${quantity}`).then(re => re.json()).then(data => {
            items = data;
            renderSummaryCart(items);
            window.sessionStorage.setItem('items', JSON.stringify(items));
        }).catch(re => {
            window.location.href = '/User/Login';
            window.sessionStorage.removeItem('items');
        });
    }
    else
        window.location.href = '/User/Login';
}

//FULL CART//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function renderFullCartItem(item) {
    let template =
        `<div class="row" id="full-cart-item-${item.productId}">
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
                <input type="number" class="form-control input-sm" value="${item.quantity}" min="1" id="full-cart-input-${item.productId}" onchange="changeQuantity(${item.productId})">
            </div>
            <div class="col-xs-2">
                <button type="button" class="btn btn-link btn-xs" onclick="removeFullCartItem(${item.productId})">
                    <span class="glyphicon glyphicon-trash"> </span>
                </button>
            </div>
        </div>
    </div>
    <hr id="margin-line-${item.productId}">`;
    return template;
}

function renderFullCartItems(items) {
    if (items.length == 0)
        return '';
    return items.reduce((template, item) => template + renderFullCartItem(item), '');
}

function renderFullCart(items) {
    let cart = document.getElementById('full-cart');
    if (items.length == 0)
        cart.innerHTML = document.getElementById('empty-full-cart').innerHTML;
    else {
        temp_items = items.map(el => el);
        cart.innerHTML = renderFullCartItems(items) + document.getElementById('update-cart-button').innerHTML;
        updateFullCartAmount();
    }
}

function changeQuantity(productId) {
    let item = temp_items.filter(i => i.productId == productId)[0];
    item.quantity = document.getElementById(`full-cart-input-${productId}`).value;
    updateFullCartAmount();
}

function updateFullCartAmount() {
    if (temp_items == 0)
        document.getElementById('full-cart-footer').innerHTML = '';
    else {
        document.getElementById('full-cart-footer').innerHTML =
            `<div class="row text-center">
            <div class="col-xs-9">
                <h4 class="text-right">Tổng cộng <strong>${cartAmount(temp_items)}</strong></h4>
            </div>
            <div class="col-xs-3">
                <button type="button" class="btn btn-success btn-block" onclick="window.location.href='/Checkout'">
                    Checkout
                </button>
            </div>
        </div>`
    }
}

function removeFullCartItem(productId) {
    temp_items = temp_items.filter(i => i.productId != productId);
    if (temp_items.length == 0)
        document.getElementById('full-cart').innerHTML = document.getElementById('empty-full-cart').innerHTML;
    else {
        document.getElementById(`full-cart-item-${productId}`).remove();
        document.getElementById(`margin-line-${productId}`).remove();
    }
    updateFullCartAmount();
}

function updateFullCart(productId) {
    console.log(temp_items);
    let item = temp_items.filter(i => i.productId == productId)[0];
    item.quantity = document.getElementById(`full-cart-input-${productId}`).value;
    updateFullCartAmount();
}

function updateCart() {
    items = temp_items.map(x => x);
    if (items.length == 0) {
        clearCart();
    }
    else {
        renderSummaryCart(items);
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

//FAV LIST/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

async function getFavList() {
    let favProductIds = JSON.parse(window.sessionStorage.getItem('favProductIds'));
    if (favProductIds != null) {
        document.getElementById('wish-list').innerHTML = favProductIds.length;
        window.sessionStorage.setItem('favProductIds', JSON.stringify(favProductIds));
    }
    else {
        favProductIds = await fetch(`/User/GetFavouriteProductIds`).then(re => re.json()).then(data => data).catch(er => null);
        if (favProductIds != null) {
            document.getElementById('wish-list').innerHTML = favProductIds.length;
            window.sessionStorage.setItem('favProductIds', JSON.stringify(favProductIds));
        }
    }
}

function addToFavList(productId) {
    let favProductIds = JSON.parse(window.sessionStorage.getItem('favProductIds'));
    if (favProductIds == null)
        window.location.href = '/User/Login';
    else {
        if (!favProductIds.includes(productId)) {
            favProductIds.push(productId);
            document.getElementById('wish-list').innerHTML = favProductIds.length;
            fetch(`/User/AddToFavourites/${productId}`).then(re => re.json()).then(data => {
                window.sessionStorage.setItem('favProductIds', JSON.stringify(favProductIds));
            }).catch(re => window.location.href = '/User/Login');
        }
        else {
            favProductIds = favProductIds.filter(i => i != productId);
            document.getElementById('wish-list').innerHTML = favProductIds.length;
            fetch(`/User/RemoveFromFavourites/${productId}`).then(re => re.json()).then(data => {
                window.sessionStorage.setItem('favProductIds', JSON.stringify(favProductIds));
            }).catch(re => {
                window.location.href = '/User/Login';
                window.sessionStorage.removeItem('favProductIds');
            });
        }
    }
}

//CHECKOUT//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
var locals = [];
fetch('/local.json').then(re => re.json()).then(data => {
    locals = data;
    document.getElementById('ProvinceId').innerHTML = '<option value="" disabled selected>Tỉnh/thành phố</option>';
    document.getElementById('DistrictId').innerHTML = '<option value="" disabled selected>Quận/huyện</option>';
    document.getElementById('WardId').innerHTML = '<option value="" disabled selected>Phường/xã</option>';
    for (let province of locals)
        document.getElementById('ProvinceId').innerHTML += `<option value="${province.id}">${province.name}</option>`
});

function selectDistricts() {
    document.getElementById('DistrictId').innerHTML = '<option value="" disabled selected>Quận/huyện</option>';
    document.getElementById('WardId').innerHTML = '<option value="" disabled selected>Phường/xã</option>';
    let province = locals.filter(p => p.id == document.getElementById('ProvinceId').value)[0];
    for (let district of province.districts)
        document.getElementById('DistrictId').innerHTML += `<option value="${district.id}">${district.name}</option>`;
}

function selectWards() {
    document.getElementById('WardId').innerHTML = '<option value="" disabled selected>Phường/xã</option>';
    let province = locals.filter(p => p.id == document.getElementById('ProvinceId').value)[0];
    let district = province.districts.filter(d => d.id == document.getElementById('DistrictId').value)[0];
    for (let ward of district.wards)
        document.getElementById('WardId').innerHTML += `<option value="${ward.id}">${ward.name}</option>`;
}

//REVIEW FORM//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
function postReview(productId) {
    let content = document.getElementById('review-content');
    let score = 1;
    document.getElementsByName('Score').forEach(e => {
        if (e.checked)
            score = e;
    });
    let newReview = {
        Score: score.value,
        Content: content.value,
        ProductId: productId
    };
    console.log(newReview);
    fetch('/Product/CreateReview', {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(newReview)
    }).then(re => {
        score.checked = false;
        content.value = null;
    });
}

//USER-PROFILE//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
function htmlbodyHeightUpdate() {
    var height3 = $(window).height()
    var height1 = $('.nav').height() + 50
    height2 = $('.main').height()
    if (height2 > height3) {
        $('html').height(Math.max(height1, height3, height2) + 10);
        $('body').height(Math.max(height1, height3, height2) + 10);
    }
    else {
        $('html').height(Math.max(height1, height3, height2));
        $('body').height(Math.max(height1, height3, height2));
    }
}
$(document).ready(function () {
    htmlbodyHeightUpdate()
    $(window).resize(function () {
        htmlbodyHeightUpdate()
    });
    $(window).scroll(function () {
        height2 = $('.main').height()
        htmlbodyHeightUpdate()
    });
});

async function changePassword() {
    let noti1 = document.getElementById('change-user-password-success');
    let noti2 = document.getElementById('invalid-user-password');
    noti1.hidden = true;
    noti2.hidden = true;
    let password = document.getElementById('user-password');
    let newPassword = document.getElementById('user-new-password');
    let form = new FormData();
    form.append('password', password.value);
    form.append('newPassword', newPassword.value);

    await fetch('/User/ChangePassword', {
        method: 'post',
        body: form
    }).then(re => re).then(re => {
        if (!re.ok)
            throw new Error("not success");
        noti1.hidden = false;
        password.value = null;
        newPassword.value = null;
    }).catch(error => {
        noti2.hidden = false;
        password.value = null;
        newPassword.value = null;
    });
}

async function getUserChangePassword() {
    let main = document.getElementById('user-profile-content');
    if (main != null) {
        main.innerHTML = await fetch('/User/GetChangePasswordPartial').then(re => re.text()).then(text => text);
        selectTab('change-password');
    }
}

async function getUserInfo() {
    let main = document.getElementById('user-profile-content');
    if (main != null) {
        main.innerHTML = await fetch('/User/GetUserInfoPartial').then(re => re.text()).then(text => text);
        selectTab('user-info');
    }
}

async function updateUserInfo() {
    let noti = document.getElementById('update-user-info-success');
    noti.hidden = true;
    let email = document.getElementById('user-email').value;
    let phoneNumber = document.getElementById('user-phoneNumber').value;
    let firstName = document.getElementById('user-firstName').value;
    let lastName = document.getElementById('user-lastName').value;
    let form = new FormData();
    form.append('email', email);
    form.append('phoneNumber', phoneNumber);
    form.append('firstName', firstName);
    form.append('lastName', lastName);
    await fetch('/User/UpdateUserInfo', {
        method: 'post',
        body: form
    }).then(re => re);
    noti.hidden = false;
}

async function GetUserOrders(page, orderStateId) {
    let main = document.getElementById('user-profile-content');
    if (main != null) {
        main.innerHTML = await fetch(`/Order/GetUserOrdersPartial?page=${page}&orderStateId=${orderStateId}`).then(re => re.text()).then(text => text);
        selectTab('user-orders');
    }
}


async function cancelUserOrder(orderId, page, orderStateId) {
    fetch(`/Admin/ChangeOrderState?orderId=${orderId}&orderStateId=2`).then(re => {
        GetUserOrders(page, orderStateId);
    });
}

function selectTab(id) {
    document.getElementById('user-info').style.color = '#CFD8DC';
    document.getElementById('favourites').style.color = '#CFD8DC';
    document.getElementById('change-password').style.color = '#CFD8DC';
    document.getElementById('user-orders').style.color = '#CFD8DC';
    document.getElementById(id).style.color = '#fff';
}
getUserInfo();

async function getFavProducts() {
    let container = document.getElementById('user-profile-content');
    if (container != null) {
        selectTab('favourites');
        container.innerHTML = await fetch('/User/GetFavProductsPartial').then(re => re.text()).then(text => text);
    }
}

async function removeFromFavProducts(productId) {
    await fetch(`/User/RemoveFromFavourites/${productId}`).then(re => {
        getFavProducts();
    })

}
// ADMIN PAGE
window.onload = function () {
    let today = new Date();
    let from = new Date(today.getTime() - 29 * 24 * 3600 * 1000);
    updateStats(from.toISOString(), today.toISOString());
}
async function getQuantityNotifications(from, to) {
    let form = new FormData();
    form.append('from', from);
    form.append('to', to);
    let container = document.getElementById('quantity-notifications');
    container.innerHTML = '';
    let newUsers = await fetch('/Admin/GetQuantityNotificationPartial?title=users', {
        method: 'post',
        body: form
    }).then(re => re.text()).then(text => text);
    container.innerHTML += newUsers;
    let newOrders = await fetch('/Admin/GetQuantityNotificationPartial?title=orders', {
        method: 'post',
        body: form
    }).then(re => re.text()).then(text => text);
    container.innerHTML += newOrders;
    let completedOrders = await fetch('/Admin/GetQuantityNotificationPartial?title=completed-orders', {
        method: 'post',
        body: form
    }).then(re => re.text()).then(text => text);
    container.innerHTML += completedOrders;
    let revenue = await fetch('/Admin/GetQuantityNotificationPartial?title=revenue', {
        method: 'post',
        body: form
    }).then(re => re.text()).then(text => text);
    container.innerHTML += revenue;
}

async function getTopStat(from, to) {
    let container = document.getElementById('top-customers-container');
    let form = new FormData();
    form.append('from', from);
    form.append('to', to);
    form.append('title', 'users')
    let topCustomers = await fetch('/Admin/GetTopStatPartial', {
        method: 'post',
        body: form
    }).then(re => re.text()).then(text => text);
    container.innerHTML = topCustomers;
    let form2 = new FormData();
    form2.append('from', from);
    form2.append('to', to);
    form2.append('title', 'products')
    container = document.getElementById('top-products-container');
    let topProducts = await fetch('/Admin/GetTopStatPartial', {
        method: 'post',
        body: form2
    }).then(re => re.text()).then(text => text);
    container.innerHTML = topProducts;
}

async function updateStats(from, to) {
    getQuantityNotifications(from, to);
    getTopStat(from, to);
    getRevenueStat(from, to);
    getRevenueStatByLocal(from, to);
    GetCategoriesStat(from, to);
}

async function updateKeywordOrderTable(el) {
    let sortBy = document.getElementById('orderTable-sortBy').innerHTML;
    let orderStateId = document.getElementById('orderTable-orderStateId').innerHTML;
    let keyword = el.value;
    await getOrderTable(sortBy, keyword, orderStateId);
}

async function updateSortOrderTable(field) {
    let keyword = document.getElementById('orderTable-keyword').innerHTML;
    let orderStateId = document.getElementById('orderTable-orderStateId').innerHTML;
    let direction = 'desc';
    let currentSortBy = document.getElementById('orderTable-sortBy').innerHTML;
    if (currentSortBy.includes(field))
        direction = currentSortBy.includes('desc') ? 'asc' : 'desc';
    let sortBy = `${field} ${direction}`;
    await getOrderTable(sortBy, keyword, orderStateId);
}

getOrderTable('createdAt desc', '', 1, 1);

async function getOrderTable(sortBy, keyword, orderStateId, page) {
    let form = new FormData();
    form.append('sortBy', sortBy);
    form.append('keyword', keyword);
    form.append('orderStateId', orderStateId);
    form.append('page', page);
    let container = document.getElementById('order-table-container');
    let orderTable = await fetch('/Admin/GetOrderTablePartial', {
        method: 'post',
        body: form

    }).then(re => re.text()).then(text => text);
    container.innerHTML = orderTable;
}

async function selectOrderState(orderStateId) {
    let sortBy = document.getElementById('orderTable-sortBy').innerHTML;
    let keyword = document.getElementById('orderTable-keyword').innerHTML;
    getOrderTable(sortBy, keyword, orderStateId);
}

async function changeOrderState(orderId, orderStateId) {
    fetch(`/Admin/ChangeOrderState?orderId=${orderId}&orderStateId=${orderStateId}`).then(re => {
        let sortBy = document.getElementById('orderTable-sortBy').innerHTML;
        let keyword = document.getElementById('orderTable-keyword').innerHTML;
        let orderStateId0 = document.getElementById('orderTable-orderStateId').innerHTML;
        getOrderTable(sortBy, keyword, orderStateId0);
    });
}

async function changeOrderPage(page) {
    let sortBy = document.getElementById('orderTable-sortBy').innerHTML;
    let keyword = document.getElementById('orderTable-keyword').innerHTML;
    let orderStateId = document.getElementById('orderTable-orderStateId').innerHTML;
    await getOrderTable(sortBy, keyword, orderStateId, page);
}

async function selectCategory(categoryId) {
    let sortBy = document.getElementById('productTable-sortBy').innerHTML;
    let keyword = document.getElementById('productTable-keyword').innerHTML;
    getProductTable(sortBy, keyword, categoryId);
}
getProductTable('createdAt desc', '', 0, 1);
async function getProductTable(sortBy, keyword, categoryId, page) {
    let form = new FormData();
    form.append('sortBy', sortBy);
    form.append('keyword', keyword);
    form.append('categoryId', categoryId);
    form.append('page', page);
    let container = document.getElementById('product-table-container');
    let productTable = await fetch('/Admin/GetProductTablePartial', {
        method: 'post',
        body: form

    }).then(re => re.text()).then(text => text);
    container.innerHTML = productTable;
}

async function updateSortProductTable(field) {
    let keyword = document.getElementById('productTable-keyword').innerHTML;
    let categoryId = document.getElementById('productTable-categoryId').innerHTML;
    let direction = 'desc';
    let currentSortBy = document.getElementById('productTable-sortBy').innerHTML;
    console.log(keyword);
    console.log(categoryId);
    console.log(currentSortBy);
    if (currentSortBy.includes(field))
        direction = currentSortBy.includes('desc') ? 'asc' : 'desc';
    let sortBy = `${field} ${direction}`;
    await getProductTable(sortBy, keyword, categoryId);
}

async function updateKeywordProductTable(el) {

    let sortBy = document.getElementById('productTable-sortBy').innerHTML;
    let categoryId = document.getElementById('productTable-categoryId').innerHTML;
    let keyword = el.value;
    console.log(keyword);
    await getProductTable(sortBy, keyword, categoryId, 1);
}

async function changeProductPage(page) {
    console.log(page);
    let sortBy = document.getElementById('productTable-sortBy').innerHTML;
    let keyword = document.getElementById('productTable-keyword').innerHTML;
    let categoryId = document.getElementById('productTable-categoryId').innerHTML;
    await getProductTable(sortBy, keyword, categoryId, page);
}

$('#reportrange').on('apply.daterangepicker', function (ev, picker) {
    updateStats(new Date(picker.startDate).toISOString(), new Date(picker.endDate).toISOString());
});