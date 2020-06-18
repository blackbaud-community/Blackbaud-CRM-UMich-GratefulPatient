/// <reference path="../../../../../../../../Blackbaud.AppFx.Server/Deploy/browser/jQuery/jquery-vsdoc.js" />
/// <reference path="../../../../../../../../Blackbaud.AppFx.Server/Deploy/browser/ExtJS/adapter/jquery/ext-jquery-adapter.js" />
/// <reference path="../../../../../../../../Blackbaud.AppFx.Server/Deploy/browser/ExtJS/ext-all-debug.js" />
/// <reference path="../../../../../../../../Blackbaud.AppFx.Server/Deploy/browser/BBUI/bbui/bbui.js" />
/// <reference path="../../../../../../../../Blackbaud.AppFx.Server/Deploy/browser/BBUI/uimodeling/servicecontracts.js" />
/// <reference path="../../../../../../../../Blackbaud.AppFx.Server/Deploy/browser/BBUI/uimodeling/service.js" />
/// <reference path="../../../../../../../../Blackbaud.AppFx.Server/Deploy/browser/BBUI/forms/utility.js" />
/// <reference path="../../../../../../../../Blackbaud.AppFx.Server/Deploy/browser/BBUI/forms/formcontainer.js" />

/*jslint bitwise: true, browser: true, eqeqeq: true, undef: true, white: true */
/*extern BBUI, Ext, $ */
/*JSLint documentation: http://www.jslint.com/lint.html */

(function (container, modelInstanceId) {
    container.on("formupdated", function () {
        // This form has a large portion whose visibility can significantly affect the size of the form.
        // Fire the form container's resize event so the search container can resize the results grid accordingly.
        container.onResize();
    });
})();