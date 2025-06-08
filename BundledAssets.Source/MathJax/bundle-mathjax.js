// bundle-mathjax.js
require('esbuild').build({
  entryPoints: ['./mathjax-entry.js'],
  bundle: true,
  platform: 'browser',
  format: 'iife',
  globalName: 'MathJaxModule',
  outfile: 'dist/mathjax-bundled.js',
}).catch(() => process.exit(1));