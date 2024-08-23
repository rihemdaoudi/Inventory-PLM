//const connection = new signalR.HubConnectionBuilder()
//    .withUrl("/chathub")
//    .configureLogging(signalR.LogLevel.Information)
//    .build();

//var Start = async () => {
//    try {
//        await connection.start();
//        console.log("SignalR Connected.");
//    } catch (err) {
//        console.log(err);
//        setTimeout(Start, 5000);
//    }
//};

//// Start the connection.
//Start();

//connection.onclose(async () => {
//    await Start();
//});

//var SendMessage = async (message) => {
//    await connection.invoke("SendMessage", message);
//}

//document.getElementById('SendMessage').addEventListener('click', async () => {
//    var message = document.getElementById('Message');

//    if (message && message.value) {
//        await SendMessage(message.value);
//        message.value = '';
//    }
//});

//connection.on("ReceiveMessage", (message) => {
//    var messages = document.getElementById('Messages');

//    if (messages) {
//        var bulletPoint = document.createElement('li');
//        bulletPoint.innerText = message;

//        messages.appendChild(bulletPoint);
//    }
//});