
function construirInfo(datos, personId, serviceId = "Sin service") {

    var contCategory = $("#nav-tabContent");
    if (contCategory) {

        var data = datos;

        for (var x = 0; x < data.length; x++) {

            var Tab = `
                        <input class="d-none serviceId" value="${serviceId}"/>
                        <input class="d-none personId" value="${personId}"/>
                        <input class="d-none in-examen_${data[x].i_CategoryId}" value="${data[x].v_ServiceComponentId}"/>
					    <div class="px-1 tab-pane j-tabPane-examen fade  w-100 mx-0" data-service-component-id="${data[x].v_ServiceComponentId}"  id="examen_${data[x].i_CategoryId}" role="tabpanel" aria-labelledby="list-examen_${data[x].i_CategoryId}" style="position: relative !important;">                            
                                <ul class="nav nav-tabs tabsCategory"  role="tablist">
                                    <li class="nav-item">
                                        <a class="nav-link js-lbl active" id="${data[x].i_CategoryId}-tab" data-toggle="tab" href="#tab_${data[x].i_CategoryId}" role="tab" aria-controls="tab_${data[x].i_CategoryId}" aria-selected="true">${data[x].v_Name}</a>
                                    </li>
                                </ul>
                                <div class="tab-content" id="myTabContent_${data[x].i_CategoryId}">
                                  <div class="tab-pane fade show active" id="tab_${data[x].i_CategoryId}" data-service-component-id="${data[x].v_ServiceComponentId}" role="tabpanel" aria-labelledby="${data[x].i_CategoryId}-tab"></div>
                                </div>
						</div>
							
						`;
            var status = data[x].i_ServiceComponentStatusId == 3 ? "culminado" : "";
            var contenidoDinamicNav = `<li  class="text-center" style="position: relative;"><a href="#examen_${data[x].i_CategoryId}" onclick="return addActiveList(event, this)" class="${status} list-group-item list-group-item-action js-anchor-tag" id="list-examen_${data[x].i_CategoryId}" data-toggle="list" role="tab" aria-controls=${data[x].i_CategoryId}">${data[x].v_Name}</a></li>`;


            $(Tab).appendTo(contCategory);

            $(contenidoDinamicNav).appendTo(ulDinamic);


            var contCategorias = $("#tab_" + data[x].i_CategoryId)

            var dataGroupComponent = data[x].GroupedComponentsName;

            for (var y = 0; y < dataGroupComponent.length; y++) {

                var arrGroupName = [];


                var Component = `
                        
					    <div class="col-12 js-group-component" id="${dataGroupComponent[y].v_ComponentId}">
                            <div class="row px-2 pt-3 contRows w-100 mx-0 justify-content-between justify-content-md-start" id="groupComponent-${x}-${y}">
                                <label class="lblName">${dataGroupComponent[y].v_GroupedComponentName}</label>                                                                      
                            </div> 
                        </div> 
					    `;

                $(Component).appendTo(contCategorias);

                var dataFields = data[x].Fields;

                for (var z = 0; z < dataFields.length; z++) {


                    if (dataFields[z].v_ComponentId === dataGroupComponent[y].v_ComponentId) {

                        var idGroupComponent = "groupComponent-" + x + "-" + y;
                        var dataGroupComponentId = $("#" + idGroupComponent);

                        var idGroup = "";
                        var dataGroupId = "";


                        var contGroup = `
                                    <div class="col-12 p-0">
                                        <div class="p-1 pt-3  contRows row-scroll w-100 mx-0 justify-content-between justify-content-md-start" >
                                            <label id="label-${z}${y}${x}" class="lblName">${dataFields[z].v_Group}</label>    
                                            <div class="m-0 row-subgroup p-0">
                                                <div class="row align-items-end m-0 row-cont-subgroup p-0" id="${dataFields[z].v_ComponentFieldId}">
                                                </div>
                                            </div>
                                                                                                            
                                        </div> 
                                    </div> 
                                    `;

                        if ($.inArray(dataFields[z].v_Group, arrGroupName) == -1) {



                            $(contGroup).appendTo(dataGroupComponentId);

                            arrGroupName.push(dataFields[z].v_Group);
                            arrGroupName.push(dataFields[z].v_ComponentFieldId);
                            arrGroupName.push(dataFields[z].v_ComponentId);


                            idGroup = dataFields[z].v_ComponentFieldId;
                            var idSubGroup = dataFields[z].v_ComponentFieldId;
                            dataGroupId = $("#" + idGroup);

                        } else {

                            var posiciongroupId = $.inArray(dataFields[z].v_Group, arrGroupName) + 2;
                            var GrupoId = arrGroupName[posiciongroupId];

                            if (GrupoId != dataFields[z].v_ComponentId) {



                                $(contGroup).appendTo(dataGroupComponentId);

                                arrGroupName.push(dataFields[z].v_Group);
                                arrGroupName.push(dataFields[z].v_ComponentFieldId);
                                arrGroupName.push(dataFields[z].v_ComponentId);

                                idGroup = dataFields[z].v_ComponentFieldId;
                                dataGroupId = $("#" + idGroup);

                            } else {

                                var pisicionId = $.inArray(dataFields[z].v_Group, arrGroupName) + 1;

                                idSubGroup = arrGroupName[pisicionId];
                                dataGroupId = $("#" + idSubGroup);

                            };

                        };

                        var componenteHtml = "";

                        var input = "";

                        dataFields[z].i_LabelWidth = dataFields[z].i_LabelWidth == 1 ? 150 : dataFields[z].i_LabelWidth;
                        input = dataFields[z].i_ControlWidth == 1 ? "d-none" : "d-inline-block";

                        switch (dataFields[z].i_ControlId) {
                            
                            case 1:
                                if (dataGroupId.attr('id') == 'N009-MF000004145' || dataGroupId.attr('id') == 'N009-MF000004177') {

                                    dataFields[z].i_Column = 8;
                                    dataFields[z].i_LabelWidth = 100;
                                    dataFields[z].i_ControlWidth = 60;
                                    dataFields[z].i_HeightControl = 20;

                                }
                                componenteHtml = `<div class="p-1 col-fields-${dataFields[z].i_Column} d-inline-block align-content-center my-1">
                                                            
                                                        <label class="d-inline-block align-self-center mr-1 lbl-dinamic my-0" style="width:${dataFields[z].i_LabelWidth}px;"  for="field_${dataFields[z].v_ComponentFieldId}">${dataFields[z].v_TextLabel}</label> 
                                                        <input data-is-delete="${dataFields[z].i_IsDeleted}" name="${dataFields[z].v_ComponentFieldId}" data-componentid="${dataFields[z].v_ComponentId}"  class="form-control ${input} input-dinamic" style="width:${dataFields[z].i_ControlWidth}px !important;height:${dataFields[z].i_HeightControl}px!important" value="${dataFields[z].v_DefaultText}" type="text" id="field_${dataFields[z].v_ComponentFieldId}" >
                                                      
                                                    </div>`;

                                break;

                            case 2:
                                componenteHtml = `<div class="p-1 col-fields-${dataFields[z].i_Column} d-inline-block align-content-center my-1">
                                            <label style="width:${dataFields[z].i_LabelWidth}px;" class="d-inline-block mr-1 align-self-center lbl-dinamic my-0" for="field_${dataFields[z].v_ComponentFieldId}">${dataFields[z].v_TextLabel}</label> 
                                            <textarea data-is-delete="${dataFields[z].i_IsDeleted}" data-componentid="${dataFields[z].v_ComponentId}"  class="d-inline-block form-control" style="font-size: 0.8rem; width:${dataFields[z].i_ControlWidth}px !important;height:${dataFields[z].i_HeightControl}px!important" id="field_${dataFields[z].v_ComponentFieldId}" >${dataFields[z].v_DefaultText}</textarea>
                                            </div>
                                            `;
                                break;

                            case 3:
                                if (dataFields[z].Formula != null) {

                                    var formula = dataFields[z].Formula[0].v_Formula;
                                    var idInputFormula = dataFields[z].Formula[0].v_TargetFieldOfCalculateId;

                                    var regex = /\[([^\]]+)]/g,//obtiene lo que esta dentro de los []
                                        match,
                                        resultado = [];

                                    //bucle para todas las coincidencias
                                    while ((match = regex.exec(formula)) !== null) {
                                        resultado.push(match[1]);
                                    }
                                    var idImplicados = JSON.stringify(resultado);
                                    var formulaJson = JSON.stringify(formula);
                                }

                                componenteHtml = `<div class="p-1 col-fields-${dataFields[z].i_Column} d-inline-block align-content-center my-1">
        												
                                                    <label style="width:${dataFields[z].i_LabelWidth}px;" class="d-inline-block align-self-center mr-1 lbl-dinamic my-0" for="field_${dataFields[z].v_ComponentFieldId}">${dataFields[z].v_TextLabel}</label> 
        										    <input data-idimplicados='${idImplicados}' data-idcalculate='${idInputFormula}' data-formula='${formulaJson}' data-is-delete="${dataFields[z].i_IsDeleted}" data-componentid="${dataFields[z].v_ComponentId}"  name="${dataFields[z].v_ComponentFieldId}"  class="${input} form-control input-dinamic" value="${dataFields[z].v_DefaultText}"  type="text"  min="${dataFields[z].r_ValidateValue1}" max="${dataFields[z].r_ValidateValue2}"  onblur="return validateInput(event, this, ${dataFields[z].i_NroDecimales});" class="entero${z}${y}${x}" onkeyup="return soloEnteros(event, this)"  style="width:${dataFields[z].i_ControlWidth}px !important;height:${dataFields[z].i_HeightControl}px!important" id="field_${dataFields[z].v_ComponentFieldId}" >

    										        </div>`;

                                break;

                            case 4:
                                //Field que contienen formulas
                                if (dataFields[z].Formula != null || dataFields[z].v_Formula != "") {
                                    if (dataFields[z].v_Formula == "") {
                                        var formula = dataFields[z].Formula[0].v_Formula;
                                        var idInputFormula = dataFields[z].Formula[0].v_TargetFieldOfCalculateId;
                                    } else {
                                        var formula = dataFields[z].v_Formula;
                                        var idInputFormula = dataFields[z].v_ComponentFieldId;
                                    }



                                    var regex = /\[([^\]]+)]/g,//obtiene lo que esta dentro de los []
                                        match,
                                        resultado = [];

                                    //bucle para todas las coincidencias
                                    while ((match = regex.exec(formula)) !== null) {
                                        resultado.push(match[1]);
                                    }
                                    var idImplicados = JSON.stringify(resultado);
                                    var formulaJson = JSON.stringify(formula);
                                } else {
                                    var idImplicados = '';
                                    var idInputFormula = '';
                                    var formulaJson = '';
                                }
                                componenteHtml = `<div class="p-1 col-fields-${dataFields[z].i_Column} d-inline-block align-content-center my-1">
                                                        
                                                    <label style="width:${dataFields[z].i_LabelWidth}px;" class="d-inline-block align-self-center mr-1 lbl-dinamic my-0" for="field_${dataFields[z].v_ComponentFieldId}">${dataFields[z].v_TextLabel}</label> 
                                                    <input data-idimplicados='${idImplicados}' data-idcalculate='${idInputFormula}' data-formula='${formulaJson}' data-is-delete="${dataFields[z].i_IsDeleted}" data-componentid="${dataFields[z].v_ComponentId}"  name="${dataFields[z].v_ComponentFieldId}"  class="${input} form-control input-dinamic" min="${dataFields[z].r_ValidateValue1}" max="${dataFields[z].r_ValidateValue2}" onblur="return validateInput(event, this, ${dataFields[z].i_NroDecimales});"  onkeypress="return filterFloat(event,this,${dataFields[z].i_NroDecimales});" style="width:${dataFields[z].i_ControlWidth}px !important;height:${dataFields[z].i_HeightControl}px!important" value="${dataFields[z].v_DefaultText}"  type="text" id="field_${dataFields[z].v_ComponentFieldId}" >

                                                    </div>`;
                                break;

                            case 5:
                                componenteHtml = `<div class="p-1 col-fields-${dataFields[z].i_Column} d-inline-block align-content-center my-1">
    									    <label style="width:${dataFields[z].i_LabelWidth}px;" class="d-inline-block align-self-center mr-1 lbl-dinamic my-0" for="$field_{dataFields[z].v_ComponentFieldId}">${dataFields[z].v_TextLabel}</label> 
    									    <input data-is-delete="${dataFields[z].i_IsDeleted}" data-componentid="${dataFields[z].v_ComponentId}"  class="d-inline-block custom-checkbox" onclick="checkBox(this)" type="checkbox" id="field_${dataFields[z].v_ComponentFieldId}" >
    									    </div>
    								    `;
                                break;

                            case 6:
                                componenteHtml = `<div class="p-1 col-fields-${dataFields[z].i_Column} d-inline-block align-content-center form-check my-1">      
                                                        
                                                    <label style="width:${dataFields[z].i_LabelWidth}px;" class=" d-inline-block align-self-center form-check-label lbl-dinamic ">SI</label>                                  
                                                    <input data-is-delete="${dataFields[z].i_IsDeleted}" data-componentid="${dataFields[z].v_ComponentId}"  class="form-check-input" type="radio" name="${idSubGroup}" id="${dataFields[z].v_ComponentFieldId}" value="0">

                                                    <label style="width:${dataFields[z].i_LabelWidth}px;" class=" d-inline-block align-self-center lbl-dinamic form-check-label">NO</label>
                                                    <input data-is-delete="${dataFields[z].i_IsDeleted}" data-componentid="${dataFields[z].v_ComponentId}"  class="form-check-input" type="radio" name="${idSubGroup}" id="${dataFields[z].v_ComponentFieldId}" value="0" >
                                                      
                                                  </div>
                                                `;
                                break;

                            case 7:
                                var datosDrop = dataFields[z].ComboValues;
                                var defaultSelect = dataFields[z].v_DefaultText;

                                //Son campos pequeños, por eso se les da un texto 'Slct'
                                
                                if (dataGroupId.attr('id') == 'N009-MF000004145' || dataGroupId.attr('id') == 'N009-MF000004177') {

                                    dataFields[z].i_Column = 8;
                                    dataFields[z].i_LabelWidth = 100;
                                    dataFields[z].i_ControlWidth = 60;
                                    dataFields[z].i_HeightControl = 20;
                                    var text = '-Slct-';
                                } else {
                                    var text = '--Select--';
                                }
                                componenteHtml = `<div class="p-1 col-fields-${dataFields[z].i_Column} d-inline-block align-content-center my-1">
                                                        <label style="width:${dataFields[z].i_LabelWidth}px;" class="d-inline-block align-self-center lbl-dinamic mr-1 my-0" for="field_${dataFields[z].v_ComponentFieldId}">${dataFields[z].v_TextLabel}</label> 
                                                        <select data-is-delete="${dataFields[z].i_IsDeleted}" data-componentid="${dataFields[z].v_ComponentId}"  class="d-inline-block custom-select input-dinamic" style="width:${dataFields[z].i_ControlWidth}px !important;height:${dataFields[z].i_HeightControl}px !important" id="field_cb_${dataFields[z].v_ComponentFieldId}" >
                                                        <option  value="-1" selected>${text}</option>

                                                        </select>

                                                </div>`;
                                break;

                            case 8:
                                componenteHtml = `
                                                <div class="custom-file" style="width:150px !important ">
                                                   <input data-is-delete="${dataFields[z].i_IsDeleted}" data-componentId="${dataFields[z].v_ComponentId}"  type="file" class="custom-file-input" id="field_${dataFields[z].v_ComponentFieldId}">
                                                   <label style="font-size:0.6rem !important; vertical-align: middle !important" class="custom-file-label" for="field_${dataFields[z].v_ComponentFieldId}">${dataFields[z].v_TextLabel}</label>
                                                </div>
                                                `;
                                break;


                            case 9:
                                var datosDrop = dataFields[z].ComboValues;
                                var defaultSelect = dataFields[z].v_DefaultText;

                                if (dataGroupId.attr('id') == 'N009-MF000004145' || dataGroupId.attr('id') == 'N009-MF000004177') {
                                    
                                    dataFields[z].i_Column = 8;
                                    dataFields[z].i_LabelWidth = 100;
                                    dataFields[z].i_ControlWidth = 60;
                                    dataFields[z].i_HeightControl = 20;
                                    var text = '-Slct-';
                                } else {
                                    var text = '--Select--';
                                }
                                componenteHtml = `<div class="p-1 col-fields-${dataFields[z].i_Column} d-inline-block align-content-center my-1">
                                                        <label style="width:${dataFields[z].i_LabelWidth}px;" class="d-inline-block align-self-center lbl-dinamic mr-1 my-0" for="field_${dataFields[z].v_ComponentFieldId}">${dataFields[z].v_TextLabel}</label> 
                                                        <select data-is-delete="${dataFields[z].i_IsDeleted}" data-componentid="${dataFields[z].v_ComponentId}"  class="d-inline-block custom-select input-dinamic" style="width:${dataFields[z].i_ControlWidth}px !important;height:${dataFields[z].i_HeightControl}px !important" id="field_cb_${dataFields[z].v_ComponentFieldId}" >
                                                        <option  value="-1" selected>${text}</option>

                                                        </select>

                                                </div>`;
                                break;
                            case 10:
                                console.log(dataFields[z]);
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
                                componenteHtml = `<div class="p-1 col-fields-${dataFields[z].i_Column} d-inline-block align-content-center my-1">
            									    <label style="width:${dataFields[z].i_LabelWidth}px;" class="d-inline-block align-self-center lbl-dinamic mr-1 my-0">${dataFields[z].v_TextLabel}</label> 
            									    <input data-is-delete="${dataFields[z].i_IsDeleted}" data-componentid="${dataFields[z].v_ComponentId}"  class="form-control d-inline-block input-dinamic" style="width:${dataFields[z].i_ControlWidth}px !important;height:${dataFields[z].i_HeightControl}px!important" id="field_${dataFields[z].v_ComponentFieldId}" type="date">
            									    </div>
    										    `;
                                break;
                            case 20:
                                componenteHtml = generateEvaluacionM(dataFields[z].v_ComponentId);
                                break;
                            case 30:
                                componenteHtml = `<div class="p-1 col-fields-${dataFields[z].i_Column} d-inline-block align-content-center form-check my-1">      
                                                        
                                                    <label style="width:${dataFields[z].i_LabelWidth}px;" class="lbl-dinamic d-inline-block align-self-center">${dataFields[z].v_TextLabel}</label>                                  
                                                    <input data-is-delete="${dataFields[z].i_IsDeleted}" data-componentid="${dataFields[z].v_ComponentId}"  class=" ${dataFields[z].v_ComponentFieldId} mr-3 d-inline-block" onchange="checkRadio(this)" type="radio" name="${idSubGroup}" id="field_${dataFields[z].v_ComponentFieldId}" value="0" >
                                                     
                                                    </div>
                                                `;
                                break;

                            default:
                                break;
                        };

                        $(componenteHtml).appendTo(dataGroupId);

                        if (dataFields[z].i_ControlId == 10) {
                            setAudiometry(arrViaAereaL, arrEMAL, arrEMOL, arrViaOseaL, arrViaAereaR, arrEMAR, arrEMOR, arrViaOseaR)
                        }

                        // para llenar los combos
                        var idSelect = $("#field_cb_" + dataFields[z].v_ComponentFieldId);
                        setCombos(datosDrop, idSelect, defaultSelect);


                    };

                };
            };
        };


    }
    cargar();

    ListOfAdditionalExams();
    setCategoryComboDiag();
    
}

