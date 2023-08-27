#include <iostream>
#include "matrix.h"

using namespace std;

int main(){
	Matrix mm("text.txt");
	mm.print();
	std::cout << "Inverse\n";
	Matrix inverse = mm.inverse();
	inverse.print();
	std::cout << "m + m:\n";
	Matrix sum = mm + mm;
	sum.print();

	Matrix m(2,2);
	m.setEntry(0,0, 10);
	m.setEntry(0,1, 9);
	m.setEntry(1,0, 1);
	m.setEntry(1,1, 1);
	m.print();
	std::cout << "Inverse:\n";
	inverse = m.inverse();
	inverse.print();
	std::cout << mm.submatrix(m) << "\n\n";

	Matrix identity(1,1);
	identity.setEntry(0, 0, 1);
	identity.setEntry(1, 1, 1);
	if (identity * identity == identity)
		std::cout << "TRUE";
	else
		std::cout << "FALSE";

	return 0;
}