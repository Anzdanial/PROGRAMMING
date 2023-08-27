#include <iostream>
#include <vector>
#include <fstream>
#include <string>
#include <cmath>
#include <algorithm>
using namespace std;

//used to store character and frequencies
struct node {
	char data;
	int occurence;
	node* left;
	node* right;
	node(){
		left = nullptr;
		right = nullptr;
		data = 0;
		occurence = 0;
	}
	node(char val, int occ){
		left = nullptr;
		right = nullptr;
		data = val;
		occurence = occ;
	}

	node(node* t)
	{
		left = t->left;
		right = t->right;
		data = t->data;
		occurence = t->occurence;
	}
};

// structure used to store huffman tree
struct huffmanData {
	string huffmancode;
	char data;

	huffmanData(string code, char d)
	{
		huffmancode = code;
		data = d;
	}
};


class minHeap {
private:
	vector<node*> _vector;
	void bubble_up(int i)
	{
		int smallest = i;
		int l = 2 * i + 1;
		int r = 2 * i + 2;

		if (l < this->_vector.size() && this->_vector[l]->occurence < this->_vector[smallest]->occurence)
			smallest = l;

		if (r < this->_vector.size() && this->_vector[r]->occurence < this->_vector[smallest]->occurence)
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
	void buildMinHeap() {

		int n = _vector.size();

		for (int i = n / 2 - 1; i >= 0; i--)
			this->bubble_up(i);
	}
	void insert(node* x) {

		_vector.push_back(x);
		this->buildMinHeap();
	}
	bool isEmpty() const {

		return (_vector.size() == 0);
	}
	node* getMin() const {

		if (!this->isEmpty())
			return _vector[0];

		return NULL;
	}
	void deleteMin() {

		if (this->isEmpty())
			return;

		int size = _vector.size();

		swap(this->_vector[0], this->_vector[size - 1]);

		this->_vector.pop_back();

		this->buildMinHeap();
	}
	int getSize()
	{
		return _vector.size();
	}
};

class encode {
private:
	node* huffmanCode_root;
	string dataString;
	vector<huffmanData*> dataHuffmanVector;
	void getInputFromFile(vector<node*>& data, string fileName)
	{
		ifstream file(fileName);
		char ch;

		while (file >> noskipws >> ch)
		{
			dataString = dataString + ch;

			int length = data.size();
			bool flag = 1;
			for (int i = 0; i < length; i++)
			{
				if (data[i]->data == ch)
				{
					data[i]->occurence += 1;
					flag = 0;
					break;
				}
			}

			if (flag) {
				node* newNode = new node(ch, 1);
				data.push_back(newNode);
			}
		}

	}

	void buildMinHeapTree(vector<node*> input)
	{
		minHeap heap;

		while (!input.empty())
		{
			heap.insert(input[input.size() - 1]);
			input.pop_back();
		}

		while (heap.getSize() != 1)
		{
			node* left = heap.getMin();
			heap.deleteMin();
			node* right = heap.getMin();
			heap.deleteMin();

			node* newNode = new node('$', left->occurence + right->occurence);

			newNode->left = left;
			newNode->right = right;

			heap.insert(newNode);
		}

		huffmanCode_root = heap.getMin();
	}

	void makeHuffmanTree(vector<node*>& v) {

		buildMinHeapTree(v);
	}

	void getCode(node* root, string s)
	{
		if (root->left == NULL && root->right == NULL)
		{
			cout << s << "  " << root->occurence << "   " << root->data << endl;
			huffmanData* newData = new huffmanData(s, root->data);
			dataHuffmanVector.push_back(newData);
			return;
		}

		if (root->left != NULL && root->right == NULL) {

			getCode(root->left, s + "0");
			return;
		}

		if (root->left == NULL && root->right != NULL) {

			getCode(root->right, s + "1");
			return;
		}

		getCode(root->left, s + "0");
		getCode(root->right, s + "1");
	}

	void createHuffman()
	{
		getCode(huffmanCode_root, "");
	}

	//int to binary
	string intToBinary(int n)
	{
		string b;

		while (n > 0)
		{
			b = b + (char((n % 2) + 48));

			n = n / 2;
		}

		reverse(b.begin(), b.end());

		while (b.length() < 8)
		{
			b = '0' + b;

		}

		return b;
	}


	//sorts huffman code from highest to lowest string length
	void sortHuffman()
	{
		int i, j, min_idx;

		for (i = 0; i < dataHuffmanVector.size() - 1; i++)
		{

			min_idx = i;
			for (j = i + 1; j < dataHuffmanVector.size(); j++)
			{
				if (dataHuffmanVector[j]->huffmancode.size() >= dataHuffmanVector[min_idx]->huffmancode.size())
					min_idx = j;
			}

			if (min_idx != i)
			{
				huffmanData* temp = dataHuffmanVector[i];
				dataHuffmanVector[i] = dataHuffmanVector[min_idx];
				dataHuffmanVector[min_idx] = temp;
			}
		}
	}

