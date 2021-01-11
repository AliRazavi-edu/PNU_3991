$(document).ready(function () {
    var oTable = $('#myInfotable').DataTable({
        "paging": false,
        "ordering": false,
        "info": false,
        "searching": false,
        "ajax": {
            "url": '/details/GetPatientDetails/',
            "data": { id: $(".patientDetail").val() },
            "type": "get",
            "datatype": "json"
        },
        "columns": [
            { "data": "FirstName", "autoWidth": true },
            { "data": "LastName", "autoWidth": true },
            { "data": "BirthDate", "autoWidth": true },
            { "data": "Age", "autoWidth": true },
            { "data": "Summary", "autoWidth": true },
            {
                "data": "ID", "width": "50px", "render": function (data) {
                    return '<a class="popup" href="/patients/edit/' + data + '">Düzenle</a>';
                }
            }
        ]
    });


    $.get("/details/GetInspections/" + $(".patientDetail").val() + "", function (data) {
        $.each(data.data, function (index, value) {
            var photoTable = $('#patientPhototable_' + value.ID).DataTable({
                "paging": false,
                "ordering": false,
                "info": false,
                "searching": false,
                "ajax": {
                    "url": '/details/GetPatientPhoto/',
                    "data": { id: value.ID },
                    "type": "get",
                    "datatype": "json"
                },
                "columns": [
                    {
                        "data": "Photo", "autoWidth": true, "render": function (data) {
                            return '<a target="_blank" href="/images/' + data + '"><img src="/images/' + data + '" max-width="315px" height="185px"/></a>';
                        }
                    },
                    { "data": "RegisterDate", "autoWidth": true },
                    {
                        "data": "ID", "width": "50px", "render": function (data) {
                            return '<a class="popup" href="/Photos/delete/' + data + '">Sil</a>';
                        }
                    }
                ]
            });

        });
    });
});

$('.tablecontainer').on('click', 'a.popup', function (e) {
    e.preventDefault();
    OpenPopup($(this).attr('href'));
});

function OpenPopup(pageUrl) {
    var $pageContent = $('<div />');
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
        });

    $('.popupWindow').on('submit', '#popupForm_Inspection', function (e) {
        var url = $('#popupForm_Inspection')[0].action;
        $.ajax({
            type: "POST",
            url: url,
            data: $('#popupForm_Inspection').serialize(),
            success: function (data) {
                $dialog.dialog('close');
                location.reload();
            }
        });
        e.preventDefault();
    });

    $('.popupWindow').on('submit', '#popupForm_Photo', function (e) {
        var url = $('#popupForm_Photo')[0].action;
        $.ajax({
            type: "POST",
            url: url,
            data: $('#popupForm_Photo').serialize(),
            success: function (data) {
                if (data.status) {
                    $dialog.dialog('close');
                    $.get("/details/GetInspections/" + $(".patientDetail").val() + "", function (data) {
                        $.each(data.data, function (index, value) {
                            $('#patientPhototable_' + value.ID).DataTable().ajax.reload();
                        });
                    });
                }
            }
        });
        e.preventDefault();
    });
    $dialog.dialog('open');
}