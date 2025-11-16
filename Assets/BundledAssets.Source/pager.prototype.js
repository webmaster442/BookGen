const urls = [
    "https://localhost/index.html",
    "https://localhost/Page1.html",
    "https://localhost/Page2.html"
];

const currentUrl = window.location.origin + window.location.pathname;
const index = urls.indexOf(currentUrl);

document.writeln('<div class="pager">');

if (index !== -1 && index + 1 < urls.length) {
    document.writeln(`<a href="${urls[index + 1]}" class="next">Next</a>`);
}

if (index > 0) {
    document.writeln(`<a href="${urls[index - 1]}" class="prev">Previous</a>`);
}

document.writeln('</div');