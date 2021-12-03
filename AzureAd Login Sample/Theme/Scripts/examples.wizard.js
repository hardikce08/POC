
(function ($) {

    'use strict';

	/*
	Wizard #1
	*/
    var $w1finish = $('#w1').find('ul.pager li.finish'),
        $w1validator = $("#w1 form").validate({
            highlight: function (element) {
                $(element).closest('.form-group').removeClass('has-success').addClass('has-error');
            },
            success: function (element) {
                $(element).closest('.form-group').removeClass('has-error');
                $(element).remove();
            },
            errorPlacement: function (error, element) {
                element.parent().append(error);
            }
        });

    $w1finish.on('click', function (ev) {
        ev.preventDefault();
        var validated = $('#w1 form').valid();
        if (validated) {
            new PNotify({
                title: 'Congratulations',
                text: 'Your configuration has been saved',
                type: 'custom',
                addclass: 'notification-success',
                icon: 'fa fa-check'
            });
            setTimeout(
                function () {
                    //alert("Called after delay.");
                    $('#w1 form').submit();
                },
                2000);
        }
    });

    //$('#w1').bootstrapWizard({
    //	tabClass: 'wizard-steps',
    //	nextSelector: 'ul.pager li.next',
    //	previousSelector: 'ul.pager li.previous',
    //	firstSelector: null,
    //	lastSelector: null,
    //	onNext: function( tab, navigation, index, newindex ) {
    //		var validated = $('#w1 form').valid();
    //		if( !validated ) {
    //			$w1validator.focusInvalid();
    //			return false;
    //		}
    //	},
    //	onTabClick: function( tab, navigation, index, newindex ) {
    //		if ( newindex == index + 1 ) {
    //			return this.onNext( tab, navigation, index, newindex);
    //		} else if ( newindex > index + 1 ) {
    //			return false;
    //		} else {
    //			return true;
    //		}
    //	},
    //	onTabChange: function( tab, navigation, index, newindex ) {
    //		var totalTabs = navigation.find('li').size() - 1;
    //		$w1finish[ newindex != totalTabs ? 'addClass' : 'removeClass' ]( 'hidden' );
    //		$('#w1').find(this.nextSelector)[ newindex == totalTabs ? 'addClass' : 'removeClass' ]( 'hidden' );
    //	}
    //});

    $('#w1').bootstrapWizard({
        tabClass: 'wizard-steps',
        nextSelector: 'ul.pager li.next',
        previousSelector: 'ul.pager li.previous',
        firstSelector: null,
        lastSelector: null,
        onNext: function (tab, navigation, index, newindex) {
            //alert('in');
            var activetab = $('.wizard-steps li.active').data("id");
            //alert(activetab);
            if (activetab == "step2") {
                if (document.getElementById('CallList').checked) {
                    if ($("#CallListFrequency option:selected").val() == "") {
                        $("#CallListFrequency").attr("required", "");
                    }
                }
                if (document.getElementById('MailList').checked) {
                    if ($("#MailListFrequency option:selected").val() == "") {
                        $("#MailListFrequency").attr("required", "");
                    }
                }
                if (document.getElementById('EmailList').checked) {
                    if ($("#EmailListFrequency option:selected").val() == "") {
                        $("#EmailListFrequency").attr("required", "");
                    }
                }
            }
            var validated = $('#w1 form').valid();
            if (!validated) {
                $w1validator.focusInvalid();
                return false;
            }
        },
        onTabClick: function (tab, navigation, index, newindex) {

            if (newindex == index + 1) {
                return this.onNext(tab, navigation, index, newindex);
            } else if (newindex > index + 1) {
                return false;
            } else {
                return true;
            }
        },
        onTabChange: function (tab, navigation, index, newindex) {
            var $total = navigation.find('li').size() - 1;
            $w1finish[newindex != $total ? 'addClass' : 'removeClass']('hidden');
            $('#w1').find(this.nextSelector)[newindex == $total ? 'addClass' : 'removeClass']('hidden');
        },
        onTabShow: function (tab, navigation, index) {
            var $total = navigation.find('li').length - 1;
            var $current = index;
            var $percent = Math.floor(($current / $total) * 100);
            $('#w1').find('.progress-indicator').css({ 'width': $percent + '%' });
            tab.prevAll().addClass('completed');
            tab.nextAll().removeClass('completed');
        }
    });


}).apply(this, [jQuery]);
