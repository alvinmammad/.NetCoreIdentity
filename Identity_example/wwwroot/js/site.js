$(function () {
    $("#countries").change(function () {
        var countryID = $(this).val();

        if (countryID) {
            $.ajax({
                url: "/Users/LoadCities?countryID=" + countryID,
                type: "POST",
                success: function (res) {
                    $("#CityID").html(res);
                    $("#CityID").prepend("<option value=''>Şəhəri seçin :</option>");
                }
            });
        }
    });
});
