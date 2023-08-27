#include <iostream>
#include <vector>
using namespace std;

int main(){
	vector <int> v1;
	for(int i=0; i<10; i++)
		v1.push_back(i);
	for(auto i = begin(v1); i != end(v1); ++i)
		cout<<*i<<endl;
	return 0;
}