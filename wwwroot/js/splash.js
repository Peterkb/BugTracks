const header = document.querySelector("nav");
const sectionOne = document.querySelector(".home-intro");
const logo = document.querySelector(".logoImg");

const sectionOneOptions = {
    rootMargin: "-500px 0px 0px 0px"
};

const sectionOneObserver = new IntersectionObserver(function(
    entries,
    sectionOneObserver
) {
    entries.forEach(entry => {
        if (!entry.isIntersecting) {
            header.classList.remove("navbar-dark");
            header.classList.add("navbar-light", "bg-light", "shadow");
            logo.src = "/img/splash/logoDark.svg";
        } else {
            header.classList.remove("navbar-light", "bg-light", "shadow");
            header.classList.add("navbar-dark");
            logo.src = "/img/splash/logoLight.svg";
        }
    });
},
sectionOneOptions);

sectionOneObserver.observe(sectionOne);

