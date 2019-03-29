$(document).ready(function () {
    if ($("#input-reagendar").length > 0) {
        var now = new Date();
        var day = ("0" + now.getDate()).slice(-2);
        var month = ("0" + (now.getMonth() + 1)).slice(-2);
        var hour = ("0" + now.getHours()).slice(-2);
        var minutes = now.getMinutes();
        var today = now.getFullYear() + "-" + (month) + "-" + (day) + "T" + (hour) + ":" + (minutes);
        $("#input-reagendar").val(today);

    }

    $(".lbl-edit-Guardar").addClass("disabled");

    var width = $(".principal").width();
    $(".contenedor").css({
        width: width * 5
    });

    $(".contenedor-hijo").css({
        width: width
    });
    var camara = new Camara($('player')[0]);    

    
});

$(window).resize(function () {

    var width = $(".principal").width();
    $(".contenedor").css({
        width: width * 5
    });

    $(".contenedor-hijo").css({
        width: width
    });

});

//$("..perfil-photo").on("click", function () {
//    $(".container-photo-pop").animate({

//    }, 400);
//});
function ViewDetailNotification(content) {
    var noRead = parseInt($("#js-view-notification span").text());
    
    if (noRead != 0) {
        $("#js-view-notification span").text( noRead - 1)
    }
    var notificationId = $(content).find(".notificationId").val();
    ViewDetail(2, notificationId, $(content));

}

$("#morePhoto").change(function () {

    if (this.files.length > 0) {
        $(".lbl-edit-Guardar").removeClass("disabled")
    }

});

$(".lbl-edit-Guardar").on("click", function () {
    if ($(this).hasClass("disabled")) {
        console.log("disabled");
    } else {
        console.log("procede");
    }
});

function viewCertificados(posicion = 1) {

    $(".posicion").val(0);
    animate(posicion);

};

function selectPhotoProfile(posicion = 4) {

    $(".posicion").val(0);
    animate(posicion);

};

function BackProfile() {
    $(".posicion").val(-1);
    $("#profile").animate({
        marginLeft: '0px'
    }, 300)
    $("html, body").animate({
        scrollTop: 0
    });
};

function editProfile(posicion = 3) {
    $(".posicion").val(0);
    animate(posicion)
};

function cancelEditProfile() {

    $(".posicion").val(-1);

    $("#profile").animate({
        marginLeft: '0px'
    }, 300);

    $("html, body").animate({
        scrollTop: 0
    })

};

function ViewNotification(posicion) {
    $(".posicion").val(posicion);
    var id = $("#cont-notification");
    $("#js-view-notification").data("posicion", posicion);


    if (id.hasClass("active")) {

        id.removeClass("active");
        id.stop().animate({
            width: '0px',

        }, 200);

    } else {

        id.addClass("active");
        id.stop().animate({
            width: '100%'
        }, 200);
    }
    $("html, body").animate({
        scrollTop: 0
    })
};

function ViewDetail(posicion, notificationId, content) {
    $(".posicion").val(0);
    $.ajax({
        cache: false,
        method: 'GET',
        dataType: 'json',
        data: { 'notificationId': notificationId },
        url: '/WorkerData/GetNotification',
        success: function (json) {

            if (json.ScheduleDateString != null && json.ScheduleDateString != "") {
                var date = json.ScheduleDateString.split(" ");
                $(".fech-cita").parent().removeClass("d-none");
                $(".hora-cita").parent().removeClass("d-none");

                $(".fech-cita").text(date[0]);
                $(".hora-cita").text(date[1]);
            } else {
                $(".fech-cita").text("").parent().addClass("d-none");
                $(".hora-cita").text("").parent().addClass("d-none");

            }
            $(".fech-envio").text(json.NotificationDateString);
            $("#title-detail").text(json.Title);
            $(".medic-name").text(json.SystemUser);
            $(".tipo-cita").text(json.TypeNotification);
            $("#body-detail").text(json.Body);
            content.addClass("read");
            var id = $("#cont-notification");
            id.removeClass("active");
            $(".cont-notification").animate({
                width: '0px',
            }, 300);

            animate(posicion);
            IsRead(notificationId);
        },
        error: function (error) {
            console.log("ERROR: " , error)
        }


    })
    
};

function IsRead(notificationId) {
    $.ajax({
        cache: false,
        method: 'GET',
        dataType: 'json',
        data: { 'notificationId': notificationId },
        url: '/WorkerData/ReadNotification',
        success: function (json) {

        },
        error: function (error) {
            console.log("ERROR: ", error)
        }


    })
};

function BackNotification() {
    var posicion = 0;
    var id = $("#cont-notification");
    id.addClass("active");
    $(".cont-notification").animate({
        width: '100%',
    }, 300);


    animate(posicion)

};

function reAgendar() {


    $("#cont-modal").toggleClass("d-none");
    $("#modal-agendar").toggleClass("d-none");
    $("#modal-agendar").animate({
        opacity: 1,
        top: 180,
    }, 500);
    $("html, body").animate({
        scrollTop: 0
    })
};