function cargar() {

    $("#ulDinamic li:eq(0)").addClass("active");
    $("#nav-tabContent div:eq(0)").addClass("show active");
    $("#ulDinamic li:eq(0) a").click();
    var textAnchorTag = $("#nav-tabContent div:eq(0) .js-lbl:eq(0)").text();
    $("#js-save-exam").val("GRABAR " + textAnchorTag)
    $("#nameCategory").empty().text(textAnchorTag);

    var iconDinamicNav = document.querySelector("#iconDinamicNavBars")
    var WinWidth = $(window).width();
    var navContent = $(".js-tab-content");


    if (WinWidth < 1200) {
        navContent.css("height", "0px");
        if (iconDinamicNav.classList.contains('fa-times')) {
            iconDinamicNav.classList.remove("fa-times");
            iconDinamicNav.classList.add("fa-bars");

            navContent.animate({
                height: "0px",
            }, 500)

        }

    } else {
        navContent.animate({
            height: navContent.get(0).scrollHeight,
        }, 500);
    }






}

function setCategoryComboDiag() {

    for (var a = 0; a < $(".cont-dinamic-select").length; a++) {
        $(".contenedor-opt-" + a).remove();
        var contenedor = `
                        <div class="contenedor-opt-${a} contenedor-opt  d-none">
                        </div> 
        
                        `;
        $(contenedor).appendTo($(".cont-dinamic-select:eq(" + a + ")"))

        for (var i = 0; i < $("#ulDinamic li").length; i++) {

            var categoryName = $("#ulDinamic li a:eq(" + i + ")").text();
            var categoryId = $("#ulDinamic li a:eq(" + i + ")").attr("id").split("_")[1];
            var length = $(".name-category").length;
            var options = `
                        <div class="opt-${categoryId}-${a} options-select pb-1">
                          <label class="name-category mb-0 px-2" id="label-${a}-${length}-opt-${categoryId}" onclick="selectOption(this)">${categoryName} <i class="fas fa-caret-right"></i></label>  
                        </div>
                        `;
            $(options).appendTo($(".contenedor-opt-" + a));

            var select = $(".componentid-for-select");
            for (let y = 0; y < select.length; y++) {

                for (let x = 0; x < $("#tab_" + categoryId + " .js-group-component").length; x++) {
                    var componentid = $("#tab_" + categoryId + " .js-group-component:eq(" + x + ")").attr("id");
                    var text = $("#tab_" + categoryId + " .js-group-component:eq(" + x + ") div .lblName:eq(0)").text();

                    if (select.eq(y).val() == componentid) {
                        select.eq(y).next().find('.lbl-category').text(text);
                        select.eq(y).parent().attr("data-componentid", componentid)
                    }
                }

            }


            for (var x = 0; x < $("#tab_" + categoryId + " .js-group-component").length; x++) {
                var id = $("#tab_" + categoryId + " .js-group-component:eq(" + x + ")").attr("id");
                var text = $("#tab_" + categoryId + " .js-group-component:eq(" + x + ") div .lblName:eq(0)").text();
                var conteOpt = `
                                <div class="pl-4 children-opt_${id} childrens" onclick="setTextSelect(this, '${id}')">${text}</div>
                            `;

                $(conteOpt).appendTo($(".opt-" + categoryId + "-" + a))


            }

        }


    }

}

