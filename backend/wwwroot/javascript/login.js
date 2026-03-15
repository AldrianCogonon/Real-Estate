const form = document.getElementById('loginForm');
const message = document.getElementById('message');

form.addEventListener('submit', async (e) => {
    e.preventDefault();

    const username = document.getElementById('username').value.trim();
    const password = document.getElementById('password').value.trim();

    if (!username || !password) {
        message.style.color = 'red';
        message.textContent = "Please enter username and password";
        return;
    }

    try {
        const res = await fetch('/api/login', { 
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ username, password })
        });

        const data = await res.json();

        if (data.success) {
            message.style.color = 'green';
            message.textContent = data.message;

            setTimeout(() => {
                window.location.href = data.redirect; 
            }, 1000);
        } else {
            message.style.color = 'red';
            message.textContent = data.message;
        }

    } catch (err) {
        message.style.color = 'red';
        message.textContent = 'Server error. Try again later';
    }
});

const password = document.getElementById("password");
const toggle = document.getElementById("togglePassword");

toggle.addEventListener("click", () => {
    if(password.type === "password"){
        password.type = "text";
        toggle.innerHTML = '<ion-icon name="eye-off-outline"></ion-icon>';
    } else {
        password.type = "password";
        toggle.innerHTML = '<ion-icon name="eye-outline"></ion-icon>';
    }
});

const hamburger = document.getElementById("hamburger");
const navMenu = document.getElementById("navMenu");

hamburger.addEventListener("click", () => {
    navMenu.classList.toggle("active");
});

