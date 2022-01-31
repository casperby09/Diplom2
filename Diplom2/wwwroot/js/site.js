
function themesite(a) {
    name = a.innerText;
    theme = a.dataset.themedark;
    nametheme = a.dataset.nametheme;
    if (theme == "off") {
        ajaxinput(false);
        link = $('link[href="/css/darktheme.css"]');
        link.remove();
        a.dataset.themedark = "on";
    }
    else if (theme == "on") {
        ajaxinput(true);
        $link = $('<link/>', {
            rel: 'stylesheet',
            href: '/css/darktheme.css'
        }).appendTo('head');
        a.dataset.themedark = "off"
    }
    a.innerText = nametheme;
    a.dataset.nametheme = name;
    return false;

    function ajaxinput(a) {
        $.ajax({
            type: "POST",
            url: "Home/ThemeSite/",
            data: { themebool: a }
        })
    }
}


