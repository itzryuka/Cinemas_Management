// clear form for all auth modals
document.addEventListener("DOMContentLoaded", function () {
	const modals = document.querySelectorAll(".auth-modal");
	modals.forEach(function (modal) {
		modal.addEventListener("hidden.bs.modal", function () {
			// clear all inputs
			modal.querySelectorAll("input").forEach(function (input) {
				if (input.type === "checkbox") {
					input.checked = false;
				}
				else {
					input.value = "";
				}
			});
			// reset all password fields
			modal.querySelectorAll("input[type='text']").forEach(function (input) {
				if (input.id.toLowerCase().includes("password")) {
					input.type = "password";
				}
			});
			// reset all eye icons
			modal.querySelectorAll("i").forEach(function (icon) {
				if (icon.classList.contains("bi-eye-slash")) {
					icon.classList.remove("bi-eye-slash");
					icon.classList.add("bi-eye");
				}
			modal.querySelector("form")?.reset();
			});
		});
	});
});
document.addEventListener("hidden.bs.modal", function () {
	if (document.querySelectorAll(".modal.show").length === 0) {
		document.body.classList.remove("modal-open");
		document.querySelectorAll(".modal-backdrop").forEach(function (el) {
			el.remove();
		});
	}
});

function switchModal(closeId, openId) {
	var closeModal = bootstrap.Modal.getInstance(document.getElementById(closeId));
	closeModal.hide();
	setTimeout(function () {
		var openModal = new bootstrap.Modal(document.getElementById(openId));
		openModal.show();
	}, 300);
}


// toggle password
function togglePassword() {
	const password = document.getElementById("password");
	const icon = document.getElementById("eyeIcon");
	if (!password) return;
	if (password.type === "password") {
		password.type = "text";
		icon.classList.remove("bi-eye");
		icon.classList.add("bi-eye-slash");
	}
	else {
		password.type = "password";
		icon.classList.remove("bi-eye-slash");
		icon.classList.add("bi-eye");
	}
}
function toggleRegPassword() {

	const password = document.getElementById("regPassword");
	const icon = document.getElementById("eyeIcon");
	if (password.type === "password") {
		password.type = "text";
		icon.classList.remove("bi-eye");
		icon.classList.add("bi-eye-slash");
	}
	else {
		password.type = "password";
		icon.classList.remove("bi-eye");
		icon.classList.add("bi-eye-slash");
	}
}
function toggleRegConfirmPassword() {
	const password = document.getElementById("regConfirmPassword");
	const icon = document.getElementById("eyeIcon");
	if (password.type === "password") {
		password.type = "text";
		icon.classList.remove("bi-eye");
		icon.classList.add("bi-eye-slash");
	}
	else {
		password.type = "password";
		icon.classList.remove("bi-eye");
		icon.classList.add("bi-eye-slash");
	}
}