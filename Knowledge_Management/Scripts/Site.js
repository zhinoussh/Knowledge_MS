﻿$(document).ready(function () {

    function toggleChevron(e) {

        $(e.target)
            .prev('.panel-heading')
            .find("i.indicator")
            .toggleClass('glyphicon-chevron-up glyphicon-chevron-down');
    }

    $('#accordion').on('hidden.bs.collapse', toggleChevron);
    $('#accordion').on('shown.bs.collapse', toggleChevron);

    if (localStorage.getItem("msg")) {
        $('.alert').slideDown(500);
        $('#alert_success').html(localStorage.getItem("msg"));
        localStorage.clear();
    }
});

$(document).on('click', '.showlogin', function() {
    $("#pnl_main").fadeOut();
    $("#pnl_about").addClass('hidden');
    $("#pnl_contact").addClass('hidden');
    $("#pnl_login").removeClass('hidden').addClass('zoomIn');
});

$(document).on('click', '.showAbout', function () {
    $("#pnl_main").fadeOut();
    $("#pnl_contact").addClass('hidden');
    $("#pnl_login").addClass('hidden');
    $("#pnl_about").removeClass('hidden').addClass('zoomIn');
});

$(document).on('click', '.showContact', function () {
    $("#pnl_main").fadeOut();
    $("#pnl_about").addClass('hidden');
    $("#pnl_login").addClass('hidden');
    $("#pnl_contact").removeClass('hidden').addClass('zoomIn');
});

$(document).on('keypress', '.form-control', function () {
      $('.log-status').removeClass('wrong-entry');
});

$(document).on('click', '#close_alert', function () {
    $("#div_alert").slideUp(500);
    return false;
});

var onLoginSuccess = function (result) {
    if (result.url) {
        // if the server returned a JSON object containing an url 
        // property we redirect the browser to that url
        window.location.href = result.url;
    }
    else {
        $('.log-status').addClass('wrong-entry');
        $('.alert').fadeIn(500);
        setTimeout("$('.alert').fadeOut(1500);", 3000);
    }
}

var Success_ProfileChange = function (result) {
    if (result.msg) {
        //$('.alert').slideDown(500);
        //$('#alert_success').html(result.msg);
        localStorage.setItem("msg", result.msg);
        location.reload();
    }
}
  
    

