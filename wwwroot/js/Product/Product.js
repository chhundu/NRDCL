$(document).ready(function () {
    var productId = $('#ProductId').val();
    if (productId == "") {
        $('#ProductId').prop("disabled", true);
    } else {
        $('#ProductId').prop("disabled", false);
    }
    $('#ProductTable').dataTable({
    });
});