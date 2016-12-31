$(document).ready(function () {
     $("#field").on('click', '.add-more', function () {
        var next = parseInt($("#count").val());
        
        var addto = "#field" + next;
        var addRemove = "#field" + (next);
        next = next + 1;
        var newIn = '<input autocomplete="off" class="txt_keyword" id="field' + next + '" name="field' + next + '" type="text" placeholder="Enter Keyword">';
        var newInput = $(newIn);
        var removeBtn = '<button id="remove' + (next - 1) + '" class="btn btn-danger remove-me">-</button></div><div id="field">';
        var removeButton = $(removeBtn);
        $(addto).after(newInput);
        $(addRemove).after(removeButton);
        $("#field" + next).attr('data-source', $(addto).attr('data-source'));
        $("#count").val(next);


        //$('.remove-me').click(function (e) {
        $(document).on('click', '.remove-me', function () {
            var fieldNum = this.id.charAt(this.id.length - 1);
            var fieldID = "#field" + fieldNum;
            $(this).remove();
            $(fieldID).remove();
        });
    });



});
