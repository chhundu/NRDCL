$("#CustomerID").change(function () {
    var customerID = $("input[id='CustomerID']").val();
    var siteSelect = $('#SiteID');
    siteSelect.empty();
    var url = "/Order/GetSiteByCustomerId/";
    $.ajax({
        url: url,
        data: { customerID: customerID },
        cache: false,
        type: "GET",
        success: function (data) {
            if (data != null && !jQuery.isEmptyObject(data)) {
                siteSelect.append($('<option/>', {
                    value: 0,
                    text: "---Select---"
                }));
                for (var x = 0; x < data.length; x++) {
                    siteSelect.append($('<option/>', {
                        value: data[x].value,
                        text: data[x].text
                    }));
                }
            }
        },
        error: function (reponse) {
            alert("error : " + reponse);
        }
    });
});

$("#Calculate").click(function () {
   /* alert("M");
    var siteId = $('#SiteID option:selected').text();
    var productId = $('#productId :selected').val();
    alert(siteId)*/

    var url = "/Order/CalculateOrderAmount/";
    $.ajax({
        url: url,
        data: $(".oderForm").serialize(),
        cache: false,
        type: "GET",
        success: function (data) {
            $("input[id='OrderAmount']").val(data);
        },
        error: function (reponse) {
            alert("error : " + reponse);
        }
    });
});

/*function validateFeilds() {
    var siteId = $('#SiteID :selected').val();
    var productId = $('#productId :selected').val();
    alert(siteId)
}*/