function GenerateSaveExam(idLi) {

    var name = $("#" + idLi.className).text();
    if ($("#js-save-exam").val() == "Examen") {

        alertafixed({
            type: 'info',
            class: 'js-examen-true',
            title: 'Aviso',
            message: 'No hay examen por guardar'
        });


    } else if ($("#EstadoAuditoria").val() == 1) {

        alertafixed({
            type: 'info',
            class: 'js-examen-true',
            title: 'Aviso',
            message: 'El estado del examen no puede ser POR INICIAR.'
        });


    } else {
        serviceId = $(".serviceId").val();
        var cClass = $("#js-save-exam").attr("class");
        var getLiId = $("#" + cClass);
        var examen = cClass.split("_")[1];
        var getTabId = $("#tab_" + examen);
        var serviceComponentId = getTabId.data("service-component-id");
        getLiId.addClass('loadingGrid');
        getTabId.addClass('loadingGrid');

        var values = [];
        var objAuditory = {
            v_Comment: $("#comentAuditoria").val(),
            i_ServiceComponentStatusId: $("#EstadoAuditoria").val(),
            i_ExternalinternalId: 0,
            i_IsApprovedId: 0,
        };
        for (let i = 0; i < $("#tab_" + examen + " input").length; i++) {


            let input = $("#tab_" + examen + " input:eq(" + i + ")");
            let componentId = input.data("componentid");
            var isDelete = $("#tab_" + examen + " input:eq(" + i + ")").data("is-delete");
            var idField = '';
            if ($("#tab_" + examen + " input").eq(i).hasClass('group-field') == false) {
                idField = input.attr("id").split("_")[1];
                var inputVal = input.val();
                let objValue = {
                    v_ServiceComponentId: serviceComponentId,
                    v_ComponentFieldId: idField,
                    v_ServiceId: serviceId,
                    v_PersonId: $(".personId").val(),
                    v_ComponentId: componentId,
                    i_IsDeleted: isDelete,
                    Frontend: $("#tab_" + examen).data("front"),
                    ServiceComponentFieldValues: [{
                        v_Value1: inputVal,
                        i_IsDeleted: isDelete,
                    }]

                }
                values.push(objValue);
            }; 
            if ($("#tab_" + examen + " input").eq(i).hasClass('enviar-este')) {
                idField = input.attr("name");
                var inputVal = input.data('value');
                let objValue = {
                    v_ServiceComponentId: serviceComponentId,
                    v_ComponentFieldId: idField,
                    v_ServiceId: serviceId,
                    v_PersonId: $(".personId").val(),
                    v_ComponentId: componentId,
                    i_IsDeleted: isDelete,
                    Frontend: $("#tab_" + examen).data("front"),
                    ServiceComponentFieldValues: [{
                        v_Value1: inputVal,
                        i_IsDeleted: isDelete,
                    }]

                }
                values.push(objValue);
            } 

            
            





        }

        for (let i = 0; i < $("#tab_" + examen + " textarea").length; i++) {

            let txtArea = $("#tab_" + examen + " textarea:eq(" + i + ")");
            let componentId = txtArea.data("componentid");
            var isDelete = $("#tab_" + examen + " textarea:eq(" + i + ")").data("is-delete");
            var idField = 'evaluacion';

            idField = txtArea.attr("id").split("_")[1];
            

            let objValue = {
                v_ServiceComponentId: serviceComponentId,
                v_ComponentFieldId: idField,
                v_ServiceId: serviceId,
                v_PersonId: $(".personId").val(),
                v_ComponentId: componentId,
                i_IsDeleted: isDelete,
                Frontend: $("#tab_" + examen).data("front"),
                ServiceComponentFieldValues: [{
                    v_Value1: txtArea.val(),
                    i_IsDeleted: isDelete,
                }]
            }
            values.push(objValue);

        }
        for (let i = 0; i < $("#tab_" + examen + " select").length; i++) {

            let select = $("#tab_" + examen + " select:eq(" + i + ")");
            let componentId = select.data("componentid");
            var isDelete = $("#tab_" + examen + " select:eq(" + i + ")").data("is-delete");
            var idField = 'evaluacion';

            idField = select.attr("id").split("_")[2];

            let objValue = {
                v_ServiceComponentId: serviceComponentId,
                v_ComponentFieldId: idField,
                v_ServiceId: serviceId,
                v_PersonId: $(".personId").val(),
                v_ComponentId: componentId,
                i_IsDeleted: isDelete,
                Frontend: $("#tab_" + examen).data("front"),
                ServiceComponentFieldValues: [{
                    v_Value1: select.val(),
                    i_IsDeleted: isDelete,
                }]
            }
            values.push(objValue);

        }

        var classSplit = cClass.split("-");
        var TabClass = $(".in-" + classSplit[1]);

        var stringServicecomponent = JSON.stringify(objAuditory);
        console.log(values);
        $.ajax({
            cache: false,
            dataType: 'json',
            method: 'POST',
            data: {
                'oServicecomponentfields': values,
                'oServicecomponent': stringServicecomponent,
                'serviceComponentId': TabClass.val(),
            },
            url: '/PatientsAssistance/SaveMedicalConsultation',
            success: function (json) {
                var splitJson = json.split("|");
                var serviceComponentId = splitJson[0];
                var user = $("#user-logg").val();
                var date = splitJson[1];
                $("#usuarioModificacion").text(user);
                $("#fechModificacion").text(date);
                if ($("#EstadoAuditoria").val() == "3") {
                    getLiId.addClass("culminado");

                };
                if (getLiId.hasClass('culminado')) {
                    $("#js-save-exam").attr("disabled", true);
                    $("#js-save-exam").css({
                        "background-color": 'rgba(173, 173, 173, 0.7)',
                        "cursor": 'default',
                    });
                } else {
                    $("#js-save-exam").attr("disabled", false);
                    $("#js-save-exam").css({
                        "cursor": "pointer",
                        "background-color": "rgb(23, 162, 184)",
                    });
                }

                okResult(cClass, name);
                getLiId.removeClass('loadingGrid');
                getTabId.removeClass('loadingGrid');
                getTabId.data("service-component-id", serviceComponentId);
                getTabId.attr("data-front", "");
                getTabId.removeData("front");
                LoadIndicators($(".personId").val());
                TimeLineByServiceId(serviceId);
            },
            error: function (error) {
                errorResult(cClass, name)
                $(getLiId).removeClass('loadingGrid');
                $(getTabId).removeClass('loadingGrid');
            },
        });
    }
}

function SaveExam(evt, idLi) {
    if ($("#EstadoAuditoria").val() == 3) {
        notificacion({
            classTitleAndButtons: "warningTitleAndButtons",
            classMessage: "warningMessage",
            title: "¡ Confirmaci&oacuten !",
            icono: "",
            contenido: "Seguro de culminar el examen ?",
            btnAceptar: "Aceptar",
            btnCancelar: "Cancelar",
            btnOk: "OK",
            mostrarBtnAceptarAndCancelar: "si",
            mostrarBtnOk: "no",
        });
        $(".bigBox-Aceptar").on("click", function () {
            $(".bigBox-Cancelar").trigger('click');
            GenerateSaveExam(idLi);
        });
    } else {
        GenerateSaveExam(idLi)
    }

}

function okResult(_class, name) {

    alertafixed({
        type: 'success',
        class: 'js-' + _class + '-true',
        title: 'Hecho',
        message: name + ' se guard&oacute correctamente.'
    });

}

function errorResult(_class, name) {

    alertafixed({
        type: 'danger',
        class: 'js-' + _class + '-true',
        title: 'Error',
        message: 'Sucedi&oacute un error al guardar ' + name + '.'
    });

}

$(window).resize(function () {

    var iconDinamicNav = document.querySelector("#iconDinamicNavBars")
    var WinWidth = $(window).width();
    var navContent = $(".js-tab-content");


    if (WinWidth < 1200) {
        navContent.css("height", "0px");
        if (iconDinamicNav.classList.contains('fa-times')) {
            iconDinamicNav.classList.remove("fa-times");
            iconDinamicNav.classList.add("fa-bars");

            navContent.animate({
                height: "0px",
            }, 500)

        }

    } else {
        navContent.animate({
            height: navContent.get(0).scrollHeight,
        }, 500);
    }

})

function buttonDinamicNav() {

    var iconDinamicNav = document.querySelector("#iconDinamicNavBars");
    var navContent = $(".js-tab-content");

    if (iconDinamicNav.classList.contains('fa-bars')) {
        iconDinamicNav.classList.remove("fa-bars");
        iconDinamicNav.classList.add("fa-times");



        navContent.stop().animate({
            height: navContent.get(0).scrollHeight,
        }, 500);


    }

    else {
        iconDinamicNav.classList.remove("fa-times");
        iconDinamicNav.classList.add("fa-bars");

        navContent.stop().animate({
            height: "0px",
        }, 500);
    }

}

