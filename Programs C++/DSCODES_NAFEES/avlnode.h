#ifndef NODE_H
#define NODE_H

#include <iostream>

struct Node{
	int value;
	Node *right, *left;

	Node() : value(0), right(nullptr), left(nullptr) {};
};

static Node *nodeCopy(Node *node){
	if (node == nullptr)
		return nullptr;
	Node *ret = new Node;
	ret->value = node->value;
	ret->left = nodeCopy(node->left);
	ret->right = nodeCopy(node->right);
	return ret;
}

static void nodeDelete(Node *node){
	if (node == nullptr)
		return;
	// Left
	if (node->left)
		nodeDelete(node->left);
	// Right
	if (node->right)
		nodeDelete(node->right);
	// V
	delete node;
	node = nullptr;
}

static int nodeHeight(Node *node){
	if (node == nullptr)
		return 0;
	int leftH = nodeHeight(node->left);
	int rightH = nodeHeight(node->right);
	if (leftH > rightH)
		return 1 + leftH;
	return 1 + rightH;
}

static int getBalance(Node *node){
	if (node == nullptr)
		return 0;
	return nodeHeight(node->left) - nodeHeight(node->right);
}

/// to fix RR
static void rotateRR(Node *&root){
	Node *a = root, *b = root->right, *c = b->right;
	a->right = b->left;
	b->left = a;
	b->right = c;
	root = b;
}

/// to fix LL
static void rotateLL(Node *&root){
	Node *a = root, *b = root->left, *c = b->left;
	a->left = b->right;
	b->right = a;
	b->left = c;
	root = b;
}

/// convert RL to RR
static void rotateRLtoRR(Node *&root){
	Node *a = root, *b = root->right, *c = b->left;
	c->right = b;
	b->left = nullptr;
	a->right = c;
}

/// convert LR to LL
static void rotateLRtoLL(Node *&root){
	Node *a = root, *b = root->left, *c = b->right;
	c->left = b;
	b->right = nullptr;
	a->left = c;
}

/// Returns: `'R'` if inserted on right subtree, or `'L'` if left. `'\0'` if
/// not inserted
static char insertBalanced(Node *&root, int value){
	if (root == nullptr){
		root = new Node;
		root->value = value;
		return 1;
	}
	if (value == root->value)
		return 0;
	if (value < root->value){
		char insDir = insertBalanced(root->left, value);
		int balance = getBalance(root);
		if (balance >= -1 && balance <= 1)
			return 'L';
		if (insDir == 'R')
			rotateLRtoLL(root);
		rotateLL(root);
		return 'L';
	}else{
		char insDir = insertBalanced(root->right, value);
		int balance = getBalance(root);
		if (balance >= -1 && balance <= 1)
			return 'R';
		if (insDir == 'L')
			rotateRLtoRR(root);
		rotateRR(root);
		return 'R';
	}
	return 0;
}

static bool insert(Node *&root, int value){
	if (root == nullptr){
		root = new Node;
		root->value = value;
		return true;
	}
	if (value == root->value)
		return false;
	if (value < root->value)
		return insert(root->left, value);
	return insert(root->right, value);
}

static void inorderPrint(const Node *root){
	if (root == nullptr)
		return;
	inorderPrint(root->left);
	std::cout << root->value << " ";
	inorderPrint(root->right);
}

static void inorderTraverse(const Node *root, void (*func)(const Node*)){
	if (root == nullptr)
		return;
	inorderTraverse(root->left, func);
	func(root);
	inorderTraverse(root->right, func);
}

static bool levelorderPrint(const Node *root, int level){
	if (root == nullptr){
		return false;
	}
	if (level == 1){
		std::cout << root->value << " ";
		return true;
	}
	if (level > 1){
		return levelorderPrint(root->left, level - 1) |
			levelorderPrint(root->right, level - 1);
	}
	return false;
}

static void levelorderPrint(const Node *root){
	int level = 1;
	bool ret = false;
	do{
		ret = ::levelorderPrint(root, level);
		std::cout << "\n";
		level ++;
	} while (ret);
	std::cout << "\n";
}

static bool isValidBST(const Node *root){
	if (root == nullptr)
		return true;
	if (root->right && root->value >= root->right->value)
		return false;
	if (root->left && root->value <= root->left->value)
		return false;
	return true;
}

static void debugPrint(const Node *root){
	if (root == nullptr)
		return;
	if (root->left == nullptr)
		std::cout << "Ø <- ";
	else
		std::cout << root->left->value << " <- ";
	std::cout << root->value << " -> ";
	if (root->right == nullptr)
		std::cout << "Ø";
	else
		std::cout << root->right->value;
}

static void htmlPrint(const Node *root){
	if (root == nullptr)
		return;
	static int depth = 0;
	depth ++;
	if (depth == 1){
		std::cout << "<div style=\"display: inline-block; margin: 10px;\">";
		inorderPrint(root);
	}
	std::cout << "<table style=\"border: solid; text-align: center;\"><tr><td colspan=2>";
	std::cout << root->value << "</td></tr>";
	if (root->left || root->right){
		std::cout << "<tr><td>";
		if (root->left)
			htmlPrint(root->left);
		else
			std::cout << "Ø";
		std::cout << "</td><td>";
		if (root->right)
			htmlPrint(root->right);
		else
			std::cout << "Ø";
	}
	std::cout << "</td></tr></table>";
	if (depth == 1){
		std::cout << "</div>";
	}
	depth --;
}

static Node *search(Node *root, int key){
	if (root == nullptr)
		return nullptr;
	if (root->value == key)
		return root;
	Node *ret = search(root->left, key);
	if (ret == nullptr)
		return search(root->right, key);
	return ret;
}

static int countNodes(const Node *root){
	if (root == nullptr)
		return 0;
	// Left, +1, Right
	return countNodes(root->left) + 1 + countNodes(root->right);
}

static int countLeafNodes(const Node *root){
	if (root == nullptr)
		return 0;
	if (root->left == nullptr && root->right == nullptr)
		return 1; // V
	// Left, Right
	return countLeafNodes(root->left) + countLeafNodes(root->right);
}

#endif
