//const fon = document.getElementsByClassName('fon');
const ruls = document.getElementById('ruls');
const servers = document.getElementById('servers');

ruls.addEventListener('click', () => {
    ruls.classList.toggle('highlight');
    servers.classList.remove('highlight');
});
servers.addEventListener('click', () => {
    servers.classList.toggle('highlight');
    ruls.classList.remove('highlight');
});