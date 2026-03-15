document.addEventListener("DOMContentLoaded", function () {

    const messageInput = document.getElementById("message");
    const chat = document.getElementById("chat");
    const sendBtn = document.getElementById("send-btn");

    messageInput.addEventListener("keydown", function (event) {
        if (event.key === "Enter") {
            event.preventDefault();
            sendMessage();
        }
    });

    sendBtn.addEventListener("click", sendMessage);

    async function sendMessage() {

        let message = messageInput.value.trim();
        if (!message) return;

        message = message.charAt(0).toUpperCase() + message.slice(1).toLowerCase();

        const userDiv = document.createElement("div");
        userDiv.classList.add("message", "user");
        userDiv.innerHTML = `<div class="bubble">${message}</div>`;
        chat.appendChild(userDiv);

        messageInput.value = "";
        chat.scrollTop = chat.scrollHeight;

        messageInput.disabled = true;
        sendBtn.disabled = true;

        const typingDiv = document.createElement("div");
        typingDiv.classList.add("message", "bot");
        typingDiv.innerHTML = `
            <img src="images/logo.png" alt="Agent" class="avatar">
            <div class="bubble typing">Typing</div>
        `;
        chat.appendChild(typingDiv);
        chat.scrollTop = chat.scrollHeight;

        const startTime = Date.now();

        try {

            const response = await fetch("/api/chat", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ message: message })
            });

            if (!response.ok) throw new Error("Network error");

            const data = await response.json();

            const elapsed = Date.now() - startTime;
            if (elapsed < 800) {
                await new Promise(resolve => setTimeout(resolve, 800 - elapsed));
            }

            chat.removeChild(typingDiv);

            const botDiv = document.createElement("div");
            botDiv.classList.add("message", "bot");
            botDiv.innerHTML = `
                <img src="images/logo.png" alt="Agent" class="avatar">
                <div class="bubble">${data.reply}</div>
            `;
            chat.appendChild(botDiv);

            chat.scrollTop = chat.scrollHeight;

        } catch (error) {

            chat.removeChild(typingDiv);

            const errorDiv = document.createElement("div");
            errorDiv.classList.add("message", "bot");
            errorDiv.innerHTML = `
                <div class="bubble">Error connecting to server</div>
            `;
            chat.appendChild(errorDiv);

            console.error(error);
        }

        messageInput.disabled = false;
        sendBtn.disabled = false;
        messageInput.focus();
    }
});

const hamburger = document.getElementById('hamburger');
const sidebar = document.querySelector('.sidebar');
const closeBtn = document.getElementById('closeBtn');

hamburger.addEventListener('click', () => {
    sidebar.classList.add('active');
});

closeBtn.addEventListener('click', () => {
    sidebar.classList.remove('active');
});