const token = document.getElementById('FinnhubToken').value;
const symbol = document.getElementById('StockSymbol').value;
const priceDisplayElement = document.getElementById('priceDisplay');
const priceElement = document.getElementById('Price');

const socket = new WebSocket(`wss://ws.finnhub.io?token=${token}`);

socket.addEventListener('open', function (event) {
    console.log('Conectado ao WebSocket da Finnhub');

    const subscribeMessage = { 'type': 'subscribe', 'symbol': symbol };
    socket.send(JSON.stringify(subscribeMessage));
});

socket.addEventListener('message', function (event) {
    const eventData = JSON.parse(event.data);

    if (eventData.type === 'trade') {
        const newPrice = eventData.data[eventData.data.length - 1].p;

        priceDisplayElement.innerText = "$ " + newPrice.toFixed(2);
        priceElement.value = newPrice;

        console.log('Preço atualizado para: ' + newPrice);
    }
});

socket.addEventListener('error', function (event) {
    console.error('Erro no WebSocket: ', event);
});

