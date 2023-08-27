#include <iostream>
using namespace std;



template <class T>
class StackADT{
public:
	virtual void clearStack() = 0;
	virtual bool isEmpty() = 0;
	virtual bool isFull() = 0;
	virtual void push(const T &nextitem);
	virtual T top() = 0;
	virtual void pop() = 0;
};

int main(){
	linkedList <int> s1;
	linkedList <int> s2;
	linkedList <int> s3;
	
	s1.push(34);
	s1.push(43);
	s1.push(27);
	
	s3 = s1; //When copying first use clearStack() to clear it to ensure that the s3 is fully empty. Then implement = operator to deep copy;
	
	while(!s3.isempty()){
		cout<<s3.top();
		s3.pop();
	}
	
	s2=s1;
	
	cout<<"After Copy"<<endl;
	testcopy(s2);
	
	return 0;
}
