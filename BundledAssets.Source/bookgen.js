function darkModeToggle() {
    document.body.classList.toggle('dark-mode');
}

function scrollUp(speed = 30) {
    const scrollStep = () => {
        const currentScroll = window.pageYOffset;

        if (currentScroll > 0) {
            window.scrollBy(0, -speed);
            requestAnimationFrame(scrollStep);
        }
    };

    requestAnimationFrame(scrollStep);
}