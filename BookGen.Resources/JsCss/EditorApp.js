var lock = false;

var editor = ace.edit("editorcontent");
editor.setTheme("ace/theme/github");
editor.session.setMode("ace/mode/markdown");
editor.session.setUseWrapMode(true);
editor.session.setTabSize(4);
editor.setShowPrintMargin(false);
editor.setOption("wrap", true);

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
        let decoded = window.atob(ReadGetParameter("file"));
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
            content: window.btoa(editor.getValue())
        }).done(function (data) {

            if (data === "OK") {
                alert("file.saved");
            }
            else {
                alert("file save error");
            }
        });
});