/***USED IN SearchInfo/SearchAll*************/
$(document).ready(function () {

    var oTable = $('#SearchQuestionDT').dataTable({
        "bServerSide": true,
        "sAjaxSource": "/User/SearchInfo/SearchQuestionAjaxHandler",
        "fnServerParams": function (aoData) {
            aoData.push({ "name": "key_id", "value": $('#hd_id_keyword').val() });
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
                             "sName": "keywords",
                             "bSearchable": false,
                             "bSortable": false,
                             "bVisible": false
                         },
                          {
                              "sName": "job_Desc",
                              "bSearchable": false,
                              "bSortable": false,
                              "bVisible": false
                          },
                           {
                               "sName": "dep_Obj",
                               "bSearchable": false,
                               "bSortable": false,
                               "bVisible": false
                           },
                            {
                                "sName": "Strategy",
                                "bSearchable": false,
                                "bSortable": false,
                                "bVisible": false
                            },
                            {
                                "sName": "fullQuestion",
                                "bSearchable": false,
                                "bSortable": false,
                                "bVisible": false
                            },
                            {
                                "sName": "radif", "sWidth": '2%', "sClass": "dt-body-center"
                                , "bSearchable": false, "bSortable": false
                            },
                        { "sName": "question", "sWidth": '80%' },
                        {
                            "sName": "details",
                            "sWidth": '2%',
                            "bSearchable": false,
                            "bSortable": false,
                            "sDefaultContent": " "
                             , "sClass": "dt-body-center",
                            "mRender": function (data, type, row) {
                                var param_array = {
                                    keywords: row[1], jobdesc: row[2], dep_objective: row[3]
                                    , strategy: row[4], fullQuestion: row[5]
                                };
                                var param_obj = [];
                                param_obj.push(param_array);
                                return "<a class='glyphicon glyphicon-list a_clickable' onclick='details(" + JSON.stringify(param_obj) + ");'></a>"

                            }
                        },
                        {
                           "sName": "Solutions",
                           "sWidth": '2%',
                           "bSearchable": false,
                           "bSortable": false,
                           "sDefaultContent": " "
                            , "sClass": "dt-body-center",
                           "mRender": function (data, type, row) {
                               return "<a class='glyphicon glyphicon-list-alt a_clickable' href='/User/Solution/Index/" + row[0] + "'></a>"
                           }

                        }
                        ,
                        {
                            "sName": "New_Solution",
                            "sWidth": '2%',
                            "bSearchable": false,
                            "bSortable": false,
                            "sDefaultContent": " "
                            , "sClass": "dt-body-center",
                            "mRender": function (data, type, row) {
                                return "<a class='glyphicon glyphicon-file a_clickable' href='/User/Solution/NewSolution/" + row[0] + "'></a>"
                            }

                        }


        ]
    });

    

});

var details = function (s) {
    var QuestionViewModel = {
        question:  s[0].fullQuestion,
        lst_keywords:s[0].keywords,
        dep_objective: (s[0].dep_objective == null ? '' : s[0].dep_objective),
        job_desc: (s[0].jobdesc == null ? '' : s[0].jobdesc),
        strategy_name: (s[0].strategy == null ? '' : s[0].strategy)
    };

    $.ajax({
        type: 'GET',
        data: QuestionViewModel,
        url: '/User/SearchInfo/QuestionDetails',
        success: function (result) {
            $('#ModalContainer').html(result);
            $('#ModalContainer').find("#DetailModal").modal('show');
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert(errorThrown);
        }
    });
}




