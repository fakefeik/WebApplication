var images;
var imagesSmall;
var maxImages;
var startIndex = 0;

function initGallery(model) {
    maxImages = model.MaxImages;
    images = model.Images;
    imagesSmall = model.ImagePreviews;
}

function indexOf(arr, x) {
    for (var i = 0; i < arr.length; i++)
        if (x.indexOf(arr[i]) != -1)
            return i;
    return -1;
}

function photoLoaded() {
    $("#loading").remove();
    $("#image-large").css("visibility", "inherit");
}

function switchPhoto(delta) {
    var imageLarge = $("#image-large");
    var nextImage = indexOf(images, imageLarge.attr("src")) + delta;
    if (nextImage < 0 || nextImage >= images.length)
        return;

    $("#image-large").remove();
    $("#over-overlay").append(
    	"<img id='image-large' src='" + images[nextImage] + "' onload='photoLoaded();' style='visibility: hidden;'/>" +
    	"<img id='loading' src='/Content/images/loading.gif' />"
    );
    imageLarge.css("visibility", "hidden");
    imageLarge.attr("src", images[indexOf(images, imageLarge.attr("src")) + delta]);
}

$(document).ready(function () {
    var imagePreviews = $(".image-previews>img");
    for (var i = 1; i < maxImages + 1; i++)
        $(imagePreviews[i]).attr("src", imagesSmall[i + startIndex - 1]);

    $(".image-previews>img").click(function (e) {
        $(document.body).append(
			"<div id='overlay'></div>" +
			"<div id='over-overlay'>" +
				"<img id='previous' src='/Content/images/arrow-previous.png' />" +
				"<img id='image-large' src='" + images[indexOf(imagesSmall, e.target.src)] + "' onload='photoLoaded();' style='visibility: hidden;'/>" +
				"<img id='loading' src='/Content/images/loading.gif' />" +
				"<img id='next' src='/Content/images/arrow-next.png' />" +
				"<img id='close' src='/Content/images/close-icon.png' />" +
			"</div>"
		);

        var closeOverlay = function (e) {
            $("#overlay").remove();
            $("#over-overlay").remove();
        };

        $("#overlay").click(closeOverlay);
        $("#close").click(closeOverlay);

        $("#previous").click(function () {
            switchPhoto(-1);
        });

        $("#next").click(function () {
            switchPhoto(1);
        });
    });

    $("#arrow-previous")
		.unbind("click")
		.click(function () {
		    if (startIndex > 0) {
		        startIndex--;
		        var imagePreviews = $(".image-previews>img");
		        for (var i = 1; i < maxImages + 1; i++)
		            $(imagePreviews[i]).attr("src", imagesSmall[i + startIndex - 1]);
		    }
		});
    $("#arrow-next")
		.unbind("click")
		.click(function () {
		    if (startIndex + maxImages < images.length) {
		        startIndex++;
		        var imagePreviews = $(".image-previews>img");
		        for (var i = 1; i < maxImages + 1; i++)
		            $(imagePreviews[i]).attr("src", imagesSmall[i + startIndex - 1]);
		    }
		});
});
