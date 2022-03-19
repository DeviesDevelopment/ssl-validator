export function setBackgroundColour(threatLevelClass) {
	document.body.className = threatLevelClass;
}

document.addEventListener("keypress", (e) => {
	if (e.key === 'Enter') {
		let modal = window.domainModal;
		if (modal === undefined) {
			window.domainModal = new bootstrap.Modal(document.getElementById("domainModalToggle"));
			modal = window.domainModal;
		}
		modal.toggle();
	}
})