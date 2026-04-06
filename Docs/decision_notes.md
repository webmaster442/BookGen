# 2026.04

- KaTex requires DOM to work, so It can't be used in the current markdown rendering pipeline.
- Mermaid requires DOM to work, so It can't be used in the current markdown rendering pipeline.
- MathJax supports native SVG rendering, so it can be used in the current markdown rendering pipeline.
- Word doesn't support WEBP correctly, so when exporting a Printable xhtml, don't use WEBP format.
- Word and LibreOffice don't support CSS style element, so in Printable xhtml, css needs to be inlined.