function addActiveList(evt, anchorTag) {
    var idElement = anchorTag.id.split("-")[1];
    var serviceComponentId = $("#" + idElement).data("service-component-id");

    if ($(anchorTag).hasClass('culminado')) {
        $("#js-save-exam").attr("disabled", true);
        $("#js-save-exam").css({
            "background-color": 'rgba(173, 173, 173, 0.7)',
            "cursor": 'default',
        });
    } else {
        $("#js-save-exam").attr("disabled", false);
        $("#js-save-exam").css({
            "cursor": "pointer",
            "background-color": "rgb(23, 162, 184)",
        });
    }
    if (serviceComponentId != null && serviceComponentId != undefined) {
        $.ajax({
            cache: false,
            method: 'GET',
            dataType: 'json',
            data: { 'serviceComponentId': serviceComponentId },
            url: '/PatientsAssistance/GetInfoServiceComponent',
            success: function (json) {
                if (json.ServiceComponentStatusId === 3) {
                    $("#js-save-exam").attr("disabled", true);
                    $("#js-save-exam").css({
                        "background-color": 'rgba(173, 173, 173, 0.7)',
                        "cursor": 'default',
                    });
                } else {
                    $("#js-save-exam").attr("disabled", false);
                    $("#js-save-exam").css({
                        "cursor": "pointer",
                        "background-color": "rgb(23, 162, 184)",
                    });
                }
                $("#EstadoAuditoria").val(json.ServiceComponentStatusId);
                $("#comentAuditoria").val(json.Comment);
                $("#lblUsusario").text(json.CreationUser);
                $("#fechInsercion").text(json.CreationDate);
                $("#usuarioModificacion").text(json.UpdateUser);
                $("#fechModificacion").text(json.UpdateDate);

            },
            error: function (error) {
                console.log(error)
            }
        });
    } else {

        $("#lblUsusario").text("");
        $("#fechInsercion").text("");
        $("#usuarioModificacion").text("");
        $("#fechModificacion").text("");
        $("#comentAuditoria").val("");
        $("#EstadoAuditoria").val(1);
    }


    if ($("#" + anchorTag.id).hasClass("no-activo") == false) {
        $(".list-group-item-action").removeClass("active")
        $(".j-tabPane-examen").addClass("d-none").fadeOut(100);


        $("#" + idElement).addClass("show active").fadeIn(100).removeClass("d-none");
        anchorTag.classList.add("active");

        var anchorTagId = $("#" + anchorTag.id);


        $("#js-save-exam").val("GRABAR " + anchorTagId.text());
        $("#nameCategory").empty().text(anchorTagId.text());
        $("#js-save-exam").attr("class", anchorTag.id);
    } else {
        $(".list-group-item-action").removeClass("active")
        $(".j-tabPane-examen").addClass("d-none").fadeOut(100);


        $("#" + idElement).addClass("show active").fadeIn(100);
        anchorTag.classList.add("active");

        var anchorTagId = $("#" + anchorTag.id);

        $("#js-save-exam").val(anchorTagId.text());

        $("#js-save-exam").attr("class", anchorTag.id);
    }



}

function setCombos(datosDrop, idSelect, defaultSelect) {

    if (datosDrop != undefined) {
        idSelect.css("font-size", "0.7rem");
        for (var i = 0; i < datosDrop.length; i++) {
            var opt = `<option value="${datosDrop[i].Id}">${datosDrop[i].Value}</option>`;
            $(opt).appendTo(idSelect);

        }

        idSelect.val(defaultSelect);

    };
}

function validateDecimalEntero(input) {
    var inputVal = input.value;
    let valor = parseFloat(inputVal);
    let min = parseFloat(input.min);
    let max = parseFloat(input.max);

    if (min > 0 || max > 0) {

        if (valor < min || valor > max) {


            alertafixed({
                type: 'warning',
                class: 'js-validate-true',
                title: 'Validación',
                message: 'Por favor, igrese números de <strong> ' + input.min + ' </strong> al <strong> ' + input.max + ' </strong>.'
            });

            document.getElementById(input.id).classList.add("border-danger");

            input.focus();
            input.value = "";
        } else {
            if (input.classList.contains('border-danger')) {
                document.getElementById(input.id).classList.remove("border-danger");
                document.getElementById(input.id).classList.add("border-success");
            };

        }

    }


}

function validateDecimalEnteroPartial(min, max, valor) {

    if (valor < min || valor > max) {

        alertafixed({
            type: 'warning',
            class: 'js-activoEntero-true',
            title: 'Validación',
            message: 'Por favor, igrese números de <strong> ' + input.min + ' </strong> al <strong> ' + input.max + ' </strong>.'
        });

        document.getElementById(input.id).classList.add("border-danger");

        input.focus();

    } else {
        if (input.classList.contains('border-danger')) {
            document.getElementById(input.id).classList.remove("border-danger");
            document.getElementById(input.id).classList.add("border-success");
        };
    }

}

function validateInput(evt, input, cantDecimal) {

    var re = /\[.+?]/;

    var idCalculate = $(input).data('idcalculate');
    var idImplicados = $(input).data('idimplicados');
    var formula = $(input).data('formula').replace(/(Pow)/, 'Math.pow');
    if (idImplicados != "" && idCalculate != '') {
        var output = formula,
            finish = false,
            iter = 0;

        while (!finish) {
            if (output.indexOf('[') >= 0 && output.indexOf(']') >= 0) {
                var value = Number($("#field_" + idImplicados[iter]).val());
                output = output.replace(
                    output.substr(output.indexOf('['), output.indexOf(']') - output.indexOf('[') + 1),
                    value);
                iter++;
            }
            else {
                finish = true;
            }
        }

        if ($(input).attr('id') != "field_" + idCalculate) {

            var evalu = eval(JSON.parse(output));
            evalu = isNaN(evalu) ? 0 : evalu;
            var newcantDecimal = cantDecimal == 0 ? 1 : cantDecimal;
            if (evalu != 0 && evalu != Infinity) {

                $("#field_" + idCalculate).val(evalu.toFixed(newcantDecimal));
            }
        }
    }

    var inputVal = input.value;

    if (cantDecimal > 0) {
        if ($.type(inputVal) === "string") {

            validateDecimalEntero(input);

        };

        if (inputVal != "") {
            var valSplit = inputVal.split(".");
            if (valSplit.length > 1) {
                input.value = valSplit[1].length < cantDecimal ? valSplit[0] + "." + valSplit[1].padEnd(cantDecimal, 0) : inputVal;
            } else {
                input.value = inputVal + ".".padEnd(cantDecimal + 1, 0);
            }

        };
        var valSplit = inputVal.split(".");
        if (valSplit[0] === "") {
            input.value = "0.".padEnd(cantDecimal + 2, 0);
        }
    } else {

        if ($.type(inputVal) === "string") {

            validateDecimalEntero(input);
        };
    }


}

function soloEnteros(evt, input) {

    input.value = input.value.replace(/[^0-9]/, '');
    if (evt.which == 190 || evt.which == 110) {

        alertafixed({
            type: 'warning',
            class: 'js-enteros',
            title: 'Validación',
            message: 'Porfavor, igrese solo números enteros'

        })
    }

}

function filterFloat(evt, input, cantDecimal) {

    var key = window.Event ? evt.which : evt.keyCode;
    var chark = String.fromCharCode(key);
    var tempValue = input.value + chark;
    if (key >= 48 && key <= 57) {
        if (filter(tempValue, cantDecimal) === false) {
            return false;
        } else {
            return true;
        }
    } else {
        if (key == 8 || key == 13 || key == 0) {
            return true;
        } else if (key == 46) {
            if (filter(tempValue, cantDecimal) === false) {

                return false;

            } else {
                return true;

            }
        } else {
            return false;
        }
    }
}

function filter(__val__, cant) {

    var ExpReg = new RegExp("^([0-9]+\.?[0-9]{0," + cant + "})$");

    if (ExpReg.test(__val__) === true) {
        return true;
    } else {

        return false;
    }

}

function checkBox(checkb) {

    if ($(checkb).val() == 1 || $(checkb).val() == "1") {
        $(checkb).val(0);
    } else {
        $(checkb).val(1)
    }
}

function checkRadio(radio) {
    var name = $(radio).attr('name');
    $("input[name=" + name + "]").val(0)
    $(radio).val(1);
}

function setAudiometry(arrViaAereaL, arrEMAL, arrEMOL, arrViaOseaL, arrViaAereaR, arrEMAR, arrEMOR, arrViaOseaR) {
    var arrViaAerea_L = [];
    var arrEMA_L = [];
    var arrEMO_L = [];
    var arrViaOsea_L = [];

    var arrViaAerea_R = [];
    var arrEMA_R = [];
    var arrEMO_R = [];
    var arrViaOsea_R = [];

    for (let i = 0; i < arrViaAereaL.length; i++) {
        var html = `
                <div class="col-auto p-0">
                    <input style="font-size: 0.9rem" data-is-delete="0" data-componentid="${arrViaAereaL[i].componentId}" class="input-audiometry" id="AU_${arrViaAereaL[i].idInput}" onblur="validateValues(event, this)" onkeyup="SetLeftAudiometryGraph(event, this)" class="form-control" value="${arrViaAereaL[i].valueInput}" type="text" />
                </div>
                `;
        $(html).appendTo($("#viaAerea-L"));
        arrViaAerea_L.push(arrViaAereaL[i].valueInput)
    }
    for (let x = 0; x < arrEMAL.length; x++) {
        var html = `
                <div class="col-auto p-0">
                    <input style="font-size: 0.9rem" data-is-delete="0" data-componentid="${arrEMAL[x].componentId}" class="input-audiometry" id="AU_${arrEMAL[x].idInput}" onblur="validateValues(event, this)" onkeyup="SetLeftAudiometryGraph(event, this)" class="form-control" value="${arrEMAL[x].valueInput}" type="text" />
                </div>
                `;

        $(html).appendTo($("#EMA-L"));
        arrEMA_L.push(arrEMAL[x].valueInput)
    }
    for (let i = 0; i < arrEMOL.length; i++) {
        var html = `
                <div class="col-auto p-0">
                    <input style="font-size: 0.9rem" data-is-delete="0" data-componentid="${arrEMOL[i].componentId}" class="input-audiometry" id="AU_${arrEMOL[i].idInput}" onblur="validateValues(event, this)" onkeyup="SetLeftAudiometryGraph(event, this)" class="form-control" value="${arrEMOL[i].valueInput}" type="text" />
                </div>
                `;
        $(html).appendTo($("#EMO-L"));
        arrEMO_L.push(arrEMOL[i].valueInput)
    }
    for (let i = 0; i < arrViaOseaL.length; i++) {
        var html = `
                <div class="col-auto p-0">
                    <input style="font-size: 0.9rem" data-is-delete="0" data-componentid="${arrViaOseaL[i].componentId}" class="input-audiometry" id="AU_${arrViaOseaL[i].idInput}" onblur="validateValues(event, this)" onkeyup="SetLeftAudiometryGraph(event, this)" class="form-control" value="${arrViaOseaL[i].valueInput}" type="text" />
                </div>
                `;
        $(html).appendTo($("#viaOsea-L"));
        arrViaOsea_L.push(arrViaOseaL[i].valueInput)
    }
    for (let i = 0; i < arrViaAereaR.length; i++) {
        var html = `
                <div class="col-auto p-0">
                    <input style="font-size: 0.9rem" data-is-delete="0" data-componentid="${arrViaAereaR[i].componentId}" class="input-audiometry" id="AU_${arrViaAereaR[i].idInput}" onblur="validateValues(event, this)" onkeyup="SetRightAudiometryGraph(event, this)" class="form-control" value="${arrViaAereaR[i].valueInput}" type="text" />
                </div>
                `;
        $(html).appendTo($("#viaAerea-R"));
        arrViaAerea_R.push(arrViaAereaR[i].valueInput)
    }
    for (let i = 0; i < arrEMAR.length; i++) {
        var html = `
                <div class="col-auto p-0">
                    <input style="font-size: 0.9rem" data-is-delete="0" data-componentid="${arrEMAR[i].componentId}" class="input-audiometry" id="AU_${arrEMAR[i].idInput}" onblur="validateValues(event, this)" onkeyup="SetRightAudiometryGraph(event, this)" class="form-control" value="${arrEMAR[i].valueInput}" type="text" />
                </div>
                `;

        $(html).appendTo($("#EMA-R"));
        arrEMA_R.push(arrEMAR[i].valueInput)
    }
    for (let i = 0; i < arrEMOR.length; i++) {
        var html = `
                <div class="col-auto p-0">
                    <input style="font-size: 0.9rem" data-is-delete="0" data-componentid="${arrEMOR[i].componentId}" class="input-audiometry" id="AU_${arrEMOR[i].idInput}" onblur="validateValues(event, this)" onkeyup="SetRightAudiometryGraph(event, this)" class="form-control" value="${arrEMOR[i].valueInput}" type="text" />
                </div>
                `;

        $(html).appendTo($("#EMO-R"));
        arrEMO_R.push(arrEMOR[i].valueInput)
    }
    for (let i = 0; i < arrViaOseaR.length; i++) {
        var html = `
                <div class="col-auto p-0">
                    <input style="font-size: 0.9rem" data-is-delete="0" data-componentid="${arrViaOseaR[i].componentId}" class="input-audiometry" id="AU_${arrViaOseaR[i].idInput}" onblur="validateValues(event, this)" onkeyup="SetRightAudiometryGraph(event, this)" class="form-control" value="${arrViaOseaR[i].valueInput}" type="text" />
                </div>
                `;

        $(html).appendTo($("#viaOsea-R"));
        arrViaOsea_R.push(arrViaOseaR[i].valueInput)
    }

    RightAudiometryGraph(arrViaAerea_R, arrEMA_R, arrEMO_R, arrViaOsea_R)
    LeftAudiometryGraph(arrViaAerea_L, arrEMA_L, arrEMO_L, arrViaOsea_L)
}

