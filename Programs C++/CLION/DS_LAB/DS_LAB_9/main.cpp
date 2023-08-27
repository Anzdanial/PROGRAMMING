#include <iostream>
#include <vector>
using namespace std;

template <class T>
class minHeap{
	vector <T> v;
	void bubble_up(int i);
	void bubble_down(int i);
public:
	minHeap(){

	}
	minHeap(T* arr, int N);
	void buildMinHeap();
	void insert(const T &x);
	bool isEmpty()const;
	const T &getMin() const;
	void deleteMin();
	bool deleteAll(T key);
};


int main(){
	cout<<"Hello World"<<endl;
	return 0;
}