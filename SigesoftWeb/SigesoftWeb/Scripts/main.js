/// <reference path="../sw__1.js" />
/// <reference path="../sw__1.js" />
//const push = require('./push');
$(document).ready(main);

function resize() {
    if ($(window).width() < 480) {
        $("#wrapper").removeClass("active");
    }
}




function alerta(Mensaje, color) {
    if ($('.alert').css("display") === "none") {
        $('.alert').show();
        if (Mensaje !== undefined)
            $('.alert span').html(Mensaje);

        var stilo = "";
        if (color !== undefined) {
            switch (color) {
                case "negro": {
                    stilo = "alert-dark";
                    break;
                }
                case "blanco": {
                    stilo = "alert-light";
                    break;
                }
                case "rojo": {
                    stilo = "alert-danger";
                    break;
                }
                case "amarillo": {
                    stilo = "alert-warning";
                    break;
                }
                case "azul": {
                    stilo = "alert-info";
                    break;
                }
                case "verde": {
                    stilo = "alert-success";
                    break;
                }
            }
            $('.alert').addClass(stilo);
        }

        window.setTimeout(function () {
            $(".alert").fadeTo(500).slideUp(500, function () {
                if (stilo !== "")
                    $('.alert').removeClass(stilo);
                $(this).hide();
            });
        }, 1000);
    }
    else {
        $('.alert').hide();
    }
}

function formatDate(date) {
    var monthNames = [
        "Enero", "Febrero", "Marzo",
        "Abril", "Mayo", "Junio", "Julio",
        "Agosto", "Setiembre", "Octubre",
        "Noviembre", "Diciembre"
    ];

    var day = date.getDate();
    var monthIndex = date.getMonth();
    var year = date.getFullYear();

    return day + ' ' + monthNames[monthIndex] + ' ' + year;
}

function main() {

    $(".firstUl li li").click(function (e) {
        $(this).children(".submenu-ebene2").stop().slideToggle();
    });

    

    $("#click-registro-prueba").on('click', function (e) {
        e.preventDefault();
        $(".subtable").toggleClass("mostrar-tabla");
    });

    $(window).resize(resize);
    resize();

}

function validateNumber(evt) {
    var theEvent = evt || window.event;
    var key = theEvent.keyCode || theEvent.which;
    key = String.fromCharCode(key);
    var regex = /[0-9]|\./;
    if (!regex.test(key)) {
        theEvent.returnValue = false;
        if (theEvent.preventDefault) theEvent.preventDefault();
    }
}


function ColapseTable(divColapse, inputHidden) {

    var Altura = $("#" + divColapse).height();

    if ($("#" + divColapse).height() === 0) {
        $("#" + divColapse).animate({ "height": $('#' + inputHidden).val() + "px" }, 400);
       
    } else {
        $('#' + inputHidden).val(Altura);
        $("#" + divColapse).animate({ "height": "0px" }, 400);
        
    }
}
