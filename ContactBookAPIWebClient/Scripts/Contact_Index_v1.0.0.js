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
                    var contact = data.contact;
                    var tags = data.tags;

                    var avatarSrc = button.parents(".thumbnail").children(":first").attr('src').substring(0, 63);
                    modal.find("#detailsAvatar").attr('src', avatarSrc+'?s=100');
                    modal.find('#detailsName').text(contact.name);
                    modal.find('#detailsDescription').text(contact.description);
                    modal.find('#detailsEmail').text(contact.email);
                    modal.find('#detailsAddressLine1').text(contact.addressLine1);
                    modal.find('#detailsAddressLine2').text(contact.addressLine2);
                    modal.find('#detailsPhoneNumber').text(contact.phoneNumber);
                    modal.find('#detailsCellNumber').text(contact.cellNumber);

                    modal.find('#detailsFacebook').text(contact.facebook);
                    modal.find('#detailsFacebook').attr('href', 'http://facebook.com/' + contact.facebook);
                    modal.find('#detailsTwitter').text(contact.twitter);
                    modal.find('#detailsTwitter').attr('href', 'http://twitter.com/' + contact.twitter);
                    modal.find('#detailsSkype').text(contact.skypeId);
                    modal.find('#detailsSkype').attr('href', 'skype:' + contact.skypeId + '?call');
                    
                    modal.find('#detailsTags').children().remove();
     
                        for (var i = 0; i < tags.length; i++) {
                            if (!isEmptyOrSpaces(tags[i])) {
                                modal.find('#detailsTags').append('<span class="tag">' + tags[i] + '</span>');
                            }
                        }
                    

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

    function isEmptyOrSpaces(str) {
        return str === null || str.match(/^ *$/) !== null;
    }
});