$(document).on("click", ".open-deleteIssueDialog", function () {
    var issueId = $(this).data('id');
    var title = $(this).data('title');

    $('.modal-body #questionDelete').html('Are you sure you want to delete issue <strong>' + title + '</strong>?');

    var link = "/Issue/Delete/" + issueId;
    $(".modal-footer #issueId").attr("href", link);
});