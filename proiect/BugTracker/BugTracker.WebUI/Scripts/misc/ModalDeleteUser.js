$(document).on("click", ".open-deleteUserDialog", function () {
    var userId = $(this).data('id');
    var userName = $(this).data('name');

    $('.modal-body #question').html('Are you sure you want to delete user <strong>' + userName + '</strong>?');

    var link = "/User/Delete/" + userId;
    $(".modal-footer #userId").attr("href", link);
});