#include <iostream>
using namespace std;

class Node {

public:
	int data;
	Node* left;
	Node* right;
	Node(int d = 0) {
		data = d;
		left = right = NULL;
	}
};

class AVL
{
	Node* root;
	void preOrder(Node* rootTemp);
	static void CurrentLevelprintG(Node* root, int level);
	Node* balanceNodes(Node* node);
	Node* insertHelper(Node* node, int key);
	Node* leftRotate(Node* node);
	Node* rightRotate(Node* node);
	int heightFind(Node* roorTemp);
	int height();
public:
	AVL() { root = NULL; }
	void CurrentLevelprint();
	int getBalance(Node* node);
	void insert(int key);
	void preOrderPrint();
};

int main() {

	AVL T;
	T.insert(0);
	T.insert(1);
	T.insert(2);
	T.insert(3);
	T.insert(4);
	T.insert(5);
	T.insert(6);
	T.insert(7);
	T.insert(8);
	T.insert(9);
	T.insert(10);
	T.insert(11);
	T.insert(12);
	T.insert(13);
	T.insert(14);
	T.insert(15);

	T.CurrentLevelprint();
	cout << endl;

	T.preOrderPrint();
}

int AVL::heightFind(Node* rootTemp) {

	if (!rootTemp)
		return 0;

	return 1 + max(heightFind(rootTemp->left), heightFind(rootTemp->right));
}

int AVL::height() {
	return heightFind(root);
}

Node* AVL::insertHelper(Node* node, int key) {

	if (node == NULL) {
		Node* newNode = new Node(key);
		node = newNode;
		return node;
	}
	else if (key < node->data) {
		node->left = insertHelper(node->left, key);
		node = balanceNodes(node);
	}
	else {
		node->right = insertHelper(node->right, key);
		node = balanceNodes(node);
	}
	return node;
}

void AVL::insert(int key) {

	if (root == NULL) {
		Node* newNode = new Node(key);
		root = newNode;
		return;
	}

	root = insertHelper(root, key);

}

int AVL::getBalance(Node* node) {

	if (!node)
		return 0;

	int left = heightFind(node->left);
	int right = heightFind(node->right);

	return right - left;
}

Node* AVL::balanceNodes(Node* node) {

	int balance = getBalance(node);

	if (balance >= -1 && balance <= 1)
		return node;

	if (balance < -1) {

		node = rightRotate(node);
	}
	else if (balance > 1) {

		node = leftRotate(node);
	}

	return node;
}

Node* AVL::leftRotate(Node* node) {

	Node* temp = node->right;
	node->right = temp->left;
	temp->left = node;
	cout << "Left-Rotation" << endl;
	return temp;
}

Node* AVL::rightRotate(Node* node) {

	Node* temp = node->left;
	node->left = temp->right;
	temp->right = node;

	cout << "Right-Rotation" << endl;
	return temp;
}

void AVL::CurrentLevelprintG(Node* root, int level) {

	if (!root)
		return;

	if (level == 0) {
		cout << root->data << " ";
		return;
	}

	CurrentLevelprintG(root->left, level - 1);
	CurrentLevelprintG(root->right, level - 1);
}

void AVL::CurrentLevelprint() {

	int heightS = height();

	for (int i = 0; i < heightS; i++) {

		CurrentLevelprintG(root, i);
		cout << endl;
	}
	cout << endl;
}

void AVL::preOrderPrint() {

	preOrder(root);
	cout << endl;
}

void AVL::preOrder(Node* rootTemp) {

	if (!rootTemp)
		return;

	cout << rootTemp->data << " ";
	preOrder(rootTemp->left);
	preOrder(rootTemp->right);
}

//
//
//
//
//Dump Code
//Node* AVL::balanceNodes(Node* node) {
//
//	int balance = getBalance(node);
//
//	if (balance >= -1 && balance <= 1)
//		return node;
//
//	if (balance < -1) {
//
//		if (getBalance(node->right) > 0) {
//			node = rightleftRotate(node);
//		}
//		else {
//			node = rightRotate(node);
//		}
//	}
//	else if (balance > 1) {
//
//		if (getBalance(node->left) > 0) {
//			node = leftrightRotate(node);
//		}
//		else {
//			node = leftRotate(node);
//		}
//	}
//
//	return node;
//}

//Node* AVL::leftrightRotate(Node* node) {
//
//	Node* temp = node->left;
//	node->left = rightRotate(temp);
//
//	cout << "Left-Right-Rotation" << endl;
//	return leftRotate(node);
//}
//
//Node* AVL::rightleftRotate(Node* node) {
//
//	Node* temp = node->right;
//	node->right = leftRotate(temp);
//	cout << "Right-Left-Rotation" << endl;
//	return rightRotate(node);
//}