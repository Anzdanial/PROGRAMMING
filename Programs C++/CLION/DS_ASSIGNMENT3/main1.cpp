#include <iostream>
#include <fstream>
#include <vector>
#include <string>

using namespace std;

struct wordStruct {
	string word;
	int frequency;

	wordStruct(string wordI)
	{
		word = wordI;
		frequency = 1;
	}
};

struct node {
	string word;
	int frequency;
	int key;
	node* next;

	node(string wordI, int freq, int keyy)
	{
		word = wordI;
		frequency = freq;
		key = keyy;
		next = NULL;
	}
};

string lowerWord(string s)
{
	int len = s.size();

	for (int i = 0; i < len; i++)
	{
		if (s[i] >= 'A' && s[i] <= 'Z')
			s[i] = s[i] + 32;
	}

	return s;
}

string getInput(string fileName)
{
	char ch;
	string data;
	ifstream file(fileName);

	while (file >> noskipws >> ch)
	{
		data = data + ch;
	}

	return data;
}

bool isLetter(char ch)
{
	if ((ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z'))
		return 1;

	return 0;
}

int getKey(string word)
{
	int len = word.size();
	int key = 0;

	for (int i = 0; i < len; i++)
	{
		int temp = 0;
		for (int j = i; j < len; j++)
			temp = temp + ((254 - j) * word[j] * (j + 1));
		key = temp + key + ((254 - i) * word[i] * (i + 1));
	}

	return key;
}

vector<wordStruct*> createList(string data)
{
	vector<wordStruct*> wordList;
	string word;
	int length = data.size();
	char ch;

	for (int i = 0; i < length; i++)
	{
		if (isLetter(data[i]))
		{
			if (data[i] >= 'a' && data[i] <= 'z')
			{
				word = word + data[i];
			}
			else
			{
				ch = data[i] + 32;
				word = word + ch;
			}
		}
		else if (data[i] == '\'')
		{
			if (i != 0 && i != length - 1)
			{
				if (isLetter(data[i - 1]) && isLetter(data[i + 1]))
				{

					word = word + data[i];
				}
			}
		}
		else
		{
			if (word != "")
			{
				int lengthV = wordList.size();
				bool flag = 1;

				for (int j = 0; j < lengthV && flag; j++)
				{
					if (wordList[j]->word == word)
					{
						wordList[j]->frequency += 1;
						flag = 0;
					}
				}

				if (flag)
				{
					wordStruct* newWord = new wordStruct(word);
					wordList.push_back(newWord);
				}
			}

			word = "";
		}
	}

	return wordList;

}

bool isPrime(int n)
{
	for (int i = 2; i <= n / 2; ++i) {
		if (n % i == 0)
			return false;
	}
	return true;
}

int getNextPrime(int num)
{
	int i = num + 1;

	while (true)
	{
		if (isPrime(i))
			return i;

		i++;
	}
}

class separateChaining {
private:
	vector<node*> hashTable;
	int capacity;
	int filled;
	int size;
	int uniqueSize;
	void rehash()
	{
		vector<node*> hashTableNew;
		int newCapacity = getNextPrime(capacity);

		for (int i = 0; i < newCapacity; i++)
		{
			node* temp = NULL;
			hashTableNew.push_back(temp);
		}

		// rehash

		for (int i = 0; i < capacity; i++)
		{
			node* temp = hashTable[i];

			while (temp != NULL)
			{
				hashTable[i] = hashTable[i]->next;

				temp->next = NULL;

				int index = temp->key % newCapacity;

				if (hashTableNew[index] == NULL)
				{
					hashTableNew[index] = temp;
				}
				else
				{
					node* newTemp = hashTableNew[index];

					while (newTemp->next != NULL)
						newTemp = newTemp->next;

					newTemp->next = temp;
				}

				temp = hashTable[i];
			}
		}

		for (int i = 0; i < capacity; i++)
			hashTable.pop_back();

		hashTable = hashTableNew;

		capacity = newCapacity;
	}
public:
	separateChaining()
	{
		size = 0;
		capacity = 11;
		filled = 0;
		uniqueSize = 0;

		for (int i = 0; i < 11; i++)
		{
			node* temp = NULL;
			hashTable.push_back(temp);
		}
	}
	float getLoadFactor()
	{
		return ((1.0) * filled) / capacity;
	}
	void put(wordStruct* data)
	{
		uniqueSize++;
		size += data->frequency;

		while (getLoadFactor() * 100.0 >= 75.00)
		{
			rehash();
		}

		int key = getKey(data->word);

		int index = key % capacity;

		node* newNode = new node(data->word, data->frequency, key);

		if (hashTable[index] == NULL) {

			filled++;
			hashTable[index] = newNode;
			return;
		}

		node* temp = hashTable[index];

		while (temp->next != NULL)
			temp = temp->next;

		temp->next = newNode;
	}
	int getFreq(int key)
	{
		node* temp = this->hashTable[key % capacity];

		while (temp != NULL)
		{
			if (temp->key == key)
			{
				return temp->frequency;
			}

			temp = temp->next;
		}

		return -1;
	}
	bool contains(int key)
	{
		node* temp = this->hashTable[key % capacity];

		while (temp != NULL)
		{
			if (temp->key == key)
			{
				return 1;
			}

			temp = temp->next;
		}

		return 0;
	}
	void deleteAt(int key)
	{
		node* temp = this->hashTable[key % capacity];
		node* slow = temp;

		while (temp != NULL)
		{
			if (temp->key == key)
			{
				slow->next = temp->next;
				uniqueSize--;
				size = size - temp->frequency;
				return;
			}

			if (temp != slow)
				slow = slow->next;
			temp = temp->next;
		}

		return;
	}
	int getSize()
	{
		return size;
	}
	int getUniqueSize()
	{
		return uniqueSize;
	}
	void printTable()
	{
		for (int i = 0; i < capacity; i++)
		{
			node* temp = hashTable[i];
			int j = 0;

			while (temp != NULL)
			{
				cout << (i + 1) << "." << (j + 1) << "  " << temp->word << "  " << temp->key << endl;
				temp = temp->next;
				j++;
			}
		}
	}
};

class doubleHashing {
private:
	vector<node*> hashTable;
	int capacity;
	int filled;
	int size;
	int uniqueSize;
	void rehash()
	{
		vector<node*> hashTableNew;
		int newCapacity = getNextPrime(capacity);

		for (int i = 0; i < newCapacity; i++)
		{
			node* temp = NULL;
			hashTableNew.push_back(temp);
		}

		// rehash
		int temp = capacity;
		capacity = newCapacity;

		for (int i = 0; i < temp; i++)
		{

			if (hashTable[i] != NULL)
			{
				int index = hash1(hashTable[i]->key);

				if (hashTableNew[index] == NULL)
				{
					hashTableNew[index] = hashTable[i];
				}
				else {

					bool flag = 1;
					int j = 0;


					while (flag)
					{
						index = hash2(hashTable[i]->key, j);

						if (hashTableNew[index] == NULL)
							flag = 0;
						j++;
					}

					hashTableNew[index] = hashTable[i];
				}
			}
		}

		for (int i = 0; i < temp; i++)
			hashTable.pop_back();

		hashTable = hashTableNew;

		capacity = newCapacity;
	}
	int hash1(int key)
	{
		return key % capacity;
	}
	int hash2(int key, int i)
	{
		int h1 = hash1(key);

		int h2 = (h1 + (i + key) % capacity) % capacity;

		return h2;
	}
public:
	doubleHashing()
	{
		size = 0;
		capacity = 11;
		filled = 0;
		uniqueSize = 0;

		for (int i = 0; i < 11; i++)
		{
			node* temp = NULL;
			hashTable.push_back(temp);
		}
	}
	float getLoadFactor()
	{
		return ((1.0) * filled) / capacity;
	}
	void put(wordStruct* data)
	{
		filled++;
		uniqueSize++;
		size += data->frequency;

		while (getLoadFactor() * 100.0 >= 75.00)
		{
			rehash();
		}

		int key = getKey(data->word);

		int index = hash1(key);

		if (hashTable[index] == NULL)
		{
			node* newNode = new node(data->word, data->frequency, key);

			hashTable[index] = newNode;

			return;
		}

		bool flag = 1;
		int i = 0;


		while (flag)
		{
			index = hash2(key, i);

			if (hashTable[index] == NULL)
				flag = 0;
			i++;
		}


		node* newNode = new node(data->word, data->frequency, key);

		hashTable[index] = newNode;
	}
	int getFreq(int key)
	{
		int index = hash1(key);

		if (hashTable[index] != NULL && hashTable[index]->key == key)
			return hashTable[index]->frequency;

		bool flag = 1;
		int i = 0;
		int index2 = 0;
		while (flag)
		{
			index2 = hash2(key, i);

			if (index2 == index)
				return -1;
			if (hashTable[index2] != NULL && hashTable[index2]->key == key)
				return hashTable[index2]->frequency;
			i++;
		}

		return -1;

	}
	bool contains(int key)
	{
		int index = hash1(key);

		if (hashTable[index] != NULL && hashTable[index]->key == key)
			return 1;

		bool flag = 1;
		int i = 0;
		int index2 = 0;
		bool firstIterate = 1;

		while (flag)
		{
			index2 = hash2(key, i);

			if (index2 == index && !firstIterate)
				return 0;
			if (hashTable[index2] != NULL && hashTable[index2]->key == key)
				return 1;
			firstIterate = 0;
			i++;
		}

		return 0;
	}
	void deleteAt(int key)
	{
		int index = hash1(key);

		if (hashTable[index] != NULL && hashTable[index]->key == key)
		{
			delete hashTable[index];
			hashTable[index] = NULL;
		}

		bool flag = 1;
		int i = 0;
		int index2 = 0;

		while (flag)
		{
			index2 = hash2(key, i);

			if (index2 == index)
				return;
			if (hashTable[index2] != NULL && hashTable[index2]->key == key)
			{
				delete hashTable[index2];
				hashTable[index2] = NULL;
				return;
			}
			i++;
		}

		return;
	}
	int getSize()
	{
		return size;
	}
	int getUniqueSize()
	{
		return uniqueSize;
	}
	void printTable()
	{
		for (int i = 0; i < capacity; i++)
		{
			if (hashTable[i] != NULL)
				cout << (i + 1) << ".     " << hashTable[i]->word << "  " << hashTable[i]->key << "  " << hashTable[i]->frequency << endl;

		}
	}
};

int main()
{
	string data = getInput("q2.txt");

	vector<wordStruct*> dataWords = createList(data);

	separateChaining sp;
	//doubleHashing sp;

	for (int i = 0; i < dataWords.size(); i++)
	{
		sp.put(dataWords[i]);
	}

	string s = "";

	cout << "> ";
	getline(cin, s);

	while (s != "")
	{
		s = lowerWord(s);

		if (s[0] == '-')
		{
			s = &s[1];
			sp.deleteAt(getKey(s));
			cout << "Word deleted" << endl;
		}
		else
		{
			int n = sp.getFreq(getKey(s));

			if (!sp.contains(getKey(s)))
			{
				cout << "Word not found" << endl;
			}
			else
			{
				cout << s << " occurs " << n << " times" << endl;
			}
		}
		s = "";
		cout << "> ";
		getline(cin, s);
	}

}