class SafeCounter {
	private int count = 0;

	// Synchronized method to increment count
	public synchronized void increment() {
		count++;
	}

	// Method to get the current count value
	public int getCount() {
		return count;
	}

	public static void main(String[] args) throws InterruptedException {
		SafeCounter counter = new SafeCounter();

		// Creating multiple threads that access the same SafeCounter instance
		Thread t1 = new Thread(new Runnable() {
			@Override
			public void run() {
				for (int i = 0; i < 5; i++) {
					counter.increment();
				}
			}
		});

		Thread t2 = new Thread(new Runnable() {
			@Override
			public void run() {
				for (int i = 0; i < 5; i++) {
					counter.increment();
				}
			}
		});

		// Start both threads
		t1.start();
		t2.start();

		// Wait for both threads to finish
		t1.join();
		t2.join();

		// Print the final count value
		System.out.println("Final count: " + counter.getCount());
	}
}
