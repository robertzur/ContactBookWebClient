
$(document).ready(function () {

    $(".modal-body-load").hide();
    $(".modal-body").hide();

    //Details modal dialog custom code
    $('#detailsModal').on('show.bs.modal', function (event) {

        //hide priority marks in modal
        $("#modalHeader").removeAttr('class');



        var button = $(event.relatedTarget) // Button that triggered the modal
        var groupId = button.data('id') // Extract info from data-* attributes
        var modal = $(this);

        $.ajax({
            url: siteRoot + "Contact/GetContactsInGroupByAjax",
            dataType: "json",
            data: {
                id: groupId
            },
            selectFirst: true,
            success: function (data) {
                var contactsItems = $('#list-group-contacts');

                if (data) {
                    contactsItems.html('');
                    if (data.length == 0) {
                        contactsItems.append('<li class="list-group-item">No members...</li>');
                    }
                    else {
                        for (var i = 0; i < data.length; i++) {

                            var hash = $.md5(data[i].email.toLowerCase());
                            contactsItems.append('<li class="list-group-item"><div class="row"><div class="col-md-2 col-sm-2"> <img src="http://www.gravatar.com/avatar/' + hash + '?s=75"></div><div class="col-md-3 col-sm-3">' + data[i].name + '</div><div class="col-md-4 col-sm-4">' + data[i].description + '</div></div></li>');

                        }
                    }
                    modal.find('input,textarea,select').attr('disabled', 'disabled');

                } else {
                    contactsItems.append('<li class="list-group-item">No members...</li>');

                }
            },
            beforeSend: function (xhr) {
                $(".modal-body-load").show();
                $(".modal-body").hide();
                $("#list-group-contacts").hide();

            },
            complete: function (xhr, status) {
                $(".modal-body-load").hide();
                $(".modal-body").show();
                $("#list-group-contacts").show();
            }
        });

    })

    $(".btn-delete").click(function (e) {
        e.preventDefault();
        if (confirm('Do you really want to remove this contact?')) {
            var id = $(this).attr('group-id');

            window.location.replace(siteRoot + "/Group/Delete/" + id);
        } else { }

    });

})