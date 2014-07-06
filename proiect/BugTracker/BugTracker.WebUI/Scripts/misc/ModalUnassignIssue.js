$(document).on("click", ".open-unassignIssueDialog", function () {
    var issueId = $(this).data('id');
    var title = $(this).data('title');

    $('.modal-body #questionUnassign').html('Do you want to unassign issue <strong>' + title + '</strong>?');

    var link = "/Issue/Unassign/" + issueId;
    $(".modal-footer #issueId").attr("href", link);
});