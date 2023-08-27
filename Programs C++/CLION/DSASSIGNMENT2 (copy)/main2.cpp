#include <iostream>
#include <cstdio>
#include <cstring>
#include <cstdlib>
#include<fstream>
using namespace std;
#define EmptyLocation '.'
#define N 9

//check if given value is used in given row
bool CheckIfUsedInRow(char grid[N][N], int row, char num);

//check if given value is used in given col
bool CheckIfUsedInColn(char grid[N][N], int col, char num);

//check if given value is used in said box
bool checkIfUsedInBox(char grid[N][N], int row, int col, char num);

void printGrid(char grid[N][N]);

//checks if there is any empty space in grid if not then returns false and if yes then the row and col are edited in function
bool FindEmptySpace(char grid[N][N], int& row, int& col);

//uses above functions to check whether we can put the number in the said box
bool CheckIfWeCanAssign(char grid[N][N], int row, int col, char num);

//main solving function (backtracking)
bool StartSolving(char grid[N][N]);

//input from file
void inputGridFile(char grid[N][N]);

int main() {

    char grid[N][N];

    //{ {'5', '3', '.', '.', '7', '.', '.', '.', '.'},
    //    { '6', '.', '.', '1', '9', '5', '.', '.', '.' },
    //    { '.', '9', '8', '.', '.', '.', '.', '6', '.' },
    //    { '8', '.', '.', '.', '6', '.', '.', '.', '3' },
    //    { '4', '.', '.', '8', '.', '3', '.', '.', '1' },
    //    { '7', '.', '.', '.', '2', '.', '.', '.', '6' },
    //    { '.', '6', '.', '.', '.', '.', '2', '8', '.' },
    //    { '.', '.', '.', '4', '1', '9', '.', '.', '5' },
    //    { '.', '.', '.', '.', '8', '.', '.', '7', '9' } };

    inputGridFile(grid);

    printGrid(grid);
    cout << endl << endl << endl;
    StartSolving(grid);
    printGrid(grid);

}

bool CheckIfUsedInColn(char grid[N][N], int col, char num) {

    for (int i = 0; i < N; i++) {
        if (grid[i][col] == num) {
            return true;
        }
    }

    return false;
}

bool CheckIfUsedInRow(char grid[N][N], int row, char num) {

    for (int i = 0; i < N; i++) {
        if (grid[row][i] == num) {
            return true;
        }
    }

    return false;
}

bool checkIfUsedInBox(char grid[N][N], int row, int col, char num) {

    for (int i = 0; i < 3; i++) {
        for (int j = 0; j < 3; j++) {
            if (grid[i + row][j + col] == num)
                return true;
        }
    }

    return false;
}

void printGrid(char grid[N][N]) {

    for (int i = 0; i < N; i++) {
        for (int j = 0; j < N; j++) {
            cout << grid[i][j] << " ";
        }
        cout << endl;
    }
}

bool FindEmptySpace(char grid[N][N], int& row, int& col) {

    for (row = 0; row < N; row++) {
        for (col = 0; col < N; col++) {
            if (grid[row][col] == EmptyLocation)
                return true;
        }
    }

    return false;
}

bool CheckIfWeCanAssign(char grid[N][N], int row, int col, char num) {

    return !checkIfUsedInBox(grid, row - (row % 3), col - (col % 3), num) && !CheckIfUsedInColn(grid, col, num) && !CheckIfUsedInRow(grid, row, num);
    // modulus is used for example 6 - (6 % 3) = 6 and for 5 is 5 - (5 % 3) = 3 i.e 3 is starting index of that box
}

bool StartSolving(char grid[N][N]) {

    int row = 0, col = 0;
    if (!FindEmptySpace(grid, row, col)) {
        return true;
    }

    //using char instead of int because using same variable to call function with char arguements
    for (char num = '1'; num <= '9'; num++) {
        if (CheckIfWeCanAssign(grid, row, col, num)) { //check the empty spaces if the number can be assigned there
            grid[row][col] = num;
            if (StartSolving(grid)) {
                return true;
            }
            grid[row][col] = EmptyLocation;
        }
    }
    return false;
}

void inputGridFile(char grid[N][N]) {

    fstream fin;
    fin.open("input.txt", ios::in);
    for (int i = 0; i < N; i++) {
        for (int j = 0; j < N; j++) {
            fin >> grid[i][j];
        }
    }
    fin.close();
}