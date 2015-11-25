var images;
var imagesSmall;
var maxImages;
var isLarge = false;
var isVideo = false;
var volume = 0.5;
var startIndex = 0;

function initGallery(model) {
    maxImages = model.MaxImages;
    images = model.Images;
    imagesSmall = model.ImagePreviews;
    isVideo = model.IsVideo;
}

function indexOf(arr, x) {
    for (var i = 0; i < arr.length; i++)
        if (x.indexOf(arr[i]) != -1)
            return i;
    return -1;
}

function volumeChanged() {
    volume = $("#image-large").prop("volume");
}

function getVideoWithSource(src) {
    return  "<video id='image-large' controls loop='1' autoplay height='100%' width='100%' volume='" + volume + "' onvolumechange='volumeChanged();'>" +
                "<source height='100%' width='100%' type='video/webm' src='" + src + "' >" +
            "</video>";
}

function getPhotoWithSource(src) {
    return  "<img id='image-large' src='" + src + "' onload='photoLoaded();' style='visibility: hidden;'/>" +
            "<img id='loading' src='/Content/images/loading.gif' />";
}

function preload(arrayOfImages) { 
    $(arrayOfImages).each(function () { 
        new Image().src = this; 
    }); 
} 

function photoLoaded() {
    $("#loading").remove();
    $("#image-large").css("visibility", "inherit");
}

function switchPreview(delta) {
    var idx = startIndex + delta;
    if (idx >= 0 && idx + maxImages <= images.length) {
        startIndex = idx;
        var imagePreviews = $(".image-previews>img");
        for (var i = 1; i < maxImages + 1; i++)
            $(imagePreviews[i]).attr("src", imagesSmall[i + startIndex - 1]);
    }
}

function switchPhoto(delta) {
    var imageLarge = isVideo ? $("#image-large>source") : $("#image-large");
    var nextImage = indexOf(images, imageLarge.attr("src")) + delta;
    if (nextImage < 0 || nextImage >= images.length)
        return;

    $("#image-large").remove();
    $("#over-overlay").append(isVideo ? getVideoWithSource(images[nextImage]) : getPhotoWithSource(images[nextImage]));
    imageLarge.css("visibility", "hidden");
    imageLarge.attr("src", images[indexOf(images, imageLarge.attr("src")) + delta]);
    if (isVideo)
        $("#image-large").prop("volume", volume);
}

function closeOverlay() {
    $("#overlay").remove();
    $("#over-overlay").remove();
    isLarge = false;
};

window.onkeydown = function (e) {
    if (e.keyCode === 27 && isLarge) {
        closeOverlay();
    }

    var delta = e.keyCode === 37 ? -1 : e.keyCode === 39 ? 1 : 0;
    if (delta === 0)
        return;

    if (isLarge) {
        switchPhoto(delta);
    } else {
        switchPreview(delta);
    }
};

$(document).ready(function () {
    preload(imagesSmall);
    var imagePreviews = $(".image-previews>img");
    for (var i = 1; i < maxImages + 1; i++)
        $(imagePreviews[i]).attr("src", imagesSmall[i + startIndex - 1]);

    $(".image-previews>img").click(function (e) {
        isLarge = true;
        $(document.body).append(
			"<div id='overlay'></div>" +
			"<div id='over-overlay'>" +
				"<img id='previous' src='/Content/images/arrow-previous.png' />" +
                (isVideo ?
                    getVideoWithSource(images[indexOf(imagesSmall, e.target.src)]) :
                    getPhotoWithSource(images[indexOf(imagesSmall, e.target.src)])) +
				"<img id='next' src='/Content/images/arrow-next.png' />" +
				"<img id='close' src='/Content/images/close-icon.png' />" +
			"</div>"
		);

        if (isVideo)
            $("#image-large").prop("volume", volume);

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
            switchPreview(-1);
        });
    $("#arrow-next")
		.unbind("click")
		.click(function () {
            switchPreview(1);
        });
});
