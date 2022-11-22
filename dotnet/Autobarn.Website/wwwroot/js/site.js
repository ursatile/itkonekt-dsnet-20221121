// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

function connectToSignalR() {
    var conn = new signalR.HubConnectionBuilder().withUrl("/hub").build();
    conn.on("ShowAPopupNotificationAboutANewVehicle", displayVehiclePopup);
    conn.start().then(function() {
        console.log("SignalR Connected and running!");
    }).catch(function(error) {
        console.log(`SignalR Failed: ${error}`);
    });
}

function displayVehiclePopup(user, message) {
    console.log(user);
    console.log(message);
    var data = JSON.parse(message);
    var $target = $("div#signalr-notifications");
    var $div = $(`<div>
        NEW VEHICLE NOTIFICATION!<br />
        ${data.ManufacturerName} ${data.ModelName} (${data.Color}, ${data.Year})<br />
        <strong>Price: ${data.Price} ${data.CurrencyCode}</strong><br />
        <a href="/vehicles/details/${data.Registration}">click for more info...</a>
        </div>`);
    $div.css("background-color", data.Color);
    $target.prepend($div);
    window.setTimeout(function () { $div.fadeOut(2000, function () { $div.remove(); }); }, 2000);
}

$(document).ready(connectToSignalR);
