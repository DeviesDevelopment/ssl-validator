export function showPrompt(message) {
	return prompt(message, 'Type anything here');
}

document.addEventListener("keypress", (e) => {
	if (e.key === 'Enter') {
		var myModal = new bootstrap.Modal(document.getElementById("exampleModal"));
		myModal.toggle();
	}
})