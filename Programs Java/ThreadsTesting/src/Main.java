class MyRunnable implements Runnable{
    private int count = 0;
    public synchronized void run() {
        for (int i = 1; i <= 5; i++) {
            count++;
            System.out.println(Thread.currentThread().getId() + " Value " + count);
            try {
                Thread.sleep(100); // Simulate some work being done
            } catch (InterruptedException e) {
                e.printStackTrace();
            }
        }
    }
}

public class Main  {
    public static void main(String[] args) {
        Thread thread1 = new Thread(new MyRunnable());
        Thread thread2 = new Thread(new MyRunnable());

        System.out.println(thread1.getId());
        System.out.println();

        // Start both threads
        thread1.start();
        thread2.start();
    }
}