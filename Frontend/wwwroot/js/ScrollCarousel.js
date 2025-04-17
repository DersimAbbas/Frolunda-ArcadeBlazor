window.scrollCarousel = (carouselElement, direction) => {
    const scrollAmount = 300;
    if (carouselElement && carouselElement.scrollBy) {
        carouselElement.scrollBy({ left: direction * scrollAmount, behavior: 'smooth' });
    }
};

window.autoScrollCarousel = (carouselElement) => {
    if (!carouselElement) return;

    const scrollAmount = 300;
    const maxScrollLeft = carouselElement.scrollWidth - carouselElement.clientWidth;

    if (carouselElement.scrollLeft + 5 >= maxScrollLeft) {
        carouselElement.scrollTo({ left: 0, behavior: 'smooth' });
    } else {
        carouselElement.scrollBy({ left: scrollAmount, behavior: 'smooth' });
    }
};