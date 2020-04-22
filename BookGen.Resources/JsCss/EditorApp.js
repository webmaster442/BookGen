var lock = false;

var editor = ace.edit("editorcontent");
editor.setTheme("ace/theme/github");
editor.session.setMode("ace/mode/markdown");
editor.session.setUseWrapMode(true);
editor.session.setTabSize(4);
editor.setShowPrintMargin(false);
editor.setOption("wrap", true);

/*-----------------------------------------------------------------------------
Util functions
-----------------------------------------------------------------------------*/

function b64EncodeUnicode(str) {
    // first we use encodeURIComponent to get percent-encoded UTF-8,
    // then we convert the percent encodings into raw bytes which
    // can be fed into btoa.
    return btoa(encodeURIComponent(str).replace(/%([0-9A-F]{2})/g,
        function toSolidBytes(match, p1) {
            return String.fromCharCode('0x' + p1);
    }));
}

function b64DecodeUnicode(str) {
    // Going backwards: from bytestream, to percent-encoding, to original string.
    return decodeURIComponent(atob(str).split('').map(function(c) {
        return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
    }).join(''));
}

/*-----------------------------------------------------------------------------
Event handlers
-----------------------------------------------------------------------------*/

editor.session.on('change', function (delta) {
    //editor text changed, update the raw view
    if (!lock) {
        var text = editor.getValue();
        $("#raw").val(text);
    }
});

function ReSize() {
    let h = window.innerHeight;
    let maxheight = (h - 150) - $("#navbar").height();
    $(".editortab").css("height", maxheight + "px");
    editor.setOption("maxLines", maxheight / 21);
    $("#raw").css("height", maxheight + "px");
}

function UpdateEditor() {
    lock = true;
    let contents = $("#raw").val();
    editor.setValue(contents);
    lock = false;
}

$("#raw").keyup(function () {
    UpdateEditor();
});

$("#raw").change(function () {
    UpdateEditor();
});

$(window).resize(function () {
    ReSize();
});

/*-----------------------------------------------------------------------------
Toolbar handler functions
-----------------------------------------------------------------------------*/

function AddEditorText(startStr, endStr) {
    let selectedText = editor.getSelectedText();
    if (selectedText == undefined || selectedText.length < 1) {
        editor.insert(startStr + " " + endStr);
    }
    else {
        let final = startStr + selectedText + endStr;
        let selectionRange = editor.getSelectionRange();
        editor.session.replace(selectionRange, final);
    }
}


function InitToolbar() {
    $("#ToolbarUndo").click(function () {
        editor.session.getUndoManager().undo(false);
    });
    $("#ToolbarRedo").click(function () {
        editor.session.getUndoManager().redo(false);
    });
    $("#ToolbarBold").click(function () {
        AddEditorText("**", "**");
    });
    $("#ToolbarItalic").click(function () {
        AddEditorText("*", "*");
    });
    $("#ToolbarQuote").click(function () {
        AddEditorText(">", "");
    });
    $("#ToolbarCode").click(function () {
        AddEditorText("```\r\n", "\r\n```");
    });
    $("#ToolbarHighlight").click(function () {
        AddEditorText("`", "`");
    });
}


/*-----------------------------------------------------------------------------
Document load and save
-----------------------------------------------------------------------------*/

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
    InitToolbar();
    $.get("/dynamic/GetContents.html?file=" + ReadGetParameter("file"), function (response) {
        let decoded = b64DecodeUnicode(ReadGetParameter("file"));
        let contents = response;
        editor.setValue(contents);
        $("#raw").val(contents);
        loaded = true;
        $("#filename").html(decoded);
        document.title = decoded;
    });

});

$("#Save").click(function () {
    if (!loaded) {
        alert("File can't be saved.");
        return;
    }

    $.post("/dynamic/Save.html",
        {
            file: ReadGetParameter("file"),
            content: b64EncodeUnicode(editor.getValue())
        }).done(function (data) {

            if (data === "OK") {
                alert("file.saved");
            }
            else {
                alert("file save error");
            }
        });
});

function LoadPreview() {
    $.post("/dynamic/Preview.html",
    {
        content: b64EncodeUnicode(editor.getValue())
    }).done(function (data) {
        $("#PrevContent").html(data);
    });
}

$("#contact-tab").click(function() {
    $("#PrevContent").html("<p>Loading ...</p><script>LoadPreview();</script>");
});