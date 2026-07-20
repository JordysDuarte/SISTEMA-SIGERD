/* ================================
   MOSTRAR / OCULTAR CONTRASEÑA
================================ */

document.addEventListener("DOMContentLoaded", function () {
    const botones = document.querySelectorAll("[data-toggle-password]");

    botones.forEach(function (boton) {
        boton.addEventListener("click", function () {
            const contenedor = boton.closest(".password-field");

            if (!contenedor) {
                return;
            }

            const input = contenedor.querySelector("input");
            const icono = boton.querySelector("i");


            if (!input) {
                return;
            }

            const estaOculta = input.getAttribute("type") === "password";

            input.setAttribute("type", estaOculta ? "text" : "password");

            boton.setAttribute(
                "title",
                estaOculta ? "Ocultar contraseña" : "Mostrar contraseña"
            );


            boton.setAttribute(
                "aria-label",
                estaOculta ? "Ocultar contraseña" : "Mostrar contraseña"
            );

            if (icono) {
                icono.classList.toggle("bi-eye", !estaOculta);
                icono.classList.toggle("bi-eye-slash", estaOculta);
            }

            input.focus();
        });
    });
});