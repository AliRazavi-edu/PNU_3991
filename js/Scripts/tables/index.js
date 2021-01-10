$(document).ready(function () {
    var oTable = $('#patientTable').DataTable({
        "ajax": {
            "url": '/patients/GetPatients',
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
                    return '<a class="popup" href="/patients/edit/' + data + '">Düzenle</a>';
                }
            },
            {
                "data": "ID", "width": "50px", "render": function (data) {
                    return '<a class="popup" href="/patients/delete/' + data + '">Sil</a>';
                }
            },
            {
                "data": "ID", "width": "50px", "render": function (data) {
                    return '<a class="" href="/details/details/' + data + '">Detaylar</a>';
                }
            }
        ]
    });

    $('#patientTable tbody').on('click', 'tr td', function () {
        var row_data = oTable.row(this).data()
        var col = $(this).index();
        if (col < 6) {
            window.location = ("details/details/" + row_data["ID"]);
        }
    });

    $('.tablecontainer_index').on('click', 'a.popup', function (e) {
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