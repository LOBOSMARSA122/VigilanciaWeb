

$('[data-validation-input="string"]').on('blur', function () {
    
    var MinMax = $(this).data("length").split("-");
    var message = $(this).data("message");
    var min = parseInt(MinMax[0]);
    var max = parseInt(MinMax[1]);

    var label = `<div class="row row-validation w-100 m-0 p-0 pl-2"><label style=" color: #dc3545; font-size: 0.8rem; margin: 0px; ">${message}</label></div>
                    `;

    if ($(this).val().length > max || $(this).val().length < min) {
        
        if (!$(this).next().hasClass("row-validation")) {

            if ($(this).next().hasClass("row-informacion")) {
                $(this).next().remove();
            }
            
            $($(this)).after(label);
            
        }
        
    } else if ($(this).next().hasClass("row-validation")) {
        //$(this).removeClass("border-danger");
        $(this).next().remove()
    }
});

$('[data-validation-input="string"]').on('keyup', function () {

    var largo = $(this).val().length;
    var MinMax = $(this).data("length").split("-");
    var min = parseInt(MinMax[0]);
    var max = parseInt(MinMax[1]);
    
    if ($(this).val().length < max || $(this).val().length > min) {
        if (!$(this).next().hasClass("row-informacion")) {

            var info = `
                <div class="row row-informacion w-100 m-0 p-0 pl-2">
                    <label style=" color: #adabab; font-size: 0.7rem; margin: 0px; ">Total: </label>
                    <label class="largo" style="color: #adabab; font-size: 0.7rem; margin: 0px;" >${largo}</label>
                    <label style=" color: #adabab; font-size: 0.7rem; margin: 0px; ">/</label>
                    <label style=" color: #adabab; font-size: 0.7rem; margin: 0px; ">${MinMax[1]}</label>
                </div>
                `;
            if ($(this).next().hasClass("row-validation")) {

                $(this).next().remove();
            }
            $($(this)).after(info);
        } else {
            $(this).next().find(".largo").text(largo); 
        };
        
    };
});