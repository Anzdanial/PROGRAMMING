#include <iostream>
using namespace std;

struct node
{
    int data;
    node *left, *right;
    node(){
        data = 0;
        left = right = nullptr;
    }
    node(int data){
        this->data = data;
        left = right = nullptr;
    }
};

class Tree
{
private:
    node *root;

public:
    Tree()
    {
        root->data = 0;
        root->left = NULL;
        root->right = NULL;
    }
    Tree(int data, node *left, node *right){
        root->data = data;
        root->left = left;
        root->right = right;
    }
    node* getRoot(){
        return root;
    }
    void printTree(node *root){
        if(root == NULL)
            return;
        printTree(root->left);
        printTree(root->right);
        cout<<root->data<<endl;
    }
};

int main(){
    Tree tree(1, new node(2), new node(3));
    tree.printTree(tree.getRoot());
    return 0;
}