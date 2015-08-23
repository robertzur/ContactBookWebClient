$(document).ready(function () {
    $('#tags').tagsInput();

    $(".dropdown-menu li a").click(function () {
        $(".dropdown > .btn:first-child").html($(this).html() + ' <span class="caret"></span>');
        $("#contact_parentId").val($(this).attr('group-id'));
    });
});