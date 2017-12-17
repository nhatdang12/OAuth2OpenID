var identityServerUrl = '', apiClientUrl = '', baseUrl = '';
var mgr = null;


function log() {
    document.getElementById('results').innerText = '';

    Array.prototype.forEach.call(arguments, function (msg) {
        if (msg instanceof Error) {
            msg = "Error: " + msg.message;
        }
        else if (typeof msg !== 'string') {
            msg = JSON.stringify(msg, null, 2);
        }
        document.getElementById('results').innerHTML += msg + '\r\n';
    });
}

function login() {
    mgr.signinRedirect();
}

function api() {
    mgr.getUser().then(function (user) {
        var xhr = new XMLHttpRequest();

        xhr.open("GET", apiClientUrl);
        xhr.onload = function () {
            log(xhr.status, JSON.parse(xhr.responseText));
        }
        xhr.setRequestHeader("Authorization", "Bearer " + user.access_token);
        xhr.send();
    });
}

function logout() {
    mgr.signoutRedirect();
}

function init() {
    baseUrl = $(location).attr('protocol') + '//' + $(location).attr('host');
    $.getJSON(baseUrl, function (data) {
        identityServerUrl = data.identityServerHostUrl;
        apiClientUrl = data.apiClientHostUrl;

        //configure and instantiate the UserManager
        var config = {
            ////authority: "http://localhost:5000",
            authority: identityServerUrl,
            client_id: "js",
            redirect_uri: baseUrl + "/callback.html",
            response_type: "id_token token",
            scope: "openid profile api1",
            post_logout_redirect_uri: baseUrl + "/index.html",
        };

        mgr = new Oidc.UserManager(config);

        mgr.getUser().then(function (user) {
            if (user) {
                log("User logged in", user.profile);
            }
            else {
                log("User not logged in");
            }
        });
    });
}


//register “click” event handlers to the three buttons
document.getElementById("login").addEventListener("click", login, false);
document.getElementById("api").addEventListener("click", api, false);
document.getElementById("logout").addEventListener("click", logout, false);
//
init();
