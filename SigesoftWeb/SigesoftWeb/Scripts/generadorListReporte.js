

function generateListReport(data) {

    var arrReport = [];
    for (var i = 0; i < data.length; i++) {
        var LiReport = `<li class="list-group-item custom-control custom-checkbox" data-service-id="${data[i].ServiceId}">
                            <input onchange="controlCheckbox(this)" class="check-report custom-control-input" type="checkbox" id="report_${data[i].ComponentId}" value="0" />
                            <label class="custom-control-label" for="report_${data[i].ComponentId}">${data[i].ComponentName}</label>
                        </li>
                        `;
        arrReport.push(LiReport);

    }
    $("#lista-report").html(arrReport.join(" "));
}

function controlCheckbox(checkbox) {

    
    if ($("#" + checkbox.id).val() == "0" || $("#" + checkbox.id).val() == 0) {
        $("#" + checkbox.id).val("1");
    } else {
        $("#" + checkbox.id).val("0");
    }

}

function GenerarReport() {
    
    if ($("#lista-report li").length > 0) {
        var arrComponentsReport = [];
        for (var i = 0; i < $("#lista-report li").length; i++) {
            var check = $("#lista-report li:eq(" + i + ") input").val();
            
            if (check == 1 || check == "1") {

                var componentId = $("#lista-report li:eq(" + i + ") input").attr("id").split("_")[1];
                var serviceId = $("#lista-report li:eq(" + i + ")").data("service-id");
                var obj = {
                    ComponentId: componentId,
                    ServiceId: serviceId,
                }
                arrComponentsReport.push(obj);
            }
        }

        if (arrComponentsReport.length > 0) {

            $(".js-cont-reports").addClass("loadingGrid");

            $.ajax({
                cache: false,
                method: 'POST',
                dataType: 'json',
                data: { 'oComponentsServiceId' : arrComponentsReport },
                url: '/PatientsAssistance/BuildReport',
                success: function (json) {
                    $(".js-cont-reports").removeClass("loadingGrid");
                    if (location.hostname === "localhost") {
                        var fileName = json + ".pdf";
                        var url = "http://localhost:1932/ESO/" + fileName;
                        window.open(url, '_blank');
                    } else {
                        var url = "https://www.omegavigilanciaservicios.com/ESO/" + json + ".pdf";
                        window.open(url, '_blank');
                    }
                    
                },
                error: function (error) {
                    console.log('ERRORR',error)
                }
            })

        } else {

            alertafixed({
                type: 'info',
                class: 'js-examen-true',
                title: 'Aviso',
                message: 'No ha seleleccionado ningún reporte.'
            });

        }
    } else {

        alertafixed({
            type: 'info',
            class: 'js-examen-true',
            title: 'Aviso',
            message: 'No hay reportes por generar.'
        });

    }
};
