$(document).ready(function () {

    var oTable = $('#SearchQuestionDT').dataTable({
        "language": {
            "url": "/Content/lang.txt"
        },
        "bServerSide": true,
        "sAjaxSource": "SearchQuestionAjaxHandler",
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
                                "sName": "radif", "sWidth": '2%', "sClass": "dt-body-center"
                            , "bSearchable": false, "bSortable": false
                            },
                        { "sName": "question", "sWidth": '80%' }
                       ,{
                              "sName": "Solutions",
                              "sWidth": '2%',
                              "bSearchable": false,
                              "bSortable": false,
                              "sDefaultContent": " "
                            , "sClass": "dt-body-center",
                              "mRender": function (data, type, row) {
                                  return "<a class='glyphicon glyphicon-list-alt a_clickable' href='/Solution/Solutions/" + row[0] + "'></a>"
                              }
                          }
                        

        ]
    });







});







