#include <iostream>
#include <vector>
using namespace std;

template<typename T>
class minHeap {
private:
	vector<T> _vector;
	void bubble_up(int i)
	{
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

	void bubble_down(int i) {

		int size = this->_vector.size();
		swap(this->_vector[i], this->_vector[size - 1]);
		this->_vector.pop_back();
		buildMinHeap();
	}
public:
	minHeap() {

	}
	minHeap(T* arr, int N) {

		for (int i = 0; i < N; i++)
			_vector.push_back(arr[i]);

		this->buildMinHeap();
	}
	void buildMinHeap() {

		int n = _vector.size();

		for (int i = n / 2 - 1; i >= 0; i--)
			this->bubble_up(i);
	}
	void insert(const T& x) {

		_vector.push_back(x);
		this->buildMinHeap();
	}
	bool isEmpty() const {

		return (_vector.size() == 0);
	}
	const T& getMin() const {

		if (!this->isEmpty())
			return _vector[0];

		return -1;
	}
	void deleteMin() {

		if (this->isEmpty())
			return;

		int size = _vector.size();

		swap(this->_vector[0], this->_vector[size - 1]);

		this->_vector.pop_back();

		this->buildMinHeap();
	}
	bool deleteAll(T key) {

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
};

template<typename T>
void HeapSort(T* arr, int size, int sorting_order) {

	minHeap<T>sortHelp(arr, size);

	if (sorting_order == 1) {

		for (int i = 0; i < size; i++) {

			arr[i] = sortHelp.getMin();

			sortHelp.deleteMin();
		}
	}
	else if(sorting_order == 0) {

		for (int i = 0; i < size; i++) {

			arr[size - i - 1] = sortHelp.getMin();

			sortHelp.deleteMin();
		}
	}
}

int main() {

	int array[] = { 5, 4, 5, 30, 3, 300 };

	minHeap<int>obj(array, 6);

	obj.deleteAll(5);

	for (int i = 0; i < 3; ++i)
	{
		cout << obj.getMin() << " ";
		obj.deleteMin();
	}

	cout << endl;

	int array2[] = { 10, 40, 50, 7, 60, 5, 20 };

	minHeap<int>obj2;

	for (int i = 0; i < 7; i++) {

		obj2.insert(array2[i]);

		cout << obj2.getMin() << " ";
	}

	cout << endl;

	cout << "Decending : ";

	HeapSort(array2, 7, 0);

	for (int i = 0; i < 7; i++) {

		cout << array2[i] << "  ";
	}

	cout << endl;

	cout << "Ascending : ";

	HeapSort(array2, 7, 1);

	for (int i = 0; i < 7; i++) {

		cout << array2[i] << "  ";
	}

}