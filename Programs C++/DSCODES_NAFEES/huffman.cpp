#include "heap.h"
#include "hashmap.h"
#include <iostream>
#include "bitstring.h"
#include <fstream>
#include <vector>
#include <string>

void countCharFreq(const char *filename, int freq[256]){
	// reset counts
	for (int i = 0; i < 256; i ++)
		freq[i] = 0;
	std::ifstream file(filename);
	if (!file)
		return;
	while (!file.eof()){
		char ch;
		file.get(ch);
		freq[ch] ++;
	}
	file.close();
}

void readCharFreq(std::ifstream &fileIn, int freq[256]){
	// reset counts
	for (int i = 0; i < 256; i ++)
		freq[i] = 0;
	int count = 0;
	fileIn >> count;
	for (int i = 0; i < count; i ++){
		int ch, chFreq;
		fileIn >> ch >> chFreq;
		freq[ch] = chFreq;
	}
}

struct Node{
	Node *left;
	Node *right;
	char ch;
	int freq;
	Node() : left(nullptr), right(nullptr), ch(0), freq(0) {}
	Node(char c, int f) : left(nullptr), right(nullptr), ch(c), freq(f) {}
};

bool operator>(const Node& lhs, const Node& rhs){
	return lhs.freq > rhs.freq;
}

Node* constructHuffmanTree(const int freq[256]){
	MinPtrHeap<Node*> heap;
	// read nodes into heap
	for (int i = 0; i < 256; i ++){
		if (freq[i] == 0)
			continue;
		heap.insert(new Node((char)i, freq[i]));
	}
	if (heap.isEmpty())
		return nullptr;
	while (heap.count() > 1){
		Node *parent = new Node();
		parent->left = heap.getMin();
		heap.deleteMin();
		parent->right = heap.getMin();
		heap.deleteMin();
		parent->freq = parent->left->freq + parent->right->freq;
		heap.insert(parent);
	}
	return heap.getMin();
}

void generateHuffmanTable(Node *node, HashMap<BitString> &table, BitString code = {}){
	if (node == nullptr)
		return;
	if (!node->left && !node->right){
		table.set(node->ch, code);
	}else{
		generateHuffmanTable(node->left, table, code + false);
		generateHuffmanTable(node->right, table, code + true);
	}
}

std::string binToStr(unsigned int num, uint8_t bits = 8){
	std::string ret;
	for (int i = bits - 1; i >= 0; i --)
		ret += 48 + ((num >> i) & 1);
	return ret;
}

void printTable(const HashMap<BitString> &table, int freq[256]){
	std::cout << "ASCII\tChar\tFreq\tCode\n";
	for (int i = 0; i < 256; i ++){
		const BitString *code = table.get(i);
		if (code)
			std::cout << i << "\t" << (char)i << "\t" << freq[i] << "\t" << 
				code->toString() << "\n";
	}
}

bool encode(const char *in, const char *out, const char *outBig){
	int freq[256];
	countCharFreq(in, freq);
	Node *node = constructHuffmanTree(freq);
	HashMap<BitString> table;
	generateHuffmanTable(node, table);
	printTable(table, freq);

	std::ifstream fileIn(in);
	std::ofstream fileOut(out), fileOutBig(outBig);
	if (!fileIn || !fileOut || !fileOutBig)
		return false;

	// put char & freq in output file
	int count = 0;
	for (int i = 0; i < 256; i ++)
		count += freq[i] != 0;
	fileOut << count << " ";
	for (int i = 0; i < 256; i ++){
		if (freq[i] != 0)
			fileOut << i << " " << freq[i] << " ";
	}

	while (true){
		char ch;
		fileIn.get(ch);
		if (fileIn.eof())
			break;
		const BitString *codePtr = table.get(ch);
		if (!codePtr){
			std::cerr << "Failed to find code for ascii " << (int)ch << "\n";
			exit(1);
		}
		fileOut << codePtr->toString();
		fileOutBig << binToStr(ch);
	}
	fileIn.close();
	fileOut.close();
	fileOutBig.close();
	return true;
}

bool decode(const char *filename, const char *out){
	int freq[256];
	std::ifstream fileIn(filename);
	std::ofstream fileOut(out);
	if (!fileIn || !fileOut)
		return false;

	readCharFreq(fileIn, freq);
	char ch;
	fileIn.get(ch);
	Node *tree = constructHuffmanTree(freq);
	Node *node = tree;
	while (true){
		fileIn.get(ch);
		if (fileIn.eof())
			break;
		if (ch == '0'){
			node = node->left;
		}else if (ch == '1'){
			node = node->right;
		}else{
			return false;
		}
		if (node->left == nullptr && node->right == nullptr){
			fileOut << node->ch;
			node = tree;
		}
	}
	fileIn.close();
	fileOut.close();
	return true;
}

int main(){
	if (encode("k.txt", "k.huff.txt", "k.bin.txt"))
		std::cout << "encode returned true\n";
	else
		std::cout << "encode returned false\n";

	if (decode("k.huff.txt", "k.decode.txt"))
		std::cout << "decode returned true\n";
	else
		std::cout << "deocde returned false\n";

	return 0;
}

