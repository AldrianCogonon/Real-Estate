document.addEventListener("DOMContentLoaded", () => {
    const password = document.getElementById("password");
    const toggle = document.getElementById("togglePassword");
    const icon = toggle.querySelector("ion-icon");

    toggle.addEventListener("click", () => {
        if (password.type === "password") {
            password.type = "text";
            icon.setAttribute("name", "eye-off-outline");
        } else {
            password.type = "password";
            icon.setAttribute("name", "eye-outline");
        }
    });

    const hamburger = document.getElementById("hamburger");
    const navMenu = document.getElementById("nav-menu");

    hamburger.addEventListener("click", () => {
        navMenu.classList.toggle("show");
    });

    const form = document.getElementById("registerForm");

    form.addEventListener("submit", async (e) => {
        e.preventDefault();

        const username = document.getElementById("username").value.trim();
        const email = document.getElementById("email").value.trim();
        const passwordValue = password.value.trim();

        if (!username || !email || !passwordValue) {
            alert("Please fill all fields");
            return;
        }

        try {
            const res = await fetch("/api/register", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({
                    username,
                    email,
                    password: passwordValue
                })
            });

            const data = await res.json();

            const messageBox = document.getElementById("formMessage");

            messageBox.style.display = "block";
            messageBox.textContent = data.message;

            if (data.success) {
                messageBox.className = "form-message success";

            form.reset();

            setTimeout(() => {
                window.location.href = "login.html";
            }, 1500);
            } else {
                messageBox.className = "form-message error";
            }
        } catch (err) {
            const messageBox = document.getElementById("formMessage");
            messageBox.style.display = "block";
            messageBox.className = "form-message error";
            messageBox.textContent = "Server error. Try again later.";
            console.error(err);
        }
    });
});