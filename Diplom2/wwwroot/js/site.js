function themesite(a) {
    name = a.innerText;
    if (name == "Light theme") {
        ajaxinput(false);
        link = $('link[href="/css/darktheme.css"]');
        link.remove();
        a.innerText = 'Dark theme';
    }
    else if (name == "Dark theme") {
        ajaxinput(true);
        $link = $('<link/>', {
            rel: 'stylesheet',
            href: '/css/darktheme.css'
        }).appendTo('head');
        a.innerText = 'Light theme';
    }
    return false;

    function ajaxinput(a) {
        $.ajax({
            type: "POST",
            url: "Home/ThemeSite/",
            data: { themebool: a }
        })
    }
}


