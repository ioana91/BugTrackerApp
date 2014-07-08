$(document).ready(function () {
    var statusChart = c3.generate({
        bindto: '#status',
        data: {
            columns: [
              [openName, openIssues],
              [inProgressName, inProgressIssues],
              [closedName, closedIssues],
              [unsolvableName, unsolvableIssues],
              [inTestingName, inTestingIssues],
            ],
            type: 'pie'
        },
    });

    var priorityChart = c3.generate({
        bindto: '#priority',
        data: {
            columns: [
              [medium, mediumIssues],
              [high, highIssues],
              [low, lowIssues],
              [critical, criticalIssues]
            ],
            type: 'pie'
        },
    });

    var typeChart = c3.generate({
        bindto: '#type',
        data: {
            columns: [
              [bug, bugIssues],
              [feature, featureIssues]
            ],
            type: 'pie',
            colors: {
                Bug: '#d62728',
                Feature: '#2CA02C'
            }
        },
    });

    $.ajax({
        type: "GET",
        async: false,
        data: { projectId: projectId },
        dataType: "json",
        url: "/Project/GetResponsibleDetails",
        contentType: "application/json; charset=utf-8",
        success: function (x) {
            var responsibleChart = c3.generate({
                bindto: '#responsible',
                data: {
                    columns: x,
                    type: 'pie'
                },
            });
        }
    });
});