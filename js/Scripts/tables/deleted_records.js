$(document).ready(function () {
    var oTable = $('#mytDeletedtable').DataTable({
        "ajax": {
            "url": '/patients/GetDeletedPatients',
            "type": "get",
            "datatype": "json"
        },
        "columns": [
            { "data": "FirstName", "autoWidth": true },
            { "data": "LastName", "autoWidth": true },
            { "data": "BirthDate", "autoWidth": true },
            { "data": "Age", "autoWidth": true },
            { "data": "RegisterDate", "autoWidth": true },
            { "data": "Summary", "autoWidth": true },
            {
                "data": "ID", "width": "50px", "render": function (data) {
                    return '<a class="popup" href="/patients/getback/' + data + '">Geri Al</a>';
                }
            }
        ]
    })

    $('.tablecontainer').on('click', 'a.popup', function (e) {
        e.preventDefault();
        OpenPopup($(this).attr('href'));
    })
    function OpenPopup(pageUrl) {
        var $pageContent = $('<div/>');
        $pageContent.load(pageUrl, function () {
            $('#popupForm', $pageContent).removeData('validator');
            $('#popupForm', $pageContent).removeData('unobtrusiveValidation');
            $.validator.unobtrusive.parse('form');

        });

        $dialog = $('<div class="popupWindow" style="overflow:auto"></div>')
            .html($pageContent)
            .dialog({
                draggable: false,
                autoOpen: false,
                resizable: false,
                model: true,
                title: '',
                height: 550,
                width: 600,
                close: function () {
                    $dialog.dialog('destroy').remove();
                }
            })

        $('.popupWindow').on('submit', '#popupForm', function (e) {
            var url = $('#popupForm')[0].action;
            $.ajax({
                type: "POST",
                url: url,
                data: $('#popupForm').serialize(),
                success: function (data) {
                    if (data.status) {
                        $dialog.dialog('close');
                        oTable.ajax.reload();
                    }
                }
            })

            e.preventDefault();
        })
        $dialog.dialog('open');
    }
})