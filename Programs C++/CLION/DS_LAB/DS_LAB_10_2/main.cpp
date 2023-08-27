#include <iostream>
#include <vector>
using namespace std;

template<class v>
struct HashItem
{
	int key;
	v value;
	HashItem* next;

	HashItem(int val = 0, int k = 0) {
		value = val;
		next = NULL;
		key = k;
	}
};

template<class v>
class HashMap
{
private:
	vector<HashItem<v>*> hashVector;
	int currentElements;

public:
	HashMap()
	{
		currentElements = 10;

		for (int i = 0; i < currentElements; i++) {

			HashItem<v>* h;
			hashVector.push_back(h);
		}
	}
	HashMap(int const capacity)
	{
		currentElements = capacity;

		for (int i = 0; i < currentElements; i++) {

			HashItem<v>* h = new HashItem<v>(0, 0);
			hashVector.push_back(h);
		}
	}
	void insert_chaining(int const key, v const value)
	{
		int index = key % currentElements;

		if (hashVector[index]->next == NULL) {

			hashVector[index]->next = new HashItem<v>(value, key);
			return;
		}

		HashItem<v>* tempItem = hashVector[index]->next;

		while (tempItem->next != NULL)
			tempItem = tempItem->next;

		tempItem->next = new HashItem<v>(value, key);
	}
	void insert_linear(int const key, v const value)
	{
		int index = key % currentElements;

		if (hashVector[index]->next == NULL)
		{
			hashVector[index]->next = new HashItem<v>(value, key);
			return;
		}

		insert_linear(key + 1, value);
	}
	void insert_quadratic(int const key, v const value)
	{
		int index = key % currentElements;

		if (hashVector[index]->next == NULL)
		{
			hashVector[index]->next = new HashItem<v>(value, key);
		}
		else {

			int i = 1;
			bool flag = 1;
			int newIndex = index;

			while (flag)
			{
				newIndex = (index + (i * i)) % currentElements;

				if (hashVector[newIndex]->next == NULL)
				{
					hashVector[newIndex]->next = new HashItem<v>(value, key);
					flag = 0;
				}

				i++;
			}
		}
	}
	void insert_doublehashing(int const key, v const value)
	{
		//first hash

		int index = key % currentElements;

		if (hashVector[index]->next == NULL)
		{
			hashVector[index]->next = new HashItem<v>(value, key);
			return;
		}

		// second hash : hash2(key) = ( hash(x) + i * (1 + x % currentElements - 1 ) ) % currentElements

		int i = 1;
		bool flag = 1;
		int newIndex = index;

		while (flag)
		{
			newIndex = (index + i * (1 + key % (currentElements - 1))) % currentElements;

			if (hashVector[newIndex]->next == NULL)
			{
				hashVector[newIndex]->next = new HashItem<v>(value, key);
				flag = 0;
			}

			i++;
		}
	}
	bool deleteKey(int const k) const
	{
		for (int i = 0; i < currentElements; i++)
		{
			if (hashVector[i]->next != NULL)
			{
				HashItem<v>* temp = hashVector[i];

				while (temp->next != NULL)
				{
					if (temp->next->key == k)
					{
						temp->next = temp->next->next;
						return true;
					}

					temp = temp->next;
				}
			}
		}
		return false;
	}
	v* get(int const k) const
	{
		for (int i = 0; i < currentElements; i++)
		{
			if (hashVector[i]->next != NULL)
			{
				HashItem<v>* temp = hashVector[i];

				while (temp->next != NULL)
				{
					if (temp->next->key == k)
					{
						return &(temp->next->value);
					}

					temp = temp->next;
				}
			}
		}

		return NULL;
	}
	~HashMap()
	{
		for (int i = 0; i < this->hashVector.size(); i++)
		{
			HashItem<v>* temp = hashVector[i];

			while (temp != NULL) {

				HashItem<v>* tempH = temp->next;

				delete temp;
				temp = tempH;
			}
		}

		while (!hashVector.empty())
		{
			hashVector.pop_back();
		}
	}
};

int main() {

	HashMap<char> h(5);

	char arr[] = { 'a', 'b', 'c', 'd', 'e' };

	for (int i = 0; i < 5; i++)
	{
		h.insert_quadratic(int(arr[i]), arr[i]);
	}

	char* temp = h.get(int(arr[1]));

	if (temp != NULL) {
		cout << *temp;
	}
	else {
		cout << "Key not found";
	}
	cout << endl << "Deleting 'b' and checking if found" << endl;

	h.deleteKey(int(arr[1]));

	temp = h.get(int(arr[1]));

	if (temp != NULL) {
		cout << *temp;
	}
	else {
		cout << "Key not found";
	}

	cout << endl << "now checking someother key" << endl;

	temp = h.get(int(arr[2]));

	if (temp != NULL) {
		cout << *temp;
	}
	else {
		cout << "Key not found";
	}
}