#ifndef HEAP_H
#define HEAP_H

#include <vector>

template <class T>
class MinHeap{
private:
	std::vector<T> _vector;

	void bubbleUp(int i){
		if (i < 0 || i >= _vector.size())
			return;
		int min = i;
		if (2*i + 1 < _vector.size() && _vector[min] > _vector[2*i + 1])
			min = 2*i + 1;
		if (2*i + 2 < _vector.size() && _vector[min] > _vector[2*i + 2])
			min = 2*i + 2;
		if (min != i){
			T temp = _vector[i];
			_vector[i] = _vector[min];
			_vector[min] = temp;
		}
		// recurse
		if (i > 0)
			bubbleUp((i-1) / 2);
	}

	void bubbleDown(int i){
		if (i < 0 || i >= _vector.size())
			return;
		int min = i;
		if (2*i + 1 < _vector.size() && _vector[min] > _vector[2*i + 1])
			min = 2*i + 1;
		if (2*i + 2 < _vector.size() && _vector[min] > _vector[2*i + 2])
			min = 2*i + 2;
		if (min != i){
			T temp = _vector[i];
			_vector[i] = _vector[min];
			_vector[min] = temp;
		}
		// recurse
		if (min != i)
			bubbleDown(min);
	}

public:
	MinHeap(){}
	virtual ~MinHeap(){}
	MinHeap(const MinHeap &from) : _vector(from._vector) {}

	MinHeap& operator=(const MinHeap &from){
		_vector = from._vector;
		return *this;
	}

	MinHeap(T* arr, int n){
		for (int i = 0; i < n; i ++)
			insert(arr[i]);
	}

	void insert(const T &x){
		_vector.push_back(x);
		bubbleUp(_vector.size() - 1);
	}

	bool isEmpty() const {
		return _vector.size() == 0;
	}

	int count() const {
		return _vector.size();
	}

	const T& getMin() const {
		return _vector[0];
	}

	T& getMin(){
		return _vector[0];
	}

	void deleteMin(){
		if (_vector.size() == 0)
			return;
		_vector[0] = _vector[_vector.size() - 1];
		_vector.pop_back();
		bubbleDown(0);
	}

	bool deleteAll(T key){
		bool ret = false;
		while (true){
			// try and find
			int index = 0;
			while (index < _vector.size() && _vector[index] != key)
				index ++;
			if (index >= _vector.size())
				return ret;
			ret = true;
			// bubble up from there
			_vector[index] = _vector[_vector.size() - 1];
			_vector.pop_back();
			bubbleUp(index);
		}
	}
};

template <class T>
class MinPtrHeap{
private:
	std::vector<T> _vector;

	void bubbleUp(int i){
		if (i < 0 || i >= _vector.size())
			return;
		int min = i;
		if (2*i + 1 < _vector.size() && *(_vector[min]) > *(_vector[2*i + 1]))
			min = 2*i + 1;
		if (2*i + 2 < _vector.size() && *(_vector[min]) > *(_vector[2*i + 2]))
			min = 2*i + 2;
		if (min != i){
			T temp = _vector[i];
			_vector[i] = _vector[min];
			_vector[min] = temp;
		}
		// recurse
		if (i > 0)
			bubbleUp((i-1) / 2);
	}

	void bubbleDown(int i){
		if (i < 0 || i >= _vector.size())
			return;
		int min = i;
		if (2*i + 1 < _vector.size() && *(_vector[min]) > *(_vector[2*i + 1]))
			min = 2*i + 1;
		if (2*i + 2 < _vector.size() && *(_vector[min]) > *(_vector[2*i + 2]))
			min = 2*i + 2;
		if (min != i){
			T temp = _vector[i];
			_vector[i] = _vector[min];
			_vector[min] = temp;
		}
		// recurse
		if (min != i)
			bubbleDown(min);
	}

public:
	MinPtrHeap(){}
	virtual ~MinPtrHeap(){}
	MinPtrHeap(const MinPtrHeap &from) : _vector(from._vector) {}

	MinPtrHeap& operator=(const MinPtrHeap &from){
		_vector = from._vector;
		return *this;
	}

	MinPtrHeap(T* arr, int n){
		for (int i = 0; i < n; i ++)
			insert(arr[i]);
	}

	void insert(const T &x){
		_vector.push_back(x);
		bubbleUp(_vector.size() - 1);
	}

	bool isEmpty() const {
		return _vector.size() == 0;
	}

	int count() const {
		return _vector.size();
	}

	const T& getMin() const {
		return _vector[0];
	}

	T& getMin(){
		return _vector[0];
	}

	void deleteMin(){
		if (_vector.size() == 0)
			return;
		_vector[0] = _vector[_vector.size() - 1];
		_vector.pop_back();
		bubbleDown(0);
	}

	bool deleteAll(T key){
		bool ret = false;
		while (true){
			// try and find
			int index = 0;
			while (index < _vector.size() && _vector[index] != key)
				index ++;
			if (index >= _vector.size())
				return ret;
			ret = true;
			// bubble up from there
			_vector[index] = _vector[_vector.size() - 1];
			_vector.pop_back();
			bubbleUp(index);
		}
	}
};

// 1 for ascending order
template <class T>
void heapSort(T *arr, int n, int sorting_order = 1){
	MinHeap<T> heap(arr, n);
	for (int i = 0; i < n; i ++){
		arr[(sorting_order == 1) * i + (sorting_order == 0) * (n - 1 - i)] =
			heap.getMin();
		heap.deleteMin();
	}
}

template <class T>
void heapSort(std::vector<T> &vec, int sorting_order = 1){
	MinHeap<T> heap;
	const int n = vec.size();
	for (int i = 0; i < n; i ++)
		heap.insert(vec[i]);
	for (int i = 0; i < n; i ++){
		vec[(sorting_order == 1) * i + (sorting_order == 0) * (n - 1 - i)] =
			heap.getMin();
		heap.deleteMin();
	}
}

#endif
