#include <iostream>
#include <queue>

void dfs(int **matrix, int n, int node, bool* visited){
	visited[node] = true;
	std::cout << node << " ";
	for (int col = 0; col < n; col ++){
		if (matrix[node][col] != 0 && visited[col] == false){
			dfs(matrix, n, col, visited);
		}
	}
}

void dfs(int **matrix, int n){
	bool *visited = new bool[n];
	for (int i = 0; i < n; i ++)
		visited[i] = false;
	dfs(matrix, n, 0, visited);
	delete[] visited;
}

// ------------------------------------------------------------------------

void bfs(int **matrix, int n){
	bool *visited = new bool[n];
	for (int i = 0; i < n; i ++)
		visited[i] = false;

	std::queue<int> q;
	q.push(0);
	visited[0] = true;
	while (!q.empty()){
		int node = q.front();
		q.pop();
		std::cout << node << " ";
		for (int col = 0; col < n; col ++){
			if (matrix[node][col] != 0 && visited[col] == false){
				q.push(col);
				visited[col] = true;
			}
		}
	}
}

int main(){
	int **matrix = new int*[4];
	for (int i = 0; i < 4; i ++){
		matrix[i] = new int[4];
		for (int j = 0; j < 4; j ++)
			std::cin >> matrix[i][j];
	}
	for (int i = 0; i < 4; i ++){
		for (int j = 0; j < 4; j ++)
			std::cout << matrix[i][j];
		std::cout << "\n";
	}
	std::cout << "dfs: ";
	dfs(matrix, 4);
	std::cout << "\n\nbfs: ";
	bfs(matrix, 4);
	std::cout << "\n";
	return 0;
}