function SetLeftAudiometryGraph(e, input) {

    if (e.which != 9) {
        //valida enteros y decimales, positivos y negativos.
        input.value = input.value.replace(/[^0-9\-]/, '');
        var viaAerea_L = [],
            EMA_L = [],
            viaOsea_L = [],
            EMO_L = [];


        for (let i = 0; i < $("#viaAerea-L div").length; i++) {
            let value = $("#viaAerea-L div:eq(" + i + ") input").val();
            if (value == "") {
                value = null;
                viaAerea_L.push(value);
            } else {
                viaAerea_L.push(parseFloat(value));
            }
        }
        for (let i = 0; i < $("#EMA-L div").length; i++) {
            let value = $("#EMA-L div:eq(" + i + ") input").val();
            if (value == "") {
                value = null;
                EMA_L.push(value);
            } else {
                EMA_L.push(parseFloat(value));
            }
        }
        for (let i = 0; i < $("#viaOsea-L div").length; i++) {
            let value = $("#viaOsea-L div:eq(" + i + ") input").val();
            if (value == "") {
                value = null;
                viaOsea_L.push(value);
            } else {
                viaOsea_L.push(parseFloat(value));
            }
        }
        for (let i = 0; i < $("#EMO-L div").length; i++) {
            let value = $("#EMO-L div:eq(" + i + ") input").val();
            if (value == "") {
                value = null;
                EMO_L.push(value);
            } else {
                EMO_L.push(parseFloat(value));
            }
        }

        LeftAudiometryGraph(viaAerea_L, EMA_L, viaOsea_L, EMO_L);
    }



}

function SetRightAudiometryGraph(e, input) {
    if (e.which != 9) {
        //valida enteros y decimales, positivos y negativos.
        input.value = input.value.replace(/[^0-9\.\-]/, '');
        var viaAerea_R = [],
            EMA_R = [],
            viaOsea_R = [],
            EMO_R = [];
        for (let i = 0; i < $("#viaAerea-R div").length; i++) {
            let value = $("#viaAerea-R div:eq(" + i + ") input").val();
            if (value == "") {
                value = null;
                viaAerea_R.push(value);
            } else {
                viaAerea_R.push(parseFloat(value));
            }
        }
        for (let i = 0; i < $("#EMA-R div").length; i++) {
            let value = $("#EMA-R div:eq(" + i + ") input").val();
            if (value == "") {
                value = null;
                EMA_R.push(value);
            } else {
                EMA_R.push(parseFloat(value));
            }
        }
        for (let i = 0; i < $("#viaOsea-R div").length; i++) {
            let value = $("#viaOsea-R div:eq(" + i + ") input").val();
            if (value == "") {
                value = null;
                viaOsea_R.push(value);
            } else {
                viaOsea_R.push(parseFloat(value));
            }
        }
        for (let i = 0; i < $("#EMO-R div").length; i++) {
            let value = $("#EMO-R div:eq(" + i + ") input").val();
            if (value == "") {
                value = null;
                EMO_R.push(value);
            } else {
                EMO_R.push(parseFloat(value));
            }
        }
        RightAudiometryGraph(viaAerea_R, EMA_R, viaOsea_R, EMO_R)
    }

}

function validateValues(e, input) {
    var entero = parseInt(input.value);
    if (entero < -10 || entero > 120) {
        alertafixed({
            type: 'warning',
            class: 'js-validation',
            title: 'Validación',
            message: 'Por favor, ingrese valores de <strong>-10</strong> al <strong>120</strong>'
        })
        input.focus();
    }

}

function LeftAudiometryGraph(viaAerea_L, EMA_L, viaOsea_L, EMO_L) {

    Highcharts.chart('chartAudiometry1', {
        title: {
            text: 'Detalle OI'
        },
        credits: { enabled: false },
        tooltip: { enabled: false },
        xAxis: {

            opposite: true,
            categories: ['_125Hz', '_250Hz', '_500Hz', '_1000Hz', '_2000Hz', '_3000Hz', '_4000Hz', '_6000Hz', '_8000Hz'],
            gridLineColor: '#197F07',
            gridLineWidth: 2

        },
        yAxis: {
            min: -10,
            max: 120,
            reversed: true,
            tickInterval: 10,
            title: {
                text: 'AU-OI'
            }
        },
        legend: {
            enabled: false
        },

        plotOptions: {
            series: {
                dataLabels: {
                    enabled: false
                },
                hover: {
                    enabled: false
                },
                states: {
                    hover: {
                        enabled: false
                    }
                }
                //pointStart: '2018-10-07'
            }
        },

        series: [
            {
                label: {
                    enabled: false,
                },
                dashStyle: 'longdash',
                lineWidth: 1,
                data: viaAerea_L,
                color: '#f74453',
                marker: {
                    symbol: 'url(/Content/Images/circulorojo.png)',
                    width: 12,
                    height: 12
                },
            },
            {
                type: 'scatter',
                data: EMA_L,
                color: '#f74453',
                marker: {
                    symbol: 'url(/Content/Images/triangulorojo.png)',
                    width: 12,
                    height: 12
                }
            },
            {
                type: 'scatter',
                data: viaOsea_L,
                color: '#f74453',
                marker: {
                    symbol: 'url(/Content/Images/menorrojo.png)',
                    width: 12,
                    height: 12
                }
            },
            {
                type: 'scatter',
                data: EMO_L,
                color: '#f74453',
                marker: {
                    symbol: 'url(/Content/Images/corcheteRojo.png)',
                    width: 15,
                    height: 15
                }
            }

        ],

        responsive: {
            rules: [{
                condition: {
                    maxWidth: 500
                },
                chartOptions: {
                    legend: {
                        layout: 'vertical',
                        align: 'center',
                        verticalAlign: 'bottom'
                    }
                }
            }]
        }

    });

}

function RightAudiometryGraph(viaAerea_R, EMA_R, viaOsea_R, EMO_R) {

    Highcharts.chart('chartAudiometry2', {
        credits: { enabled: false },
        tooltip: { enabled: false },
        title: {
            text: 'Detalle OD'
        },

        subtitle: {
            text: ''
        },

        yAxis: {

            min: -10,
            max: 120,
            reversed: true,
            tickInterval: 10,

            title: {
                text: 'AU-OD'
            }
        },
        legend: {
            enabled: false
        },

        plotOptions: {

            series: {
                dataLabels: {
                    enabled: false
                },
                hover: {
                    enabled: false
                },
                states: {
                    hover: {
                        enabled: false
                    }
                }

            }
        },
        xAxis: {

            opposite: true,
            categories: ['_125Hz', '_250Hz', '_500Hz', '_1000Hz', '_2000Hz', '_3000Hz', '_4000Hz', '_6000Hz', '_8000Hz'],
            //labels: {

            //    skew3d: true,
            //    style: {
            //        fontSize: '10px'
            //    }
            //}
        },
        series: [

            {
                label: {
                    enabled: false,
                },
                data: viaAerea_R,
                dashStyle: 'longdash',
                lineWidth: 1,
                color: '#4589f7',
                marker: {
                    symbol: 'url(/Content/Images/xazul.png)',
                    width: 12,
                    height: 12
                },
            },
            {

                data: EMA_R,
                type: 'scatter',
                color: '#4589f7',
                marker: {
                    symbol: 'url(/Content/Images/cuadroazul.png)',
                    width: 12,
                    height: 12
                },
            },
            {

                data: viaOsea_R,
                type: 'scatter',
                color: '#4589f7',
                marker: {
                    symbol: 'url(/Content/Images/mayorazul.png)',
                    width: 12,
                    height: 12
                },

            },
            {

                data: EMO_R,
                type: 'scatter',
                color: '#4589f7',
                marker: {
                    symbol: 'url(/Content/Images/corcheteAzul.png)',
                    width: 15,
                    height: 18
                },
            },

        ],

        responsive: {
            rules: [{
                condition: {
                    maxWidth: 500
                },
                chartOptions: {
                    legend: {
                        layout: 'vertical',
                        align: 'center',
                        verticalAlign: 'bottom'
                    }
                }
            }]
        }

    });

}

