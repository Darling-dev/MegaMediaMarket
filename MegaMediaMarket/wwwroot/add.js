$( document ).ready(function() {

    $('body').on('click touch', '.crypto_btn', function(){
        if($('.payment_master').hasClass('disabled')){
            $('.payment_master').removeClass('disabled');
            $('.crypto_btn').addClass('active');
        }
    });

    $('.burgerLink').on('click touch', function(){
        mobileMenu = $('.mobile_bar');
        if(mobileMenu.hasClass('active')){
            mobileMenu.removeClass('active');
        }
        else{
            mobileMenu.addClass('active');
        }
    });
    $('.mobile_bar-close').on('click touch', function(){
        mobileMenu = $('.mobile_bar');
        if(mobileMenu.hasClass('active')){
            mobileMenu.removeClass('active');
        }
    });
    $('.mobile_bar-out').on('click touch', function(){
        mobileMenu = $('.mobile_bar');
        if(mobileMenu.hasClass('active')){
            mobileMenu.removeClass('active');
        }
    });

    SVGInject(document.getElementsByClassName('js-inject'));

    $('.payment_selector').on('change', function() {
        valchange = this.value;
        newhref = $(this).parent().find('.buynow_link').attr('href').split('&type')[0];
        $(this).parent().find('.buynow_link').attr('href', newhref+'&type='+valchange);
        newhreflink = $(this).parent().find('.buynow_link').attr('href').split('&code')[0];
        valcode = $('.buyModal input.promocode').val();
        if(valcode.length > 0){
            $(this).parent().find('.buynow_link').attr('href', newhreflink+'&code='+valcode);
        }
        checkval = $(this).parent().find('.profilecheck').val();
        if(checkval.length > 0){
            linkOld = $(this).parent().find('.buynow_link').attr('href');
            if($(this).parent().find('.profilecheck').is(":checked")){
                $(this).parent().find('.buynow_link').attr('href', linkOld+'&profile=1');
            }
            else {
                $(this).parent().find('.buynow_link').attr('href', linkOld+'&profile=0');
            }
        }
    });

    $(".profilecheck").change(function() {
        var linkButton = $(this).parent().parent().find('.buynow_link');
        if($(this).is(":checked")){
            linkButton.attr('href', linkButton.attr('href').split('&profile')[0]+'&profile=1');
        }
        else {
            linkButton.attr('href', linkButton.attr('href').split('&profile')[0]+'&profile=0');
        }
    });

    $('input[type="tel"]').inputmask("79999999999");

    $('.mailInput').keyup(function(){
        if($('.phoneNumber').length){
            newBuyHref = $('.paymentBtn').attr('href').split('&mail')[0]+'&mail='+$('.mailInput').val()+'&phone='+$('.phoneNumber').val();
            $('.paymentBtn').attr('href', newBuyHref);
            if($('.phoneNumber').val().length == 11){
                if($('.mailInput').val().length > 5){
                    $('.paymentBtn').removeClass('disabled');
                }
                else {
                    if(!$('.paymentBtn').hasClass('disabled')){
                        $('.paymentBtn').addClass('disabled');
                    }
                }
            }
            else {
                if(!$('.paymentBtn').hasClass('disabled')){
                    $('.paymentBtn').addClass('disabled');
                }
            }
        }
        else {
            newBuyHref = $('.paymentBtn').attr('href').split('&mail')[0]+'&mail='+$('.mailInput').val();
            $('.paymentBtn').attr('href', newBuyHref);
            if($('.mailInput').val().length > 5){
                $('.paymentBtn').removeClass('disabled');
            }
            else {
                if(!$('.paymentBtn').hasClass('disabled')){
                    $('.paymentBtn').addClass('disabled');
                }
            }  
        }
    });
      
    $('.phoneNumber').keyup(function(){
        if($('.mailInput').length){
            newBuyHref = $('.paymentBtn').attr('href').split('&mail')[0]+'&mail='+$('.mailInput').val()+'&phone='+$('.phoneNumber').val();
            $('.paymentBtn').attr('href', newBuyHref);
            if($('.mailInput').val().length > 5){
                if($('.phoneNumber').val().length == 11){
                    $('.paymentBtn').removeClass('disabled');

                }
                else {
                    if(!$('.paymentBtn').hasClass('disabled')){
                        $('.paymentBtn').addClass('disabled');
                    }
                }
            }
            else {
                if(!$('.paymentBtn').hasClass('disabled')){
                    $('.paymentBtn').addClass('disabled');
                }
            }
        }
        else {
            newBuyHref = $('.paymentBtn').attr('href').split('&mail')[0]+'&mail=&phone='+$('.phoneNumber').val();
            $('.paymentBtn').attr('href', newBuyHref);
            if($('.phoneNumber').val().length == 11){
                $('.paymentBtn').removeClass('disabled');
            }
            else {
                if(!$('.paymentBtn').hasClass('disabled')){
                    $('.paymentBtn').addClass('disabled');
                }
            }  
        }
    });
    
	$('.sub__button').on('click touch', function(){
		dataTab = $(this).data('tab');
		$('.sub__button').removeClass('sub__button-active');
		$(this).addClass('sub__button-active');
		$('.sub_tabs .sub_tab').removeClass('active');
		$('.sub_tabs .sub_tab[data-tab="'+dataTab+'"]').addClass('active');
	});

	$('.searchBtn').on('click touch', function(){
		$('#searchmodal').modal({
		  fadeDuration: 100
		});
        setTimeout(function(){
            $('.blurInput').focus();
        },200);
	});

    $('.instructionRun').on('click touch', function(){
        $('#instructionModal').modal({
          fadeDuration: 100
        });
    });

    $('.openPayment').on('click touch', function(){
        $('#buyModal').modal({
          fadeDuration: 100
        });
        tabnum = ( $('.sub__cont .sub__button-active').data('tab') - 1 );
        linkP1 = $('.buyModal .buynow_link').attr('href').split('&tab=')[0];
        linkP2 = $('.buyModal .buynow_link').attr('href').split('&lang')[1];
        newhref = linkP1+'&tab='+tabnum+'&lang'+linkP2;
        $('.buyModal .buynow_link').attr('href', newhref);
    });

    $('.openPaymentEU').on('click touch', function(){

        $('#buyModalEu').modal({
          fadeDuration: 100
        });

        $('.buyModalEu a.other_btn').attr('href', $(this).data('link'));

        tabnum = ( $('.sub__cont .sub__button-active').data('tab') - 1 );
        

        linkP1 = $('.buyModal .buynow_link').attr('href').split('&tab=')[0];
        linkP2 = $('.buyModal .buynow_link').attr('href').split('&lang')[1];
        newhref = linkP1+'&tab='+tabnum+'&lang'+linkP2;
        $('.buyModalEu .buynow_link').attr('href', newhref);
    });

    $('.buyModal input.promocode').keyup(function(){
        newhref = $(this).parent().find('.buynow_link').attr('href').split('&code')[0];
        valcode = $('.buyModal input.promocode').val();
        $('.buyModal .buynow_link').attr('href', newhref+'&code='+valcode);
    });

    $('.instructionEnRun').on('click touch', function(){
        $('#instructionModalEn').modal({
          fadeDuration: 100
        });
    });

    $('.select__start').on('click touch', function(){
        $('.new__select-wrapper .select__wrapper').animate({scrollLeft: 0}, 500);
    });
    $('.select__end').on('click touch', function(){
        
        scrollLeftNow = $('.new__select-wrapper .select__wrapper').scrollLeft() + $('.new__select-wrapper .select__wrapper').width();
        $('.new__select-wrapper .select__wrapper').animate({scrollLeft: scrollLeftNow}, 500);
    });

	$('.translateRu').on('click touch', function(){
       $.ajax({
            url: '/ajax/lang',
			data: {
                lang: 'ru',
            },
            success: function(data){
                if(data.success){
                    setTimeout(function(){
                    	// location.reload();
                        if($(location).attr('href').search('profile') != -1){
                            location.reload();
                        }
                        else {
                            if($(location).attr('href').search('pay') != -1){
                                location.reload();
                            }
                            else {
                                if($(location).attr('href').search('.xyz/en') != -1){
                                    window.location.href = $(location).attr('href').split('.xyz/en')[1];
                                }
                                else {
                                    window.location.href = $(location).attr('href').split('.xyz')[1];
                                }
                            }
                        }
                    },300);
                }
                else {
                    alertify.error('#1 Error changing LANG');
                }
            },
            type: "POST", dataType: "json"
        });
	});
	$('.translateEn').on('click touch', function(){
       $.ajax({
            url: '/ajax/lang',
			data: {
                lang: 'en',
            },
            success: function(data){
                if(data.success){
                    setTimeout(function(){
                    	// location.reload();
                        if($(location).attr('href').search('profile') != -1){
                            location.reload();
                        }
                        else {
                            if($(location).attr('href').search('pay') != -1){
                                location.reload();
                            }
                            else {
                                if($(location).attr('href').search('.xyz/en') != -1){
                                    window.location.href = '/en'+$(location).attr('href').split('.xyz/en')[1];
                                }
                                else {
                                     window.location.href = '/en'+$(location).attr('href').split('.xyz')[1];
                                }
                            }
                        }
                    },300);
                }
                else {
                    alertify.error('#1 Error changing LANG');
                }
            },
            type: "POST", dataType: "json"
        });
	});
});