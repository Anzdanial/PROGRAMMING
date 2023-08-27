#ifndef HASHMAP_H
#define HASHMAP_H

#include <vector>
#include <iostream>

template <class T>
class HashMap{
private:
	struct Item{
		uint32_t key;
		T value;
		Item *next;
		Item() : key(0), next(nullptr) {};
		Item(uint32_t k, T v, Item *n) : key(k), value(v), next(n) {};
	};
	std::vector<Item*> _vec;
	uint32_t _itemCount;
	uint32_t _freeBucketCount;

	uint32_t _hash(const uint32_t val) const {
		return (val ^ 1431655765) & ~(1 << 31) % _vec.size();
	}

	void _rehash(){
		if (_freeBucketCount * 100 / _vec.size() >= 25)
			return;
		std::vector<Item*> items = _vec;
		_vec = std::vector<Item*>(items.size() * 2);
		for (int i = 0; i < items.size(); i ++){
			Item *ptr = items[i];
			while (ptr){
				set(ptr->key, ptr->value);
				ptr = ptr->next;
			}
		}
	}

public:
	HashMap(){
		for (uint32_t i = 0; i < 10; i ++)
			_vec.push_back(nullptr);
		_freeBucketCount = _vec.size();
	}
	HashMap(uint32_t const capacity){
		for (uint32_t i = 0; i < capacity; i ++)
			_vec.push_back(nullptr);
		_freeBucketCount = _vec.size();
	}
	HashMap(const HashMap &from) : _itemCount(from._itemCount),
		_freeBucketCount(from._freeBucketCount), _vec(from._vec) {}
	virtual ~HashMap(){
		for (uint32_t i = 0; i < _vec.size(); i ++){
			Item *ptr = _vec[i]->next;
			while (ptr){
				Item *next = ptr->next;
				delete ptr;
				ptr = next;
			}
		}
	}

	/// Returns: number of items stored
	uint32_t size() const {
		return _itemCount;
	}

	/// Sets a value against a key
	void set(uint32_t key, const T value){
		const uint32_t index = _hash(key);
		Item *curr = _vec[index];
		while (curr){
			if (curr->key == key){
				curr->value = value;
				return;
			}
			curr = curr->next;
		}
		_vec[index] = new Item(key, value, _vec[index]);
		if (_vec[index]->next == nullptr)
			_freeBucketCount --;
		_itemCount ++;
		_rehash();
	}

	/// Removes a key's entry
	/// Returns: true if done, false if it didnt exist
	bool remove(uint32_t key){
		// start assuming it was inserted with chaining, so deletion is almost O(1)
		const uint32_t index = _hash(key);
		Item *ptr = _vec[index];
		if (!ptr)
			return false;
		Item **prevPtr = &(_vec[index]);
		while (ptr){
			if (ptr->key == key){
				*prevPtr = ptr->next;
				delete ptr;
				_itemCount --;
				if (_vec[index] == nullptr)
					_freeBucketCount ++;
				return true;
			}
			prevPtr = &ptr->next;
			ptr = ptr->next;
		}
		return false;
	}

	const T* get(const uint32_t key) const{
		const uint32_t index = _hash(key);
		Item *curr = _vec[index];
		while (curr){
			if (curr->key == key)
				return &curr->value;
			curr = curr->next;
		}
		return nullptr;
	}

	bool exists(const uint32_t key) const {
		const uint32_t index = _hash(key);
		Item *curr = _vec[index];
		while (curr){
			if (curr->key == key)
				return true;
			curr = curr->next;
		}
		return false;
	}
};

#endif
