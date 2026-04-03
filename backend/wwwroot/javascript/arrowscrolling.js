document.addEventListener("DOMContentLoaded", () => {

    document.querySelectorAll('.carousel-wrapper').forEach(wrapper => {
        const carousel  = wrapper.querySelector('.carousel');
        const btnLeft   = wrapper.querySelector('.arrow.left');
        const btnRight  = wrapper.querySelector('.arrow.right');

        // recalculated each click in case of resize
        const cardWidth = () => {
            const card = carousel.querySelector('.property-card');
            return card.offsetWidth + parseFloat(getComputedStyle(carousel).gap);
        };

        function updateButtons() {
            btnLeft.disabled  = carousel.scrollLeft <= 0;
            btnRight.disabled = carousel.scrollLeft >= carousel.scrollWidth - carousel.clientWidth - 1;
        }

        btnRight.addEventListener('click', () => {
            carousel.scrollBy({ left: cardWidth(), behavior: 'smooth' });
        });

        btnLeft.addEventListener('click', () => {
            carousel.scrollBy({ left: -cardWidth(), behavior: 'smooth' });
        });

        carousel.addEventListener('scroll', updateButtons);
        updateButtons();
    });
});