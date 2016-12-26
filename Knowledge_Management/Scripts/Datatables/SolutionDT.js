/***USED IN Solution/YourSolution*************/


$(document).ready(function () {

    var oTable = $('#SolutionDT').dataTable({
        "language": {
            "url": "/Content/lang.txt"
        },
        "bServerSide": true,
        "sAjaxSource": "YourSolutionAjaxHandler",
        "bProcessing": true,
        "pagingType": "numbers"
        , "aoColumns": [
                         {
                             "sName": "Sol_ID",
                             "bSearchable": false,
                             "bSortable": false,
                             "bVisible": false
                         },
                        {
                            "sName": "QID",
                            "bSearchable": false,
                            "bSortable": false,
                            "bVisible": false
                        },
                            {
                                "sName": "radif", "sWidth": '2%', "sClass": "dt-body-center"
                                , "bSearchable": false, "bSortable": false
                            },
                        { "sName": "question", "sWidth": '40%' },
                        { "sName": "solution", "sWidth": '40%', "bSearchable": false, "bSortable": false }
                        , {
                            "sName": "upload_num", "sWidth": '3%', "bSearchable": false, "bSortable": false,
                            "sClass": "dt-body-center"
                        }
                       , {
                           "sName": "confirm_status",
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
                       }, {
                           "sName": "EDIT",
                           "sWidth": '2%',
                           "bSearchable": false,
                           "bSortable": false,
                           "sDefaultContent": " "
                            , "sClass": "dt-body-center",
                            "mRender": function (data, type, row) {
                                var url = 'NewSolution/' + row[1] + '?solution_id=' + row[0];
                                return "<a class='glyphicon glyphicon-edit a_clickable' href='" + url + "'></a>"

                           }
                       }
                        , {
                            "sName": "DELETE",
                            "sWidth": '2%',
                            "bSearchable": false,
                            "bSortable": false,
                            "sDefaultContent": " "
                            , "sClass": "dt-body-center"
                            , "mRender": function (data, type, row) {
                                return "<a class='glyphicon glyphicon-trash a_clickable' onclick='delete_dialog(" + row[0] + ")'></a>"
                            }
                        }


        ]
    });

    $(".close").click(function () {
        $("#div_alert").slideDown(500);
        return false;
    });

});



var delete_dialog = function (job_id) {

    var url = "/Solution/Delete_Solution"; // the url to the controller
    $.get(url + '/' + job_id, function (data) {
        $('#confirm-container').html(data);
        $('#DeleteModal').modal('show');
    });
}