function generateEvaluacionM(componentId) {

    var html = `
            <div class="col-12 p-0 py-2">
			<div class="row w-100 m-0 p-0">
				<section class="col-12 p-0">
					<article class="row bor-top bor-le-ri w-100 m-0 p-0">
						<div class="col-2 text-center p-0"><label class="lbl-evaluacion">APTITUD</label></div>
						<div class="col-2 bor-left text-center p-0"><label class="lbl-evaluacion">Excelente: 1</label></div>
						<div class="col-2 bor-left text-center p-0"><label class="lbl-evaluacion">Promedio: 2</label></div>
						<div class="col-2 bor-left text-center p-0"><label class="lbl-evaluacion">Regular: 3</label></div>
						<div class="col-2 bor-left text-center p-0"><label class="lbl-evaluacion">Pobre: 4</label></div>
						<div class="col-1 bor-left text-center p-0"><label class="lbl-evaluacion">Puntos</label></div>
						<div class="col-1 bor-left text-center p-0"><label class="lbl-evaluacion">Observación</label></div>
					</article>
					<article class="row bor-top bor-le-ri w-100 m-0 p-0">
						<div class="col-2 d-flex justify-content-center align-items-center py-2 p-0"><label class="text-center lbl-evaluacion">Flexibilidad/Fuerza ABDOMEN</label></div>
						<div class="col-2 bor-left p-0 py-2">
							<div class="row w-100 m-0 p-0">	
								<div class="col-12 d-flex justify-content-center align-items-center m-0 p-0">
									<input data-is-delete="0" data-componentid="${componentId}" type="radio"  onchange="SumaAptitudes(this)" class="js-rad-aptitud group-field" data-input-class="total-suma" data-aptitud="js-input-abdomen" data-value="1" id="" name="N009-OTS00000001">
								</div>
								<div class="col-12 m-0 p-0">
									<figure class="m-0 d-flex justify-content-center align-items-center">
										<img src="/Content/Images/usercontrol/flex01.jpg">
									</figure>
								</div>
							</div>

						</div>
						<div class="col-2 bor-left p-0 py-2">
							<div class="row w-100 m-0 p-0">	
								<div class="col-12 d-flex justify-content-center align-items-center m-0 p-0">
									<input data-is-delete="0" data-componentid="${componentId}" type="radio" onchange="SumaAptitudes(this)" class="js-rad-aptitud group-field" data-input-class="total-suma" data-aptitud="js-input-abdomen" data-value="2" name="N009-OTS00000001">
								</div>
								<div class="col-12 m-0 p-0">
									<figure class="m-0 d-flex justify-content-center align-items-center">
										<img src="/Content/Images/usercontrol/flex02.jpg">
									</figure>
								</div>
							</div>

						</div>
						<div class="col-2 bor-left p-0 py-2">
							<div class="row w-100 m-0 p-0">	
								<div class="col-12 d-flex justify-content-center align-items-center m-0 p-0">
									<input data-is-delete="0" data-componentid="${componentId}" type="radio" onchange="SumaAptitudes(this)" class="js-rad-aptitud group-field" data-input-class="total-suma" data-aptitud="js-input-abdomen" data-value="3" name="N009-OTS00000001">
								</div>
								<div class="col-12 m-0 p-0">
									<figure class="m-0 d-flex justify-content-center align-items-center">
										<img src="/Content/Images/usercontrol/flex03.jpg">
									</figure>
								</div>
							</div>

						</div>
						<div class="col-2 bor-left p-0 py-2">
							<div class="row w-100 m-0 p-0">	
								<div class="col-12 d-flex justify-content-center align-items-center m-0 p-0">
									<input data-is-delete="0" data-componentid="${componentId}" type="radio" onchange="SumaAptitudes(this)" class="js-rad-aptitud group-field" data-input-class="total-suma" data-aptitud="js-input-abdomen" data-value="4" name="N009-OTS00000001">
								</div>
								<div class="col-12 m-0 p-0">
									<figure class="m-0 d-flex justify-content-center align-items-center">
										<img src="/Content/Images/usercontrol/flex04.jpg">
									</figure>
								</div>
							</div>

						</div>
						<div class="col-1 bor-left d-flex justify-content-center align-items-center p-0">
							<input id="field_N009-OTS00000002" data-is-delete="0" data-componentid="${componentId}" style="width:20px; height:20px;" type="text" class="form-control js-input-abdomen js-input-aptitud" disabled>
						</div>
						<div class="col-1 bor-left d-flex justify-content-center align-items-center p-0">
							<textarea id="field_N009-OTS00000003" data-is-delete="0" data-componentid="${componentId}" class="css-textarea form-control"></textarea>
						</div>
					</article>
					<article class="row bor-top bor-le-ri w-100 m-0 p-0 ">
						<div class="col-2 py-2 d-flex justify-content-center align-items-center p-0"><label class="text-center lbl-evaluacion">CADERA</label></div>
						<div class="col-2  bor-left py-2 p-0">
							<div class="row w-100 m-0 p-0">	
								<div class="col-12 d-flex justify-content-center align-items-center m-0 p-0">
									<input data-is-delete="0" data-componentid="${componentId}" type="radio" onchange="SumaAptitudes(this)" class="js-rad-aptitud group-field" data-input-class="total-suma" data-aptitud="js-input-cadera" data-value="1" name="N009-OTS00000004">
								</div>
								<div class="col-12 m-0 p-0">
									<figure class="m-0 d-flex justify-content-center align-items-center">
										<img src="/Content/Images/usercontrol/cad01.jpg">
									</figure>
								</div>
							</div>

						</div>
						<div class="col-2 bor-left py-2 p-0">
							<div class="row w-100 m-0 p-0">	
								<div class="col-12 d-flex justify-content-center align-items-center m-0 p-0">
									<input data-is-delete="0" data-componentid="${componentId}" type="radio" onchange="SumaAptitudes(this)" class="js-rad-aptitud group-field" data-input-class="total-suma" data-aptitud="js-input-cadera" data-value="2" name="N009-OTS00000004">
								</div>
								<div class="col-12 m-0 p-0">
									<figure class="m-0 d-flex justify-content-center align-items-center">
										<img src="/Content/Images/usercontrol/cad02.jpg">
									</figure>
								</div>
							</div>

						</div>
						<div class="col-2 bor-left py-2 p-0">
							<div class="row w-100 m-0 p-0">	
								<div class="col-12 d-flex justify-content-center align-items-center m-0 p-0">
									<input data-is-delete="0" data-componentid="${componentId}" type="radio" onchange="SumaAptitudes(this)" class="js-rad-aptitud group-field" data-input-class="total-suma" data-aptitud="js-input-cadera" data-value="3" name="N009-OTS00000004">
								</div>
								<div class="col-12 m-0 p-0">
									<figure class="m-0 d-flex justify-content-center align-items-center">
										<img src="/Content/Images/usercontrol/cad03.jpg">
									</figure>
								</div>
							</div>

						</div>
						<div class="col-2 bor-left py-2 p-0">
							<div class="row w-100 m-0 p-0">	
								<div class="col-12 d-flex justify-content-center align-items-center m-0 p-0">
									<input data-is-delete="0" data-componentid="${componentId}" type="radio" onchange="SumaAptitudes(this)" class="js-rad-aptitud group-field" data-input-class="total-suma" data-aptitud="js-input-cadera" data-value="4" name="N009-OTS00000004">
								</div>
								<div class="col-12 m-0 p-0">
									<figure class="m-0 d-flex justify-content-center align-items-center">
										<img src="/Content/Images/usercontrol/cad04.jpg">
									</figure>
								</div>
							</div>

						</div>
						<div class="col-1 bor-left d-flex justify-content-center align-items-center p-0">
							<input id="field_N009-OTS00000005" data-is-delete="0" data-componentid="${componentId}" style="width:20px; height:20px;" type="text" class="form-control js-input-cadera js-input-aptitud" disabled>
						</div>
						<div class="col-1 bor-left d-flex justify-content-center align-items-center p-0">
							<textarea id="field_N009-OTS00000006" data-is-delete="0" data-componentid="${componentId}" class="css-textarea form-control"></textarea>
						</div>
					</article>
					<article class="row bor-top bor-le-ri w-100 m-0 p-0">
						<div class="col-2 py-2 d-flex justify-content-center align-items-center p-0"><label class="text-center lbl-evaluacion">MUSLO</label></div>
						<div class="col-2 bor-left py-2 p-0">
							<div class="row w-100 m-0 p-0">	
								<div class="col-12 d-flex justify-content-center align-items-center m-0 p-0">
									<input data-is-delete="0" data-componentid="${componentId}" type="radio" onchange="SumaAptitudes(this)" class="js-rad-aptitud group-field" data-input-class="total-suma" data-aptitud="js-input-muslo" data-value="1" name="N009-OTS00000007">
								</div>
								<div class="col-12 m-0 p-0">
									<figure class="m-0 d-flex justify-content-center align-items-center">
										<img src="/Content/Images/usercontrol/muslo01.jpg">
									</figure>
								</div>
							</div>

						</div>
						<div class="col-2 bor-left py-2 p-0">
							<div class="row w-100 m-0 p-0">	
								<div class="col-12 d-flex justify-content-center align-items-center m-0 p-0">
									<input data-is-delete="0" data-componentid="${componentId}" type="radio" onchange="SumaAptitudes(this)" class="js-rad-aptitud group-field" data-input-class="total-suma" data-aptitud="js-input-muslo" data-value="2" name="N009-OTS00000007">
								</div>
								<div class="col-12 m-0 p-0">
									<figure class="m-0 d-flex justify-content-center align-items-center">
										<img src="/Content/Images/usercontrol/muslo02.jpg">
									</figure>
								</div>
							</div>

						</div>
						<div class="col-2 bor-left py-2 p-0">
							<div class="row w-100 m-0 p-0">	
								<div class="col-12 d-flex justify-content-center align-items-center m-0 p-0">
									<input data-is-delete="0" data-componentid="${componentId}" type="radio" onchange="SumaAptitudes(this)" class="js-rad-aptitud group-field" data-input-class="total-suma" data-aptitud="js-input-muslo" data-value="3" name="N009-OTS00000007">
								</div>
								<div class="col-12 m-0 p-0">
									<figure class="m-0 d-flex justify-content-center align-items-center">
										<img src="/Content/Images/usercontrol/muslo03.jpg">
									</figure>
								</div>
							</div>

						</div>
						<div class="col-2 bor-left py-2 p-0">
							<div class="row w-100 m-0 p-0">	
								<div class="col-12 d-flex justify-content-center align-items-center m-0 p-0">
									<input data-is-delete="0" data-componentid="${componentId}" type="radio" onchange="SumaAptitudes(this)" class="js-rad-aptitud group-field" data-input-class="total-suma" data-aptitud="js-input-muslo" data-value="4" name="N009-OTS00000007">
								</div>
								<div class="col-12 m-0 p-0">
									<figure class="m-0 d-flex justify-content-center align-items-center">
										<img src="/Content/Images/usercontrol/muslo04.jpg">
									</figure>
								</div>
							</div>

						</div>
						<div class="col-1 bor-left d-flex justify-content-center align-items-center p-0">
							<input id="field_N009-OTS00000008" data-is-delete="0" data-componentid="${componentId}" style="width:20px; height:20px;" type="text" class="form-control js-input-muslo js-input-aptitud" disabled>
						</div>
						<div class="col-1 bor-left d-flex justify-content-center align-items-center p-0">
							<textarea id="field_N009-OTS00000009" data-is-delete="0" data-componentid="${componentId}" class="css-textarea form-control"></textarea>
						</div>
					</article>
					<article class="row bor-top bor-le-ri bor-bot w-100 m-0 p-0">
						<div class="col-2 py-2 d-flex justify-content-center align-items-center p-0"><label class="text-center lbl-evaluacion">ABDOMEN LATERAL</label></div>
						<div class="col-2 bor-left py-2 p-0">
							<div class="row w-100 m-0 p-0">	
								<div class="col-12 d-flex justify-content-center align-items-center m-0 p-0">
									<input data-is-delete="0" data-componentid="${componentId}" type="radio" onchange="SumaAptitudes(this)" class="js-rad-aptitud group-field" data-input-class="total-suma" data-aptitud="js-input-abdomen-lat" data-value="1" name="N009-OTS00000010">
								</div>
								<div class="col-12 m-0 p-0">
									<figure class="m-0 d-flex justify-content-center align-items-center">
										<img src="/Content/Images/usercontrol/abd01.jpg">
									</figure>
								</div>
							</div>

						</div>
						<div class="col-2 bor-left py-2 p-0">
							<div class="row w-100 m-0 p-0">	
								<div class="col-12 d-flex justify-content-center align-items-center m-0 p-0">
									<input data-is-delete="0" data-componentid="${componentId}" type="radio" onchange="SumaAptitudes(this)" class="js-rad-aptitud group-field" data-input-class="total-suma" data-aptitud="js-input-abdomen-lat" data-value="2" name="N009-OTS00000010">
								</div>
								<div class="col-12 m-0 p-0">
									<figure class="m-0 d-flex justify-content-center align-items-center">
										<img src="/Content/Images/usercontrol/abd02.jpg">
									</figure>
								</div>
							</div>

						</div>
						<div class="col-2 bor-left py-2 p-0">
							<div class="row w-100 m-0 p-0">	
								<div class="col-12 d-flex justify-content-center align-items-center m-0 p-0">
									<input data-is-delete="0" data-componentid="${componentId}" type="radio" onchange="SumaAptitudes(this)" class="js-rad-aptitud group-field" data-input-class="total-suma" data-aptitud="js-input-abdomen-lat" data-value="3" name="N009-OTS00000010">
								</div>
								<div class="col-12 m-0 p-0">
									<figure class="m-0 d-flex justify-content-center align-items-center">
										<img src="/Content/Images/usercontrol/abd03.jpg">
									</figure>
								</div>
							</div>

						</div>
						<div class="col-2 bor-left py-2 p-0">
							<div class="row w-100 m-0 p-0">	
								<div class="col-12 d-flex justify-content-center align-items-center m-0 p-0">
									<input data-is-delete="0" data-componentid="${componentId}" type="radio" onchange="SumaAptitudes(this)" class="js-rad-aptitud group-field" data-input-class="total-suma" data-aptitud="js-input-abdomen-lat" data-value="4" name="N009-OTS00000010">
								</div>
								<div class="col-12 m-0 p-0">
									<figure class="m-0 d-flex justify-content-center align-items-center">
										<img src="/Content/Images/usercontrol/abd04.jpg">
									</figure>
								</div>
							</div>

						</div>
						<div class="col-1 bor-left d-flex justify-content-center align-items-center p-0">
							<input id="field_N009-OTS00000011" data-is-delete="0" data-componentid="${componentId}" style="width:20px; height:20px;" type="text" class="form-control js-input-abdomen-lat js-input-aptitud" disabled>
						</div>
						<div class="col-1 bor-left d-flex justify-content-center align-items-center p-0">
							<textarea id="field_N009-OTS00000012" data-is-delete="0" data-componentid="${componentId}" class="css-textarea form-control"></textarea>
						</div>
					</article>
					<article class="row  w-100 m-0 p-0 p-0 py-2">
						<div style="padding-right: 109px" class="col-12 d-flex justify-content-end">
							<label>TOTAL:</label>
							<input id="field_N009-OTS00000026" data-is-delete="0" data-componentid="${componentId}" type="text" class="ml-2 form-control total-suma" style="width:20px; height:20px" disabled>
						</div>
					</article>
				</section>
				<section class="col-12 p-0">
					<article class="row w-100 m-0 p-0 py-2"><label>RANGOS ARTICULARES</label></article>

					<article class="row bor-top bor-le-ri w-100 m-0 p-0">
						<div class="col-3 text-center p-0"><label class="lbl-rango">RANGOS</label></div>
						<div class="col-2 bor-left text-center p-0"><label class="lbl-evaluacion">Optimo: 1</label></div>
						<div class="col-2 bor-left text-center p-0"><label class="lbl-evaluacion">Limitado: 2</label></div>
						<div class="col-2 bor-left text-center p-0"><label class="lbl-evaluacion">Muy Limitado: 3</label></div>
						<div class="col-1 bor-left text-center p-0"><label class="lbl-evaluacion">Puntos</label></div>
						<div class="col-2 bor-left text-center p-0"><label class="lbl-evaluacion">Dolor contra resistencia</label></div>
					</article>
					<article class="row bor-top bor-le-ri w-100 m-0 p-0">
						<div class="col-3 d-flex justify-content-center align-items-center py-2 p-0"><label class="text-center lbl-rango">ABDUCCIÓN DE HOMBRO (NORMAL 0° - 180°)</label></div>
						<div class="col-2 bor-left p-0 py-2">
							<div class="row w-100 m-0 p-0">	
								<div class="col-12 d-flex justify-content-center align-items-center m-0 p-0">
									<input data-is-delete="0" data-componentid="${componentId}" type="radio" onchange="SumaAptitudes(this)" class="js-rad-rangos group-field" data-input-class="total-suma-rango" data-aptitud="js-input-180" data-value="1" name="N009-OTS00000013">
								</div>
								<div class="col-12 m-0 p-0">
									<figure class="m-0 d-flex justify-content-center align-items-center">
										<img src="/Content/Images/usercontrol/ab01.jpg">
									</figure>
								</div>
							</div>
						</div>
						<div class="col-2 bor-left p-0 py-2">
							<div class="row w-100 m-0 p-0">	
								<div class="col-12 d-flex justify-content-center align-items-center m-0 p-0">
									<input data-is-delete="0" data-componentid="${componentId}" type="radio" onchange="SumaAptitudes(this)" class="js-rad-rangos group-field" data-input-class="total-suma-rango" data-aptitud="js-input-180" data-value="2" name="N009-OTS00000013">
								</div>
								<div class="col-12 m-0 p-0">
									<figure class="m-0 d-flex justify-content-center align-items-center">
										<img src="/Content/Images/usercontrol/ab02.jpg">
									</figure>
								</div>
							</div>
						</div>
						<div class="col-2 bor-left p-0 py-2">
							<div class="row w-100 m-0 p-0">	
								<div class="col-12 d-flex justify-content-center align-items-center m-0 p-0">
									<input data-is-delete="0" data-componentid="${componentId}" type="radio" onchange="SumaAptitudes(this)" class="js-rad-rangos group-field" data-input-class="total-suma-rango" data-aptitud="js-input-180" data-value="3" name="N009-OTS00000013">
								</div>
								<div class="col-12 m-0 p-0">
									<figure class="m-0 d-flex justify-content-center align-items-center">
										<img src="/Content/Images/usercontrol/ab03.jpg">
									</figure>
								</div>
							</div>
						</div>
						<div class="col-1 bor-left d-flex justify-content-center align-items-center p-0">
							<input id="field_N009-OTS00000014" data-is-delete="0" data-componentid="${componentId}" style="width:20px; height:20px;" type="text" class="form-control js-input-180 js-input-rango" disabled>
						</div>
						<div class="col-2 bor-left d-flex justify-content-center align-items-center p-0">

							<input id="field_N009-OTS00000015" data-is-delete="0" data-componentid="${componentId}" data-value="1" type="radio" class="group-field" onchange="checkRadioUser(this)" name="N009-OTS00000015">
							<label class="m-0 ml-1 mr-3">SI</label>
							<input id="field_N009-OTS00000015" data-is-delete="0" data-componentid="${componentId}" data-value="2" type="radio" class="group-field" onchange="checkRadioUser(this)" name="N009-OTS00000015">
							<label class="m-0 ml-1">NO</label>

						</div>
					</article>
					<article class="row bor-top bor-le-ri w-100 m-0 p-0">
						<div class="col-3 d-flex justify-content-center align-items-center py-2 p-0"><label class="text-center lbl-rango">ABDUCCIÓN DE HOMBRO (NORMAL 0° - 60°)</label></div>
						<div class="col-2 bor-left p-0 py-2">
							<div class="row w-100 m-0 p-0">	
								<div class="col-12 d-flex justify-content-center align-items-center m-0 p-0">
									<input data-is-delete="0" data-componentid="${componentId}" type="radio" onchange="SumaAptitudes(this)" class="js-rad-rangos group-field" data-input-class="total-suma-rango" data-aptitud="js-input-60" data-value="1" name="N009-OTS00000016">
								</div>
								<div class="col-12 m-0 p-0">
									<figure class="m-0 d-flex justify-content-center align-items-center">
										<img src="/Content/Images/usercontrol/ad01.jpg">
									</figure>
								</div>
							</div>
						</div>
						<div class="col-2 bor-left p-0 py-2">
							<div class="row w-100 m-0 p-0">	
								<div class="col-12 d-flex justify-content-center align-items-center m-0 p-0">
									<input data-is-delete="0" data-componentid="${componentId}" type="radio" onchange="SumaAptitudes(this)" class="js-rad-rangos group-field" data-input-class="total-suma-rango" data-aptitud="js-input-60" data-value="2" name="N009-OTS00000016">
								</div>
								<div class="col-12 m-0 p-0">
									<figure class="m-0 d-flex justify-content-center align-items-center">
										<img src="/Content/Images/usercontrol/ad02.jpg">
									</figure>
								</div>
							</div>
						</div>
						<div class="col-2 bor-left p-0 py-2">
							<div class="row w-100 m-0 p-0">	
								<div class="col-12 d-flex justify-content-center align-items-center m-0 p-0">
									<input data-is-delete="0" data-componentid="${componentId}" type="radio" onchange="SumaAptitudes(this)" class="js-rad-rangos group-field" data-input-class="total-suma-rango" data-aptitud="js-input-60" data-value="3" name="N009-OTS00000016">
								</div>
								<div class="col-12 m-0 p-0">
									<figure class="m-0 d-flex justify-content-center align-items-center">
										<img src="/Content/Images/usercontrol/ad03.jpg">
									</figure>
								</div>
							</div>
						</div>
						<div class="col-1 bor-left d-flex justify-content-center align-items-center p-0">
							<input id="field_N009-OTS00000017" data-is-delete="0" data-componentid="${componentId}" style="width:20px; height:20px;" type="text" class="form-control js-input-60 js-input-rango" disabled>
						</div>
						<div class="col-2 bor-left d-flex justify-content-center align-items-center p-0">

							<input id="field_N009-OTS00000018" data-is-delete="0" data-componentid="${componentId}" data-value="1" type="radio" class="group-field" onchange="checkRadioUser(this)" name="N009-OTS00000018">
							<label class="m-0 ml-1 mr-3">SI</label>
							<input id="field_N009-OTS00000018" data-is-delete="0" data-componentid="${componentId}" data-value="2" type="radio" class="group-field" onchange="checkRadioUser(this)" name="N009-OTS00000018">
							<label class="m-0 ml-1">NO</label>

						</div>
					</article>
					<article class="row bor-top bor-le-ri w-100 m-0 p-0">
						<div class="col-3 d-flex justify-content-center align-items-center py-2 p-0"><label class="text-center lbl-rango">ROTACIÓN EXTERNA (0° - 60°)</label></div>
						<div class="col-2 bor-left p-0 py-2">
							<div class="row w-100 m-0 p-0">	
								<div class="col-12 d-flex justify-content-center align-items-center m-0 p-0">
									<input data-is-delete="0" data-componentid="${componentId}" type="radio" onchange="SumaAptitudes(this)" class="js-rad-rangos group-field" data-input-class="total-suma-rango" data-aptitud="js-input-90" data-value="1" name="N009-OTS00000019">
								</div>
								<div class="col-12 m-0 p-0">
									<figure class="m-0 d-flex justify-content-center align-items-center">
										<img src="/Content/Images/usercontrol/rot9001.jpg">
									</figure>
								</div>
							</div>
						</div>
						<div class="col-2 bor-left p-0 py-2">
							<div class="row w-100 m-0 p-0">	
								<div class="col-12 d-flex justify-content-center align-items-center m-0 p-0">
									<input data-is-delete="0" data-componentid="${componentId}" type="radio" onchange="SumaAptitudes(this)" class="js-rad-rangos group-field" data-input-class="total-suma-rango" data-aptitud="js-input-90" data-value="2" name="N009-OTS00000019">
								</div>
								<div class="col-12 m-0 p-0">
									<figure class="m-0 d-flex justify-content-center align-items-center">
										<img src="/Content/Images/usercontrol/rot9002.jpg">
									</figure>
								</div>
							</div>
						</div>
						<div class="col-2 bor-left p-0 py-2">
							<div class="row w-100 m-0 p-0">	
								<div class="col-12 d-flex justify-content-center align-items-center m-0 p-0">
									<input data-is-delete="0" data-componentid="${componentId}" type="radio" onchange="SumaAptitudes(this)" class="js-rad-rangos group-field" data-input-class="total-suma-rango" data-aptitud="js-input-90" data-value="3" name="N009-OTS00000019">
								</div>
								<div class="col-12 m-0 p-0">
									<figure class="m-0 d-flex justify-content-center align-items-center">
										<img src="/Content/Images/usercontrol/rot9003.jpg">
									</figure>
								</div>
							</div>
						</div>
						<div class="col-1 bor-left d-flex justify-content-center align-items-center p-0">
							<input id="field_N009-OTS00000020" data-is-delete="0" data-componentid="${componentId}" style="width:20px; height:20px;" type="text" class="form-control js-input-90 js-input-rango" disabled>
						</div>
						<div class="col-2 bor-left d-flex justify-content-center align-items-center p-0">

							<input id="field_N009-OTS00000021" data-is-delete="0" data-componentid="${componentId}" data-value="1" type="radio" class="group-field" onchange="checkRadioUser(this)" name="N009-OTS00000021">
							<label class="m-0 ml-1 mr-3">SI</label>
							<input id="field_N009-OTS00000021" data-is-delete="0" data-componentid="${componentId}" data-value="2" type="radio" class="group-field" onchange="checkRadioUser(this)" name="N009-OTS00000021">
							<label class="m-0 ml-1">NO</label>

						</div>
					</article>
					<article class="row bor-top bor-le-ri  bor-bot w-100 m-0 p-0">
						<div class="col-3 d-flex justify-content-center align-items-center py-2 p-0"><label class="text-center lbl-rango">ROTACIÓN EXTERNA DE HOMBRO (INTERNA)</label></div>
						<div class="col-2 bor-left p-0 py-2">
							<div class="row w-100 m-0 p-0">	
								<div class="col-12 d-flex justify-content-center align-items-center m-0 p-0">
									<input data-is-delete="0" data-componentid="${componentId}" type="radio" onchange="SumaAptitudes(this)" class="js-rad-rangos group-field" data-input-class="total-suma-rango" data-aptitud="js-input-interna" data-value="1" name="N009-OTS00000022">
								</div>
								<div class="col-12 m-0 p-0">
									<figure class="m-0 d-flex justify-content-center align-items-center">
										<img src="/Content/Images/usercontrol/rot9001.jpg">
									</figure>
								</div>
							</div>
						</div>
						<div class="col-2 bor-left p-0 py-2">
							<div class="row w-100 m-0 p-0">	
								<div class="col-12 d-flex justify-content-center align-items-center m-0 p-0">
									<input data-is-delete="0" data-componentid="${componentId}" type="radio" onchange="SumaAptitudes(this)" class="js-rad-rangos group-field" data-input-class="total-suma-rango" data-aptitud="js-input-interna" data-value="2" name="N009-OTS00000022">
								</div>
								<div class="col-12 m-0 p-0">
									<figure class="m-0 d-flex justify-content-center align-items-center">
										<img src="/Content/Images/usercontrol/rot9002.jpg">
									</figure>
								</div>
							</div>
						</div>
						<div class="col-2 bor-left p-0 py-2">
							<div class="row w-100 m-0 p-0">	
								<div class="col-12 d-flex justify-content-center align-items-center m-0 p-0">
									<input data-is-delete="0" data-componentid="${componentId}" type="radio" onchange="SumaAptitudes(this)" class="js-rad-rangos group-field" data-input-class="total-suma-rango" data-aptitud="js-input-interna" data-value="3" name="N009-OTS00000022">
								</div>
								<div class="col-12 m-0 p-0">
									<figure class="m-0 d-flex justify-content-center align-items-center">
										<img src="/Content/Images/usercontrol/rot9003.jpg">
									</figure>
								</div>
							</div>
						</div>
						<div class="col-1 bor-left d-flex justify-content-center align-items-center p-0">
							<input id="field_N009-OTS00000023" data-is-delete="0" data-componentid="${componentId}" style="width:20px; height:20px;" type="text" class="form-control js-input-interna js-input-rango" disabled>
						</div>
						<div class="col-2 bor-left d-flex justify-content-center align-items-center p-0">

							<input id="field_N009-OTS00000024" data-is-delete="0" data-componentid="${componentId}" data-value="1" type="radio" class="group-field" onchange="checkRadioUser(this)" name="N009-OTS00000024">
							<label class="m-0 ml-1 mr-3">SI</label>
							<input id="field_N009-OTS00000024" data-is-delete="0" data-componentid="${componentId}" data-value="2" type="radio" class="group-field" onchange="checkRadioUser(this)" name="N009-OTS00000024">
							<label class="m-0 ml-1">NO</label>

						</div>
					</article>

					<article class="row  w-100 m-0 p-0 p-0 py-2">
						<div style="padding-right: 185px" class="col-12 d-flex justify-content-end">
							<label>TOTAL:</label>
							<input id="field_N009-OTS00000027" data-is-delete="0" data-componentid="${componentId}" type="text" class="ml-2 form-control total-suma-rango" style="width:20px; height:20px" disabled>
						</div>
					</article>

					<article class="row w-100 m-0 p-0 py-2">
						<div class="col-12"><label>OBSERVACIONES</label></div>
						<div class="col-12">
							<textarea id="field_N009-OTS00000025" data-is-delete="0" data-componentid="${componentId}" class="form-control" style="font-size: 0.8rem; width: 100%; height: 40px; resize: none; padding: 2px"></textarea>
						</div>
					</article>
				</section>
			</div>
		</div>
            `;

    return html;
}

