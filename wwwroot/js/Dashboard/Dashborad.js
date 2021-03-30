
function GetProductChart() {
    var url = "/Report/GetProductOrderChart/";
    var totalOrder = 0;
    $.ajax({
        url: url,
        cache: false,
        type: "GET",
        success: function (data) {
            if (data != null && !jQuery.isEmptyObject(data)) {
                totalOrder = data;
            }
        }, error: function (reponse) {
            alert("error : " + reponse);
        }
    });
}