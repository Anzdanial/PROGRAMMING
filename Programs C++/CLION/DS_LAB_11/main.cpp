#include <iostream>
#include <fstream>
#include <queue>
#include <stack>
using namespace std;

class graphs {
private:
	int **matrix;
	int vertices;
	bool isDirected;
public:
	graphs(int Tvertices, bool dir) {
		vertices = Tvertices;
		isDirected = dir;
		matrix = new int *[vertices];
		for (int i = 0; i < vertices; i++)
			matrix[i] = new int[vertices];
		for (int i = 0; i < vertices; i++) {
			for (int j = 0; j < vertices; j++) {
				matrix[i][j] = 0;
			}
		}
	}

	graphs(const graphs &obj) {
		vertices = obj.vertices;
		isDirected = obj.isDirected;
		for (int i = 0; i < vertices; i++) {
			for (int j = 0; j < vertices; j++) {
				matrix[i][j] = obj.matrix[i][j];
			}
		}
	}

	graphs(string fName) {
		string values;
		ifstream ins(fName);
		getline(ins, values);
		vertices = values[0] - '0';
		isDirected = values[2] - '0';
		matrix = new int *[vertices];
		for (int i = 0; i < vertices; i++)
			matrix[i] = new int[vertices];
		for (int i = 0; i < vertices; i++) {
			for (int j = 0; j < vertices; j++) {
				matrix[i][j] = 0;
			}
		}
		while (!ins.eof()) {
			getline(ins, values);
			for(int i = 1; values[i]!='\0'; i++) {
				if (values[i] != ' ') {
					addEdge(values[0] - '0', values[i] - '0');
				}
			}
		}
		ins.close();
	}

	bool addEdge(int x, int y) {
		if (x >= vertices || y >= vertices)
			return false;
		else {
			if(isDirected)
				matrix[x][y] = 1;
			else
				matrix[x][y] = matrix[y][x] = 1;
			return true;
		}
	}

	bool removeEdge(int x, int y) {
		if (x > 5 || y > 5)
			return false;
		else {
			if(isDirected)
				matrix[x][y] = 0;
			else
				matrix[x][y] = matrix[y][x] = 0;
			return true;
		}
	}

	void printGraph(){
		for(int i = 0; i < vertices; i++) {
			for (int j = 0; j < vertices; j++) {
				cout<<matrix[i][j]<<" ";
			}
			cout<<endl;
		}
	}

	bool isConnected(int x, int y){
		if(x >= vertices || y >= vertices)
			return false;
		else{
			return (matrix[x][y] == 1 || matrix[y][x] == 1);
		}
	}




	bool dfsStack(int visiting, int dest, vector<int> &path, int* &visited) {
		path.push_back(visiting);
		visited[visiting] = 1;

		if (visiting == dest)
			return dest;

		for (int i = 0; i < vertices; i++) {

			if (visited[i] != true && isConnected(visiting, i)) {
				return dfsStack(i, dest, path, visited);
			}
		}
	}

	void printPathDFS(int src, int dest){
		if (src >= vertices || dest >= vertices) {
			cout << "Not valid!" << endl;
			return;
		}

		int* visited = new int[vertices];
		for (int i = 0; i < vertices; i++) {
			visited[i] = 0;
		}

		vector<int> path;
		int visiting = src;
		dfsStack(visiting, dest, path, visited);
		cout << "Path: ";
		for (int i = 0; i < path.size(); i++) {
			cout << path[i] << ", ";
		}
		cout << endl;
	}

	void shortestPath(int src, int dest){
		int* distances = new int[vertices];
		int* visited = new int[vertices];
		int* parents = new int[vertices];
		for (int i = 0; i < vertices; i++) {
			distances[i] = 99;
			visited[i] = 0;
			parents[i] = -1;
		}


		queue<int> q;
		vector<int> path;
		distances[src] = 0;
		visited[src] = 1;
		q.push(src);

		while (!q.empty()) {
			int front = q.front();
			q.pop();

			for (int i = 0; i < vertices; i++) {
				if (isConnected(front, i) && visited[i] == 0) {

					visited[i] = 1;
					distances[i] = distances[front] + 1;
					parents[i] = front;
					q.push(i);

					if (i == dest) {
						cout << "Dest found!\n";


						int traverse = dest;
						path.push_back(traverse);
						while (parents[traverse] != -1) {
							path.push_back(parents[traverse]);
							traverse = parents[traverse];
						}

						for (int i = path.size()-1 ; i >= 0; i--) {
							cout << path[i] << ", ";
						} cout << endl;

						return;
					}
				}
			}
		}
		cout << "Dest not found!\n";
	}

	void printPathBFS(int src, int dest) {
		if (src >= vertices || dest >= vertices) {
			cout << "Not valid!" << endl;
			return;
		}

		queue<int> q;
		int* visited = new int[vertices];
		for (int i = 0; i < vertices; i++) {
			visited[i] = 0;
		}

		vector<int> path;

		int visiting = src;
		visited[src] = 1;
		q.push(visiting);

		while (!q.empty() && visiting != dest) {
			visiting = q.front();
			path.push_back(visiting);

			q.pop();

			for (int i = 0; i < vertices; i++) {
				//	cout << "Standing at " << visiting << ". Checking " << i;
				if (visited[i] != true && visiting != vertices && isConnected(visiting, i)) {
					//	cout << ". Pushed!" << endl;
					q.push(i);
					visited[i] == 1;

				}
				else {
					//	cout << ". Not pushed!" << endl;
				}

			}



		}

		cout << "Path: ";
		for (int i = 0; i < path.size(); i++) {
			cout << path[i] << ", ";
		}
		cout << endl;
	}

	int getIndegree(int x){
		if(x >= vertices)
			return -1;
		int counter = 0;
		for(int i = 0; i < vertices; i++) {
			if(matrix[i][x] == 1)
				counter++;
		}
		return counter;
	}

	int getOutdegree(int x){
		if(x >= vertices)
			return -1;
		int counter = 0;
		for(int i = 0; i < vertices; i++) {
			if(matrix[x][i] == 1)
				counter++;
		}
		return counter;
	}

	void printAllAdjc(int x){
		cout<<"Adjacent Values are: ";
		for(int i = 0; i < vertices; i++) {
			if (matrix[x][i] == 1)
				cout << i << " ";
		}
		cout<<endl;
	}


	~graphs() {
		for (int i = 0; i < vertices; i++)
				delete []matrix[i];
		delete []matrix;
	}

};



int main() {
	//graphs g1("directedGraph.txt");
	//g1.printGraph();
	//g1.printAllAdjc(1);
	//cout<<g1.getIndegree(2)<<endl;
	//cout<<g1.getOutdegree(2)<<endl;

	/*graphs direc("directed.txt");
	cout << "\n Matrix:\n";
	direc.printGraph();

	cout << "\n BFS (5->1) :\n";
	direc.printPathBFS(5, 1);

	cout << "\n DFS (5->1) :\n";
	direc.printPathDFS(5, 1);

	cout << "\n Indegree :\n";
	direc.getIndegree(0);

	cout << "\n Outdegree :\n";
	direc.getOutdegree(1);
	cout << endl;

	cout << "\nShortest path :\n";
	direc.shortestPath(5, 1);*/

	return 0;
	}