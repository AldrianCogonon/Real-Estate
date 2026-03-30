document.addEventListener("DOMContentLoaded", () => {
    const slider = document.getElementById('slider');
    const dotsContainer = document.getElementById('dots');
    const cards = document.querySelectorAll('.property-card');
    const cardWidth = cards[0].offsetWidth + 24; // card + gap

    // CREATE DOTS
    cards.forEach((_, index) => {
        const dot = document.createElement('span');
        if (index === 0) dot.classList.add('active');
        dot.addEventListener('click', () => {
            slider.scrollTo({ left: cards[index].offsetLeft - 40, behavior: 'smooth' });
            // subtract 40 to account for the container's left padding
        });
        dotsContainer.appendChild(dot);
    });

    const dots = dotsContainer.querySelectorAll('span');

    // UPDATE ACTIVE DOT
    slider.addEventListener('scroll', () => {
        let closestIndex = 0;
        let closestDist = Infinity;
        cards.forEach((card, index) => {
            const dist = Math.abs(card.offsetLeft - 40 - slider.scrollLeft);
            if (dist < closestDist) { closestDist = dist; closestIndex = index; }
        });
        dots.forEach(d => d.classList.remove('active'));
        dots[closestIndex].classList.add('active');
    });

    // ARROWS
    document.querySelector('.arrow.right').addEventListener('click', () => {
        slider.scrollBy({ left: cardWidth, behavior: 'smooth' });
    });
    document.querySelector('.arrow.left').addEventListener('click', () => {
        slider.scrollBy({ left: -cardWidth, behavior: 'smooth' });
    });
});