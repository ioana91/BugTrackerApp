$(document).on("click", ".open-assignIssueDialog", function () {
    var issueId = $(this).data('id');
    var title = $(this).data('title');

    $('.modal-body #questionAssign').html('Do you want to assign issue <strong>' + title + '</strong>?');

    var link = "/Issue/Assign/" + issueId;
    $(".modal-footer #issueId").attr("href", link);
});