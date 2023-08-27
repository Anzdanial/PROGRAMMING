#include <iostream>
#include <vector>
using namespace std;

template<typename T>
class minHeap {
private:
	vector<T> _vector;
	void bubble_up(int i);
	void bubble_down(int i);

public:
	minHeap();
	minHeap(T* arr, int N);
	void buildMinHeap();
	void insert(const T& x);
	bool isEmpty() const;
	const T& getMin() const;
	void deleteMin();
	bool deleteAll(T key);
};

template<typename T>
void minHeap<T>::bubble_up(int i){

	int smallest = i;
	int l = 2 * i + 1;
	int r = 2 * i + 2;

	if (l < this->_vector.size() && this->_vector[l] < this->_vector[smallest])
		smallest = l;

	if (r < this->_vector.size() && this->_vector[r] < this->_vector[smallest])
		smallest = r;

	if (smallest != i) {

		swap(this->_vector[i], this->_vector[smallest]);
		this->bubble_up(smallest);
	}

}

template<typename T>
void minHeap<T>::bubble_down(int i) {

	int size = this->_vector.size();
	swap(this->_vector[i], this->_vector[size - 1]);
	this->_vector.pop_back();
	buildMinHeap();
}

template<typename T>
minHeap<T>::minHeap() {

}

template<typename T>
minHeap<T>::minHeap(T* arr, int N) {

	for (int i = 0; i < N; i++)
		_vector.push_back(arr[i]);

	this->buildMinHeap();
}

template<typename T>
void minHeap<T>::buildMinHeap() {

	int n = _vector.size();

	for (int i = n / 2 - 1; i >= 0; i--)
		this->bubble_up(i);
}

template<typename T>
void minHeap<T>::insert(const T& x) {

	_vector.push_back(x);
	this->buildMinHeap();
}

template<typename T>
bool minHeap<T>::isEmpty() const {

	return (_vector.size() == 0);
}

template<typename T>
const T& minHeap<T>::getMin() const{

	if (!this->isEmpty())
		return _vector[0];

	return -1;
}

template<typename T>
void minHeap<T>::deleteMin() {

	if (this->isEmpty())
		return;

	int size = _vector.size();

	swap(this->_vector[0], this->_vector[size - 1]);

	this->_vector.pop_back();

	this->buildMinHeap();
}

template<typename T>
bool minHeap<T>::deleteAll(T key) {

	int size = this->_vector.size();
	int count = 0;

	for (int i = 0; i < size; i++) {

		if (this->_vector[i] == key) {

			size--;
			bubble_down(i);
		}
	}

	return 1;
}