	//writes binary code in file
	void writeBinaryInFile(string fileName)
	{
		ofstream file(fileName);

		int len = dataString.length();
		int len2 = dataHuffmanVector.size();
		int count = 0;
		string stringof8 = "";

		// storing the data
		for (int i = 0; i < len; i++)
		{

			for (int j = 0; j < len2; j++)
			{
				if (dataHuffmanVector[j]->data == dataString[i])
				{
					string temp = dataHuffmanVector[j]->huffmancode;

					file << temp;
					break;
				}
			}
		}

		// storing huffman code

		sortHuffman();

		//ofstream fileH("huffmanOutput.txt");
		ofstream fileH("huffmanOutput.txt");

		len = dataHuffmanVector.size();

		for (int i = 0; i < len; i++)
		{
			//cout << [i]->data << intToBinary(data[i]->data) << endl;

			fileH << intToBinary(dataHuffmanVector[i]->data) << endl;
			string d = dataHuffmanVector[i]->huffmancode;
			//int count = correctHuffman(d);
			fileH << d << endl;
			//fileH << intToBinary(count) << endl;

		}
	}

public:

	void Activate_Encoder(string fileName)
	{
		vector<node*> huffmanVector;

		getInputFromFile(huffmanVector, fileName);

		makeHuffmanTree(huffmanVector);

		createHuffman();

		string fileNameH = "output.txt";

		cout << "Enter output File Name = ";
		getline(cin, fileName);

		if (fileName[fileName.length() - 4] != '.')
		{
			cout << "Invalid File Name" << endl;
			return;
		}

		writeBinaryInFile(fileNameH);

		ofstream fout("comapre.txt");

		for (int i = 0; i < dataString.size(); i++)
		{
			fout << intToBinary(dataString[i]);
		}
	}
};

class decode {
private:
	vector<huffmanData*> huffmanDataVector;
	node* huffmanTree;
	string binaryString;

	int binaryToInt(string data)
	{
		int num = 0;
		int length = data.size();

		for (int i = 0; i < length; i++)
		{
			num = num + ((data[length - i - 1] - 48) * pow(2, i));
		}

		return num;
	}

	//creates vector of huffman tree
	void getInputHuffman()
	{
		ifstream file("huffmanOutput.txt");

		string data1;
		string data2;

		while (!file.eof())
		{
			getline(file, data1);
			if (data1[0] == '\0')
				break;
			getline(file, data2);

			char a = binaryToInt(data1);

			huffmanData* h = new huffmanData(data2, a);

			huffmanDataVector.push_back(h);
		}
	}

	//helper function to make tree
	void createTree(node*& root, string data, char c)
	{

		if (data.size() == 1) {

			node* newNode = new node(c, 0);
			if (data[0] == '1')
			{
				root->right = newNode;
				return;
			}

			root->left = newNode;
			return;
		}

		if (data[0] == '1')
		{
			if (root->right == NULL)
			{
				node* newNode = new node('$', 0);
				root->right = newNode;
			}
			data = &data[1];
			createTree(root->right, data, c);
		}
		else
		{
			if (root->left == NULL)
			{
				node* newNode = new node('$', 0);
				root->left = newNode;
			}
			data = &data[1];
			createTree(root->left, data, c);
		}
	}

	//creates tree
	void getHuffmanTree()
	{
		huffmanTree = new node('$', 0);

		for (int i = 0; i < huffmanDataVector.size(); i++)
		{
			createTree(huffmanTree, huffmanDataVector[i]->huffmancode, huffmanDataVector[i]->data);
		}
	}

	//gets binary string
	void getStringBinary(string fileName)
	{
		ifstream file(fileName);

		getline(file, binaryString);

	}

	void writeInFile_Binary_To_String(string fileName)
	{
		node* temp = huffmanTree;
		int length = binaryString.size();
		ofstream file(fileName);

		for (int i = 0; i < length; i++)
		{
			if (binaryString[i] == '1')
			{
				temp = temp->right;
			}
			else if (binaryString[i] == '0')
			{
				temp = temp->left;
			}

			if (temp->left == NULL && temp->right == NULL)
			{
				file << temp->data;
				temp = huffmanTree;
			}
		}
	}


public:

	void Activate_Decoder(string fileName_Initial, string fileName_Final)
	{

		getStringBinary(fileName_Initial);

		getInputHuffman();

		getHuffmanTree();

		getStringBinary(fileName_Initial);

		cout << binaryString << endl << endl;

		writeInFile_Binary_To_String(fileName_Final);
	}

};

int main()
{
	encode e;

	e.Activate_Encoder("q1.txt");

	decode D;

	D.Activate_Decoder("output.txt", "decodeOutput.txt");
}