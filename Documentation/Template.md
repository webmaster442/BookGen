# Template tags

BookGen uses so called shortcodes for template tags. These shortcodes can be used in HTML templates and markdown content also, but using most of them only makes sense in the HTML template.

Shortcodes look like this: `<!--{code}-->`. As you can see it's an HTML comment and the actual code is wrapped with the ```{``` and ```}``` symbols. Shortcode names are case sensitive, so pay attention to writing them. If a code can't be parsed then it doesn't display anything, due to the HTML comment nature.

Currently the following shortcodes are supported:

* `<!--{toc}-->`
    
    Will be replaced with the contents of the Table of Contents file

* `<!--title}-->`
    
    Will be replaced with the currently processed page title (1st H1 element of the page)
    
* `<!--{content}-->`

    Will be replaced with the currently processed page content

* `<!--{host}-->`

    Will be replaced with the host name specified in the configuration file

* `<!--{metadata}-->`

    Will be replaced with the generated HTML metadata for the currently processed page

* `<!--{BuildTime}-->`

    Will be replaced with the current time and date.

* `<!--{CookieWarnIfEnabledInTarget}-->` 

    If the config file has the option enabled, that you will need cookie warnings then this shortcode will be replaced with a cookie consent warning.

## Shortcodes with arguments

* `<!--{InlineFile file="file.html"}-->`

    Simply include or inline the specified file

* `<!--{SriDependency file="file.css"}-->`

    Creates a script tag or a css include with Subresource Integrity. This is a security feature that enables browsers to verify that resources they fetch (for example, from a CDN) are delivered without unexpected manipulation. It works by allowing you to provide a cryptographic hash that a fetched resource must match. For more info see: https://developer.mozilla.org/en-US/docs/Web/Security/Subresource_Integrity
    
* `<!--{? ID}-->`

    In this case the ID represents a translation ID. Translation ID's are defined in the Translations section of the configuration file. A Translatable ID can only contain the following chars: Letters (A-Z and a-z), Numbers (0-9) and the _ (underscore) char. If an ID is found valid and the Translations section of the configuration file contains the ID then it's replaced with the associated string-