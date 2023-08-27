#include <iostream>
#include <fstream>
using namespace std;

int main(){
	ifstream ins("directedGraph.txt");
	string values;
	while(!ins.eof()) {
		getline(ins,values);
		for (int i = 0; values[i] != '\0'; i++)
			cout << values[i];
	}
	ins.close();
	return 0;
}