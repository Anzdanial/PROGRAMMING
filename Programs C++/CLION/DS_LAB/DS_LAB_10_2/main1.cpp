#include <iostream>
#include <vector>
#include <fstream>
#include <string>
#include "minHeap.h"
using namespace std;

struct node {
	char data;
	int frequency;
	node* left;
	node* right;
	node()
	{
		left = NULL;
		right = NULL;
		data = 0;
		frequency = 0;
	}
	node(char d, int freq)
	{
		left = NULL;
		right = NULL;
		data = d;
		frequency = freq;
	}

	node(node* t)
	{
		left = t->left;
		right = t->right;
		data = t->data;
		frequency = t->frequency;
	}
};

struct huffmanData {
	string huffmancode;
	char data;

	huffmanData(string code, char d)
	{
		huffmancode = code;
		data = d;
	}
};

void swapNodes(node*& left, node*& right)
{
	node* temp = left;
	left = right;
	right = temp;
}

void SelectionSort(vector<node*>& input)
{
	int i, j, min_idx;

	for (i = 0; i < input.size() - 1; i++)
	{

		min_idx = i;
		for (j = i + 1; j < input.size(); j++)
		{
			if (input[j]->frequency <= input[min_idx]->frequency)
				min_idx = j;
		}

		if (min_idx != i)
			swap(input[min_idx], input[i]);
	}
}

void buildMaxHeapTree(vector<node*>& input, node*& root)
{
	vector<node*> copy;

	for (int i = 0; i < input.size(); i++)
	{
		copy.push_back(new node(input[i]));
	}

	while (copy.size() != 1)
	{
		node* newNode = new node('$', copy[0]->frequency + copy[1]->frequency);

		newNode->left = copy[0];
		newNode->right = copy[1];

		copy[0] = newNode;
		//delete copy[1];

		for (int i = 1; i < copy.size() - 1; i++)
		{
			copy[i] = copy[i + 1];
		}

		copy.pop_back();

		SelectionSort(copy);
	}

	root = copy[0];
}

node* makeHuffmanTree(vector<node*>v) {

	node* root;
	buildMaxHeapTree(v, root);

	return root;
}

void getCode(vector<huffmanData*>& data, node* root, string s)
{
	if (root->data != '$')
	{
		huffmanData* newData = new huffmanData(s, root->data);
		data.push_back(newData);
		return;
	}

	getCode(data, root->left, s + "0");
	getCode(data, root->right, s + "1");
}

vector<huffmanData*> createHuffman(node* root)
{
	vector<huffmanData*> data;

	getCode(data, root, "");

	return data;
}

int main(){
	vector<node*>v;
	// opening file and count frequency of each character in file
	string text;
	ifstream file("test.txt");
	while (getline(file, text))
	{
		cout << text;
	}

	cout << endl;
	file.close();
	int len = text.length();
	for (int i = 0; i < len; i++)
	{
		bool b = false;

		for (int a = 0; a < v.size(); a++)
		{
			if (v[a]->data == text[i])
			{
				v[a]->frequency++;
				b = true;
			}
		}
		if (b == false)
		{
			node* temp = new node();
			temp->data = text[i];
			temp->frequency = 1;
			v.push_back(temp);
		}
	}

	SelectionSort(v);

	node* root = makeHuffmanTree(v);

	for (int a = 0; a < v.size(); a++)
	{
		cout << v[a]->data << " ";
		cout << v[a]->frequency;
		cout << endl;
	}

	cout << endl << endl;

	vector<huffmanData*> codes = createHuffman(root);

	for (int a = 0; a < codes.size(); a++)
	{
		cout << codes[a]->data << " ";
		cout << codes[a]->huffmancode;
		cout << endl;
	}
}