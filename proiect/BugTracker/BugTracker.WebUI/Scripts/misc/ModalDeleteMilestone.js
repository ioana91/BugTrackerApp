$(document).on("click", ".open-deleteMilestoneDialog", function () {
    var id = $(this).data('id');
    var name = $(this).data('name');

    $('.modal-body #questionDelete').html('Are you sure you want to delete milestone <strong>' + name + '</strong>?');

    var link = "/Milestone/Delete/" + id;
    $(".modal-footer #userId").attr("href", link);
});