// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

function connectToSignalR() {
    var conn = new signalR.HubConnectionBuilder().withUrl("/hub").build();
    conn.on("ShowAPopupNotificationAboutANewVehicle",
        (user, message) => {
            console.log(user);
            console.log(message);
        });
    conn.start().then(function() {
        console.log("SignalR Connected and running!");
    }).catch(function(error) {
        console.log(`SignalR Failed: ${error}`);
    });
}

$(document).ready(connectToSignalR);
