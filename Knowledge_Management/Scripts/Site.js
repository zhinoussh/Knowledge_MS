$(document).ready(function () {



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

  
    

