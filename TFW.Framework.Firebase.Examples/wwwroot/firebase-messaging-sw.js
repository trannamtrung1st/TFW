// Give the service worker access to Firebase Messaging.
// Note that you can only use Firebase Messaging here. Other Firebase libraries
// are not available in the service worker.
importScripts('https://www.gstatic.com/firebasejs/8.3.1/firebase-app.js');
importScripts('https://www.gstatic.com/firebasejs/8.3.1/firebase-messaging.js');

// Initialize the Firebase app in the service worker by passing in
// your app's Firebase config object.
// https://firebase.google.com/docs/web/setup#config-object
var firebaseConfig = {
    apiKey: "AIzaSyDRcNjr7HJaAdlc4F8FCri9Vcv8P5WMSg8",
    authDomain: "tfw-firebase.firebaseapp.com",
    projectId: "tfw-firebase",
    storageBucket: "tfw-firebase.appspot.com",
    messagingSenderId: "367167893610",
    appId: "1:367167893610:web:7ed726c2ff2004482f5a13",
    measurementId: "G-RDEGD30Q23"
};
firebase.initializeApp(firebaseConfig);
const messaging = firebase.messaging();

messaging.onBackgroundMessage((payload) => {
    console.log('[firebase-messaging-sw.js] Received background message ', payload);
    // Customize notification here
    const notificationTitle = 'Background Message Title';
    const notificationOptions = {
        body: 'Background Message body.',
        icon: '/firebase-logo.png'
    };

    self.registration.showNotification(notificationTitle,
        notificationOptions);
});