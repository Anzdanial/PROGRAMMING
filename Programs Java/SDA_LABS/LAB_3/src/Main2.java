import java.util.Scanner;
class Passenger{
    private Elevator elevator;
    private Button button;
    private buttonType buttonT;
    private int floorNum;
    private int departureFloor;
    private int arrivalFloor;
    public Passenger(){
        elevator = new Elevator();
        button = new Button();
        floorNum = 0;
        arrivalFloor = 1;
        departureFloor = 0;
    }
    public void callElevator(){
        Scanner input = new Scanner(System.in);
        System.out.println("Enter the Departure Floor: ");
        departureFloor = input.nextInt();
        button.turnLightOn();
        buttonT = buttonType.External;
        System.out.println("Presses External Button");
        elevator.openElevatorDoor();
        System.out.println("Elevator Arrived");
        elevator.inElevator();
        floorNum = departureFloor;
        selectDestinationFloor();
    }
    public void selectDestinationFloor(){
        Scanner input = new Scanner(System.in);
        System.out.println("Enter the Destination Floor: ");
        arrivalFloor = input.nextInt();
        buttonT = buttonType.Internal;
        System.out.println("Presses Internal Button");
        elevator.closeElevatorDoor();
        elevator.moveElevator();
        while(floorNum != arrivalFloor){
            floorNum++;
        }
        elevator.stopElevator();
        System.out.println("Destination Reached");
        elevator.outElevator();
        button.turnLightOff();
    }
}

class Elevator{
    public Elevator(){
        elevatorDoorState = false;
        movingState = false;
        inElevatorState = false;
    }
    private boolean elevatorDoorState;
    private boolean movingState;
    private boolean inElevatorState;
    public void openElevatorDoor(){
        elevatorDoorState = true;
    }
    public void closeElevatorDoor(){
        elevatorDoorState = false;
    }

    public void moveElevator(){
        movingState = true;
    }

    public void stopElevator(){
        movingState = false;
    }

    public void inElevator(){
        inElevatorState = true;
    }
    public void outElevator(){
        inElevatorState = false;
    }
}

class Button{
    private boolean buttonLight;
    public void turnLightOn(){
        buttonLight = true;
    }
    public void turnLightOff(){
        buttonLight = false;
    }
}
enum buttonType{
    Internal,
    External
}


public class Main2 {
    public static void main(String args[]){
    }
}
