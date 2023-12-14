function generateHexGrid() {
    const svg = document.getElementById("hexGrid");
    const screenWidth = window.innerWidth;
    const screenHeight = window.innerHeight;

    let rows = 19; // количество рядов
    let cols = 42; // количество колонок

    const hexWidth = screenWidth / cols;
    const hexHeight = screenHeight / rows;

    for (let row = 0; row < rows; row++) {
        for (let col = 0; col < cols; col++) {
            const hexagon = document.createElementNS("http://www.w3.org/2000/svg", "polygon");

            const x = row % 2 === 0 ? col * hexWidth * 1.735 : col * hexWidth * 1.735 + (hexWidth * 0.86);
            const y = row * hexHeight * 1.5;
            const points = setPosition(x, y, hexWidth, hexHeight);
            hexagon.setAttribute("points", points.join(" "));
            hexagon.setAttribute("style", "fill:#333333;stroke:black;stroke-width:4");

            svg.appendChild(hexagon);
        }
    }
}

function setPosition(x, y, hexWidth, hexHeight) {
    const points = [];
    for (let i = 0; i < 6; i++) {
        const angleDeg = 60 * i + 30; // Угол поворота
        const angleRad = (Math.PI / 180) * angleDeg;
        const pointX = x + hexWidth * Math.cos(angleRad);
        const pointY = y + hexHeight * Math.sin(angleRad);
        points.push(`${pointX},${pointY}`);
    }

    return points;
}

setInterval(() => {
    const svg = document.getElementById("hexGrid");
    svg.innerHTML = ""; // Очистить существующие гексагоны
    generateHexGrid(); // Перегенерировать сетку
}, 500); // Обновление каждые 3 секунды

// Генерация сетки при загрузке страницы
window.onload = function () {
    generateHexGrid(); // Генерация сетки при загрузке страницы

    window.addEventListener('resize', () => {
        const svg = document.getElementById("hexGrid");
        svg.innerHTML = ""; // Очистить существующие гексагоны
        generateHexGrid(); // Перегенерировать сетку при изменении размеров окна
    });
};