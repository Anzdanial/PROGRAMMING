#include <iostream>
using namespace std;

void extractNumToChar(int num){

}

int isPalindrome(int num1, int num2){
	int num3 = num1 * num2;
	string numStr = to_string(num3);
	cout << numStr << endl;
	int length = numStr.length();
	bool flag = true;
	for(int start = 0, end = length - 1; start < end; start++, end--){
		if(numStr[start] != numStr[end]) {
			flag = false;
			break;
		}
	}
	if(flag)
		return num3;
	return 0;
}

int main(){
//	extractNumToChar(1000);
	int val = isPalindrome(91,99);
	return 0;
}