self.addEventListener('push', function (event) {

    if (!(self.Notification && self.Notification.permission === 'granted')) {
        return;
    }

    var data = {};
    if (event.data) {
        data = event.data.json();
        console.log('DATA JSON' ,data);
    }

    console.log('Notification Received:');
    //console.log(data);

    var title = data.title;
    var message = data.message;
    var icon = "https://www.omegavigilancia.com/favicon.ico"; 
    var badge = "https://www.omegavigilancia.com/favicon.ico"; 

    event.waitUntil(self.registration.showNotification(title, {
        body: message,
        icon: icon,
        badge: badge,
        vibrate: [125, 75, 125, 275, 200, 275, 125, 75, 125, 275, 200, 600, 200, 600],
        openUrl:'/'
    }));

});

self.addEventListener('notificationclick', function (event) {
    console.log('AMC_notificationclick', event);

    const respuesta = clients.matchAll()
        .then(clientes => {
            let cliente = clientes.find(c => {
                return c.visibilityState === 'visible';
            });

            if (cliente !== undefined) {
                cliente.navigate('https://www.omegavigilancia.com/WorkerData');
                cliente.focus();
            } else {
                clients.openWindow('https://www.omegavigilancia.com/WorkerData');
            }

            event.notification.close();

        });

    event.waitUntil(respuesta);
});


