# BookGen - A C# documentation generator

Bookgen is inspired by Gitbook. Mainly I wrote it, because Node.js development is not my world.

Features:
* Ligtweight code base
* Uses MarkDig that support a lot of markdown features
* Easily extensible templates
* Built in debug web server
* Single bookgen.json configuration (if run without args, creates default config)
* Customizeable

## Template tags:

**[[Content]]**

Will be replaced with rendered HTML

**[[Title]]**

Page title, first heading content will be rendered here from the document.

**[[TableOfContents]]**

Table of contents file contents will be rendered here.

**[[HostUrl]]**

The server Host address. Can be used to include pages & stuff

**[[AssetsUrl]]**

Assets directory for templates. Can be used to include pages & stuff
