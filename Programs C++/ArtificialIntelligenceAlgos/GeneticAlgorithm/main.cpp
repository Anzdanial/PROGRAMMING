//#include <iostream>
//#include <SFML/Graphics.hpp>
//using namespace sf;
//using namespace std;
//
//const int GRID_SIZE = 40;
//const int CELL_SIZE = 20;
//
//class geneticAlgo{
//private:
//    RenderWindow window;
//    RectangleShape cell;
//public:
//    void drawGrid(){
//        while(window.isOpen()){
//            Event event;
//            while(window.pollEvent(event)){
//                if(event.type == sf::Event::Closed)
//                    window.close();
//            }
//            window.clear(Color::White);
//
//            //Draw grid cells
//            for (int i = 0; i < GRID_SIZE; ++i) {
//                for (int j = 0; j < GRID_SIZE; ++j) {
//                    cell.setSize(sf::Vector2f(CELL_SIZE, CELL_SIZE));
//                    cell.setPosition(i * CELL_SIZE, j * CELL_SIZE);
//                    //Note that the default color for the cell is set as White
//                    cell.setFillColor(sf::Color::White);
//                    //Setting the thickness and Color of the outline of each cell
//                    cell.setOutlineColor(Color::Black);
//                    cell.setOutlineThickness(3);
//                    window.draw(cell);
//                }
//            }
//            window.display();
//        }
//    }
//    void createMap(){
//        //This sets the window size with the specified height and width by using the pixels of each cell and multiplying by the total grid size
//        //VideoMode keyword specifies the type of videomode that can be used in fullscreen
//        window.create(VideoMode(GRID_SIZE * CELL_SIZE,GRID_SIZE*CELL_SIZE),"GRID MAP Vizualization");
//    }
//
//    void initializingPopulation(){
//        int startPointX, startPointY, endPointX, endPointY;
//        vector<int>roadBlocks;
//        startPointX = rand() % 40;
//        startPointY = rand() % 40;
//        endPointX = rand() % 40;
//        endPointY = rand() % 40;
//        while(startPointX == endPointX && endPointY == startPointY){
//            endPointX = rand() % 40;
//            endPointY = rand() % 40;
//        }
//        cout<<startPointX<<endl;
//        cout<<startPointY<<endl;
//        cout<<endPointX<<endl;
//        cout<<endPointY<<endl;
//
//    }
//    geneticAlgo(){
//        createMap();
//        drawGrid();
//        initializingPopulation();
//    }
//};
//
//class aStarSearch{
//private:
//    RenderWindow window;
//    RectangleShape cell;
//    Vector2f valueSet;
//public:
//    aStarSearch(){
//        createMap();
//    }
//
//    void searchImplementation(){
//
//    }
//
//    Vector2<float> getVectorSet(){
//        return cell.getSize();
//    }
//
//    //Sets the Value of a Cell to the given parameter.
//    void setGridVal(float X, float y){
//
//    }
//
//    void createMap(){
//        window.create(VideoMode(GRID_SIZE*CELL_SIZE, GRID_SIZE*CELL_SIZE),"GRID MAP VIZUALIZATION");
//
//        while(window.isOpen()) {
//            Event event;
//            while (window.pollEvent(event)) {
//                if (event.type == sf::Event::Closed)
//                    window.close();
//            }
//            window.clear(Color::White);
//
//            for (int i = 0; i < GRID_SIZE; i++) {
//                for (int j = 0; j < GRID_SIZE; j++) {
//                    cell.setSize(Vector2f(CELL_SIZE,CELL_SIZE));
//                    cell.setPosition(i * CELL_SIZE, j * CELL_SIZE);
//                    cell.setFillColor(Color::White);
//                    cell.setOutlineThickness(3);
//                    cell.setOutlineColor(Color::Black);
//                    window.draw(cell);
//                }
//            }
//            window.display();
//        }
//
//    }
//    void drawGrid(){
//
//    }
//};
//
//int main() {
////    geneticAlgo newMap;
//    aStarSearch search;
//    return 0;
//}



//#include <SFML/Graphics.hpp>
//
//using namespace sf;
//
//const int GRID_SIZE = 10; // Adjust this as needed
//const int CELL_SIZE = 40; // Adjust this as needed
//
//RenderWindow window;
//RectangleShape cell(Vector2f(CELL_SIZE, CELL_SIZE));
//int gridValues[GRID_SIZE][GRID_SIZE]; // 2D array to hold values for each cell
//
//void createMap() {
//    window.create(VideoMode(GRID_SIZE * CELL_SIZE, GRID_SIZE * CELL_SIZE), "GRID MAP VISUALIZATION");
//
//    // Initialize gridValues array with some sample values
//    for (int i = 0; i < GRID_SIZE; i++) {
//        for (int j = 0; j < GRID_SIZE; j++) {
//            gridValues[i][j] = rand() % 100; // Sample random values (0-99)
//        }
//    }
//
//    while (window.isOpen()) {
//        Event event;
//        while (window.pollEvent(event)) {
//            if (event.type == Event::Closed)
//                window.close();
//        }
//        window.clear(Color::White);
//
//        for (int i = 0; i < GRID_SIZE; i++) {
//            for (int j = 0; j < GRID_SIZE; j++) {
//                // Set color based on value (for example, a gradient from blue to red)
//                int value = gridValues[i][j];
//                Color cellColor = Color(255 * (1 - value / 100.0), 0, 255 * (value / 100.0)); // Example gradient
//
//                cell.setSize(Vector2f(CELL_SIZE, CELL_SIZE));
//                cell.setPosition(i * CELL_SIZE, j * CELL_SIZE);
//                cell.setFillColor(cellColor);
//                cell.setOutlineThickness(3);
//                cell.setOutlineColor(Color::Black);
//                window.draw(cell);
//            }
//        }
//        window.display();
//    }
//}
//
//int main() {
//    createMap();
//    return 0;
//}



