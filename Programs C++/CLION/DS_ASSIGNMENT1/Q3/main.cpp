#include <iostream>
#include "LinkedList.h"

using namespace std;

int main(){
	LinkedList a;
	a.insert(0);
	a.insert(1);
	a.insert(2);
	a.swap(1,0);
	std :: cout<< a << std::endl;

	return 0;
}