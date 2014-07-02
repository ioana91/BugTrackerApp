$(document).ready(function () {
    $('#button-radio').each(function () {

        // Settings
        var $widget = $(this),
            $buttons = $widget.find('button'),
            settings = {
                on: {
                    icon: 'icon-radio-checked'
                },
                off: {
                    icon: 'icon-radio-unchecked'
                }
            };

        // Event Handlers
        $buttons.on('click', function (e) {
            var sel = $(this).data('title');
            var tog = $(this).data('toggle');
            $('#' + tog).prop('value', sel);

            $('button[data-toggle="' + tog + '"]').not('[data-title="' + sel + '"]').find('.state-icon').removeClass();
            $('button[data-toggle="' + tog + '"]').not('[data-title="' + sel + '"]').find('i').addClass('state-icon ' + settings['off'].icon);
            $('button[data-toggle="' + tog + '"][data-title="' + sel + '"]').find('.state-icon').removeClass();
            $('button[data-toggle="' + tog + '"][data-title="' + sel + '"]').find('i').addClass('state-icon ' + settings['on'].icon);
        });

        // Initialization
        function init() {
            // Inject the icon if applicable
            if ($buttons.find('.state-icon').length === 0) {
                $buttons.prepend('<i class="state-icon ' + settings['off'].icon + '"></i> ');
            }

            //set the default option
            $('button[data-title="Medium"]').find('.state-icon').removeClass();
            $('button[data-title="Medium"]').find('i').addClass('state-icon ' + settings['on'].icon);
            $('#Priority').prop('value', 'Medium');
        }
        init();
    });
});