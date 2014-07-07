$(document).ready(function () {
    var nowTemp = new Date();
    var now = new Date(nowTemp.getFullYear(), nowTemp.getMonth(), nowTemp.getDate(), 0, 0, 0, 0);

    $('#dp1').datepicker({
        format: "dd/mm/yyyy", onRender: function (date) {
            return date.valueOf() < now.valueOf() ? 'disabled' : '';
        }
    }).on('changeDate', function (ev) {
        $('#Date').prop('value', ev.date.getDate());
        $('#Month').prop('value', ev.date.getMonth());
        $('#Year').prop('value', ev.date.getUTCFullYear());
    });

    $('#dp1').datepicker('setValue', new Date(year, month, date));
}); 