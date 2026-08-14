document.addEventListener("DOMContentLoaded", function () {
    const botones = document.querySelectorAll(".btn-confirmar-estado-envio");

    botones.forEach(function (boton) {
        boton.addEventListener("click", function () {
            const formulario = boton.closest("form");

            if (!formulario) {
                return;
            }

            Swal.fire({
                title: boton.dataset.titulo || "Confirmar acción",
                text: boton.dataset.mensaje || "¿Deseas continuar?",
                icon: boton.dataset.icono || "question",
                showCancelButton: true,
                confirmButtonText: boton.dataset.confirmar || "Sí, continuar",
                cancelButtonText: boton.dataset.cancelar || "Cancelar",
                confirmButtonColor: "#0F4C81",
                cancelButtonColor: "#6c757d",
                reverseButtons: true
            }).then(function (result) {
                if (result.isConfirmed) {
                    formulario.submit();
                }
            });
        });
    });
});