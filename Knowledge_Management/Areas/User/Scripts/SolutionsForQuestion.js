/***USED IN Solution/Index*************/

$(document).ready(function () {

    var oTable = $('#SoutionListDT').dataTable({
        "bServerSide": true,
        "sAjaxSource": "/User/Solution/SolutionsForQuestionAjaxHandler",
        "fnServerParams": function (aoData) {
            aoData.push({ "name": "q_id", "value": $('#hd_id_question').val() });
        },
        "bProcessing": true,
        "pagingType": "numbers"
        , "aoColumns": [
                        {
                            "sName": "ID",
                            "bSearchable": false,
                            "bSortable": false,
                            "bVisible": false
                        },
                        {
                            "sName": "radif", "sWidth": '2%', "sClass": "dt-body-center"
                                , "bSearchable": false, "bSortable": false
                        },
                        { "sName": "solution", "sWidth": '80%' }
                        ,
                        { "sName": "upload_count", "sWidth": '5%', "sClass": "dt-body-center", "bSearchable": false, "bSortable": false },
                        {
                            "sName": "Show_FullSolution",
                            "sWidth": '2%',
                            "bSearchable": false,
                            "bSortable": false,
                            "sDefaultContent": " "
                             , "sClass": "dt-body-center",
                            "mRender": function (data, type, row) {
                                return '<a class="glyphicon glyphicon-list a_clickable" href="/User/Solution/FullSolution/' + row[0] + '"></a>'

                            }
                        }


        ]
    });

});


