$(function () {
    $('#txt_disease').keydown(function (e) {
        if (e.keyCode === 9) {
            var Disease = $("#txt_disease").val();
            var PatientId = $(".patientDetail").val();

            $.ajax({
                url: '/Diseases/Edit/',
                type: 'POST',
                data: JSON.stringify({ "Disease": Disease, "PatientID": PatientId, "Active": true }),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (result) {
                    $('#patientDiseasetable').DataTable().ajax.reload();
                }
            });
        }
    });

    $('#txt_surgery').keydown(function (e) {
        if (e.keyCode === 9) {
            var Surgery = $("#txt_surgery").val();
            var PatientId = $(".patientDetail").val();

            $.ajax({
                url: '/Surgeries/Edit/',
                type: 'POST',
                data: JSON.stringify({ "Surgery": Surgery, "PatientID": PatientId, "Active": true }),
                dataType: "json",
                contentType: "application/json; charset=utf-8"
            });
        }
    });

    $('#txt_information').keydown(function (e) {
        if (e.keyCode === 9) {
            var Information = $("#txt_information").val();
            var PatientId = $(".patientDetail").val();
            $.ajax({
                url: '/Informations/Edit/',
                type: 'POST',
                data: JSON.stringify({ "Information": Information, "PatientID": PatientId, "Active": true }),
                dataType: "json",
                contentType: "application/json; charset=utf-8"
            });
        }
    });

    $('#txt_medicine').keydown(function (e) {
        if (e.keyCode === 9) {
            var Medicine = $("#txt_medicine").val();
            var PatientId = $(".patientDetail").val();

            $.ajax({
                url: '/Medicines/Edit/',
                type: 'POST',
                data: JSON.stringify({ "Medicine": Medicine, "PatientID": PatientId, "Active": true }),
                dataType: "json",
                contentType: "application/json; charset=utf-8"
            });
        }
    });

    $.get("/details/GetInspections/" + $(".patientDetail").val() + "", function (data) {
        $.each(data.data, function (index, value) {
            $('#tab_muayene_complains_' + value.ID).keydown(function (e) {
                if (e.keyCode === 13) {
                    console.log("enter");
                    var Complain = $("#txt_complain_" + value.ID).val();
                    var InspectionID = (value.ID);

                    $.ajax({
                        url: '/Complains/Edit/',
                        type: 'POST',
                        data: JSON.stringify({ "Complain": Complain, "InspectionID": InspectionID, "Active": true }),
                        dataType: "json",
                        contentType: "application/json; charset=utf-8"
                    });
                }
            });

        });
    });

    $.get("/details/GetInspections/" + $(".patientDetail").val() + "", function (data) {
        $.each(data.data, function (index, value) {
            $('#tab_muayene_findings_' + value.ID).keydown(function (e) {
                if (e.keyCode === 13) {
                    console.log("enter");
                    var Finding = $("#txt_finding_" + value.ID).val();
                    var InspectionID = (value.ID);

                    $.ajax({
                        url: '/Findings/Edit/',
                        type: 'POST',
                        data: JSON.stringify({ "Finding": Finding, "InspectionID": InspectionID, "Active": true }),
                        dataType: "json",
                        contentType: "application/json; charset=utf-8"
                    });
                }
            });

        });
    });

    $.get("/details/GetInspections/" + $(".patientDetail").val() + "", function (data) {
        $.each(data.data, function (index, value) {
            $('#tab_muayene_diagnosis_' + value.ID).keydown(function (e) {
                if (e.keyCode === 13) {
                    console.log("enter");
                    var Description = $("#txt_diagnosis_" + value.ID).val();
                    var InspectionID = (value.ID);

                    $.ajax({
                        url: '/Diagnosis/Edit/',
                        type: 'POST',
                        data: JSON.stringify({ "Description": Description, "InspectionID": InspectionID, "Active": true }),
                        dataType: "json",
                        contentType: "application/json; charset=utf-8"
                    });
                }
            });

        });
    });

    $.get("/details/GetInspections/" + $(".patientDetail").val() + "", function (data) {
        $.each(data.data, function (index, value) {
            $('#tab_muayene_treatments_' + value.ID).keydown(function (e) {
                if (e.keyCode === 13) {
                    console.log("enter");
                    var Treatment = $("#txt_treatment_" + value.ID).val();
                    var InspectionID = (value.ID);

                    $.ajax({
                        url: '/Treatments/Edit/',
                        type: 'POST',
                        data: JSON.stringify({ "Treatment_Plan": Treatment, "InspectionID": InspectionID, "Active": true }),
                        dataType: "json",
                        contentType: "application/json; charset=utf-8"
                    });
                }
            });

        });
    });

    $.get("/details/GetInspections/" + $(".patientDetail").val() + "", function (data) {
        $.each(data.data, function (index, value) {
            $('#tab_muayene_examinations_' + value.ID).keydown(function (e) {
                if (e.keyCode === 13) {
                    console.log("enter");
                    var Examination = $("#txt_examination_" + value.ID).val();
                    var InspectionID = (value.ID);

                    $.ajax({
                        url: '/Examinations/Edit/',
                        type: 'POST',
                        data: JSON.stringify({ "Examination": Examination, "InspectionID": InspectionID, "Active": true }),
                        dataType: "json",
                        contentType: "application/json; charset=utf-8"
                    });
                }
            });

        });
    });
});