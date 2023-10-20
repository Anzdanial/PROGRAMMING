#include <iostream>
using namespace std;

class UndoStack{
private:
    int stack[101];
    int top;
    int temp;
public:
    UndoStack(){
        top = -1;
    }
    void push(int val){
        if(top == 100){
            temp = val;
            pop();
        }
        else {
            top++;
            stack[top] = val;
        }
    }
    int pop(){
        if(top == 100){
            int startVal = stack[0];
            for (int i = 0; i < 100; i++) {
                stack[i] = stack[i + 1];
            }
            stack[top] = temp;
            top--;
            return startVal;
        }
        else {
            int poppedValue = stack[top];
            top--;
            return poppedValue;
        }
    }
    int peek(){
       return stack[top];
    }
    void display(){
        for(int i = 0; i < top+1; i++){
            cout << stack[i] << " ";
        }
    }
};

int main() {
    UndoStack undoStack;
    for(int i = 0; i < 101; i++){
        undoStack.push(i);
    }
    cout<< "The value is " << undoStack.pop()<<endl;
    undoStack.display();

    return 0;
}
