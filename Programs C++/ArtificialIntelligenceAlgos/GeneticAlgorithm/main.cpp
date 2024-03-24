#include <SFML/Graphics.hpp>
#include <iostream>
#include <vector>
#include <cmath>
#include <cstdlib> // For random number generation
#include <ctime>   // For seeding the random number generator

// Define grid parameters
const int GRID_SIZE = 40;
const int CELL_SIZE = 15;
const int WINDOW_SIZE = GRID_SIZE * CELL_SIZE;

// Define a structure to represent a cell/node in the grid
struct Cell {
    int x, y; // Coordinates of the cell
    bool obstacle; // Whether the cell is an obstacle or not
    bool start; // Whether the cell is the start point
    bool end;   // Whether the cell is the end point
    float g; // Cost from start to current cell
    float h; // Heuristic cost from current cell to destination
    float f; // Total cost (g + h)
    sf::RectangleShape rect; // Rectangle representing the cell in SFML

    // Constructor
    Cell(int _x, int _y) : x(_x), y(_y), obstacle(false), start(false), end(false), g(0), h(0), f(0) {
        rect.setSize(sf::Vector2f(CELL_SIZE, CELL_SIZE));
        rect.setPosition(x * CELL_SIZE, y * CELL_SIZE);
        rect.setOutlineThickness(1);
        rect.setOutlineColor(sf::Color::Black);
    }

    // Function to calculate heuristic cost (Manhattan distance)
    void calculateHeuristic(int destX, int destY) {
        h = std::abs(destX - x) + std::abs(destY - y);
    }
};

// Function to initialize the grid
std::vector<std::vector<Cell>> initializeGrid() {
    std::vector<std::vector<Cell>> grid(GRID_SIZE, std::vector<Cell>(GRID_SIZE, Cell(0, 0)));

    // Seed the random number generator
    std::srand(std::time(nullptr));

    // Set random obstacles
    for (int i = 0; i < GRID_SIZE; ++i) {
        for (int j = 0; j < GRID_SIZE; ++j) {
            grid[i][j] = Cell(j, i); // Corrected the order of x and y coordinates

            if (std::rand() % 100 < 20) { // 20% chance of setting an obstacle
                grid[i][j].obstacle = true;
                grid[i][j].rect.setFillColor(sf::Color::Black);
            }
        }
    }

    // Set random start and end points
    int startX = std::rand() % GRID_SIZE;
    int startY = std::rand() % GRID_SIZE;
    int endX, endY;
    do {
        endX = std::rand() % GRID_SIZE;
        endY = std::rand() % GRID_SIZE;
    } while (endX == startX && endY == startY); // Ensure end point is different from start point

    grid[startY][startX].start = true; // Corrected the order of x and y coordinates
    grid[startY][startX].rect.setFillColor(sf::Color::Green);

    grid[endY][endX].end = true; // Corrected the order of x and y coordinates
    grid[endY][endX].rect.setFillColor(sf::Color::Red);

    return grid;
}

// Function to draw the grid
void drawGrid(sf::RenderWindow& window, const std::vector<std::vector<Cell>>& grid) {
    for (const auto& row : grid) {
        for (const auto& cell : row) {
            window.draw(cell.rect);
        }
    }
}

int main() {
    // Create SFML window
    sf::RenderWindow window(sf::VideoMode(WINDOW_SIZE, WINDOW_SIZE), "A* Search");

    // Initialize the grid
    std::vector<std::vector<Cell>> grid = initializeGrid();

    // Main loop
    while (window.isOpen()) {
        // Handle events
        sf::Event event;
        while (window.pollEvent(event)) {
            if (event.type == sf::Event::Closed)
                window.close();
        }

        // Clear the window
        window.clear(sf::Color::White);

        // Draw the grid
        drawGrid(window, grid);

        // Display the window
        window.display();
    }

    return 0;
}