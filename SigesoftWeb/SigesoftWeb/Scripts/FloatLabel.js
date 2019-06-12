let inputs = document.getElementsByClassName("inputLabelFloat");
for (var i = 0; i < inputs.length; i++) {
    inputs[i].addEventListener('keyup', function() {
        if (this.value.length > 0) {
            this.nextElementSibling.classList.add('static');
        } else {
            this.nextElementSibling.classList.remove('static');
        }
    })
}