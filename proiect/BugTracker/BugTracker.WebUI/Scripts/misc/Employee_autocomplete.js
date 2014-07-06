$(document).ready(function () {
    var employees;

    $.ajax({
        type: "GET",
        async: false,
        dataType: "json",
        url: "/Project/GetEmployees",
        contentType: "application/json; charset=utf-8",
        success: function (x) {
            employees = x;
            $('.query').autocomplete({
                lookup: employees
            });
        }
    });

    $(function () {
        $(document).on('click', '.btn-add', function (e) {
            e.preventDefault();

            var controlForm = $(e.target).closest('.controls'),
                currentEntry = $(this).parents('.entry:first'),
                newEntry = $(currentEntry.clone()).appendTo(controlForm);

            newEntry.find('input').val('');
            controlForm.find('.entry:not(:last) .btn-add')
                .removeClass('btn-add').addClass('btn-remove')
                .removeClass('btn-success').addClass('btn-danger')
                .html('<span class="glyphicon glyphicon-minus"></span>');

            $('.query').autocomplete({
                lookup: employees
            });
        }).on('click', '.btn-remove', function (e) {
            $(this).parents('.entry:first').remove();

            e.preventDefault();
            return false;
        });
    });

    $('#saveBtn').click(function () {
        var projects = $('.controls');

        var i;
        var projectsArray = [];
        for (i = 0; i < projects.length; i++) {
            var ProjectName = $('.controls')[i].id;

            var Employees = $("[id='" + ProjectName + "'] input[name='employees[]']").map(function (idx, elem) {
                return $(elem).val();
            }).get();

            var project = { ProjectName: ProjectName, Employees: Employees };
            projectsArray.push(project);
        }
                
        $.ajax({
            type: "POST",
            async: true,
            dataType: "json",
            data: JSON.stringify(projectsArray),
            url: "/Project/Index",
            contentType: "application/json; charset=utf-8"
        });
    });
    
});