$(document).on("click", ".open-deleteUserDialog", function () {
    var userId = $(this).data('id');
    var link = "/User/Delete/" + userId;
    $(".modal-footer #userId").attr("href", link);
});