$(document).ready(function () {

    var oTable = $('#EmployeeDT').dataTable({
        "language": {
            "url": "/Content/lang.txt"
        },
        "bServerSide": true,
        "sAjaxSource": "EmployeeAjaxHandler",
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
                                        return '<input disabled  type=\"checkbox\" checked value="' + data + '">';
                                    } else {
                                        return '<input disabled  type=\"checkbox\" value="' + data + '">';
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
                                        return '<input disabled  type=\"checkbox\" checked value="' + data + '">';
                                    } else {
                                        return '<input disabled  type=\"checkbox\" value="' + data + '">';
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
                                return "<a class='glyphicon glyphicon-list a_clickable' href='/Question?pId=" + row[0] + "'></a>"

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
                                return "<a class='glyphicon glyphicon-list-alt a_clickable' href='/Solution?pId=" + row[0] + "'></a>"
                            }
                        }

        ]
    });



});



