#include <iostream>
#include <string.h>
using namespace std;

char stack[50];
int top=-1;

int checkPrecedence(char sym){
	if (sym=='^')
		return 3;
	else if(sym=='/' || sym== '*')
		return 2;
	else if (sym=='+' || sym== '-')
		return 1;
	else
		return -1;
}

void push(char item){
	top++;
	if(top>50){
		cout<<"\nStack overflow"<<endl;
		return;
	}
	stack[top]=item;
}


char Top(){
	char c=stack[top];
	return c;
}

bool isEmpty(){
	if(top==-1)
		return true;
	else
		return false;
}

void pop(){
	char s1[top];
	for(int i=0; i<top-1; i++)
		s1[i]=stack[i];
	strcpy(stack,s1);
	top--;
}

void infixToPostfix(string s){
	string postfix="";
	for(int i=0 ; i<s.size();i++ ){
		char c=s[i];
		if((c>='a' && c<='z') || (c>='A' && c<='Z') || (c>='0' && c<='9'))
			postfix+=c;
		else if (c=='(')
			push('(');
		else if (c==')'){
			while (stack[top] != '('){
				postfix+=stack[top];
				pop();
			}
			pop();
		}
		else{
			int len=top;
			while((len!=0 && checkPrecedence(s[i])) <= checkPrecedence(stack[top])){
				if(c=='^' && stack[top] != '^'){
					len--;
					break;
				}
				else{
					postfix+=stack[top];
					len--;
				}
			}
			push(c);
		}
	}
	cout<<"\nPostfix conversion: "<<postfix;
}


int main()
{
	string postfix;
	cout<<"Enter the expression: ";
	cin>>postfix;
	infixToPostfix(postfix);
	return 0;
}
