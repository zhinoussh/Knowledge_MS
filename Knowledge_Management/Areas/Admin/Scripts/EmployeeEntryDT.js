/***USED IN ViewEntrybyEmployee/Personel*************/


$(document).ready(function () {

    var oTable = $('#EmployeeEntryDT').dataTable({
        "bServerSide": true,
        "sAjaxSource": "/Admin/ViewEntrybyEmployee/EmployeeAjaxHandler",
        "bProcessing": true,
        "pagingType": "numbers"
        , "aoColumns": [
                        {
                            "sName": "ID",
                            "bSearchable": false,
                            "bSortable": false,
                            "bVisible": false
                        }, {
                            "sName": "radif", "sWidth": '2%', "sClass": "dt-body-center"
                            , "bSearchable": false, "bSortable": false
                        },
                        { "sName": "fname", "sWidth": '15%' },
                         { "sName": "lname", "sWidth": '20%' },
                         { "sName": "pcode", "sWidth": '15%' }
                            , { "sName": "department", "sWidth": '20%' },
                            { "sName": "job", "sWidth": '20%' }
                            , {
                                "sName": "dt_entry",
                                "sWidth": '2%',
                                "bSearchable": false,
                                "bSortable": false,
                                "sDefaultContent": " "
                            , "sClass": "dt-body-center",
                                "mRender": function (data, type, row) {

                                    if (data == "True") {
                                        return '<div class=\"checkbox checkbox-info\"><input  type=\"checkbox\" id=\"check_dataEntry\" disabled checked value="' + data + '"><label for=\"check_dataEntry\"></label></div>';
                                    } else {
                                        return '<div class=\"checkbox checkbox-info\"><input  type=\"checkbox\" id=\"check_dataEntry\" disabled value="' + data + '"><label for=\"check_dataEntry\"></label></div>';
                                    }
                                }
                            }
                            , {
                                "sName": "dt_view",
                                "sWidth": '2%',
                                "bSearchable": false,
                                "bSortable": false,
                                "sDefaultContent": " "
                            , "sClass": "dt-body-center",
                                "mRender": function (data, type, row) {
                                    if (data == "True") {
                                        return '<div class=\"checkbox checkbox-info\"><input  type=\"checkbox\" id=\"check_dataView\" disabled checked value="' + data + '"><label for=\"check_dataView\"></label></div>';
                                    } else {
                                        return '<div class=\"checkbox checkbox-info\"><input  type=\"checkbox\" id=\"check_dataView\" disabled value="' + data + '"><label for=\"check_dataView\"></label></div>';
                                    }
                                }
                            }
                        , {
                            "sName": "Show_Questions",
                            "sWidth": '2%',
                            "bSearchable": false,
                            "bSortable": false,
                            "sDefaultContent": " "
                            , "sClass": "dt-body-center",
                            "mRender": function (data, type, row) {
                                return "<a class='glyphicon glyphicon-comment a_clickable' href='/Admin/ViewEntrybyEmployee/Question/" + row[0] + "'></a>"

                            }
                        }
                        , {
                            "sName": "Show_Solutions",
                            "sWidth": '2%',
                            "bSearchable": false,
                            "bSortable": false,
                            "sDefaultContent": " "
                            , "sClass": "dt-body-center"
                            , "mRender": function (data, type, row) {
                                return "<a class='glyphicon glyphicon-list-alt a_clickable' href='/Admin/ViewEntrybyEmployee/EmployeeSolutions/" + row[0] + "'></a>"
                            }
                        }

        ]
    });



});

var SuccessDeleteUpload = function (result) {
    if (result.msg) {
        $('#DeleteModal').modal('hide');
        $("#alert_success").html(result.msg);
        $("#div_alert").slideDown(500);
        var $STTable = $("#UploadDT").dataTable({ bRetrieve: true });
        $STTable.fnDraw();
    }
}

var SuccessDeleteSolution = function (result) {
    if (result.msg) {
        $('#DeleteModal').modal('hide');
        $("#alert_success").html(result.msg);
        $("#div_alert").slideDown(500);
        var $STTable = $("#SoutionListDT").dataTable({ bRetrieve: true });
        $STTable.fnDraw();
    }
}

var SuccessDeleteQuestion= function (result) {
    if (result.msg) {
        $('#DeleteModal').modal('hide');
        $("#alert_success").html(result.msg);
        $("#div_alert").slideDown(500);
        var $STTable = $("#SearchQuestionDT").dataTable({ bRetrieve: true });
        $STTable.fnDraw();
    }
}