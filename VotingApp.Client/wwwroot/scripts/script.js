SetFocusToId = (id) => {
    const element = document.getElementById(id);
    element.focus();
};

CopyUrl = () => {
    navigator.clipboard.writeText(window.location.href).then(function () {
        alert("Copied url to clipboard!");
    })
        .catch(function (error) {
            alert(error);
    });
};