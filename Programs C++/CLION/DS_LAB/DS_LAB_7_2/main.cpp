#include <iostream>
using namespace std;

int stringCompare(char const* string1, char const* string2) {

	if (*string1 == 0  && *string2 == 0)
		return 0;

	if (*string1 == 0 || *string2 == 0)
		return -1;

	int r = stringCompare(string1 + 1, string2 + 1);

	if (r == 0) {
		if (*string1 == *string2) {

			return 0;
		}
		else if (*string1 != *string2) {

			if (*string1 > *string2)
				return 1;
			else
				return -1;
		}
	}
	else
		return r;
}

int main() {

	cout << stringCompare("ab", "abC");
}