

function generateList(data) {
    $("#lista-examen").empty();
    for (var i = 0; i < data.length; i++) {
        var listCategory = `
                            <li data-tab-id="categoryId-${data[i].CategoryId}" class="list-group-item">
                                <i id="icon-categoryId-${data[i].CategoryId}" onclick="return colapse(this)" class="fas fa-plus"></i> ${data[i].CategoryName}
                                <ul id="contComponents-${data[i].CategoryId}">
                                    

                                </ul>
                            </li>
                            `;


        $(listCategory).appendTo($("#lista-examen"));

        var components = data[i].Components;
        for (var x = 0; x < components.length; x++) {

            if ($("#" + components[x].ComponenId ).length == 0) {
                var listComponents = `
                            <li data-name-component="${components[x].ComponentName}" class="lista-examen" data-name-category=" ${data[i].CategoryName}" data-tab-id="${data[i].CategoryId}" data-component-id="${components[x].ComponenId}" id="component_${components[x].ComponenId}">
                                ${components[x].ComponentName}
                                <ul class="d-none cont-componentsField_${components[x].ComponenId}">
                                    

                                </ul>
                            </li>
                            `;
            } else {
                var listComponents = `
                            <li data-name-component="${components[x].ComponentName}" data-name-category=" ${data[i].CategoryName}" data-tab-id="${data[i].CategoryId}" data-component-id="${components[x].ComponenId}" id="component_${components[x].ComponenId}">
                                ${components[x].ComponentName}<i style='color: red; cursor: pointer;' class=' ml-2 fas fa-times-circle' id='icon-${components[x].ComponenId}' onclick='return deleteField(this , "component_${components[x].ComponenId}")'></i>
                                <ul class="d-none cont-componentsField_${components[x].ComponenId}">
                                    

                                </ul>
                            </li>
                            `;
            }
            


            $(listComponents).appendTo($("#contComponents-" + data[i].CategoryId));

            var componentsFields = data[i].Components[x].fields;

            var arrListComponentFields = [];
            for (var y = 0; y < componentsFields.length; y++) {
                var code = window.btoa(componentsFields[y].ComboValues);
                componentsFields[y].ValidateValue2 = componentsFields[y].ValidateValue2 == 0 ? 1000 : componentsFields[y].ValidateValue2; 

                if (componentsFields[y].FormulaList != null) {

                    var formula = componentsFields[y].FormulaList[0].v_Formula;
                    var idInputFormula = componentsFields[y].FormulaList[0].v_TargetFieldOfCalculateId;

                    var regex = /\[([^\]]+)]/g, //obtiene lo que esta dentro de los []
                        match,
                        resultado = [];

                    //bucle para todas las coincidencias
                    while ((match = regex.exec(formula)) !== null) {
                        resultado.push(match[1]);
                    }
                    var idImplicados = JSON.stringify(resultado);
                    var formulaJson = formula;
                } else {
                    var idImplicados = '';
                    var idInputFormula = '';
                    var formulaJson = '';
                }

                var listComponentField = `
                                        <li data-field-id="${componentsFields[y].ComponentFieldId}"
                                            data-idimplicados='${idImplicados}'
                                            data-idcalculate='${idInputFormula}'
                                            data-formula='${formulaJson}'
                                            data-width="${componentsFields[y].WidthControl}"
                                            data-height="${componentsFields[y].HeightControl}"
                                            data-label-width="${componentsFields[y].LabelWidth}"
                                            data-label="${componentsFields[y].TextLabel}"
                                            data-column="${componentsFields[y].Column}"
                                            data-control-id="${componentsFields[y].ControlId}"
                                            data-min="${componentsFields[y].ValidateValue1}"
                                            data-max="${componentsFields[y].ValidateValue2}"
                                            data-cant-decimal="${componentsFields[y].NroDecimales}"
                                            data-name-subgroup="${componentsFields[y].Group}"
                                            data-default-text="${componentsFields[y].DefaultText}"
                                            data-values='${JSON.stringify(componentsFields[y].ComboValues)}'>

                                        </li>
                                        `; 
                arrListComponentFields.push(listComponentField);
                //console.log(code);
            }

            $(".cont-componentsField_" + components[x].ComponenId).html(arrListComponentFields.join(""));
        }

    }

    //if (!$("#lista-examen li ul").hasClass("toggle")) {
    //    $("#lista-examen li ul").animate({ height: 'toggle' });
        
    //}
    $("#lista-examen li ul").css({
        overflow: 'hidden',
    });
    $("#lista-examen li ul").animate({
        height: '0px',
    });

    $("#lista-examen li ul li ").draggable({
        helper: 'clone',
    });

    $("#contExamenes").droppable({

        drop: eventodrop, /*no se agrega las parentesis*/

    });
}