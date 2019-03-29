(function () {

    notificacion = function (opciones) {
        opciones = $.extend({
            title: "Advertencia",
            icono: "",
            contenido: "Contenido para agregar",
            btnAceptar: "",
            btnCancelar: "",
            btnOk: "OK",
            mostrarBtnAceptarAndCancelar: "",
            mostrarBtnOk: "no",
            mostrarIcono: "",
            classTitleAndButtons: "dangerTitleAndButtons",
            classMessage: "dangerMessage",
        }, opciones);

        controlBotones();
        var contenido = "";
        contenido = `    <div class="pluginContenedor d-flex justify-content-center">
								<div class="bigBox-Fondo"></div>
								<div class="row justify-content-center bigBox-contenedor `+ opciones.classTitleAndButtons +`" align="center">
									<div class="col-12 m-0 p-0 ">
                                        <div class="row rowTitleAndButton">
											<div class="  col-12 mt-2 mb-2 d-flex align-items-center justify-content-between">
                                                <h2 class="tileNotification">` + opciones.title + `</h2>
											    <button type="button"  class="close close-alert">
                                                   <span aria-hidden="true">&times;</span>
                                                </button>
                                            </div>
										</div>
										<div class="row rowText `+ opciones.classMessage +`">
											<div class="bigBox-Contenido   pr-0 d-flex justify-content-strat align-items-center flex-wrap col-12">
												<span class="bigBox-Texto m-0 p-0">`+ opciones.contenido + `</span>
											</div>
											<div class="contentIcon col-2 ml-0 p-0 d-flex justify-content-start">
												<i class=" m-0 icon `+ opciones.icono + `"></i>	
											</div>
										</div>
										<div class="row rowTitleAndButton `+ opciones.mostrarBtnAceptarAndCancelar +  ` justify-content-between m-0 p-0">									
                                            <div class=" m-0  p-0 col-auto">
												<button class="pt-0 pb-0 mt-3 btn  bigBox-Cancelar" accesskey="c">`+ opciones.btnCancelar + `</button>	
											</div>			
											<div class=" m-0 p-0 col-auto">
												<button class="pt-0 pb-0 mt-3 btn e bigBox-Aceptar" accesskey="a">`+ opciones.btnAceptar + `</button>
											</div>	
                                        </div>
                                        <div class="row rowTitleAndButton justify-content-end m-0 p-0">
											<div class="`+ opciones.mostrarBtnOk + ` col-auto m-0 p-0">
												<button class="pt-0 pb-0 mt-3 btn  bigBox-Ok"><i class="icon-ok" accesskey="o"></i>`+ opciones.btnOk + `</button>
											</div>							
										</div>
									</div>															
								</div>
							</div>	`
        $("body").append(contenido);

        animar_entrada();
        
        controlBotones();
        controlIcono();
        controlTeclas();

        //Funcion de cancelar
        $("body").on("click", ".bigBox-Cancelar", function () {
            animar_salida();

        });
        $("body").on("click", ".bigBox-Ok", function () {
            animar_salida();

        });
        $("body").on("click", ".close-alert", function () {
            animar_salida();

        });

        $(window).resize(function () {
            $('.bigBox-contenedor').animate({

                zIndex: '100000000',
                top: 30,

            }, 0);

        });

        //Controlar los botones

        function controlBotones() {

            if (opciones.mostrarBtnAceptarAndCancelar === "si") {   
                
                opciones.mostrarBtnAceptarAndCancelar = ""

            } else if (opciones.mostrarBtnAceptarAndCancelar === "no") {
                opciones.mostrarBtnAceptarAndCancelar = "d-none"
            }

            if (opciones.mostrarBtnOk === "si") {
                opciones.mostrarBtnOk = ""
            } else if (opciones.mostrarBtnOk === "no") {
                opciones.mostrarBtnOk = "d-none"
            }
        }
        function controlIcono() {
            if (opciones.icono === "") {
                $(".contentIcon").remove();
            }
        }

        //Animar la entrada
        function animar_entrada() {
            var $fondo = $(".bigBox-Fondo");

            var $contenedor = $(".bigBox-contenedor");

            $fondo.show();
            $contenedor.show();


            $fondo.css({
                opacity: 0.7,
            })

            $(".pluginContenedor").css({
                opacity: 1,
            });

            $contenedor.animate({

                zIndex: '100000000',
                top: 30,
                opacity: 1,

            }, 500);
        }

        function animar_salida() {
            $('.bigBox-contenedor').animate({

                opacity: 0,
                top: 0,

            }, 300, function () {
                    $(".pluginContenedor").css({
                        opacity: 0,
                    }).remove();

                });

        }

        function controlTeclas() {
            $(document).keypress(function (e) {
                if (e.keyCode === 13) {
                    $(".bigBox-Aceptar").click();
                }
            });
            $(document).keyup(function (e) {
                if (e.which === 27) {
                    $(".bigBox-Cancelar").click();
                }
            });
            $(document).keypress(function (e) {
                if (e.keyCode === 13) {
                    $(".bigBox-Ok").click();
                }
            });
        }
    };

    alertafixed = function (options) {
        options = $.extend({
            title: "¡ AVISO !",
            type: "info",
            message: "",
            class: ""
        }, options);
        var cant = document.getElementsByClassName(options.class).length;

        class2 = options.class + cant;

        var contAlert = `<div id="contAlert" class="mt-5 mx-0">

                        </div>

                        `;

        var alertaTrue = `
                            <div class="row ${class2} ${options.class} alertBack back-${options.type}">
                                <div class="col-md-12">
                                    <h2>¡ ${options.title} !</h2>
                                    <p class="center m-0" style="color:#fff; font-size:0.9rem">${options.message}</p>
                                </div>
                            </div>
                            `;
        if ($("#contAlert").length == 0) {
            $(contAlert).appendTo($("body"));
        }

        $(alertaTrue).appendTo($("#contAlert"));
        $("." + class2).fadeOut(4000);
        setTimeout("$('." + class2 +"').remove();", 5000);
    }
})();