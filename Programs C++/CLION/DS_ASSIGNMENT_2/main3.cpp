#include <iostream>
#include <string>
#include <stack>


bool isValid(std::string binstr) {
	std::stack<char> stk;
	char prev = 0;
	for (int i = 0; i < binstr.length(); i++) {
		char ch = binstr[i];
		if (ch == '0') {
			if (prev == '1' && !stk.empty())
				return false;
			stk.push(ch);
		} else if (ch == '1') {
			if (stk.empty())
				return false;
			stk.pop();
		}
		prev = ch;
	}
	return stk.empty();
}


int main() {
	std::string inp;
	std::cout << "Enter binary string to test:\n";
	std::cin >> inp;
	if (isValid(inp))
		std::cout << "Is-Valid\n";
	else
		std::cout << "Is-Not-Valid\n";
	return 0;
}