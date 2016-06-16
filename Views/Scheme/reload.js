var t;

function autoreload() {
    loadImg();
    t = setTimeout("autoreload", 20000); // perioda v ms
}

function loadImg() {
    _im = $("<img>");

    //_im.bind("load", function () { $(this).show(); $('div#schema').html(_im); } );
    _im.attr('src', "/scheme/getImage");
    _im.attr('alt', "schema");
}
