// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.

function serializeFormToObj(form) {
    var obj = {};
    $.each(form.serializeArray(), function (_, kv) {
        if (obj.hasOwnProperty(kv.name)) {
            obj[kv.name] = $.makeArray(obj[kv.name]);
            obj[kv.name].push(kv.value);
        }
        else {
            obj[kv.name] = kv.value;
        }
    });
    return obj;
}
