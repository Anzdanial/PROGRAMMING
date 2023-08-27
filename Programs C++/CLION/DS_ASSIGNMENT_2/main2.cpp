#include <iostream>
#include <vector>
using namespace std;

vector<vector<int>> m_rows;
vector<vector<int>> m_cols;
vector<vector<int>> m_boxes;
int N;

bool canPlaceNumber(int n, int row, int col) {
	int box_idx = (row / 3) * 3 + (col / 3);
	return m_rows[row][n] == 0 && m_cols[col][n] == 0 && m_boxes[box_idx][n] == 0;
}

void placeNumber(int n, int row, int col) {
	int box_idx = (row / 3) * 3 + (col / 3);
	m_rows[row][n]++;
	m_cols[col][n]++;
	m_boxes[box_idx][n]++;
}

void removeNumber(int n, int row, int col) {
	int box_idx = (row / 3) * 3 + (col / 3);
	m_rows[row][n]--;
	m_cols[col][n]--;
	m_boxes[box_idx][n]--;
}

bool doSolveSudoku(vector<vector<char>>& board) {
	for (int row = 0; row < N; ++row) {
		for (int col = 0; col < N; ++col) {
			if (board[row][col] == '.') {
				for (int n = 1; n <= 9; ++n) {
					if (canPlaceNumber(n, row, col)) {
						placeNumber(n, row, col);
						board[row][col] = '0' + n;

						if (doSolveSudoku(board)) {
							return true;
						} else {
							removeNumber(n, row, col);
							board[row][col] = '.';
						}
					}
				}
				return false;
			}
		}
	}
	return true;
}

void solveSudoku(vector<vector<char>>& board) {
	N = board.size();
	m_rows = vector<vector<int>>(N, vector<int>(N + 1, 0));
	m_cols = vector<vector<int>>(N, vector<int>(N + 1, 0));
	m_boxes = vector<vector<int>>(N, vector<int>(N + 1, 0));

// Intialize the m_rows, m_cols and m_boxes arrays
	for (int row = 0; row < N; ++row) {
		for (int col = 0; col < N; ++col) {
			if (board[row][col] != '.') {
				placeNumber(board[row][col] - '0', row, col);
			}
		}
	}
	doSolveSudoku(board);
}

int main(){
	return 0;
}