var serviceWorker = '/sw.js';
var isSubscribed = false;

function notificarme() {

    console.log('11111');

    Notification.requestPermission().then(function (status) {
        if (status === 'denied') {
            console.log('Denegado');
        } else if (status === 'granted') {
            console.log('Permitido');
            initialiseServiceWorker();
        }
    });
    
    //subscribe();
}

function initialiseServiceWorker() {
    if ('serviceWorker' in navigator) {
        navigator.serviceWorker.register(serviceWorker).then(handleSWRegistration);
        $(".reci-notification").addClass("d-none").removeAttr('src').attr('src', 'Content/Images/bell1.png').removeAttr('onclick').removeClass("recibe-no").removeClass("d-none");
    } else {
        console.log("Este navegador no soporta notificaciones");
    }
};

function handleSWRegistration(reg) {
    if (reg.installing) {
        console.log('Service worker installing');
    } else if (reg.waiting) {
        console.log('Service worker installed');
    } else if (reg.active) {
        console.log('Service worker active');
        ChangeToBell1();
    }

    initialiseState(reg);
}

// Once the service worker is registered set the initial state
//Una vez que el trabajador de servicio está registrado, establecer el estado inicial
function initialiseState(reg) {
    // Are Notifications supported in the service worker?
    //// ¿Son compatibles las notificaciones en el trabajador del servicio?
    if (!(reg.showNotification)) {
        console.log('Notifications aren\'t supported on service workers.');
        return;
    }

    // Check if push messaging is supported
    //Compruebe si la mensajería push es compatible
    if (!('PushManager' in window)) {
        console.log('Push messaging isn\'t supported.');
        return;
    }

    // We need the service worker registration to check for a subscription
    //Necesitamos el registro de trabajador de servicio para verificar una suscripción
    navigator.serviceWorker.ready.then(function (reg) {
        // Do we already have a push message subscription?
        // ¿Ya tenemos una suscripción de mensaje push?
        reg.pushManager.getSubscription()
            .then(function (subscription) {
                isSubscribed = subscription;
                if (isSubscribed) {
                    console.log('El usuario ya está suscrito a notificaciones push');
                } else {
                    console.log('El usuario aún no está suscrito a las notificaciones push');
                    subscribe();
                }
            })
            .catch(function (err) {
                console.log('Unable to get subscription details.', err);
            });
    });
}


function subscribe() {
    navigator.serviceWorker.ready.then(function (reg) {
        var subscribeParams = { userVisibleOnly: true };

        //Setting the public key of our VAPID key pair.
        //Configuración de la clave pública de nuestro par de claves VAPID.
        var applicationServerKey = urlB64ToUint8Array(applicationServerPublicKey);
        subscribeParams.applicationServerKey = applicationServerKey;

        reg.pushManager.subscribe(subscribeParams)
            .then(function (subscription) {
                isSubscribed = true;

                var p256dh = base64Encode(subscription.getKey('p256dh'));
                var auth = base64Encode(subscription.getKey('auth'));

                console.log(subscription);

                Subscribe(subscription);
                $('#PushEndpoint').val(subscription.endpoint);
                $('#PushP256DH').val(p256dh);
                $('#PushAuth').val(auth);
            })
            .catch(function (e) {
                console.log('Unable to subscribe to push');
            });
    });
}


function Subscribe(data) {
    $.ajax({
        cache: false,
        method: 'POST',
        dataType: 'json',
        data: { 'data': JSON.stringify(data) },
        url: '/WorkerData/Subscribe',
        success: function (json) {
            console.log('hola AMCCC');
        },
        error: function (error) {
            console.log(error);
        }
    });
}

function ChangeToBell1() {
    $("#notificacion").attr("src", "/Content/Images/bell1.png").removeClass("js-suscrito view-notification");
}

function urlB64ToUint8Array(base64String) {
    var padding = '='.repeat((4 - base64String.length % 4) % 4);
    var base64 = (base64String + padding)
        .replace(/\-/g, '+')
        .replace(/_/g, '/');

    var rawData = window.atob(base64);
    var outputArray = new Uint8Array(rawData.length);

    for (var i = 0; i < rawData.length; ++i) {
        outputArray[i] = rawData.charCodeAt(i);
    }
    return outputArray;
}

function base64Encode(arrayBuffer) {
    return btoa(String.fromCharCode.apply(null, new Uint8Array(arrayBuffer)));
}


