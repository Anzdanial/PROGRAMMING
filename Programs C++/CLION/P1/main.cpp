#include <iostream>
using namespace std;

void reversePrint(const char *string){
	if(*string == '\0')
		return;
	reversePrint(string+1);
	cout<<*string<<endl;
}

int main(){
	char *str = "Normal or Reverse";
	reversePrint(str);
	cout<<endl;
	return 0;
}