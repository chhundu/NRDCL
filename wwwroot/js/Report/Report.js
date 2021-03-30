$(document).ready(function () {
    $('#CustomerReportTable').dataTable({
    });
    $('#ProductReportTable').dataTable({
    });
});

$("#pdfBtn").click(function () {
    var sHtml = $("#pdfContainer").html();
    sHtml = sHtml.replace(/</g, "StrTag").replace(/>/g, "EndTag");
    window.open('../Report/GeneratePdf?html=' + sHtml, '_blank');
});
