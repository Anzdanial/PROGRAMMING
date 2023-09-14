import java.util.Scanner;

class test <T extends Number>{
    T num1;
    T num2;
    public T Locker(){
        return num1+num2;
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
