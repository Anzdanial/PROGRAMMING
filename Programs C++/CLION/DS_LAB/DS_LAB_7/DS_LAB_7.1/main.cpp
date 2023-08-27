#include <iostream>

using namespace std;

int countVowels(char const *str) {
	if (str[0] == '\0')
		return 0;
	int counter = 0;
	if (*str == 'a' || *str == 'A' || *str == 'e' || *str == 'E' || *str == 'i' || *str == 'I' || *str == 'o' ||
	    *str == 'O' || *str == 'u' || *str == 'U') {
		counter++;
		str++;
		if (*str == '\0')
			return counter;
		else
			return counter + countVowels(str);
	} else {
		str++;
		return countVowels(str);
	}
}

int sumArray(int numbers[], int length) {
	if (length <= 0)
		return 0;
	cout << length << endl;
	return (sumArray(numbers, length - 1) + numbers[length - 1]);
}

void printBackward(char const *str, int length) {
	if (str[0] == '\0')
		return;
	if (length < 0)
		return;
	cout << str[length - 1] << endl;
	return printBackward(str, length - 1);
}

bool palindrome(const char *test, int lastIndex) {
	if (test[0] == '\0')
		return true;
	if (lastIndex <= 1)
		return true;

	if(*test == test[lastIndex]) {
		test++;
		lastIndex-=2;
		return palindrome(test, lastIndex);
	}
	else
		return false;

}

int power(int x, int y){
	if(y >= 0) {
		if (y == 0)
			return 1;
		else if (y == 1)
			return x;
		else
			return x * (x , y - 1);
	}
	else
		return 1/(power(x, y));
}

int fibonacciTerm(int num){
	if(num == 0)
		return 0;
	else if(num == 1)
		return 1;
	else
		return fibonacciTerm(num-1) + fibonacciTerm(num-2);
}

int multiplyThruAddition(int m, int n){
	if(n == 0 || m == 0)
		return 0;
	if(n == 1)
		return m;
	else if( m == 1)
		return n;
	else
		return m + multiplyThruAddition(m, n-1);
}

void starPattern(int num){
	if (num > 0) {
		for (int i = 0; i < num; i++)
			std::cout << "*";
		std::cout << "\n";
		starPattern(num - 1);
		for (int i = 0; i < num; i++)
			std::cout << "*";
		std::cout << "\n";
	}
}

/*void starPattern(int num){
	if(num == -num) 
		return;

	if(num > 0) {
		for (int i = 0; i <= num; i++)
			cout<<"*";
		cout<<endl;
		return starPattern(num-1);
	}
	if(num == 0)
		cout<<"*"<<endl;
	if(num < 0) {
		for (int i = num; i >= 0; i++)
			cout<<"*";
		cout<<endl;
		return starPattern(num - 1);
	}
}*/

bool checkPrime(int num){
	return false;
}


int main() {
//	int input;
//	cout<<"Enter the Number for Pattern Generation: ";
//	cin>>input;
//	starPattern(input-1);

	//cout<<multiplyThruAddition(2,16)<<endl;

	const char *name = "abba";
	int length = 0;
	while (name[length] != '\0')
		length++;
	cout<<palindrome(name,length-1)<<endl;
//	name++;
//	cout<<name[0]<<endl;

	//const char *name = "Anas";
	//cout<<countVowels(name)<<endl;

//	int array[4] = {1, 2, 3, 4};
//	cout << sumArray(array, 4) << endl;

	//const char *name = "Anas";
	//int length = 4;
	//printBackward(name, length);
	return 0;
}