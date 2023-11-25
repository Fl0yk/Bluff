const svg = document.getElementById("hexGrid");
const svg1 = document.getElementById("bet");
const screenWidth = window.innerWidth; // Ширина экрана

let rows = 19; // количество рядов
let cols = 21; // количество колонок

const hexWidth = screenWidth / cols; // ширина шестиугольника
// const hexWidth = svg.offsetWidth / cols;
const hexHeight = window.innerHeight / rows; // высота шестиугольника

cols = 11;
rows = 5;

let rowStart = 3
let colStart = 7

for (colStart; colStart < cols + 7; colStart++) {
    const x = rowStart % 2 === 0 ? colStart * hexWidth * 1.735 : colStart * hexWidth * 1.735 + (hexWidth * 0.86);
    const y = rowStart * hexHeight * 1.5;

    points = setPosition(x, y);
    createHexagon();
}

rowStart = 4
colStart = 17
for (rowStart; rowStart < rows + 4; rowStart++) {

    const x = rowStart % 2 === 0 ? colStart * hexWidth * 1.735 : colStart * hexWidth * 1.735 + (hexWidth * 0.86);
    const y = rowStart * hexHeight * 1.5;

    points = setPosition(x, y);
    createHexagon();
}

rowStart = 9
colStart = 17
for (colStart; colStart > cols - 5; colStart--) {
    const x = rowStart % 2 === 0 ? colStart * hexWidth * 1.735 : colStart * hexWidth * 1.735 + (hexWidth * 0.86);
    const y = rowStart * hexHeight * 1.5;

    points = setPosition(x, y);
    createHexagon();
}

rowStart = 4
colStart = 8
for (rowStart; rowStart < rows + 4; rowStart++) {
    const x = rowStart % 2 === 0 ? colStart * hexWidth * 1.735 : (colStart - 1) * hexWidth * 1.735 + (hexWidth * 0.86);
    const y = rowStart * hexHeight * 1.5;
    points = setPosition(x, y);
    createHexagon();
}


function setPosition(x, y) {
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

function createHexagon() {
    const hexagon = document.createElementNS("http://www.w3.org/2000/svg", "polygon");
    hexagon.setAttribute("id", "1")
    hexagon.setAttribute("points", points.join(" "));
    hexagon.setAttribute("style", "fill:#a3c8f0;stroke:#164980;stroke-width:4");

    svg1.appendChild(hexagon);
}


cols = 21;
rows = 13
for (let row = 0; row < rows; row++) {
    for (let col = 0; col < cols; col++) {
        const hexagon = document.createElementNS("http://www.w3.org/2000/svg", "polygon");

        const x = row % 2 === 0 ? col * hexWidth * 1.735 : col * hexWidth * 1.735 + (hexWidth * 0.86);
        const y = row * hexHeight * 1.5;

        points = setPosition(x, y);
        hexagon.setAttribute("points", points.join(" "));
        hexagon.setAttribute("style", "fill:#333333;stroke:black;stroke-width:4");

        svg.appendChild(hexagon);
    }
}

