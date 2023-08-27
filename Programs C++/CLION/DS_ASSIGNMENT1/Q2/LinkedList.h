#include "Node.h"
#include<iostream>

class LinkedList{
public:
    LinkedList();
    ~LinkedList();

    void insert(int value);
    friend std::ostream& operator <<(std::ostream& ostr, const LinkedList& rhs);
    void del();
    void mergeList(const LinkedList&);
    Node* gethead();
    void compress();


private:
    Node *head;
};
