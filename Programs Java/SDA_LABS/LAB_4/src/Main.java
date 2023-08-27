class ATM {
    enum State{
        PowerOff, PerformingStartUpAction, SelfTesting, OutOfService, Idle, ServingCustomer;
    }
    public static class ATM_Exe{
        private State state;
        private String condition;

        private void execute(){

            state = State.PowerOff;
            condition = "pass";

            this.printState();

            this.turnOn();
        }

        private void turnOn(){
            state = State.PerformingStartUpAction;

            this.printState();

            this.enterSelftTestState();
        }

        private void enterSelftTestState(){
            state = State.SelfTesting;

            this.printState();

            if("pass".equals(this.condition)){

                this.enterIdle();
            }
            else{

                this.enterOutOfService();
            }
        }

        private void enterOutOfService(){

            state = State.OutOfService;

            this.printState();
        }

        private void enterIdle(){

            state = State.Idle;

            this.printState();

            this.insertCard();
        }

        private void insertCard(){

            state = State.ServingCustomer;

            this.printState();

            this.cancelTransaction();
        }

        private void cancelTransaction(){

            //using exit here because goes in loop
            System.exit(0);

            this.enterIdle();

        }

        private void printState(){

            switch(state){
                case PowerOff:
                    System.out.println("PowerOff");
                    break;
                case PerformingStartUpAction:
                    System.out.println("PerformingStartUpAction");
                    break;
                case SelfTesting:
                    System.out.println("SelfTesting");
                    break;
                case OutOfService:
                    System.out.println("OutOfService");
                    break;
                case Idle:
                    System.out.println("OutOfService");
                    break;
                case ServingCustomer:
                    System.out.println("ServingCustomer");
                    break;
            }

        }
    }

    public static void main(String[] args){

        ATM_Exe a;
        a = new ATM_Exe();

        a.execute();
    }
}