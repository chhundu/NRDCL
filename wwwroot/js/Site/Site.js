$(document).ready(function () {
    var siteId = $('#SiteId').val();
    if (siteId == "") {
        $('#SiteId').prop("disabled", true);
    } else {
        $('#SiteId').prop("disabled", false);
    }
    $('#SiteTable').dataTable({
    });
});

$(".edit").on("click", function () {
    var siteId = $(this).prop("id");
    $("#SiteId").val(siteId);
    $('#SiteId').prop("disabled", false);
});

