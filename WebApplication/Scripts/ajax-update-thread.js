$(document).ready(function () {
    var refreshButton = $("#refresh");
    refreshButton.click(function () {
        $.ajax({
            type: "POST",
            url: refreshButton.data("url"),
            data: {
                threadId: refreshButton.data("thread-id"),
                currentCount: $(".post").length
            }
        }).success(function (ans) {
            for (var i = 0; i < ans.length; i++) {
                $(".post").last().after(
                    "<div class='post list-group-item clickable' data-link='#'>" +
                        "<p>" + (ans[i].Username === undefined ? "DELETED" : ans[i].Username) + "</p>" +
                        "<p>" + ans[i].Id +  "</p>" +
                        "<p>" + ans[i].Timestamp + "</p>" +
                        "<p>" + ans[i].Topic + "</p>" +
                        "<p>" + ans[i].Text + "</p>" +
                    "</div>"
                );
            }
            console.log(ans);
        });
    });
});
