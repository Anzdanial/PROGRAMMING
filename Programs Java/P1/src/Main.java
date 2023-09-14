import java.util.Scanner;

class test <T>{
    public <T extends Number> T add(T num1, T num2) {
        return num1 + num2; // This will work because T is guaranteed to be a Number.
    }
}

public class Main {
    public static void main(String[] args) {
        Scanner input = new Scanner(System.in);
        int readNumber = -1;
        try {
            readNumber = Integer.parseInt(input.nextLine());
        }
        catch(Exception e){
            System.out.println("");
        }
    }
}
