#include <iostream>
using namespace std;

int sumDigit(int data){
	if(data == 0)
		return 0;
	return data % 10+ sumDigit(data/10);
}


int main(){
	cout<<sumDigit(12345)<<endl;
	return 0;
}
