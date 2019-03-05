$("#searchbutton").click(function () {

    var html = "";
    var matches = 0;
    $("#searchresults").html("");

    $("#searchcontents div").each(function () {
        var terms = $("#searchtext").val();
        var startindex = $(this).text().indexOf(terms);
        if (startindex < 0) return;

        var title = $(this).attr("title");
        var url = $(this).attr("data-link");
        var len = $(this).text().length - startindex;
        if (len > 180) {
            len = 180;
        }

        matches += 1;

        var link = "<h3><a href=\"" + url + "\">" + title + "</a></h3>";
        var content = "<p>" + $(this).text().substr(startindex, len) + "...</p>";

        html += link;
        html += content;
    });

    var final = "<p>Results: " + matches + "</p>" + html;

    if (html.length > 0) {
        $("#searchresults").html(final);
    }

});