function SumaAptitudes(input) {
    var name = $(input).attr('name');
    
    $('input[name=' + name + ']').removeClass('enviar-este');
    $(input).addClass('enviar-este');

    var inputValue = $(input).data("aptitud");
    var className = $(input).attr("class").split(" ")[0];

    var inputClass = $(input).data("input-class");
    if (className == "js-rad-rangos") {
        className = "js-input-rango"
    } else {
        className = "js-input-aptitud"
    }
    $("." + inputValue).val($(input).data("value"));
    var suma = 0;
    for (var i = 0; i < $("." + className).length; i++) {
        var value = $("." + className + ":eq(" + i + ")").val() == ""
            ? 0 : $("." + className + ":eq(" + i + ")").val();

        suma = suma + parseInt(value);
    }
    $("." + inputClass).val(suma);
}

function checkRadioUser(checkb) {

    var name = $(checkb).attr('name');

    $('input[name=' + name + ']').removeClass('enviar-este');
    $(checkb).addClass('enviar-este');

}

function setUserInputs() {

    var valueFlex = $('#field_N009-OTS00000002').val();
    var valueCadera = $('#field_N009-OTS00000005').val();
    var valueMuslo = $('#field_N009-OTS00000008').val();
    var valueAbdoLat = $('#field_N009-OTS00000011').val();
    var valueAb180 = $('#field_N009-OTS00000014').val();
    var valueAb60 = $('#field_N009-OTS00000017').val();
    var valueRot60 = $('#field_N009-OTS00000020').val();
    var valueRotInter = $('#field_N009-OTS00000023').val();

    for (var i = 0; i < $('input[data-aptitud="js-input-abdomen"]').length; i++) {
        if ($('input[data-aptitud="js-input-abdomen"]').eq(i).data('value') == valueFlex) {
            $('input[data-aptitud="js-input-abdomen"]').eq(i).prop("checked", true).addClass('enviar-este');
        }
        if ($('input[data-aptitud="js-input-cadera"]').eq(i).data('value') == valueCadera) {
            $('input[data-aptitud="js-input-cadera"]').eq(i).prop("checked", true).addClass('enviar-este');
        }
        if ($('input[data-aptitud="js-input-muslo"]').eq(i).data('value') == valueMuslo) {
            $('input[data-aptitud="js-input-muslo"]').eq(i).prop("checked", true).addClass('enviar-este');
        }
        if ($('input[data-aptitud="js-input-abdomen-lat"]').eq(i).data('value') == valueAbdoLat) {
            $('input[data-aptitud="js-input-abdomen-lat"]').eq(i).prop("checked", true).addClass('enviar-este');
        }


        if ($('input[data-aptitud="js-input-180"]').eq(i).data('value') == valueAb180) {
            $('input[data-aptitud="js-input-180"]').eq(i).prop("checked", true).addClass('enviar-este');
        }
        if ($('input[data-aptitud="js-input-60"]').eq(i).data('value') == valueAb60) {
            $('input[data-aptitud="js-input-60"]').eq(i).prop("checked", true).addClass('enviar-este');
        }
        if ($('input[data-aptitud="js-input-90"]').eq(i).data('value') == valueRot60) {
            $('input[data-aptitud="js-input-90"]').eq(i).prop("checked", true).addClass('enviar-este');
        }
        if ($('input[data-aptitud="js-input-interna"]').eq(i).data('value') == valueRotInter) {
            $('input[data-aptitud="js-input-interna"]').eq(i).prop("checked", true).addClass('enviar-este');
        }
    };


    
}

