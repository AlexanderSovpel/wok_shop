$(document).ready(function () {
    var count;

    //recalculate total price
    function recalcTotal() {
        var totals = $('.item-total');
        var total = 0;
        for (var i = 0; i < totals.length; ++i) {
            total += parseInt(totals[i].innerText);
        }

        var totalText;
        if (total == 0)
            totalText = "Корзина пуста";
        else
            totalText = "Итого: " + total + " BYR";
        $('.total_price').text(totalText);
        $('.total1').first().text(total + " BYR");

        $('.cart-items').children('h1').text("Корзина (0)");
    }

    $('.delete-from-cart').click(function () {
        var id = $(this).next().children('.item-id').val();
        $.get("../Home/DeleteFromCart/" + id, function (data, status) {
            if (status == "success") {
                $.get("../Cart/GetCartTotal", function (data, status) {
                    if (status == "success")
                        $('.simpleCart_total').text(data + " BYR");
                });

                $.get("../Cart/GetCartCount", function (data, status) {
                    if (status == "success") {
                        $('.simpleCart_quantity').text(data + " шт");
                        count = data;
                    }
                });
            }
        });

        $(this).parent().fadeOut('fast', function(c){
            $(this).remove();
            recalcTotal();
        });
    });

    $('.add-to-cart').click(function () {
        var id = $(this).siblings(':first').val();
        var count = $(this).prev().val();

        if (count <= 0) {
            $('.cart-message').css("color", "#ed645c").text("Количество продуктов может быть только положительным");
            return;
        }

        $.get("../Home/AddToCart?id=" + id + "&count=" + count, function (data, status) {
            if (status == "success") {
                //alert("Data: " + data + "\nStatus: " + status);
                $('.cart-message').css("color", "#690").text(data);

                $.get("../Cart/GetCartTotal", function (data, status) {
                    if (status == "success")
                        $('.simpleCart_total').text(data + " BYR");
                });

                $.get("../Cart/GetCartCount", function (data, status) {
                    if (status == "success") {
                        $('.simpleCart_quantity').text(data + " шт");
                        count = data;
                    }
                });
            }
        });
    });

    $('.count').change(function () {
        if ($(this).val() <= 0)
        {
            $('.cart-message').css("color", "#ed645c").text("Количество продуктов может быть только положительным");
            return;
        }

        var id = $(this).parent().parent().siblings(':first').val();

        //calculating the diffrence between current and previous values
        var prev = $(this).attr('prev');
        var curr = $(this).val();
        var diffrence = curr - prev;

        //change FoodList according to diffrence
        if (diffrence > 0) {
            $.get("../Home/AddToCart?id=" + id + "&count=" + diffrence, function (data, status) {
                if (status == "success") {
                    $.get("../Cart/GetCartTotal", function (data, status) {
                        if (status == "success")
                            $('.simpleCart_total').text(data + " BYR");
                    });

                    $.get("../Cart/GetCartCount", function (data, status) {
                        if (status == "success") {
                            $('.simpleCart_quantity').text(data + " шт");
                            count = data;
                        }
                    });
                }
            });
        }
        else {
            $.get("../Home/DeleteSomeFromCart?id=" + id + "&count=" + Math.abs(diffrence), function (data, status) {
                if (status == "success") {
                    $.get("../Cart/GetCartTotal", function (data, status) {
                        if (status == "success")
                            $('.simpleCart_total').text(data + " BYR");
                    });

                    $.get("../Cart/GetCartCount", function (data, status) {
                        if (status == "success")
                            $('.simpleCart_quantity').text(data + " шт");
                    });
                }
            });
        }

        $(this).attr('prev', curr);

        //recalculate price for current count-changed item
        var itemTotal = $(this).val() * $(this).parent().next('.price').val();
        $(this).parent().next().next().text(itemTotal + ' BYR');

        recalcTotal();
    });

    function checkFilled() {
        if ($('[name=name]').val() == "")
            return false
        else if ($('[name=phone]').val() == "")
            return false
        else if ($('[name=address]').val() == "")
            return false
        else
            return true;
    }

    function clearFields() {
        $('[name=name]').val("");
        $('[name=phone]').val("");
        $('[name=address]').val("");
        $('[name=comment]').text("");
    }

    $('[name=makeOrder]').click(function (e) {
        e.preventDefault();

        if (checkFilled()) {
            var regex = /\+375(29|33|44|17)\d{7}/;
            var p = $('[name=phone]').val();
            if (regex.test(p)) {
                if ($('.simpleCart_quantity').text() != "0 шт") {
                    var person = $('[name=name]').val();
                    var email = $('[name=email]').val();
                    var phone = $('[name=phone]').val();
                    var address = $('[name=address]').val();
                    //var address = $('[name=street]').val() + ", " + $('[name=house]').val() + "-" + $('[name=apartment]').val();
                    var deliveryType = $('[name=deliveryType]:checked').val();
                    var payBy = $('[name=payBy]:checked').val();

                    var request = "../Order/MakeOrder?person=" + person +
                        "&email=" + email +
                        "&phone=" + phone +
                        "&address=" + address +
                        "&deliveryType=" + deliveryType +
                        "&payBy=" + payBy;
                    $.get(request, function (data, status) {
                        $('.message').css("color", "#690").text(data);
                        clearFields();
                        $('.cart-header').remove();
                        recalcTotal();
                        if (status == "success") {
                            $.get("../Cart/GetCartTotal", function (data, status) {
                                if (status == "success")
                                    $('.simpleCart_total').text(data + " BYR");
                            });

                            $.get("../Cart/GetCartCount", function (data, status) {
                                if (status == "success")
                                    $('.simpleCart_quantity').text(data + " шт");
                            });
                        }
                    });
                }
                else {
                    $('.message').css("color", "#ed645c").text("Добавьте продукты в корзину");
                }
            }
            else {
                $('.message').css("color", "#ed645c").text("Введите корректный номер телефона");
            }
        }
        else {
            $('.message').css("color", "#ed645c").text("Пожалуйста, заполните все поля");
        }
    });

    $.get("../Cart/GetCartTotal", function (data, status) {
        if (status == "success")
            $('.simpleCart_total').text(data + " BYR");
    });

    $.get("../Cart/GetCartCount", function (data, status) {
        if (status == "success")
            $('.simpleCart_quantity').text(data + " шт");
    });

    //$('[name=add]').click(function (e) {
    //    e.preventDefault();

    //    var category = $('[name=category]').val();
    //    var name = $('[name=name]').val();
    //    var description = $('[name=description]').val();
    //    var image = document.querySelector('[name=image]').files[0];

    //        //$('[name=image]').files[0];
    //    var weight = $('[name=weight]').val();
    //    var price = $('[name=price]').val();

        //var xhr = new XMLHttpRequest();
        //xhr.setRequestHeader('Content-Type', 'multipart/form-data');
        //xhr.open('POST', '../Foods/AddFood', true);

        /*var body = {
            category: category,
            name: name,
            description: description,
            image: image,
            weight: weight,
            price: price
        };

        xhr.send(body);

        xhr.onreadystatechange = function () {
            if (xhr.readyState != 4) return;

            if (xhr.status != 200) {
                alert(xhr.status + ': ' + xhr.statusText);
            } else {
                alert(xhr.responseText);
            }
        }*/

    //    $.post("../Foods/AddFood", {
    //        category: category,
    //        name: name,
    //        description: description,
    //        image: image,
    //        weight: weight,
    //        price: price
    //        },
    //        function (data, status) {
    //            alert("Data: " + data + "\nStatus: " + status);
    //        });
    //});

    //$('[name=save]').click(function (e) {
    //    e.preventDefault();

    //    var category = $('[name=category]').val();
    //    var name = $('[name=name]').val();
    //    var description = $('[name=description]').val();
    //    var image = document.querySelector('[name=image]').files[0];
    //    var weight = $('[name=weight]').val();
    //    var price = $('[name=price]').val();


    //    //var xhr = new XMLHttpRequest();
    //    //xhr.setRequestHeader('Content-Type', 'multipart/form-data');
    //    //xhr.open('POST', '../Foods/ChangeFood', true);

    //    var body = {
    //        category: category,
    //        name: name,
    //        description: description,
    //        image: image,
    //        weight: weight,
    //        price: price
    //    };

    //    //xhr.send(body);

    //    /*xhr.onreadystatechange = function () {
    //        if (xhr.readyState != 4) return;

    //        if (xhr.status != 200) {
    //            alert(xhr.status + ': ' + xhr.statusText);
    //        } else {
    //            alert(xhr.responseText);
    //        }
    //    }*/

    //    $.post("../Foods/ChangeFood", body, function (data, status) {
    //        alert("Data: " + data + "\nStatus: " + status);
    //    });
    //});

    $('[name=cancel]').click(function (e) {
        e.preventDefault();

        window.location.pathname = "/Foods/Foods";
    });

    $('.continue').click(function (e) {
        e.preventDefault();
        
        var offset = $('#contact-form').offset();
        $('body').animate({
            scrollTop: offset.top
        }, "fast");
        
    });

    $('#login').click(function (e) {
        e.preventDefault();

        if ($('[name=email]').val() != "" && $('[name=password]').val() != "") {
            $.post("../Account/Login",
                {
                    email: $('[name=email]').val(),
                    password: $('[name=password]').val()
                },
                function (data, status) {
                    location.href = "/Foods/Foods";
                });
        }
        else
            $('.message').css("color", "#ed645c").text("Заполните все поля");
    });

    $('#register').click(function (e) {
        e.preventDefault();

        if ($('[name=email]').val() != "" && $('[name=password]').val() != "" && $('[name=passwordSubmit]').val() != "") {
            $.post("../Account/Register",
                {
                    email: $('[name=email]').val(),
                    password: $('[name=password]').val(),
                    confirmPassword: $('[name=passwordSubmit]').val()
                },
                function (data, status) {
                    alert(data);
                });
        }
        else
            $('.message').css("color", "#ed645c").text("Заполните все поля");
    });

});