$(document).ready(function () {
     $("#menu-toggle").click(function (e) {
         e.preventDefault();
         $("#wrapper").toggleClass("active");
         $("#main_icon").toggleClass('glyphicon-chevron-right').toggleClass('glyphicon-chevron-left');
     });

});