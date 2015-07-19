$(document).ready(function () {

    $(".modal-body-load").hide();
    $(".modal-body").hide();

    //Details modal dialog custom code
    $('#detailsModal').on('show.bs.modal', function (event) {

        //hide priority marks in modal
        $("#modalHeader").removeAttr('class');



        var button = $(event.relatedTarget) // Button that triggered the modal
        var contactId = button.data('id') // Extract info from data-* attributes
        var modal = $(this);

        $.ajax({
            url: siteRoot + "Contact/GetContactDetailsByAjax",
            dataType: "json",
            data: {
                id: contactId
            },
            selectFirst: true,
            success: function (data) {
                if (data) {

                    var avatarSrc = button.parents(".thumbnail").children(":first").attr('src').substring(0, 63);
                    modal.find("#detailsAvatar").attr('src', avatarSrc+'?s=100');
                    modal.find('#detailsName').text(data.name);
                    modal.find('#detailsDescription').text(data.description);
                    modal.find('#detailsEmail').text(data.email);
                    modal.find('#detailsAddressLine1').text(data.addressLine1);
                    modal.find('#detailsAddressLine2').text(data.addressLine2);
                    modal.find('#detailsPhoneNumber').text(data.phoneNumber);
                    modal.find('#detailsCellNumber').text(data.cellNumber);

                    modal.find('#detailsFacebook').text(data.facebook);
                    modal.find('#detailsFacebook').attr('href', 'http://facebook.com/' + data.facebook);
                    modal.find('#detailsTwitter').text(data.twitter);
                    modal.find('#detailsTwitter').attr('href', 'http://twitter.com/' + data.twitter);
                    modal.find('#detailsSkype').text(data.skypeId);
                    modal.find('#detailsSkype').attr('href','skype:'+data.skypeId+'?call');


                    modal.find('input,textarea,select').attr('disabled', 'disabled');

                }
            },
            beforeSend: function (xhr) {
                $(".modal-body-load").show();
                $(".modal-body").hide();

            },
            complete: function (xhr, status) {
                $(".modal-body-load").hide();
                $(".modal-body").show();
            }
        });

    })

    $(".btn-delete").click(function(e) {
        e.preventDefault();
        if (confirm('Do you really want to remove this contact?')) {
            var id = $(this).attr('contact-id');

            window.location.replace(siteRoot + "/Contact/Delete/" + id);
        } else { }

    });
});