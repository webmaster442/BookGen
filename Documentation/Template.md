# Template tags

BookGen uses two kinds of template tags, which are called shortcode tags and translatable tags.

## Translatable tags

Translatable tags are strings wrapped in ```{{``` and ```}}``` Translatable tags can only contain letters from the english alphabet, the ```_``` char and numbers in any combination.

The translatable tags are replaced with strings specified in the Translations section of the bookgen.json configuration.

## Shortcodes

Shortcode tags are strings wrapped with the ```[``` and ```]``` symbols. Shortcode names are case sensitive. Currently the following shortcodes are supported.

* ** ```<<toc>>```**
    
    Will be replaced with the contents of the Table of Contents file

* **```<<title>>```**
    
    Will be replaced with the currently processed page title (1st H1 element of the page)
    
* **```<<content>>```**

    Will be replaced with the currently processed page content

* **```<<host>>```**

    Will be replaced with the host name specified in the configuration file

* **```<<metadata>>```**

    Will be replaced with the generated HTML metadata for the currently processed page

* **```<<BuildTime>>```**

    Will be replaced with the current time and date.

* **```<<CookieWarnIfEnabledInTarget>>```** 

    If the config file has the option enabled, that you will need cookie warnings then this shortcode will be replaced with a cookie consent warning.

## Shortcodes with arguments

* **```<<InlineFile file="file.html">>```**

    Simply include or inline the specified file

* **```<<SriDependency file="file.css">>```**

Creates a script tag or a css include with Subresource Integrity. This is a security feature that enables browsers to verify that resources they fetch (for example, from a CDN) are delivered without unexpected manipulation. It works by allowing you to provide a cryptographic hash that a fetched resource must match. For more info see: https://developer.mozilla.org/en-US/docs/Web/Security/Subresource_Integrity