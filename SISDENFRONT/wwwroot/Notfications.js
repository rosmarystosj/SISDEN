window.requestNotificationPermission = function (title, message) {
    Notification.requestPermission().then(function (permission) {
        if (permission === 'granted') {
            new Notification(title, {
                body: message,
                icon: '/path/to/icon.png' // Puedes también pasar la ruta del icono como parámetro si lo deseas.
            });
        }
    });
};

window.registerServiceWorker = function () {
    if ('serviceWorker' in navigator && 'PushManager' in window) {
        navigator.serviceWorker.register('/sw.js').then(function (swReg) {
            console.log('Service Worker registrado', swReg);
        }).catch(function (error) {
            console.error('Error registrando el Service Worker', error);
        });
    }
};