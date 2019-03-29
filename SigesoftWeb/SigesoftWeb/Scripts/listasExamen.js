

function eventodrop(event, ui) {

        /*Data Control*/
    
    var tabId = ui.draggable.data("tab-id"),
        componentId = ui.draggable.data("component-id"),
        nameComponent = ui.draggable.data("name-component"),
        nameCategory = ui.draggable.data("name-category"),
        liId = ui.draggable.attr("id"),
        cantGroup = $(".js-tab-" + tabId + " .js-group-component").length;
    if (!$("#list-examen_" + tabId).hasClass('culminado')) {
        if ($("#examen_" + tabId).length > 0) {

            if ($("#examen_" + tabId).hasClass("d-none")) {
                $("#list-examen_" + tabId).removeClass("no-activo");
                $("#examen_" + tabId).removeClass("d-none");
            };

            if ($("#" + componentId).length > 0) {
                if ($("#" + componentId).hasClass("d-none")) {

                    $("#" + componentId).removeClass("d-none").removeClass('deleted-true');

                    for (var i = 0; i < $("#" + componentId + " div.row-cont-subgroup").length; i++) {

                        for (var x = 0; x < $("#" + componentId + " div.row-cont-subgroup div input").length; x++) {
                            $("#" + componentId + " div.row-cont-subgroup div input:eq(" + x + ")").removeAttr("data-is-delete").attr("data-is-delete", "0");
                        }

                        for (var x = 0; x < $("#" + componentId + " div.row-cont-subgroup div select").length; x++) {
                            $("#" + componentId + " div.row-cont-subgroup div select:eq(" + x + ")").removeAttr("data-is-delete").attr("data-is-delete", "0");
                        }

                        for (var x = 0; x < $("#" + componentId + " div.row-cont-subgroup div textarea").length; x++) {
                            $("#" + componentId + " div.row-cont-subgroup div textarea:eq(" + x + ")").removeAttr("data-is-delete").attr("data-is-delete", "0");
                        }


                    }


                    var remove = `<i style='color: red; cursor: pointer;' class=' ml-2 fas fa-times-circle' id='icon-${componentId}' onclick='return deleteField(this , "${liId}")'></i>`;
                    $("#" + liId).toggleClass("lista-examen");
                    $(remove).appendTo($("#" + liId));

                    alertafixed({
                        type: 'info',
                        class: 'js-' + componentId + '-recup',
                        title: 'Aviso',
                        message: 'Se recuper&oacute los datos de ' + nameComponent + '.'
                    });

                } else {

                    alertafixed({
                        type: 'warning',
                        class: 'js-' + tabId + '-true',
                        title: 'Validaci&oacuten',
                        message: 'Ya existe el componente ' + nameComponent + '.'
                    });

                }

            } else {

                var Group = `
                            <div class="col-12 js-group-component" id="${componentId}">
                                <div class="row px-2 pt-3 contRows w-100 mx-0 justify-content-between justify-content-md-start" id="groupComponent-${tabId}-${componentId}">
                                    <label class="lblName js-group-name">${nameComponent}</label>                                                                      
                                </div> 
                            </div> 
                            `;


                $(Group).appendTo($("#tab_" + tabId));


                var subGroups = [];
                var idsubgroups = [];
                var listas = ui.draggable.children("ul");

                for (var i = 0; i < listas.children("li").length; i++) {
                    var datas = listas.children("li:eq(" + i + ")");

                    var nameSubgroup = datas.data("name-subgroup");
                    var SubgroupId = datas.data("field-id");
                    if ($.inArray(nameSubgroup, subGroups) == -1) {
                        subGroups.push(nameSubgroup);
                        idsubgroups.push(SubgroupId);
                    }

                }

                for (var x = 0; x < idsubgroups.length; x++) {

                    var GroupSubGroup = `
                                        <div class="col-12 p-0">
                                            <div class="p-1 pt-3 row-scroll contRows w-100 mx-0 justify-content-between justify-content-md-start" >
                                                <label id="label-${tabId}${cantGroup}" class="lblName">${subGroups[x]}</label>  
                                                <div class="m-0 row-subgroup p-0"" >
                                                    <div class="row align-items-end m-0 row-cont-subgroup p-0" id="${idsubgroups[x]}" >
                                                
                                                    </div>
                                                </div>                                                                                                                                                         
                                            </div> 
                                        </div> 
                                        `;

                    $(GroupSubGroup).appendTo($("#groupComponent-" + tabId + "-" + componentId));

                    for (var y = 0; y < listas.children("li").length; y++) {

                        var datas = listas.children("li:eq(" + y + ")");
                        var nameSubgroup = datas.data("name-subgroup");
                        if (nameSubgroup == subGroups[x]) {

                            var controlId = parseInt(datas.data("control-id")),
                                column = parseInt(datas.data("column")),
                                width = datas.data("width"),
                                height = datas.data("height"),
                                label = datas.data("label"),
                                labelWidth = datas.data("label-width"),
                                idComponentField = datas.data("field-id"),
                                min = datas.data("min"),
                                max = datas.data("max"),
                                cantDecimal = datas.data("cant-decimal"),
                                liId = ui.draggable.attr("id"),
                                TabId = datas.data("tab-id"),
                                defaultText = datas.data("default-text"),
                                defaultValues = datas.data("values"),
                                implicados = datas.data("idimplicados"),
                                idinputformula = datas.data("idcalculate"),
                                formula = datas.data("formula");

                            var obj = {
                                controlId: controlId,
                                column: column,
                                width: width,
                                height: height,
                                label: label,
                                labelWidth: labelWidth,
                                idComponentField: idComponentField,
                                idComponentFieldSubGroup: idsubgroups[x],
                                min: min,
                                max: max,
                                cantDecimal: cantDecimal,
                                nameComponent: nameComponent,
                                liId: liId,
                                componentId: componentId,
                                defaultText: defaultText,
                                defaultValuesCombo: JSON.stringify(defaultValues),
                                isDelete: 0,
                                implicados: JSON.stringify(implicados),
                                idinputformula: idinputformula,
                                formula: formula

                            };

                            switchComponent(obj);

                        }

                    }


                }
                var remove = `<i style='color: red; cursor: pointer;' class=' ml-2 fas fa-times-circle' id='icon-${componentId}' onclick='return deleteField(this , "${liId}")'></i>`;

                $("#" + liId).toggleClass("lista-examen");

                $(remove).appendTo($("#" + liId));

                alertafixed({
                    type: 'success',
                    class: 'js-' + componentId + '-true',
                    title: 'Hecho',
                    message: nameComponent + ' se agreg&oacute correctamente.'
                });
            }

        } else {


            var Tab = `
                        
					    <div class="px-1 tab-pane j-tabPane-examen fade  w-100 mx-0" id="examen_${tabId}" role="tabpanel" aria-labelledby="list-examen_${tabId}" style="position: relative !important;">                            
                                <ul class="nav nav-tabs" id="myTab" role="tablist">
                                    <li class="nav-item">
                                        <a class="nav-link js-lbl active" id="${tabId}-tab" data-toggle="tab" href="#tab_${tabId}" role="tab" aria-controls="tab_${tabId}" aria-selected="true">${nameCategory}</a>
                                    </li>
                                </ul>
                                <div class="tab-content" id="myTabContent_${tabId}">
                                  <div  data-front="FRONTEND" class="tab-pane fade show active" id="tab_${tabId}" role="tabpanel" aria-labelledby="${tabId}-tab"></div>
                                </div>
						</div>
							
						`;


            var contenidoDinamicNav = `<li  class="text-center" style="position: relative;" id="${tabId}"><a href="#examen_${tabId}" onclick="return addActiveList(event, this)" class="list-group-item js-anchor-tag list-group-item-action" id="list-examen_${tabId}" data-toggle="list" role="tab" aria-controls=${tabId}">${nameCategory}</a></li>`;

            if ($("#list-examen_" + tabId + "").length > 0) {
                $("#list-examen_" + tabId + "").parent().remove();
            }
            $(Tab).appendTo($("#nav-tabContent"));
            $(contenidoDinamicNav).appendTo($("#ulDinamic"));

            var ulHeight = $("#ulDinamic").height();
            var listHeight = $("#ulDinamic li:eq(0)").height();

            $("#ulDinamic").css({
                height: ulHeight + listHeight
            });
            var Group = `
                            <div class="col-12 js-group-component" id="${componentId}">
                                <div class="row px-2 pt-3 contRows w-100 mx-0 justify-content-between justify-content-md-start" id="groupComponent-${tabId}-${componentId}">
                                    <label class="lblName js-group-name">${nameComponent}</label>                                                                      
                                </div> 
                            </div> 
                            `;


            $(Group).appendTo($("#tab_" + tabId));


            var subGroups = [];
            var idsubgroups = [];
            var listas = ui.draggable.children("ul");

            for (var i = 0; i < listas.children("li").length; i++) {
                var datas = listas.children("li:eq(" + i + ")");
                var SubgroupId = datas.data("field-id");
                var nameSubgroup = datas.data("name-subgroup");
                if ($.inArray(nameSubgroup, subGroups) == -1) {
                    subGroups.push(nameSubgroup);
                    idsubgroups.push(SubgroupId);
                }
            }

            for (var x = 0; x < idsubgroups.length; x++) {

                var GroupSubGroup = `
                                        <div class="col-12 p-0">
                                            <div class="p-1 pt-3 row-scroll contRows w-100 mx-0 justify-content-between justify-content-md-start" >
                                                <label id="label-${tabId}${cantGroup}" class="lblName">${subGroups[x]}</label>  
                                                <div class="m-0 row-subgroup p-0"" >
                                                    <div class="row align-items-end m-0 row-cont-subgroup p-0" id="${idsubgroups[x]}" >
                                                
                                                    </div>
                                                </div>                                                                                                                                                         
                                            </div> 
                                        </div> 
                                        `;

                $(GroupSubGroup).appendTo($("#groupComponent-" + tabId + "-" + componentId));

                for (var y = 0; y < listas.children("li").length; y++) {

                    var datas = listas.children("li:eq(" + y + ")");

                    var datas = listas.children("li:eq(" + y + ")");
                    var nameSubgroup = datas.data("name-subgroup");

                    if (nameSubgroup == subGroups[x]) {

                        var controlId = parseInt(datas.data("control-id")),
                            column = parseInt(datas.data("column")),
                            width = datas.data("width"),
                            height = datas.data("height"),
                            label = datas.data("label"),
                            labelWidth = datas.data("label-width"),
                            idComponentField = datas.data("field-id"),
                            min = datas.data("min"),
                            max = datas.data("max"),
                            cantDecimal = datas.data("cant-decimal"),
                            defaultText = datas.data("default-text"),
                            defaultValues = datas.data("values"),
                            TabId = datas.data("tab-id"),
                            implicados = datas.data("idimplicados"),
                            idinputformula = datas.data("idcalculate"),
                            formula = datas.data("formula");

                        var obj = {
                            controlId: controlId,
                            column: column,
                            width: width,
                            height: height,
                            label: label,
                            labelWidth: labelWidth,
                            idComponentField: idComponentField,
                            idComponentFieldSubGroup: idsubgroups[x],
                            min: min,
                            max: max,
                            cantDecimal: cantDecimal,
                            nameComponent: nameComponent,
                            componentId: componentId,
                            defaultText: defaultText,
                            defaultValuesCombo: JSON.stringify(defaultValues),
                            isDelete: 0,
                            implicados: JSON.stringify(implicados),
                            idinputformula: idinputformula,
                            formula: formula
                        };

                        switchComponent(obj);

                    }

                }

            }
            var remove = `<i style='color: red; cursor: pointer;' class=' ml-2 fas fa-times-circle' id='icon-${componentId}' onclick='return deleteField(this , "${liId}")'></i>`;

            $("#" + liId).toggleClass("lista-examen");

            $(remove).appendTo($("#" + liId));

            $("#" + tabId + " a").click();

            alertafixed({
                type: 'success',
                class: 'js-' + componentId + '-true',
                title: 'Hecho',
                message: nameComponent + ' se agreg&oacute correctamente.'
            });

        };
    } else {
        alertafixed({
            type: 'info',
            class: 'js-culminado',
            title: 'Aviso',
            message: 'Esta categoria se encuentra culminada, no se puede realizar cambios.'
        });
    }
        


}

