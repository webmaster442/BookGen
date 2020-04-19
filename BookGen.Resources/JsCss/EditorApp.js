var editor = ace.edit("editorcontent");
editor.setTheme("ace/theme/github");
editor.session.setMode("ace/mode/markdown");
editor.session.setUseWrapMode(true);
editor.session.setTabSize(4);
editor.setShowPrintMargin(false);
editor.setOption("wrap", true);

function ReSize() {
    let h = window.innerHeight;
    let maxheight = (h - 120) - $("#navbar").height();
    $(".editortab").css("height", maxheight + "px");
    editor.setOption("maxLines", maxheight/21);
    $("#raw").css("height", maxheight + "px");
}

function ReadGetParameter(parameterName) {
    var result = null,
        tmp = [];
    location.search
        .substr(1)
        .split("&")
        .forEach(function (item) {
            tmp = item.split("=");
            if (tmp[0] === parameterName) result = decodeURIComponent(tmp[1]);
        });
    return result;
}

$(function () {
    //loaded handler
    ReSize();

    $.get("/dynamic/GetContents.html?file=" + ReadGetParameter("file"), function (response) {
        let decoded = window.atob(ReadGetParameter("file"));
        let contents = response;
        editor.setValue(contents);
        $("#raw").val(contents);
        loaded = true;
        $("#filename").html(decoded);
        document.title = decoded;
    });

});

$(window).resize(function () {
    ReSize();
});

$("#Save").click(function () {
    if (!loaded) {
        alert("File can't be saved.");
        return;
    }

    $.post("/dynamic/Save.html",
        {
            file: ReadGetParameter("file"),
            content: window.btoa(editor.value())
        }).done(function (data) {

            if (data === "OK") {
                alert("file.saved");
            }
            else {
                alert("file save error");
            }
        });
});