#include <iostream>
using namespace std;

int **compress(int** image, int rows, int columns, int value){
    int counter = 0, zeroes = 0;
    while(counter != rows){
		for(int i=0; i<columns; i++){
			if(image[counter][i] == 0)
				zeroes++;
		}
		counter++;
	}
}

int main()
{
    int rows, cols;
    int **input;
    cout<<"Enter the Rows: ";
    cin>>rows;
    cout<<"Enter the Cols: ";
    cin>>cols;

	compress(input, rows, cols);
	
	
    cout<<"Hello"<<endl;
}


First run through the loop first time and check for zeroes.
After evaluating for first run, increase the counter by the amount of 0's encountered.
Create a new dynamic sized array by the number of countered indexes.
Keep repeating until you reach the end of the image.
