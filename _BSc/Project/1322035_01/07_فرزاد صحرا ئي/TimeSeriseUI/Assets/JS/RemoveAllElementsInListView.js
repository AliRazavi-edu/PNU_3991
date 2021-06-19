$(function () {

    $('.tj-lv-checkbox-header > input[type=checkbox]').change(function () {

        var _this = $(this);
        if (_this.is(':checked')) {

            if (_this.attr('ID') == 'shakhesselect') {
                $('input[type=checkbox]', $('[columnid=shakhesselect]')).prop('checked', 'checked');
            }

        }
        else {

            if (_this.attr('ID') == 'shakhesselect') {
                $('input[type=checkbox]', $('[columnid=shakhesselect]')).prop('checked', false);
            }

        }

    });
});