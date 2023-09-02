import java.util.Scanner;

public class Main {
    static void checkPalindrome(int [] array, int length) {
        int tag = 0;
        for (int i = 0; i <= length / 2 && length != 0; i++) {
            if (array[i] != array[length - i - 1]) {
                tag = 1;
                break;
            }
        }
        if (tag == 1)
            System.out.println("Not Palindrome");
        else
            System.out.println("Palindrome");
    }
    static void transpose(int [][] A, int [][] B, int size) {
        int i, j;
        for (i = 0; i < size; i++)
            for (j = 0; j < size; j++)
                B[i][j] = A[j][i];
    }

    public static void main(String[] args) {
    //Question 1
    int number;
    System.out.println("Enter the Number to Formulate the Multiplication Table: ");
    Scanner input = new Scanner(System.in);
    number = input.nextInt();
    for(int i = 0; i < 10; i++){
        System.out.println(number + "x" + (i+1) + "=" + number*(i+1));
    }

    //Question 2

    int [] numbers = {1, 2, 3, 4, 5};

    int rotate = 2;
    System.out.println("Given array is: ");
    for (int i = 0; i < numbers.length; i++) {
        System.out.print(numbers[i] + " ");
    }
    for(int i = 0; i < rotate; i++){
        int j, end;
        end = numbers[numbers.length-1];
        for(j = numbers.length-1; j > 0; j--){
            numbers[j] = numbers[j-1];
        }
        numbers[0] = end;
    }
    System.out.println();
    System.out.println("Array after "+ rotate +" right rotations: ");
    for(int i = 0; i< numbers.length; i++){
        System.out.print(numbers[i] + " ");
    }

    //Question 3

    int number, sum, temp;
    Scanner input = new Scanner(System.in);
    number = input.nextInt();
    if(number > 0 && number < 1000){
        if(number < 10)
            System.out.println(number);
        if(number > 10 && number < 100) {
            sum = (number % 10) + (number / 10);
            System.out.println(sum);
        }
        if(number > 100 && number < 1000){
            temp = number / 10;
            sum = temp / 10 + temp % 10 + number % 10;
            System.out.println(sum);
        }
    }
    else
        System.out.println("Out of Range");



    //Question 4
    int length = 3;
    int A[][] = { { 1, 2, 3},
                { 4, 5, 6 },
                { 7, 8, 9 }};

    int B[][] = new int[length][length], i, j;

    // Function call
    transpose(A, B, length);

    System.out.print("Result matrix is \n");
    for (i = 0; i < length; i++) {
        for (j = 0; j < length; j++)
            System.out.print(B[i][j] + " ");
        System.out.println();
    }


    //Question 5
    //F LETTER

    for(int i = 0; i < 10; i++){
        System.out.print("*");
    }
    System.out.println();
    for(int i = 0; i < 4; i++){
        System.out.println("*");
    }
    for(int i = 0; i < 10; i++){
        System.out.print("*");
    }
    System.out.println();
    for(int i = 0; i < 4; i++){
        System.out.println("*");
    }
    System.out.println();
    System.out.println();


    //A LETTER
    for(int i = 0; i < 10; i++){
        System.out.print("*");
    }
    System.out.println();
    for(int i = 0; i < 4; i++){
        System.out.print("*");
        for(int j = 0; j < 8; j++){
            System.out.print(" ");
        }
        System.out.print("*");
        if(i != 3)
            System.out.println();
    }
    System.out.println();

    for(int i = 0; i < 10; i++){
        System.out.print("*");
    }
    System.out.println();
    for(int i = 0; i < 4; i++){
        System.out.print("*");
        for(int j = 0; j < 8; j++){
            System.out.print(" ");
        }
        System.out.print("*");
        if(i != 3)
            System.out.println();
    }
    System.out.println();
    System.out.println();
    System.out.println();

    //S Letter
    for(int i = 0; i < 10; i++){
        System.out.print("*");
    }
    System.out.println();
    for(int i = 0; i < 4; i++){
        System.out.println("*");
    }
    for(int i = 0; i < 10; i++){
        System.out.print("*");
    }
    System.out.println();
    for(int i = 0; i < 3; i++){
        for(int j = 0; j < 9; j++){
            System.out.print(" ");
        }
        System.out.println("*");
    }
    for(int i = 0; i < 10; i++){
        System.out.print("*");
    }
    System.out.println();
    System.out.println();

    //T Letter
    for(int i = 0; i < 10; i++){
        System.out.print("*");
    }
    System.out.println();

    for(int k = 0; k < 9; k++) {
        for (int i = 0; i < 1; i++) {
            for (int j = 0; j < 4; j++) {
                System.out.print(" ");
            }
            System.out.println("*");
            for (int z = 0; z < 4; z++) {
                System.out.print(" ");
            }
        }
        System.out.println();
    }
    System.out.println();
    System.out.println();


    //Question 6

    int [] array = {1,2,3,4,5};
    int sum = 0;
    for(int i = 0; i < array.length; i++){
        if(array[i] % 2 != 0)
            sum += array[i];
    }
    System.out.println(sum);


    //Question 7
    System.out.println("Enter the Operation to Perform: '*' '+' '-' '/' ");
    Scanner input = new Scanner(System.in);
    String command = input.nextLine();
    System.out.println("Enter the First Number (Dividend): ");
    float num1;
    input = new Scanner(System.in);
    num1 = input.nextFloat();
    System.out.println("Enter the Second Number (Divisor): ");
    float num2;
    input = new Scanner(System.in);
    num2 = input.nextFloat();
    if(command.equals("+")){
        System.out.println(num1+num2);
    }
    if(command.equals("*")){
        System.out.println(num1*num2);
    }
    if(command.equals("-")){
        System.out.println(num1-num2);
    }
    if(command.equals("/")){
        System.out.println(num1/num2);
    }

    //Question 8
	int [] array = { 1, 2, 3, 2, 1 };
    int size = array.length;
    checkPalindrome(array, size);

    }
}