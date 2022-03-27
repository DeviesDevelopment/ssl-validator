export function setBackgroundColour(threatLevelClass) {
	document.body.className = threatLevelClass;
}

export function toggleAddDomainModal() {
	let modal = window.addDomainModal;
	if (modal === undefined) {
		window.addDomainModal = new bootstrap.Modal(document.getElementById("addDomainModalToggle"));
		modal = window.addDomainModal;
	}
	modal.toggle();
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