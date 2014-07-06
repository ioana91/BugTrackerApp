var currentSelectedProject;
var currentTags;
var tagInput;

$(document).ready(function () {
    $('#ProjectSelectId').on('change', GetMilestones);
    $('#ProjectSelectId').on('change', GetTags);

    tagInput = $('#magicSuggest').magicSuggest({
        allowFreeEntries: true,
        cls:"form-control",
        data: GetTags,
        valueField: "name"
    });
});

function GetMilestones() {
    var projectId = $('#ProjectSelectId').val();

    if (projectId !== "") {
        $.ajax({
            type: "GET",
            async: false,
            data: { projectId: projectId },
            dataType: "json",
            url: "/Issue/GetMilestones",
            contentType: "application/json; charset=utf-8",
            success: function (milestones) {
                $('#MilestoneSelectId').empty();
                $('#MilestoneSelectId').append('<option>Select milestone</option>');

                var i;
                for (i = 0; i < milestones.length; i++) {
                    $('#MilestoneSelectId').append('<option value="' + milestones[i].MilestoneId + '">' + milestones[i].Name + '</option>');
                }
            }
        });
    }
}

function GetTags() {
    var projectId = $('#ProjectSelectId').val();

    if (projectId !== "" && projectId !== currentSelectedProject) {
        $.ajax({
            type: "GET",
            async: false,
            data: { projectId: projectId },
            dataType: "json",
            url: "/Issue/GetTags",
            contentType: "application/json; charset=utf-8",
            success: function (tags) {
                currentTags = tags;
                tagInput.clear();
            }
        });

        currentSelectedProject = projectId;
    }

    if (typeof currentTags == "undefined")
        currentTags = [];

    return currentTags;
}