function colapse(icon) {
    if ($("#" + icon.id).hasClass("fa-plus")) {
        $("#" + icon.id).siblings().stop().animate({
            height: '100%',
        }, 300)
    } else {
        $("#" + icon.id).siblings().stop().animate({
            height: '0px'
        }, 300)
    }
    
    $("#" + icon.id).toggleClass("fa-plus").toggleClass("fa-minus");
}

function switchComponent(obj) {
    obj.formula = JSON.stringify(obj.formula);

    var componenteHtml = "";
    switch (obj.controlId) {
        case 1:
            if (obj.idComponentFieldSubGroup == 'N009-MF000004177' || obj.idComponentFieldSubGroup == 'N009-MF000004145') {
                obj.labelWidth = 100;
                obj.column = 8;
                obj.height = 20;
                obj.width = 60;
            } 
            componenteHtml = `<div class="p-1 col-fields-${obj.column} d-inline-block align-content-center my-1">
                                        
                                        <label class="d-inline-block align-self-center mr-1 lbl-dinamic my-0" style="width:${obj.labelWidth}px !important;"  for="field-${obj.idComponentField}">${obj.label}</label> 
                                        <input data-is-delete="${obj.isDelete}" value="${obj.defaultText}" data-componentid="${obj.componentId}" class="form-control d-inline-block input-dinamic" style="width:${obj.width}px !important;height:${obj.height}px!important" type="text" id="field_${obj.idComponentField}" >
                                  
                            </div>`;
            break;

        case 2:
            componenteHtml = `<div class="p-1 col-fields-${obj.column} d-inline-block align-content-center my-1">
                            <label style="width:${obj.labelWidth}px !important;" class="d-inline-block mr-1 align-self-center lbl-dinamic my-0" for="field-${obj.idComponentField}">${obj.label}</label> 
                            <textarea data-is-delete="${obj.isDelete}" data-componentid="${obj.componentId}"  class="d-inline-block form-control" style="width:${obj.width}px !important;height:${obj.height}px!important" id="field_${obj.idComponentField}" >${obj.defaultText}</textarea>
                          </div>
                          `;
            break;

        case 3:
            componenteHtml = `<div class="p-1 col-fields-${obj.column} d-inline-block align-content-center my-1">
                                    
                                    <label style="width:${obj.labelWidth}px !important;" class="d-inline-block mr-1 align-self-center lbl-dinamic my-0" for="field-${obj.idComponentField}">${obj.label}</label> 
                                    <input data-idimplicados='${obj.implicados}' data-idcalculate='${obj.idinputformula}' data-formula='${obj.formula}' data-is-delete="${obj.isDelete}" data-is-delete="${obj.isDelete}" data-componentid="${obj.componentId}" value="${obj.defaultText}" class=" d-inline-block form-control input-dinamic" type="text"  min="${obj.min}" max="${obj.max}"  onblur="return validateInput(event, this, ${obj.cantDecimal});" onkeyup="return soloEnteros(event, this)"  style="width:${obj.width}px !important;height:${obj.height}px!important" id="field_${obj.idComponentField}" >

                                  </div>`;

            break;

        case 4:
            componenteHtml = `<div class="p-1 col-fields-${obj.column} d-inline-block align-content-center my-1">
                                    
                                    <label style="width:${obj.labelWidth}px !important;" class="d-inline-block mr-1 align-self-center lbl-dinamic my-0" for="field-${obj.idComponentField}">${obj.label}</label> 
                                    <input data-idimplicados='${obj.implicados}' data-idcalculate='${obj.idinputformula}' data-formula='${obj.formula}'  data-is-delete="${obj.isDelete}" data-componentid="${obj.componentId}" value="${obj.defaultText}" class=" d-inline-block form-control input-dinamic" min="${obj.min}" max="${obj.max}" onblur="return validateInput(event, this, ${obj.cantDecimal});"  onkeypress="return filterFloat(event,this,${obj.cantDecimal});" style="width:${obj.width}px !important;height:${obj.height}px!important" type="text" id="field_${obj.idComponentField}" >

                                  </div>`;
            break;

        case 5:
            componenteHtml = `<div class="p-1 col-fields-${obj.column} d-inline-block align-content-center my-1">
                            <label style="width:${obj.labelWidth}px !important;" class="d-inline-block align-self-center mr-1 lbl-dinamic my-0" for="${obj.idComponentField}">${obj.label}</label> 
                            <input data-is-delete="${obj.isDelete}" data-componentid="${obj.componentId}" class=" d-inline-block custom-checkbox" type="checkbox" id="field_${obj.idComponentField}" >
                          </div>
                        `;
            break;

        case 6:
            componenteHtml = `<div class="p-1 col-fields-${obj.column} d-inline-block align-content-center form-check my-1">      
                                    
                                    <label style="width:${obj.labelWidth}px !important;" class="d-inline-block align-self-center form-check-label lbl-dinamic ">SI</label>                                  
                                    <input data-is-delete="${obj.isDelete}" data-componentid="${obj.componentId}" class=" d-inline-block form-check-input" type="radio" name="${obj.idComponentField}" id="${obj.idComponentField}" >

                                    <label style="width:${obj.labelWidth}px !important;" class="d-inline-block align-self-center form-check-label lbl-dinamic ">NO</label>
                                    <input data-is-delete="${obj.isDelete}" data-componentid="${obj.componentId}" class=" d-inline-block form-check-input" type="radio" name="${obj.idComponentField}" id="${obj.idComponentField}" >
                                  
                                  </div>
                              `;
            break;
        case 7:
            if (obj.idComponentFieldSubGroup == 'N009-MF000004177' || obj.idComponentFieldSubGroup == 'N009-MF000004145') {
                obj.labelWidth = 100;
                obj.column = 8;
                obj.height = 20;
                obj.width = 60;
                var text = '-Slct-';
            } else {
                var text = '--Select--';
            }
            componenteHtml = `<div class="p-1 col-fields-${obj.column} d-inline-block align-content-center my-1">
                                    <label style="width:${obj.labelWidth}px;" class="d-inline-block align-self-center lbl-dinamic mr-1 my-0" for="field_cb_${obj.idComponentField}">${obj.label}</label> 
                                    <select data-is-delete="${obj.isDelete}" data-componentid="${obj.componentId}" class="d-inline-block custom-select input-dinamic" style="width:${obj.labelWidth}px !important;height:${obj.height}px!important" id="field_cb_${obj.idComponentField}" >
                                    <option value="-1">${text}</option>

                                    </select>

                            </div>`;
            break;

        case 8:
            componenteHtml = `
                                <div class="custom-file" style="width:150px !important ">
                                    <input data-is-delete="${obj.isDelete}" data-componentid="${obj.componentId}" type="file" class=" d-inline-block custom-file-input" id="field_${obj.idComponentField}">
                                    <label style="font-size:0.6rem !important; vertical-align: middle !important" class="custom-file-label" for="field_${obj.idComponentField}">${obj.label}</label>
                                </div>
                                `;
            break;


        case 9:
            if (obj.idComponentFieldSubGroup == 'N009-MF000004177' || obj.idComponentFieldSubGroup == 'N009-MF000004145') {
                obj.labelWidth = 100;
                obj.column = 8;
                obj.height = 20;
                obj.width = 60;
                var text = '-Slct-';
            } else {
                var text = '--Select--';
            }
            componenteHtml = `<div class="p-1 col-fields-${obj.column} d-inline-block align-content-center my-1">
                                     <label style="width:${obj.labelWidth}px !important;" class="d-inline-block align-self-center lbl-dinamic mr-1 my-0" for="field_${obj.idComponentField}">${obj.label}</label> 
                                     <select data-is-delete="${obj.isDelete}" data-componentid="${obj.componentId}" value="${obj.defaultText}" class=" d-inline-block custom-select input-dinamic" style="width:${obj.width}px !important;height:${obj.height}px!important" id="field_cb_${obj.idComponentField}" >
                                        <option value="-1">${text}</option>

                                     </select>

                                </div>`;
            break;
        case 10:
            var arrViaAereaL = [];
            var arrEMAL = [];
            var arrEMOL = [];
            var arrViaOseaL = [];
            var arrViaAereaR = [];
            var arrEMAR = [];
            var arrEMOR = [];
            var arrViaOseaR = [];
            for (let i = 0; i < 9; i++) {
                var valuesOI = {
                    idInput: 'ViaAereaL-' + i + 1,
                    valueInput: "",
                    componentId: "N002-ME000000005",
                };
                arrViaAereaL.push(valuesOI);
            };
            for (let x = 0; x < 9; x++) {
                var valuesOI = {
                    idInput: 'EMAL-' + x + 1,
                    valueInput: "",
                    componentId: "N002-ME000000005",
                };
                arrEMAL.push(valuesOI);
            };
            for (let i = 0; i < 9; i++) {
                var valuesOI = {
                    idInput: 'EMOL-' + i + 1,
                    valueInput: "",
                    componentId: "N002-ME000000005",
                };
                arrEMOL.push(valuesOI);
            };
            for (let x = 0; x < 9; x++) {
                var valuesOI = {
                    idInput: 'ViaOseaL-' + x + 1,
                    valueInput: "",
                    componentId: "N002-ME000000005",
                };
                arrViaOseaL.push(valuesOI);
            };
            for (let i = 0; i < 9; i++) {
                var valuesOD = {
                    idInput: 'ViaAereaR-' + i + 1,
                    valueInput: "",
                    componentId: "N002-ME000000005",
                };
                arrViaAereaR.push(valuesOD);
            };
            for (let x = 0; x < 9; x++) {
                var valuesOD = {
                    idInput: 'EMAR-' + x + 1,
                    valueInput: "",
                    componentId: "N002-ME000000005",
                };
                arrEMAR.push(valuesOD);
            };
            for (let i = 0; i < 9; i++) {
                var valuesOD = {
                    idInput: 'EMOR-' + i + 1,
                    valueInput: "",
                    componentId: "N002-ME000000005",
                };
                arrEMOR.push(valuesOD);
            };
            for (let x = 0; x < 9; x++) {
                var valuesOD = {
                    idInput: 'ViaOseaR-' + x + 1,
                    valueInput: "",
                    componentId: "N002-ME000000005",
                };
                arrViaOseaR.push(valuesOD);
            };

            componenteHtml = `
                                                    <div class="col-12 p-0">
                                                        <div class="row">
                                                            <div class="col-6  p-0">
                                                                <div class="row w-100 m-0 p-0">
                                                                    <article class="col-12 d-flex justify-content-center align-items-center">
                                                                        <h5>OI</h5>
                                                                    </article>
                                                                    <section class="col-2 pt-3">
                                                                        <label class="lbl-disa">V&iacutea A&eacuterea</label>  
                                                                        <label class="lbl-disa">EM-A</label>
                                                                        <label class="lbl-disa">V&iacutea &Oacutesea</label>
                                                                        <label class="lbl-disa">EM-O</label>                                                                      
                                                                    </section>
                                                                    <section class="col-10 p-0" id="contAudiLeft">
                                                                        <aside class="row w-100 m-0 p-0 justify-content-between">
                                                                            <article class="col-auto art-lbl p-0">
                                                                                <label class="lbl-oiod">125Hz</label>
                                                                            </article>
                                                                            <article class="col-auto art-lbl p-0">
                                                                                <label class="lbl-oiod">250Hz</label>
                                                                            </article>
                                                                            <article class="col-auto art-lbl p-0">
                                                                                <label class="lbl-oiod">500Hz</label>
                                                                            </article>
                                                                            <article class="col-auto art-lbl p-0">
                                                                                <label class="lbl-oiod">1000Hz</label>
                                                                            </article>
                                                                            <article class="col-auto art-lbl p-0">
                                                                                <label class="lbl-oiod">2000Hz</label>
                                                                            </article>
                                                                            <article class="col-auto art-lbl p-0">
                                                                                <label class="lbl-oiod">3000Hz</label>
                                                                            </article>
                                                                            <article class="col-auto art-lbl p-0">
                                                                                <label class="lbl-oiod">4000Hz</label>
                                                                            </article>
                                                                            <article class="col-auto art-lbl p-0">
                                                                                <label class="lbl-oiod">6000Hz</label>
                                                                            </article>
                                                                            <article class="col-auto art-lbl p-0">
                                                                                <label class="lbl-oiod">8000Hz</label>
                                                                            </article>
                                                                        </aside>
                                                                        <aside id="viaAerea-L" class="row w-100 m-0 p-0  mb-2 justify-content-between">

                                                                        </aside>
                                                                        <aside id="EMA-L" class="row w-100 m-0 p-0  mb-2 justify-content-between">

                                                                        </aside>
                                                                        <aside id="viaOsea-L" class="row w-100 m-0 p-0  mb-2 justify-content-between">

                                                                        </aside>
                                                                        <aside id="EMO-L" class="row w-100 m-0 p-0  mb-2 justify-content-between">

                                                                        </aside>
                                                                    </section>
                                                                </div>
                                                                <div class="row contGraphic mt-3">
                                                                    <div class="col-12" id="chartAudiometry1">

                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="mt-4 mt-md-0 p-0 col-6">
                                                                <div class="row  w-100 m-0 ml-2 p-0">
                                                                    <article class="col-12 d-flex justify-content-center align-items-center">
                                                                        <h5>OD</h5>
                                                                    </article>
                                                                    <section class="col-2 row-right pt-3">
                                                                        <label class="lbl-disa">V&iacutea A&eacuterea</label>  
                                                                        <label class="lbl-disa">EM-A</label>
                                                                        <label class="lbl-disa">V&iacutea &Oacutesea</label>
                                                                        <label class="lbl-disa">EM-O</label>   
                                                                    </section>
                                                                    <section class="col-10 p-0" id="contAudiRight">
                                                                        <aside class="row w-100 m-0 p-0 justify-content-between">
                                                                            <article class="col-auto art-lbl p-0">
                                                                                <label class="lbl-oiod">125Hz</label>
                                                                            </article>
                                                                            <article class="col-auto art-lbl p-0">
                                                                                <label class="lbl-oiod">250Hz</label>
                                                                            </article>
                                                                            <article class="col-auto art-lbl p-0">
                                                                                <label class="lbl-oiod">500Hz</label>
                                                                            </article>
                                                                            <article class="col-auto art-lbl p-0">
                                                                                <label class="lbl-oiod">1000Hz</label>
                                                                            </article>
                                                                            <article class="col-auto art-lbl p-0">
                                                                                <label class="lbl-oiod">2000Hz</label>
                                                                            </article>
                                                                            <article class="col-auto art-lbl p-0">
                                                                                <label class="lbl-oiod">3000Hz</label>
                                                                            </article>
                                                                            <article class="col-auto art-lbl p-0">
                                                                                <label class="lbl-oiod">4000Hz</label>
                                                                            </article>
                                                                            <article class="col-auto art-lbl p-0">
                                                                                <label class="lbl-oiod">6000Hz</label>
                                                                            </article>
                                                                            <article class="col-auto art-lbl p-0">
                                                                                <label class="lbl-oiod">8000Hz</label>
                                                                            </article>
                                                                        </aside>
                                                                        <aside id="viaAerea-R" class="row w-100 m-0 p-0  mb-2 justify-content-between">

                                                                        </aside>
                                                                        <aside id="EMA-R" class="row w-100 m-0 p-0  mb-2 justify-content-between">

                                                                        </aside>
                                                                        <aside id="viaOsea-R" class="row w-100 m-0 p-0  mb-2 justify-content-between">

                                                                        </aside>
                                                                        <aside id="EMO-R" class="row w-100 m-0 p-0  mb-2 justify-content-between">

                                                                        </aside>
                                                                    </section>
                                                                </div>
                                                                <div class="row contGraphic mt-3">
                                                                    <div class="col-12" id="chartAudiometry2">

                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                `;
            break;
        case 12:
            componenteHtml = `<div class="p-1 col-fields-${obj.column} d-inline-block align-content-center my-1">
                                    <label style="width:${obj.labelWidth}px !important;" class="d-inline-block align-self-center lbl-dinamic mr-1 my-0">${obj.label}</label> 
                                    <input data-is-delete="${obj.isDelete}" data-componentid="${obj.componentId}" value="${obj.defaultText}" class="form-control d-inline-block input-dinamic" style="width:${obj.width}px !important;height:${obj.height}px!important" id="field_${obj.idComponentField}" type="date">
                                  </div>
                              `;

        case 20:
            componenteHtml = generateEvaluacionM(obj.componentId);
            break;

        case 30:
            componenteHtml = `<div class="p-1 col-fields-${obj.column} d-inline-block align-content-center form-check my-1">      
                                    
                                    <label style="width:${obj.labelWidth}px !important;" class="lbl-dinamic d-inline-block align-self-center">${obj.label}</label>                                  
                                    <input data-is-delete="${obj.isDelete}" data-componentid="${obj.componentId}" class=" mr-3 d-inline-block" type="radio" name="${obj.idComponentField}" id="field_${obj.idComponentField}" >
                                 
                                  </div>
                              `;
            break;

        default:
            break;
    };

    $(componenteHtml).appendTo($("#" + obj.idComponentFieldSubGroup ));
    if (obj.controlId == 10) {
        setAudiometry(arrViaAereaL, arrEMAL, arrEMOL, arrViaOseaL, arrViaAereaR, arrEMAR, arrEMOR, arrViaOseaR)
    }

    if (obj.controlId == 7 || obj.controlId == 9) {
        setCombosDrop(obj.defaultValuesCombo, $("#field_cb_" + obj.idComponentField))
    }
    setCategoryComboDiag()
}

