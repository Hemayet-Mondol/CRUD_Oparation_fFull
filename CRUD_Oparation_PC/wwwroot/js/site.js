// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    
    $('#CountryId').change(function () {
        $.ajax({
            type: "get",
            url: "/CRUD/StateGetStatesByCountryId",
            data: { countryId: $('#CountryId').val() },
          
            datatype: "json",
            traditional: true,
            success: function (data) {
                var state = "<select id='StateId'>";
                state = state + '<option value="">Select State</option>';
                for (var i = 0; i < data.length; i++) {
                    state = state + '<option value=' + data[i].id + '>' +
                        data[i].stateName + '</option>';
                }
                state = state + '</select>';
                $('#StateId').html(state);
            }
        });
    });
    // Get Cities by State ID
    $('#StateId').change(function () {
        $.ajax({
            type: "get",
            url: "/CRUD/GetCitiesByStateId",
            data: { stateId: $('#StateId').val() },
            datatype: "json",
            traditional: true,
            success: function (data) {
                var city = "<select id='CityId'>";
                city = city + '<option value="">Select City</option>';
                for (var i = 0; i < data.length; i++) {
                    city = city + '<option value=' + data[i].id + '>' + data[i].cityName
                        + '</option>';
                }
                city = city + '</select>';
                $('#CityId').html(city);
            }
        });
    });

    //Checkbox Checked
    //var $ssc = $("#Ssc");
    //var $hsc = $("#Hsc");
    //var $bsc = $("#Bsc");
    //var $msc = $("#Msc");
 
    $('#Hsc').on('click', function () {
        var anycheck = $('#Hsc').is(':checked');
        $('#Ssc').prop('checked', anycheck);
        });
    $('#Bsc').on('click', function () {
        var anycheck = $('#Bsc').is(':checked');
        $('#Ssc').prop('checked', anycheck);
        $('#Hsc').prop('checked', anycheck);
    });
    $('#Msc').on('click', function () {
        var anycheck = $('#Msc').is(':checked');
        $('#Ssc').prop('checked', anycheck);
        $('#Hsc').prop('checked', anycheck);
        $('#Bsc').prop('checked', anycheck);
    });
    
});

//Image Preview
function PreviewImage() {
    var OfReader = new FileReader();
    OfReader.readAsDataURL(document.getElementById('FileUpload').files[0]);
    OfReader.onload = function (oFREvent) {
        document.getElementById('UploadFile').src = oFREvent.target.result;
    }
}




