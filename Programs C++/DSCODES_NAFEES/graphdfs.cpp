#include <iostream>
#include <fstream>
#include <string>
#include <vector>
#include "hashmap.h"

class Graph{
private:
	int **_matrix;
	int _vertices;
	bool _directed;

	std::vector<int> _dfsShortestPath(int src, int dst, bool *visited) const {
		std::vector<int> ret;
		ret.push_back(src);
		if (src == dst)
			return ret;
		std::vector<int> path;
		for (int i = 0; i < _vertices; i ++){
			if (!_matrix[src][i] || visited[i])
				continue;
			visited[i] = true;
			// recursive calls...
			std::vector<int> temPath = _dfsShortestPath(i, dst, visited);
			if (temPath.size() <= 1)
				continue;
			if (path.size() == 0 || path.size() > temPath.size())
				path = temPath;
		}
		if (path.size() == 0)
			return ret;
		for (int i = 0; i < path.size(); i ++)
			ret.push_back(path[i]);
		return ret;
	}
	
	/// TODO incomplete
	std::vector<int> _bfsShortestPath(int src, int dst, bool *visited) const {
		std::vector<int> ret;
		ret.push_back(src);
		if (src == dst)
			return ret;
		std::vector<int> path;
		for (int i = 0; i < _vertices; i ++){
			if (!_matrix[src][i] || visited[i])
				continue;
			visited[i] = true;
			std::vector<int> temPath =
				_dfsShortestPath(i, dst, visited);
			if (temPath.size() <= 1)
				continue;
			if (path.size() == 0 ||
					path.size() > temPath.size())
				path = temPath;
		}
		if (path.size() == 0)
			return ret;
		for (int i = 0; i < path.size(); i ++)
			ret.push_back(path[i]);
		return ret;
	}

public:
	Graph(int vertices, bool directed){
		_vertices = vertices;
		_directed = directed;
		_matrix = new int*[_vertices];
		for (int i = 0; i < _vertices; i ++){
			_matrix[i] = new int[_vertices];
			for (int j = 0; j < _vertices; j ++)
				_matrix[i][j] = 0;
		}
	}

	Graph(const Graph &from){
		_vertices = from._vertices;
		_directed = from._directed;
		_matrix = new int*[_vertices];
		for (int i = 0; i < _vertices; i ++){
			_matrix[i] = new int[_vertices];
			for (int j = 0; j < _vertices; j ++)
				_matrix[i][j]  = from._matrix[i][j];
		}
	}

	Graph (std::string filename){
		std::ifstream file(filename);
		if (!file)
			throw "failed to open file";
		file >> _vertices;
		file >> _directed;
		_matrix = new int*[_vertices];
		for (int i = 0; i < _vertices; i ++){
			_matrix[i] = new int[_vertices];
			for (int j = 0; j < _vertices; j ++)
				file >> _matrix[i][j];
		}
	}

	bool addEdge(int x, int y){
		if (x < 0 || y < 0 || x >= _vertices || y >= _vertices)
			return false;
		if (_directed){
			_matrix[x][y] = 1;
		}else{
			_matrix[x][y] = 1;
			_matrix[y][x] = 1;
		}
		return true;
	}

	bool removeEdge(int x, int y){
		if (x < 0 || y < 0 || x >= _vertices || y >= _vertices)
			return false;
		if (_directed){
			_matrix[x][y] = 0;
		}else{
			_matrix[x][y] = 0;
			_matrix[y][x] = 0;
		}
		return true;
	}

	bool isConnected(int x, int y) const {
		if (x < 0 || y < 0 || x >= _vertices || y >= _vertices)
			return false;
		return _matrix[x][y];
	}

	void printPathBFS(int src, int dest) const {
		bool *visited = new bool[_vertices];
		for (int i = 0; i < _vertices; i ++)
			visited[i] = false;
		std::vector<int> path = _bfsShortestPath(src, dest, visited);
		if (path.size() <= 1){
			std::cout << "No Path Exists\n";
		}else{
			for (int i = 0; i < path.size(); i ++)
				std::cout << path[i] << " ";
			std::cout << "\n";
		}
		delete[] visited;
	}

	void printPathDFS(int src, int dest) const {
		bool *visited = new bool[_vertices];
		for (int i = 0; i < _vertices; i ++)
			visited[i] = false;
		std::vector<int> path = _dfsShortestPath(src, dest, visited);
		if (path.size() <= 1){
			std::cout << "No Path Exists\n";
		}else{
			for (int i = 0; i < path.size(); i ++)
				std::cout << path[i] << " ";
			std::cout << "\n";
		}
		delete[] visited;
	}

	bool areConnected(int src, int dest) const {
		bool *visited = new bool[_vertices];
		for (int i = 0; i < _vertices; i ++)
			visited[i] = false;
		std::vector<int> path = _dfsShortestPath(src, dest, visited);
		bool ret = path.size() > 1;
		delete[] visited;
		return ret;
	}

	int getInDegree(int v) const {
		if (v < 0 || v > _vertices)
			return 0;
		int ret = 0;
		for (int i = 0; i < _vertices; i ++)
			ret += _matrix[i][v] != 0;
		return ret;
	}

	int getOutDegree(int v) const {
		if (v < 0 || v > _vertices)
			return 0;
		int ret = 0;
		for (int i = 0; i < _vertices; i ++)
			ret += _matrix[v][i] != 0;
		return ret;
	}

	void printAllAdjacent() const {
		for (int i = 0; i < _vertices; i ++){
			std::cout << i << " : ";
			for (int j = 0; j < _vertices; j ++){
				if (_matrix[i][j])
					std::cout << j << ", ";
			}
			std::cout << "\n";
		}
	}

	bool isComplete() const {
		for (int i = 0; i < _vertices; i ++){
			bool found = false;
			for (int j = 0; !found && j < _vertices; j ++)
				found = _matrix[i][j] != 0;
			if (!found)
				return false;
		}
		return true;
	}

	void printGraph() const {
		for (int i = 0; i < _vertices; i ++){
			for (int j = 0; j < _vertices; j ++)
				std::cout << _matrix[i][j] << "\t";
			std::cout << "\n";
		}
		std::cout << "\n";
	}
};
