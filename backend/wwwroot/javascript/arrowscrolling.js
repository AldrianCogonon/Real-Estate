document.addEventListener("DOMContentLoaded", () => {
    const carousel = document.getElementById("carousel");
    const dotsContainer = document.getElementById("dots");

    const cards = carousel.querySelectorAll(".property-card");
    const visibleCards = 3;
    const totalSlides = Math.ceil(cards.length / visibleCards);

    let currentIndex = 0;

    // CREATE DOTS
    for (let i = 0; i < totalSlides; i++) {
        const dot = document.createElement("span");
        if (i === 0) dot.classList.add("active");

        dot.addEventListener("click", () => {
            currentIndex = i;
            updateCarousel();
        });

        dotsContainer.appendChild(dot);
    }

    function updateCarousel() {
        const cardWidth = cards[0].offsetWidth + 20;
        carousel.scrollTo({
            left: cardWidth * visibleCards * currentIndex,
            behavior: "smooth"
        });

        document.querySelectorAll(".carousel-dots span")
            .forEach((dot, i) => {
                dot.classList.toggle("active", i === currentIndex);
            });
    }

    // BUTTON CONTROL
    function scrollCarousel(direction) {
        currentIndex += direction;

        if (currentIndex < 0) currentIndex = 0;
        if (currentIndex >= totalSlides) currentIndex = totalSlides - 1;

        updateCarousel();
    }
});