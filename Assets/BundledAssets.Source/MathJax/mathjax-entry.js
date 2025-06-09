// mathjax-entry.js
import { mathjax } from 'mathjax-full/js/mathjax.js';
import { TeX } from 'mathjax-full/js/input/tex.js';
import { SVG } from 'mathjax-full/js/output/svg.js';
import { liteAdaptor } from 'mathjax-full/js/adaptors/liteAdaptor.js';
import { RegisterHTMLHandler } from 'mathjax-full/js/handlers/html.js';

const adaptor = liteAdaptor();
RegisterHTMLHandler(adaptor);

const tex = new TeX({ packages: ['base'] });
const svg = new SVG({ fontCache: 'none' });

const html = mathjax.document('', {
  InputJax: tex,
  OutputJax: svg
});

export function typeset(latex, scale = 1.0) {
  const size = 16 * scale;
  const node = html.convert(latex, { 
    display: true,
    em: size,
   });
  return adaptor.outerHTML(node);
}
