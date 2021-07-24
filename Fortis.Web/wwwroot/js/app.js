//All App related code goes here
var App = (function() {

    var _url = '/api/',

        startUp = function() {
            //Document event binding
            $(document).on('click', '#apitestsettings', function() {
                $.ajax({
                    type: 'GET',
                    url: _url + 'testsettings',
                    dataType: 'html',
                    beforeSend: function() {
                        $('div.feedback').text('Running Test...');
                    },
                    success: function(data) {
                        console.log(data);
                        $('div.feedback').removeClass('error success').addClass(data.toLowerCase().indexOf('failed') > -1 ? 'error' : 'success').text(data);
                    }
                });
            }).on('click', 'table#sqlinputsqueries div.editbuttons i.fa-play-circle', function(e) {
                var nameord = $(this).closest('table').find('th:contains("Name")').index(),
                    jobname = $(this).closest('tr').find('td:nth(' + nameord + ')')[0].innerText;
                $.ajax({
                    type: 'POST',
                    url: _url + 'runtemplate',
                    headers: { "Content-Type": "application/json" },
                    data: JSON.stringify(jobname),
                    beforeSend: function() {
                        $(e.target).removeClass().addClass('fa fa-cog fa-spin');
                    },
                    success: function(data) {
                        if (data.id && data.id != null && data.id > 0) {
                            $(e.target).removeClass().addClass('fa fa-check-square success');
                        } else {
                            $(e.target).removeClass().addClass('fa fa-exclamation-triangle error');
                        };
                    },
                    error: function(x, s, er) {
                        $(e.target).removeClass().addClass('fa fa-exclamation-circle error').prop('title', er);
                    }
                });
            });
        };

    return {
        init: startUp
    };
})();