function setCombosDrop(datos, idSelect) {
    var data = JSON.parse(datos);
    if (data != undefined && data != null ) {
        idSelect.css("font-size", "0.7rem");
        for (var i = 0; i < data.length; i++) {
            var opt = `<option value="${data[i].Id}">${data[i].Value}</option>`;
            $(opt).appendTo(idSelect);
        }

        idSelect.val("-1");

    };
}

function deleteField(icon, idLi) {

    var categoryName = $("#" + idLi).data("name-component");
    var categoryId = $("#" + idLi).data("tab-id");
    if ($("#list-examen_" + categoryId).hasClass('culminado')) {
        alertafixed({
            type: 'info',
            class: 'js-examen-culminado',
            title: 'Aviso',
            message: 'Esta categoria se encuentra culminada, no se puede realizar cambios.'
        })
    } else {
        notificacion({
            classTitleAndButtons: "warningTitleAndButtons",
            classMessage: "warningMessage",
            title: "Confirmaci&oacuten !",
            icono: "",
            contenido: "Seguro de eliminar " + categoryName + " ? ",
            btnAceptar: "Aceptar",
            btnCancelar: "Cancelar",
            btnOk: "OK",
            mostrarBtnAceptarAndCancelar: "",
            mostrarBtnOk: "no",
        });

        $(".bigBox-Aceptar").on("click", function () {
            $(".bigBox-Cancelar").trigger('click');

            var LiId = idLi.split("_")[1];
            var idRemove = $("#" + LiId);

            var navTabId = $("#" + idLi).data("tab-id");

            $("#" + icon.id).remove();

            $("#" + idLi).toggleClass("lista-examen");

            var divs = idRemove.parent().find(".js-group-component").length;
            var divsNone = idRemove.parent().find(".deleted-true").length;
            var cantDiv = divs - divsNone;

            if (cantDiv === 1) {
                console.log("tab eliminado");
                var removeNavId = $("#list-examen_" + navTabId);


                $("#examen_" + navTabId).addClass("d-none");
                idRemove.addClass("d-none").addClass('deleted-true');

                for (var i = 0; i < $("#" + LiId + " div.row-cont-subgroup").length; i++) {

                    for (var x = 0; x < $("#" + LiId + " div.row-cont-subgroup div input").length; x++) {
                        $("#" + LiId + " div.row-cont-subgroup div input:eq(" + x + ")").removeAttr("data-is-delete").attr("data-is-delete", "1");
                    }

                    for (var y = 0; y < $("#" + LiId + " div.row-cont-subgroup div select").length; y++) {
                        $("#" + LiId + " div.row-cont-subgroup div select:eq(" + y + ")").removeAttr("data-is-delete").attr("data-is-delete", "1");
                    }

                    for (var z = 0; z < $("#" + LiId + " div.row-cont-subgroup div textarea").length; z++) {
                        $("#" + LiId + " div.row-cont-subgroup div textarea:eq(" + z + ")").removeAttr("data-is-delete").attr("data-is-delete", "1");
                    }

                }
                removeNavId.addClass("no-activo");

            } else {
                idRemove.addClass("d-none").addClass('deleted-true');
                for (var i = 0; i < $("#" + LiId + " div.row-cont-subgroup").length; i++) {

                    for (var x = 0; x < $("#" + LiId + " div.row-cont-subgroup div input").length; x++) {
                        $("#" + LiId + " div.row-cont-subgroup div input:eq(" + x + ")").removeAttr("data-is-delete").attr("data-is-delete", "1");
                    }

                    for (var y = 0; y < $("#" + LiId + " div.row-cont-subgroup div select").length; y++) {
                        $("#" + LiId + " div.row-cont-subgroup div select:eq(" + y + ")").removeAttr("data-is-delete").attr("data-is-delete", "1");
                    }

                    for (var z = 0; z < $("#" + LiId + " div.row-cont-subgroup div textarea").length; z++) {
                        $("#" + LiId + " div.row-cont-subgroup div textarea:eq(" + z + ")").removeAttr("data-is-delete").attr("data-is-delete", "1");
                    }

                }
            }
            console.log(alertafixed);
            alertafixed({
                type: 'success',
                class: 'js-' + LiId + '-true',
                title: 'Hecho',
                message: categoryName + ' se elimin&oacute correctamente.'
            });

        });
    }
    

}
