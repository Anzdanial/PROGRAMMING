import java.util.Scanner;
import java.util.Random;


public class Main{
    static void printMatrix(int columns, int rows){
        for(int i = 0; i < 10; i++){
            for(int j = 0; j < 10; j++){
                if(i == rows && j == columns)
                    System.out.print("X ");
                else
                    System.out.print(0 + " ");
            }
            System.out.println();
        }
    }
    static public void main(String []args) {
        int rows = 0, columns = 0, counter;
        Random random = new Random();
        while(true){
            counter = random.nextInt(6) + 1;
            rows = rows + counter;

            if(rows > 9){
                rows = rows % 10;
                columns = columns + 1;
            }

            if(columns > 9){
                columns  = columns % 10;
            }



            System.out.println("Counter " + counter + ", rows: " + rows& + ", columns: " + columns);

            printMatrix(rows,columns);

            if(rows == 9 && columns == 9)
                break;
        }
    }
}