function cancelReagendar() {

    $("#modal-agendar").animate({
        opacity: 0,
        top: 0,
    }, 500, function () {
        $("#cont-modal").toggleClass("d-none");
        $("#modal-agendar").toggleClass("d-none");
    });
    $("html, body").animate({
        scrollTop: 0
    })

};

history.pushState(null, document.title, location.href);

window.addEventListener('popstate', function (event) {
    var $posicion = $(".posicion").val();

    console.log($posicion);

    switch ($posicion) {
        case "0":
            animate(0);
            $(".posicion").val(-1);
            var id = $("#cont-notification");

            break;
        case "1":
            animate(1)
            break;
        case "2":
            animate(2)
            var id = $("#cont-notification");
            id.removeClass("active");
            $(".posicion").val(0);

            break;
        case "3":
            ViewNotification();
            $(".posicion").val(0);

            break;
        default:
            window.history.back()
            break;
    }
    history.pushState(null, document.title, location.href);
});

function animate(posicion) {

    var width = $(".principal").width();
    $("#profile").animate({
        marginLeft: width * -posicion
    }, 300);

    $("html, body").animate({
        scrollTop: 0
    })

};

//function selectphotoprofile() {
//    $("#cont-modal-edit").toggleClass("d-none");
//    $("#modal-photo-edit").toggleClass("d-none");
//    $("#modal-photo-edit").animate({
//        opacity: 1,
//        top: 180,
//    }, 500);
//    $("html, body").animate({
//        scrolltop: 0
//    })
//}
function editDataProfile() {

    $("#cont-modal-update").toggleClass("d-none");
    $("#modal-update").toggleClass("d-none");
    $("#modal-update").animate({
        opacity: 1,
        top: 180,
    }, 500);
    $("html, body").animate({
        scrolltop: 0
    })
};

function modalConfirmation() {
    if (!$(".reci-notification").hasClass("recibe-no")) {

        $("#cont-modal-unsubscrib").toggleClass("d-none");
        $("#modal-unsubscrib").toggleClass("d-none");
        $("#modal-unsubscrib").animate({
            opacity: 1,
            top: 180,
        }, 500);
        $("html, body").animate({
            scrolltop: 0
        })
    } else {
        var contalerta = `<div id="contAlert" class="mt-5 mx-0">
    
                         </div>`;
        var alerta = `
                    <div class="row alertaSubscri alertBack back-info">
                        <div class="col-md-12">
    	                    <h2 style="font-size: 0.8rem; color: #fff">Aviso!</h2>
                            <p  style="font-size: 0.7rem; color: #fff" class="center m-0">Usted no se encuentra subscrito.</p>                  
                        </div>
                    </div>

					`;
      
            $("#contAlert").remove();
            $(contalerta).appendTo($("#profile"));
            $(alerta).appendTo($("#contAlert"));

            $(".alertaSubscri").fadeOut(2000);
            setTimeout("$('.alertaSubscri').remove();", 3000);

        
    } 
};

function cancelModal(modal,fondoModal) {
    $("#" + modal).animate({
        opacity: 0,
        top: 0,
    }, 500, function () {
        $("#" + fondoModal).toggleClass("d-none");
        $("#" + modal).toggleClass("d-none");
    });
    $("html, body").animate({
        scrollTop: 0
    })
    if (modal == 'modal-update') {

        $(".row-validation-email").remove();
        $(".border-danger").removeClass("border-danger");
        $("#cellphone-worker").val($(".lbl-cell-phone").text());
        $("#email-worker").val($(".lbl-email").text());
    }
};

function cancelarSuscripcion() {

    $.ajax({
        cache: false,
        method: 'POST',
        dataType: 'json',
        url: '/WorkerData/UnSubscribe',
        success: function (json) {
            cancelModal('modal-unsubscrib', 'cont-modal-unsubscrib');
            $(".reci-notification").attr('src', 'Content/Images/bell2.png').addClass("recibe-no").attr('onclick', 'notificarme()');

        },
        error: function (error) {
            console.log("ERROR:", error);
        }
    });

};

function selectPhotoProfile() {

    //const camara = new Camara($('player')[0]);
    camara.encender();
}

function cancelPhotoProfile() {
    $("#modal-photo-edit").animate({
        opacity: 0,
        top: 0,
    }, 500, function () {
        $("#cont-modal-edit").toggleClass("d-none");
        $("#modal-photo-edit").toggleClass("d-none");
    });
    $("html, body").animate({
        scrollTop: 0
    })
    
    $(".lbl-edit-Guardar").addClass("disabled");
};

function cancelAddFoto () {
    animate(2)
};

function buscarValidacion(email) {
    var arrEmail = [...email];
    var encontrados = 0;

    var position = [];
    for (var i = 0; i < arrEmail.length; i++) {
        if (arrEmail[i] == '@' || arrEmail[i] == ' ') {
            encontrados = encontrados + 1;

            position.push(i);
        }
    };
    encontrados = arrEmail[0] == '@' ? encontrados + 1 : encontrados;
    var obj = {
        positions: position,
        encontrados: encontrados
    };
    
    return obj;
}