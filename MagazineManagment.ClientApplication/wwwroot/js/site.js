//// load image 
//var loadImage = function (event) {
//    var image = document.getElementById('img2');
//    image.style.width = "400px";
//    image.src = URL.createObjectURL(event.target.files[0]);
//}

////request processor animation play when form is valid only 
//var loader = document.getElementById("loader");
//loader.style.display = "none";
//function formVlidation() {
//    let validationMessages = document.getElementsByTagName("span");
//    var couter = 0;
//    for (var item = 0; item < validationMessages.length; item++) {
//        if (validationMessages[item].innerHTML == "") {
//            couter++;
//        }
//    }
//    console.log(couter);
//    if (couter == validationMessages.length) {
//        loader.style.display = "block";
//        window.scrollTo({ top: 0 });
//    }
//}