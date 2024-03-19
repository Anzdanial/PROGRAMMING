#include <iostream>

template <class T>
class Stack{
private:
    int maxsize;
    int top;
    T *items;

public:
    Stack(){
        items = nullptr;
        maxsize = 0;
        top = 0;
    }


    Stack(int size){
        items = new T(size);
        maxsize = size;
        top = 0;
    }
    void push(T item){
        if(top == maxsize)
            cout<<"Overflow Error Occured"<<endl;
        else {
            items[top] = item;
            top++;
        }
    }

    T pop(){
        if(top == 0)
            cout<<"Underflow Error";
        else{
            T temp = items[top];
            top--;
            return temp;
        }
    }

    int currentSize(){
        return top;
    }

    int maxSize(){
        return maxsize;
    }

    ~Stack();
};


int main() {
    std::cout << "Hello, World!" << std::endl;
    return 0;
}
