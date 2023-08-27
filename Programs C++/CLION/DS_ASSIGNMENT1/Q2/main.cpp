#include <iostream>
#include "LinkedList.h"

using std::endl;

using namespace std;

int main(){
	LinkedList a;
	LinkedList b;
	for(int i = 1; i < 6; i++){
		b.insert(i);
	}
	a.insert(7);
	a.insert(6);
	a.insert(6);
	a.insert(6);
	a.insert(8);
	a.compress();
	std :: cout<< a << std::endl;
	a.mergeList(b);
	std::cout << a;
	std::cout << endl;
	return 0;
}
