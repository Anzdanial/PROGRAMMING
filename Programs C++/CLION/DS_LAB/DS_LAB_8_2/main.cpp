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

class BST {
	Node* root;
	static void inOrderPrint(Node* rootTemp) {

		if (!rootTemp) {
			return;
		}

		cout << rootTemp->data << " ";
		inOrderPrint(rootTemp->left);
		inOrderPrint(rootTemp->right);
	}
	static void CurrentLevelprintG(Node* root, int level) {

		if (!root)
			return;

		if (level == 0) {
			cout << root->data << " ";
			return;
		}

		CurrentLevelprintG(root->left, level - 1);
		CurrentLevelprintG(root->right, level - 1);
	}
	static int heightFind(Node* rootTemp) {

		if (!rootTemp)
			return 0;

		return 1 + max(heightFind(rootTemp->left), heightFind(rootTemp->right));
	}
	static Node* searchNode(Node* rootTemp, int key) {

		if (!rootTemp) {

			return NULL;
		}

		if (key == rootTemp->data) {
			return rootTemp;
		}
		if (key < rootTemp->data) {
			return searchNode(rootTemp->left, key);
		}
		else {
			return searchNode(rootTemp->right, key);
		}
	}
	static int countNodesPrivate(Node* rootTemp) {

		if (!rootTemp)
			return 0;

		return 1 + countNodesPrivate(rootTemp->left) + countNodesPrivate(rootTemp->right);
	}
	static int countLeafNodesPrivate(Node* rootTemp) {

		if (!rootTemp)
			return 0;
		else if (!rootTemp->left && !rootTemp->right)
			return 1;

		return countLeafNodesPrivate(rootTemp->left) + countLeafNodesPrivate(rootTemp->right);
	}
	static void destructor(Node*& rootTemp) {

		if (!rootTemp)
			return;

		destructor(rootTemp->left);
		destructor(rootTemp->right);
		delete rootTemp;
	}
	static Node* copyNode(Node* node) {

		if (!node)
			return NULL;

		Node* newNode = new Node(node->data);
		newNode->left = copyNode(node->left);
		newNode->right = copyNode(node->right);
		return newNode;
	}
public:
	BST() { root = NULL; }
	BST(const BST& obj) {

		this->root = copyNode(obj.root);
	}
	bool insert(int value) {

		if (root == NULL) {
			Node* newNode = new Node(value);
			root = newNode;
			return 1;
		}

		Node* rootTemp = root;

		while (rootTemp != NULL) {

			if(value == rootTemp->data){
				return 0;
			}
			if (value < rootTemp->data) {

				if (rootTemp->left == NULL) {
					Node* newNode = new Node(value);
					rootTemp->left = newNode;
					return 1;
				}

				rootTemp = rootTemp->left;
			}
			else {

				if (rootTemp->right == NULL) {
					Node* newNode = new Node(value);
					rootTemp->right = newNode;
					return 1;
				}
				rootTemp = rootTemp->right;
			}
		}
	}
	void inOrder() {
		inOrderPrint(root);
		cout << endl;
	}
	int height() {
		return heightFind(root);
	}
	void CurrentLevelprint() {

		int heightS = height();

		for (int i = 0; i < heightS; i++) {

			CurrentLevelprintG(root, i);
			cout << endl;
		}
		cout << endl;
	}
	Node* search(int key) {
		return searchNode(root, key);
	}
	int countNodes() {
		return countNodesPrivate(root);
	}
	int countLeafNodes() {
		return countLeafNodesPrivate(root);
	}
	~BST() {
		destructor(root);
	}
};

int main() {

	BST B;

	/*B.insert(8);
	B.insert(5);
	B.insert(3);
	B.insert(20);
	B.insert(4);
	B.insert(7);
	B.insert(11);
	B.insert(30);*/

	B.insert(10);
	B.insert(20);
	B.insert(30);
	B.insert(40);
	B.insert(50);
	B.insert(25);
	B.insert(9);


	B.inOrder();
	B.CurrentLevelprint();
	cout << B.search(7)->data << endl;
	cout << B.countNodes() << endl;
	cout << B.countLeafNodes() << endl;

	cout << endl << endl << "Testing on Copied" << endl << endl;

	BST A(B);

	A.inOrder();
	A.CurrentLevelprint();
	cout << A.search(7)->data << endl;
	cout << A.countNodes() << endl;
	cout << A.countLeafNodes() << endl;

	BST C;

	C.insert(8);
	cout << C.countLeafNodes();
}