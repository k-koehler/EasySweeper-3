var API = {

    test: function (string_username, func_callback) {
        $.ajax({
            type: "POST",
            url: "Service.aspx/Get",
            data: JSON.stringify({ username: string_username }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data, textStatus, jqXHR) {
                func_callback(data.d);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert(textStatus + '\n' + errorThrown);
            }
        });
    }

}