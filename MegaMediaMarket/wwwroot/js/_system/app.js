var body;
function itemVars(e) {
    var t = e.closest(".itemRow");
    null != e.data("price") && (parseInt(e.data("price")) > 0 ? $(".priceBlock", t).show() : $(".priceBlock", t).hide(), $(".itemPrice", t).text(numberFormat(e.data("price"), "", " ", " "))),
        null != e.data("image") && "" != e.data("image") && $(".itemImage").attr("src", e.data("image"));
}
function arrFind(e, t) {
    var r = !1;
    return (
        $.each(e, function (e, a) {
            a == t && (r = e);
        }),
        r
    );
}
function validateForm(e) {
    var t = !1;
    return (
        0 == $(".form-message", e).length && $(".form-message", e.parent()),
        $(".required", e).removeClass("form-error"),
        $(".required", e).removeClass("form-success"),
        $(".required", e).each(function () {
            var e = $(this).attr("type");
            if (
                (("text" != e && "email" != e && "tel" != e && "password" != e) || "" != $(this).val() ? $(this).addClass("form-success") : ((t = !0), $(this).addClass("form-error")),
                ("radio" != e && "checkbox" != e) || $(this).prop("checked") ? $(this).addClass("form-success") : ((t = !0), $(this).addClass("form-error")),
                $(this).hasClass("required-if-not"))
            ) {
                var r = $($(this).data("selector")),
                    a = $(this).data("value");
                arrFind((a = a.split(",")), r.val());
            }
        }),
        t && ($(".form-error", e).first().focus(), alertify.error("Заполните все обязательные поля 1")),
        t
    );
}
function submitLoading(e, t) {
    e.prop("disabled", t);
    var r = e.html();
    t ? (e.addClass("loading"), e.text("Загрузка..."), e.data("text", r)) : (e.removeClass("loading"), e.html(e.data("text")));
}
function numberFormat(e, t, r, a) {
    var i, o;
    return (
        isNaN((t = Math.abs(t))) && (t = 2),
        null == r && (r = ","),
        null == a && (a = "."),
        (o = (i = parseInt((e = (+e || 0).toFixed(t))) + "").length) > 3 ? (o %= 3) : (o = 0),
        (o ? i.substr(0, o) + a : "") +
            i.substr(o).replace(/(\d{3})(?=\d)/g, "$1" + a) +
            (t
                ? r +
                  Math.abs(e - i)
                      .toFixed(t)
                      .replace(/-/, 0)
                      .slice(2)
                : "")
    );
}
function t(e) {
    return null != _t[e] ? _t[e] : e;
}
$(document).ready(function () {
    $(".phone").mask("+7 (999) 999-99-99");
}),
    $(function () {
        (body = $("body")).on("click", ".certShow", function () {
            return $(".certsAll").show(), $(this).hide(), !1;
        }),
            body.on("submit", ".pjaxFilters", function () {
                var e = $(this);
                return $.pjax({ url: e.attr("action") + "?" + e.serialize(), container: e.data("grid"), scrollTo: !1 }), !1;
            }),
            body.on("click", ".goTo", function () {
                return window.location.replace($(this).attr("href")), !1;
            }),
            body.on("click", ".goSubmit", function () {
                $(this).closest("form").submit();
            }),
            body.on("change", ".goSubmitOnChange", function () {
                $(this).closest("form").submit();
            }),
            body.on("change", ".reloadByValue", function () {
                return window.location.replace($(this).val()), !1;
            }),
            body.on("click touch", ".oneclick", function () {
                var e = $(this).parent().parent().parent().parent().find("h1").text(),
                    t = $(this).parent().find('input[type="number"]').val();
                if (0 == t) var r = 1;
                else r = t;
                var a = $(this).parent().find(".item-cost_new").text();
                $('.mfp-content #fast-buy-popup-anim form input[name="buyBusket"]').val("Название товара - " + e + " | Количество - " + r + " | Цена за шт- " + a);
            }),
            body.on("click touch", ".form-check.inp_2", function () {
            	console.log(1);
            	$('#fast-buy-popup-anim .mfp-close').click();
            	setTimeout(function(){
            		$(".boxberry_select").click();
            	}, 300);
            }),
            body.on("change", "select.category__select", function () {
                var e = window.location.pathname,
                    t = this.value;
                if (1 == t) {
                    console.log(1);
                    var r = e + "?sort=name";
                }
                2 == t && (console.log(2), (r = e + "?sort=pricedown")), 3 == t && (console.log(3), (r = e + "?sort=priceup")), (1 != t && 2 != t && 3 != t) || (window.location.href = r);
            }),
            body.on("change", '.item-sum input[type="number"]', function () {
                var e = $(this).val();
                $('form.addToCartForm input[name="qty"]').val(e);
            }),
            body.on("click touch", ".item_allsettings", function () {
                $([document.documentElement, document.body]).animate({ scrollTop: $(".tabs .tabs__caption").offset().top - 20 }, 500), $('.tabs .tabs__caption li:contains("Характеристики")').click();
            }),
            body.on("click", ".loadMore", function () {
                var e = $(this),
                    t = $(".loadMoreContainer");
                return (
                    $.post(
                        "/ajax/loadmore",
                        e.data(),
                        function (r) {
                            t.append(r.html),
                                e.data("page", parseInt(e.data("page")) + 1),
                                $.each($(".ibg"), function (e, t) {
                                    $(this).css("background-image", 'url("' + $(this).find("img").attr("src") + '")');
                                }),
                                r.last && e.remove();
                        },
                        "json"
                    ),
                    !1
                );
            }),
            body.on("submit", "form.ajaxFormFast", function () {
                var e = $(this);
                return (
                    validateForm(e) ||
                        $.ajax({
                            url: "/ajax/fastform",
                            data: e.serializeArray(),
                            success: function (t) {
                                t.success
                                    ? ($.magnificPopup.instance.close(),
                                      $("input:not(:submit)", e).val(""),
                                      $("textarea", e).val(""),
                                      0 == t.url ? window.location.replace("/thanks") : window.location.replace(t.url),
                                      $("#oneclickModal").hasClass("show") && $("#oneclickModal").modal("hide"))
                                    : alertify.error(t.error), console.log(t.errorL);
                            },
                            type: "POST",
                            dataType: "json",
                        }),
                    !1
                );
            });
        var e = 1;
        $(".fileupload").length > 0 &&
            $(".fileupload").each(function () {
                var t = $(this);
                t.fileupload({
                    url: "/ajax/upload",
                    dataType: "json",
                    done: function (r, a) {
                        $.each(a.result.files, function (r, a) {
                            $(t.data("files")).append("<input type='hidden' name='Файл " + e + "' value='" + a + "'><div>" + a + "</div>"), (e += 1);
                        });
                    },
                })
                    .prop("disabled", !$.support.fileInput)
                    .parent()
                    .addClass($.support.fileInput ? void 0 : "disabled");
            }),
            body.on("submit", "form.form-ajax", function () {
                var e = $(this);
                return (
                    validateForm(e) ||
                        $.ajax({
                            url: "/ajax/form",
                            data: e.serializeArray(),
                            success: function (t) {
                                t.success
                                    ? ($("input:not(:submit)", e).val(""),
                                      $("textarea", e).val(""),
                                      alertify.success(t.msg))
                                    : alertify.error(t.error);
                            },
                            type: "POST",
                            dataType: "json",
                        }),
                    !1
                );
            }),
            body.on("submit", "form.addComment", function () {
                var e = $(this);
                return (
                    validateForm(e) ||
                        $.ajax({
                            url: "/ajax/addcomment",
                            data: e.serializeArray(),
                            success: function (t) {
                                t.success ? ($("input:not(:submit)", e).val(""), $("textarea", e).val(""), alertify.success(t.msg)) : alertify.error(t.error);
                            },
                            type: "POST",
                            dataType: "json",
                        }),
                    !1
                );
            }),
            body.on("submit", ".addItemReview", function () {
                var e = $(this);
                return (
                    validateForm(e) ||
                        $.post(
                            "/ajax/additemreview",
                            e.serializeArray(),
                            function (t) {
                                t.success ? (alertify.success(t.msg), $(".clearVal", e).val("")) : alertify.error(t.error);
                            },
                            "json"
                        ),
                    !1
                );
            }),
            body.on("submit", ".addReview", function () {
                var e = $(this);
                return (
                    validateForm(e) ||
                        $.post(
                            "/ajax/addreview",
                            e.serializeArray(),
                            function (t) {
                                t.success ? (alertify.success(t.msg), $(".clearVal", e).val("")) : alertify.error(t.error);
                            },
                            "json"
                        ),
                    !1
                );
            }),
            body.on("click", ".getItemPhotos", function () {
                var e = $(this);
                return (
                    $.post(
                        "/ajax/getitemphotos",
                        e.data(),
                        function (e) {
                            e.success ? $(".ajaxItemPhotos").html(e.html) : alertify.error(e.error);
                        },
                        "json"
                    ),
                    !1
                );
            }),
            body.on("click", ".changeLang", function () {
                var e = $(this);
                return (
                    $.post(
                        "/ajax/changelang",
                        e.data(),
                        function (e) {
                            e.success && location.reload();
                        },
                        "json"
                    ),
                    !1
                );
            }),

            body.on("submit", "#loginForm", function () {
                var e = $(this);
                return (
                    $.post(
                        "/profile/login",
                        e.serializeArray(),
                        function (e) {
                            if(e.success){
                                // alertify.success(e.msg);
                                window.location.href = "/profile";
                            }
                            else {
                                $('.login_form').find('input').val('');
                                $('.login_form').find('input').addClass('error');
                                $('.login_form').find('.error_info').addClass('active');
                                // alertify.error(e.error)
                            }
                        },
                        "json"
                    ),
                    !1
                );
            }),
            
            body.on("submit", "#registerForm", function () {
                var e = $(this);
                return (
                    $.post(
                        "/profile/register",
                        e.serializeArray(),
                        function (e) {
                            if(e.success){
                                window.location.href = "/profile";
                            }
                            else {
                                $('.login_form').find('input').val('');
                                $('.login_form').find('input').addClass('error');
                                $('.login_form').find('.error_info').addClass('active');
                                console.log(e.error);
                            }
                            // e.success ? (alertify.success(e.msg), location.reload()) : alertify.error(e.error);
                        },
                        "json"
                    ),
                    !1
                );
            }),
            body.on("submit", "#repairForm", function () {
                var e = $(this);
                return (
                    $.post(
                        "/profile/repair",
                        e.serializeArray(),
                        function (e) {
                            if(e.success){
                                window.location.href = "/profile/repairsuccess";
                            }
                            else {
                                $('.login_form').find('input').val('');
                                $('.login_form').find('input').addClass('error');
                                $('.login_form').find('.error_info').addClass('active');
                            }
                            // e.success ? alertify.success(e.msg) : alertify.error(e.error);
                        },
                        "json"
                    ),
                    !1
                );
            });

            body.on("keyup", ".iSearch", function () {
                var myLang = $(this).data('lang');
                lang = '';
                if (myLang == 'ru') {
                    lang = 'ru';
                }
                else {
                    lang = 'en';
                }
                var e = $(this),
                    t = e.val();
                $(".iSearchResults").remove(),
                    t.length > 2 &&
                        $.post(
                            "/ajax/isearch",
                            { query: t },
                            function (t) {
                                console.log(t.item);
                                var r = "";
                                var rI = "";
                                var rC = "";
                                var countC = 0;
                                var countI = 0;
                                if(lang == 'ru'){
                                    priceText = 'Цена от:';
                                    productsText = 'Товаров:';
                                    priceSymbol = '₽';
                                }
                                else {
                                    priceText = 'Price from:';
                                    productsText = 'Products:';
                                    priceSymbol = '$';
                                }
                                if(t.item){
                                    if(t.item.length > 0){
                                        var countI = t.item.length;
                                        $.each(t.item, function (e, tt) {
                                            rI += '<a href="'+tt.link+'"><div class="table"><div class="row">';
                                            rI += '<div class="d-flex">';

                                                rI += '<div class="d-flex">';
                                                    rI += '<div class="img">';
                                                        rI += '<img src="'+tt.image+'">';
                                                    rI += "</div>";
                                                    rI += "<div>";
                                                        rI += '<div class="itemtitle">'+tt.name+'</div>';
                                                        rI += '<div class="d-flex catblock">';
                                                            rI += '<img src="'+tt.caticon+'">';
                                                            rI += '<span class="cattitle">'+tt.catname+'</span>';
                                                        rI += '</div>';
                                                    rI += "</div>";
                                                rI += "</div>";
                                                if(tt.undetected == '1'){
                                                    rI += '<div class="undetected"><div class="undetected__circle"></div><p class="undetected__text">Undetected</p></div>';
                                                }
                                                else {
                                                    rI += '<div class="update"><div class="update__circle"></div><p class="update__text">On update</p></div>';
                                                }

                                                rI += '<div class="catprice"><span>'+priceText+'</span><b>'+tt.price+priceSymbol+'</b></div>';

                                            rI += "</div>";
                                            rI += "</div></div></a>";
                                            return rI;
                                        });
                                    }
                                }

                                if(t.cat){
                                    if(t.cat.length > 0){
                                        var countC = t.cat.length;
                                        $.each(t.cat, function (e, tc) {
                                            rC += '<a href="'+tc.link+'"><div class="table"><div class="row">'
                                            //
                                            rC += '<div class="d-flex">';
                                            rC += '<div class="d-flex"><img src="/sitefiles/Categories/'+tc.icon+'" class="catimage"><span class="itemtitle">'+tc.name+'</span></div>';
                                            rC += '<div class="catitems">'+productsText+' '+tc.count+'</div>';
                                            rC += '<div class="catprice"><span>'+priceText+'</span><b>'+tc.price+priceSymbol+'</b></div>';
                                            rC += '</div>';
                                            //
                                            rC += "</div></div></a>"
                                            return rC;
                                        });
                                    }
                                }
                                rCF = '';
                                rIF = '';
                                console.log(lang);
                                if(lang == 'ru'){
                                    gamesText = 'Игры';
                                    iteText = 'Товары';
                                    resText = 'Результатов:';
                                    searchText = 'По запросу ничего не найдено';
                                }
                                else {
                                    gamesText = 'Games';
                                    iteText = 'Products';
                                    resText = 'Results:';
                                    searchText = 'No results found';
                                }
                                if(rC.length > 0){
                                    rCF = '<div class="results cat">' +
                                            '<div class="d-flex searchHead">' +
                                                '<div class="title">'+gamesText+'</div>'+
                                                '<div class="count">'+resText+' '+t.cat.length+'</div>'+
                                            '</div>'+
                                            '<div class="group">'+
                                                '<div class="group-items">'+
                                                    '<div class="result">' +
                                                        rC +
                                                    '</div>'+
                                                '</div>'+
                                                '<div class="clearfix"></div>'+
                                            '</div>'+
                                        '</div>';
                                }
                                if(rI.length > 0){
                                    rIF = '<div class="results items">' +
                                            '<div class="d-flex searchHead">' +
                                                '<div class="title">'+iteText+'</div>'+
                                                '<div class="count">'+resText+' '+t.item.length+'</div>'+
                                            '</div>'+
                                            '<div class="group">'+
                                                '<div class="group-items">'+
                                                    '<div class="result">' +
                                                        rI +
                                                    '</div>'+
                                                '</div>'+
                                                '<div class="clearfix"></div>'+
                                            '</div>'+
                                        '</div>';
                                }

                                rF = rCF + rIF,
                                    "" == rF && (rF = "<div class='noResults'>"+searchText+"</div>"),
                                    e.closest(".iSearchBlock")
                                    .append(
                                        '<div class="iSearchResults" style="position: absolute; left: 0; top: 45px; width: 100%; display: block;">'+rF+'</div>'
                                    );

                            },
                            "json"
                        );
            }),
            body.on("change", ".changeVar", function () {
                return itemVars($(this)), !1;
            }),
            $(".changeVar").length > 0 &&
                $(".changeVar:checked").each(function () {
                    itemVars($(this));
                }),
            body.on("click", ".addToFav", function () {
                var e = $(this);
                return (
                    $.post(
                        "/ajax/favorites",
                        e.data(),
                        function (e) {
                            e.success ? ($(".imFavorites").html(e.html), alertify.success(e.msg)) : alertify.error(e.error);
                        },
                        "json"
                    ),
                    !1
                );
            });

            if($('.cart').length) {
	            body.on("click touch", "#boxberry-popup-anim button.mfp-close", function () {
	                "" == $("input.pricer_input").val() && $("#delivery_2").prop("checked", !1);
	            });
	            $(document).mouseup(function (e) {
	                var t = $("#boxberry-popup-anim");
	                t.is(e.target) || 0 !== t.has(e.target).length || ("" == $("input.pricer_input").val() && $("#delivery_2").prop("checked", !1));
	            });
	        }

            body.on("click touch", ".boxberry_select", function () {
            	console.log('ok');
            	
            	if($('section.item').length) {
            		if(parseInt($('.item-sum input').val().replace(/\D+/g, "")) > 1) {
            			var e = parseInt($(".item-cost_new").text().replace(/\D+/g, ""))*parseInt($('.item-sum input').val().replace(/\D+/g, ""));
            		}
            		else {
            			var e = parseInt($(".item-cost_new").text().replace(/\D+/g, ""));
            		}
	                var t = $("p.weig"),
	                    r = parseInt($("p.weil").text().split("x")[0].replace(/\D+/g, "")),
	                    a = parseInt($("p.weil").text().split("x")[1].replace(/\D+/g, "")),
	                    i = parseInt($("p.weil").text().split("x")[2].replace(/\D+/g, "")),
	                    o = 0;
            	}
            	else {
	                var e = parseInt($(".cartPrices .total-big").text().replace(/\D+/g, "")),
	                    t = $(".cart .prod-card-weight"),
	                    r = parseInt($(".cart .prod-card-hei").text().split("Размер упаковки: ")[1].split("x")[0].replace(/\D+/g, "")),
	                    a = parseInt($(".cart .prod-card-hei").text().split("Размер упаковки: ")[1].split("x")[1].replace(/\D+/g, "")),
	                    i = parseInt($(".cart .prod-card-hei").text().split("Размер упаковки: ")[1].split("x")[2].replace(/\D+/g, "")),
	                    o = 0;
            	}

            	if($('section.item').length) {
            		if(parseInt($('.item-sum input').val().replace(/\D+/g, "")) > 1) {
            			o = parseInt(t.text().replace(/\D+/g, "")) * parseInt($('.item-sum input').val().replace(/\D+/g, ""));
            		}
            		else {
						o = parseInt(t.text().replace(/\D+/g, ""));
            		}
	            }
	            else {
	                t.each(function () {
	                    $(this).parent().parent().find('input[name="qty"]').val() > 1
	                        ? (o += parseInt($(this).text().replace(/\D+/g, "")) * $(this).parent().parent().find('input[name="qty"]').val())
	                        : (o += parseInt($(this).text().replace(/\D+/g, "")));
	                });
	            }

                var s = o;
                var cityInput = $('input[name="Orders[city]"]').val();
                if(cityInput.length > 1) {
                	var cityVal = cityInput;
                }
                else {
                	var cityVal = '';
                }
                if($('section.item').length) {
	                $("#boxberry_map iframe").remove();
	                boxberry.openOnPage("boxberry_map");
	                console.log(e);
                    if(e < 2000) {
                        boxberry.open(callback_function, "1$K48Rhdsz7PCzFtViAArRayVx84Oxnk5U", cityVal, "", e, s, 0, r, a, i);
                    }
                    else {
                    	boxberry.open(callback_function, "0", cityVal, "", e, s, 0, "", "", "");
                    }
	            }
	            else {
	            	console.log(2);
	            	$("#boxberry_map iframe").remove(),
	                    boxberry.openOnPage("boxberry_map"),
	                    parseInt($(".total-big").text().replace(/\D+/g, "")) < 2e3
	                        ? boxberry.open(callback_function, "1$K48Rhdsz7PCzFtViAArRayVx84Oxnk5U", cityVal, "", e, s, 0, r, a, i)
	                        : boxberry.open(callback_function, "0", cityVal, "", e, s, 0, "", "", "");
	            }

            }),
            body.on("click", ".addToCompare", function () {
                var e = $(this);
                return (
                    $.post(
                        "/ajax/compares",
                        e.data(),
                        function (e) {
                            e.success ? ($(".imCompares").html(e.html), alertify.success(e.msg)) : alertify.error(e.error);
                        },
                        "json"
                    ),
                    !1
                );
            }),
            body.on("click", ".repeatOrder", function () {
                var e = $(this);
                return (
                    $.post(
                        "/ajax/repeatorder/",
                        e.data(),
                        function (t) {
                            t.success ? ($(".imCart").html(t.html), alertify.success(t.msg), e.remove()) : alertify.error(t.error);
                        },
                        "json"
                    ),
                    !1
                );
            }),
            body.on("submit", ".addToCartForm", function () {
                var e = $(this);
                return (
                    submitLoading($('[type="submit"]', e), !0),
                    $.post(
                        "/ajax/cart",
                        e.serializeArray(),
                        function (t) {
                            t.success ? ($(".imCart").html(t.html), submitLoading($('[type="submit"]', e), !1), alertify.success(t.msg)) : alertify.error(t.error);
                        },
                        "json"
                    ),
                    $("html, body").animate({ scrollTop: 0 }, "fast"),
                    setTimeout(function () {
                        $(".header-cart").addClass("cart_active");
                    }, 300),
                    !1
                );
            }),
            body.on("change", ".changeCartQty", function () {
                var e = $(this),
                    t = e.closest(".itemBlock");
                return (
                    $.post(
                        "/ajax/changecart",
                        { id: e.data("id"), qty: e.val() },
                        function (e) {
                            e.success
                                ? ($(".imCart").html(e.html),
                                  $(".cartPrices").html(e.cart_prices),
                                  $(".cartPricesMain").html(e.cart_prices_main),
                                  $(".itemPrice", t).html(e.item_price),
                                  $(".itemOldPrice", t).html(e.item_old_price),
                                  $(".changeDeliveries:checked").change())
                                : alertify.error(e.error);
                        },
                        "json"
                    ),
                    !1
                );
            }),
            body.on("click", ".changeCartBtn", function () {
                var e = $(this).closest(".changeCartQtyBlock");
                $(".changeCartQty", e).change();
            }),
            body.on("change", ".changePayments", function () {
                $(".paymentsBlock").hide(), $(".paymentsBlock_" + $(this).val()).show();
            }),
            $(".changePayments").length > 0 && $(".changePayments").first().prop("checked", !0).change(),
            body.on("change", ".changeDeliveries", function () {
                'Доставка до терминала транспортной компании "Boxberry"' == $(this).parent().find("span").text() ||
                    ($(".deliveriesBlock_" + $(this).val()).show(),
                    $(".paymentdelivery_" + $(this).val()).show(),
                    $.post(
                        "/ajax/changedeliveries",
                        { id: $(this).val() },
                        function (e) {
                            e.success
                                ? ($('input[name="Orders[delivery_price]"]').val(e.price),
                                  $(".cartPrices").html(e.cart_prices),
                                  $(".cartPricesMain").html(e.cart_prices_main),
                                  $(".deliveryPrice").val(e.price),
                                  $(".deliveryPriceText").html(e.price))
                                : alertify.error(e.error);
                        },
                        "json"
                    ));
            }),
            $(".changeDeliveries").length > 0 && $(".changeDeliveries").first().prop("checked", !0).change(),
            body.on("submit", ".setPromocode", function () {
                var e = $(this);
                return (
                    $.post(
                        "/ajax/promocode",
                        e.serializeArray(),
                        function (t) {
                            t.success ? ($(".cartPrices").html(t.cart_prices), $(".cartPricesMain").html(t.cart_prices_main), alertify.success(t.msg), $('button[type="submit"]', e).remove()) : alertify.error(t.error);
                        },
                        "json"
                    ),
                    !1
                );
            }),
            body.on("submit", "#orderForm", function () {
                return !validateForm($(this));
            });
    }),
    $(document).delegate(".filters .form-check label", "click", function () {
        $(".filters .submit_filter").remove(), $(this).parent().append($('<button class="btn btn-dark submit_filter">Подобрать</button>'));
    }),
    $(document).delegate(".filters #brand_search", "keyup", function () {
        var e = $(this).val().toLowerCase();
        console.log(e),
            $(".filter-brands-list li").each(function (t) {
                $(this).data("name").toLowerCase().indexOf(e) < 0 ? $(this).hide() : $(this).show();
            });
    });