#include <SFML/Graphics.hpp>
#include <iostream>
#include <vector>
#include <queue>
#include <cmath>
#include <functional>

using namespace sf;

const int GRID_SIZE = 10;
const int CELL_SIZE = 40;

RenderWindow window;
RectangleShape cell(Vector2f(CELL_SIZE, CELL_SIZE));
int gridValues[GRID_SIZE][GRID_SIZE]; // Values for each cell

struct Node {
    int x, y; // Coordinates of the cell
    int g, h; // Cost functions for A*
    Node* parent; // Parent node for path reconstruction

    Node(int _x, int _y) : x(_x), y(_y), g(0), h(0), parent(nullptr) {}

    int f() const {
        return g + h;
    }
};

std::vector<Node*> findPath(Node start, Node goal) {
    std::vector<Node*> path;
    std::priority_queue<Node*, std::vector<Node*>, std::function<bool(Node*, Node*)>> openList(
            [](Node* a, Node* b) {
                return a->f() > b->f();
            });

    openList.push(&start);

    while (!openList.empty()) {
        Node* current = openList.top();
        openList.pop();

        if (current->x == goal.x && current->y == goal.y) {
            // Reconstruct path
            while (current != nullptr) {
                path.push_back(current);
                current = current->parent;
            }
            break;
        }

        // Define possible movements (assuming 4-connectivity)
        int dx[] = {0, 1, 0, -1};
        int dy[] = {-1, 0, 1, 0};

        for (int i = 0; i < 4; ++i) {
            int newX = current->x + dx[i];
            int newY = current->y + dy[i];

            if (newX >= 0 && newX < GRID_SIZE && newY >= 0 && newY < GRID_SIZE && gridValues[newX][newY] == 0) {
                Node* neighbor = new Node(newX, newY);
                neighbor->parent = current;
                neighbor->g = current->g + 1; // Assuming uniform cost for each step
                neighbor->h = std::abs(newX - goal.x) + std::abs(newY - goal.y); // Manhattan distance as heuristic
                openList.push(neighbor);
            }
        }
    }

    return path;
}

void createMap() {
    window.create(VideoMode(GRID_SIZE * CELL_SIZE, GRID_SIZE * CELL_SIZE), "GRID MAP VISUALIZATION");

    // Sample values for grid
    for (int i = 0; i < GRID_SIZE; i++) {
        for (int j = 0; j < GRID_SIZE; j++) {
            gridValues[i][j] = 0; // Initialize with 0, assuming all cells are open
        }
    }

    Node start(0, 0); // Start node
    Node goal(GRID_SIZE - 1, GRID_SIZE - 1); // Goal node
    std::vector<Node*> path = findPath(start, goal);

    while (window.isOpen()) {
        Event event;
        while (window.pollEvent(event)) {
            if (event.type == Event::Closed)
                window.close();
        }
        window.clear(Color::White);

        // Draw grid
        for (int i = 0; i < GRID_SIZE; i++) {
            for (int j = 0; j < GRID_SIZE; j++) {
                cell.setSize(Vector2f(CELL_SIZE, CELL_SIZE));
                cell.setPosition(i * CELL_SIZE, j * CELL_SIZE);
                cell.setOutlineThickness(3);
                cell.setOutlineColor(Color::Black);
                if (gridValues[i][j] == 1) {
                    cell.setFillColor(Color::Black); // Obstacle cells
                } else {
                    cell.setFillColor(Color::White); // Empty cells
                }
                window.draw(cell);
            }
        }

        // Draw path
        for (Node* node : path) {
            int x = node->x;
            int y = node->y;
            cell.setSize(Vector2f(CELL_SIZE * 0.5, CELL_SIZE * 0.5)); // Adjust size for path nodes
            cell.setPosition(x * CELL_SIZE + CELL_SIZE * 0.25, y * CELL_SIZE + CELL_SIZE * 0.25); // Adjust position for path nodes
            cell.setFillColor(Color::Green); // Color for path nodes
            window.draw(cell);
        }

        window.display();
    }
}

int main() {
    createMap();
    return 0;
}
