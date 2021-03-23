$(document).ready(function () {
    var siteId = $('#SiteId').val();
    if (siteId == "") {
        $('#OrderID').prop("disabled", true);
    } else {
        $('#OrderID').prop("disabled", false);
    }

    $('#OrderTable').dataTable({
    });
});

$(".edit").on("click", function () {
    var orderID = $(this).prop("id");
    $("#OrderID").val(orderID);
    $('#OrderID').prop("disabled", false);
});

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
    var url = "/Order/CalculateOrderAmount/";
    if (validateFeilds()) {
        $.ajax({
            url: url,
            data: $(".oderForm").serialize(),
            cache: false,
            type: "GET",
            success: function (data) {
                //$("input[id='OrderAmount']").val(data);
                $("input[id='OrderAmount']").val(parseFloat(data).toFixed(2));
            },
            error: function (reponse) {
                alert("error : " + reponse);
            }
        });
    }
       
});

function validateFeilds() {
    var customerId = $('#CustomerID').val();
    var siteId = $('#SiteID option:selected').val();
    var productId = $('#productId :selected').val();

    if (customerId == "" || customerId == null) {
        swal({
            title: "Please enter Customer ID.",
            text: "Click OK to exit",
            type: "warning"
        }, function (e) {
                $('#CustomerID').focus();
        });
        return false;
    }
    if (siteId == 0) {
        swal({
            title: "Please select site.",
            text: "Click OK to exit",
            type: "warning"
        }, function (e) {
                $('#SiteID').focus();
        });
        return false;
    }
    if (productId == 0) {
        swal({
            title: "Please select product.",
            text: "Click OK to exit",
            type: "warning"
        }, function (e) {
                $('#productId').focus();
        });
        return false;
    }
    return true;
}
