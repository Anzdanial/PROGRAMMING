#include <iostream>
using namespace std;

template<class T>
class stack {
	T array[100];
	int top;
public:
	stack() {
		top = 0;
	}

	void push(int val) {
		//Base Case for evaluating if the stack is full. (Not needed in Dynamic Arrays)
		if (isFull())
			cout << "Stack OverFlow" << endl;
		else {
			//General Case for pushing the value onto the stack, by iterating the top pointer.
			array[top] = val;
			top++;
		}
	}

	T pop() {
		//Base Case, evaluating if the stack is already empty to stop popping.
		if (isEmpty()) {
			cout << "Stack already empty" << endl;
			return 0;
		} else {
			//General case, to decrement the pointer, and output it to the function.
			top--;
			return array[top];
		}
	}

	void print() {
		//Printing the stack as per the Last entry to first.
		for (int i = top - 1; i >= 0; i--)
			cout << array[i] << endl;
	}

	bool isEmpty() {
		return (top == -1);
	}

	bool isFull() {
		return (top == 100);
	}

	//Getter Function for retreiving top pointer.
	int getTop() {
		return top;
	}

	//TASK#1
	void convertString() {
		//The function is called/used to convert an existing stack into string.
		int length = getTop();
		char string[length];

		//Due to the stack being LIFO, the stack is iterated in reverse and saved in string per character.
		for (int i = length-1; i >= 0; i--)
			string[i] = pop();

		//String is Printed.
		for(int i=0; i<length; i++)
			cout<<string[i];
		cout<<endl;
	}

	//TASK #2
	void stackToReverseString(){
		//When a stack is transferred, from one to another, the pushed values are reversed and hence are saved in the string.
		//A temporary stack is made to reverse the stack.
		stack <char> temp;
		//Stack values are popped and pushed into the temporary stack
		for(int i = top-1; i >= 0; i--)
			temp.push(pop());
		//The temporary stack containing the reverse stack, is then saved in a string using convertString.
		temp.convertString();
	}

};

//TASK #2
char *reverseStringAndRemoveNonAlpha(char *string) {
	//The ASCII conditions are used to negate spaces, punctuations and symbols.
	stack<char> s;
	for (int i = 0; string[i] != '\0'; i++) {
		if ((string[i] > 47 && string[i] < 58) || (string[i] > 64 && string[i] < 91) ||
		    (string[i] > 96 && string[i] < 123))
			//Only Alphanumeric characters are pushed to stack.
			s.push(string[i]);
	}

	for (int i = 0; string[i] != '\0'; i++)
		string[i] = '\0';

	//Stack is popped and reverse values are then stored in the string.
	for (int i = (s.getTop() - 1), counter = 0; i >= 0; i--, counter++) {
		string[counter] = s.pop();
	}
	return string;
}

//TASK #3
char *reverseString(char *string) {
	stack<char> s;
	//The string is pushed into the stack for reversal due to the property of LIFO.
	for (int i = 0; string[i] != '\0'; i++)
		s.push(string[i]);

	//The original string is initialized to NULL for storing the reverse stack, so that overiding for data doesn't occur.
	for (int i = 0; string[i] != '\0'; i++)
		string[i] = '\0';

	//The stack is popped and stored in the string.
	for (int i = (s.getTop() - 1), counter = 0; i >= 0; i--, counter++) {
		string[counter] = s.pop();
	}
	return string;
}

//TASK #3
//The palindrome is being evaluated by comparing the original string and its reverse string, if they match, then PALINDROME!!
bool isPalindrome(char *pString) {
	bool flag = false;
	for (int i = 0; pString[i] != '\0'; i++) {
		//Original vs Reverse
		if (pString[i] != reverseString(pString)[i])
			return false;
		else
			flag = true;
	}
	return flag;
}



int main() {
	//TASK#1
	stack <char> s1;
	s1.push('a');
	s1.push('n');
	s1.push('a');
	s1.push('s');
	s1.stackToReverseString();

	//TASK #2

	/*char palindrome[] = "abba";
	if (isPalindrome(palindrome))
		cout << "Is Palindrome" << endl;
	else
		cout << "Is not Palindrome" << endl;*/


	//TASK #3

	/*char email[] = "!!anas";
	char *string1;
	string1 = reverseStringAndRemoveNonAlpha(email);
	for(int i=0; string1[i]!='\0'; i++)
		cout<<string1[i];
	cout<<endl;*/

	return 0;
}