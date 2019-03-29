
importScripts("/Scripts/sw-utils.js");

const STATIC_CACHE = "static-v1";
const DYNAMIC_CACHE = "dynamic-v1";
const INMUTABLE_CACHE = "inmutable-v1";


const APP_SHELL = [
    "/",
    "Generals",
    "WorkerData",
    "/Content/css/estilosPWA.css",
    "/favicon.ico",
    "/Scripts/main.js",
    "/Scripts/indexPWA.js",
    "/Content/Images/OmegaNet_2.png",
    "/Content/Images/fondo.png",
    "/Scripts/sw-utils.js"
];

const APP_SHELL_INMUTABLE = [
    "/Scripts/jquery-ui-1.12.1.js",
    "/Scripts/umd/popper.min.js",
    "/Scripts/moment.js",
    "/Scripts/bootstrap.js",
    "/Scripts/respond.js",
    "/Scripts/jquery-3.3.1.js",
    "/fontawesome/css/all.css",
    "/Content/bootstrap.css",
    "/Content/bootstrap.min.css",
    "/Content/css/estilos.css",
    "/fontawesome/css/all.css",
];


self.addEventListener("install", e => {

    const cacheStatic = caches.open(STATIC_CACHE).then(cache => {
        cache.addAll(APP_SHELL);
    });

    const cacheInmutable = caches.open(INMUTABLE_CACHE).then(cache => {
        cache.addAll(APP_SHELL_INMUTABLE);
    });

    e.waitUntil( Promise.all([cacheStatic,cacheInmutable]) );
});


self.addEventListener("activate", e => {

    const respuesta = caches.keys().then(keys => {
        keys.forEach(key => {
            if (key !== STATIC_CACHE && key.includes("static")) {
                return caches.delete(key);
            }
        });
    });

    e.waitUntil(respuesta);

});

self.addEventListener('push', function (event) {
    console.log('[Service Worker] Push Received.');
    console.log(`[Service Worker] Push had this data: "${event.data.text()}"`);

    const title = 'Push Codelab';
    const options = {
        body: 'Yay it works.',
        icon: 'images/icon.png',
        badge: 'images/badge.png'
    };

    event.waitUntil(self.registration.showNotification(title, options));
});


//self.addEventListener("fetch", e => {

//    //console.log(e.request);
//  const respuesta =  caches.match(e.request).then(res => {

//            if (res) {
//                return res;
//            } else {
//                return fetch(e.request).then(newRes => {

//                    actualizaCacheDinamico(DYNAMIC_CACHE, e.request, newRes);

//                });
//            }

//  });

//    e.respondWith(respuesta);
//});








