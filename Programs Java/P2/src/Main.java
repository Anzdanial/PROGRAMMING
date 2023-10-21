import javax.swing.*;
import java.awt.*;

public class Main {
	public static void main(String[] args) {
		JFrame frame = new JFrame();
		JTextField field = new JTextField();
		JButton[] numberButtons = new JButton[10];
		JButton[] operatorButtons = new JButton[4];
		JButton equalsButton = new JButton("=");
		JTextField field = new JTextField("");


		JPanel mainPanel = new JPanel(new BorderLayout());
		JPanel numPanel = new JPanel(new GridLayout(3, 3)); // Changed to 4 rows to accommodate the 0 button
		JPanel lastPanel = new JPanel(new GridLayout(1, 2));
		JPanel operatorPanel = new JPanel(new GridLayout(4, 1)); // Adjusted to a vertical column
		JPanel centrePanel = new JPanel(new GridLayout(2,1));

		numberButtons[0] = new JButton("0");
		operatorButtons[0] = new JButton("+");
		operatorButtons[1] = new JButton("-");
		operatorButtons[2] = new JButton("*");
		operatorButtons[3] = new JButton("/");

		for (int i = 1; i < 10; i++) {
			numberButtons[i] = new JButton(String.valueOf(i));
			numPanel.add(numberButtons[i]);
		}

		lastPanel.add(numberButtons[0]);
		lastPanel.add(equalsButton);

		for (JButton operatorButton : operatorButtons) {
			operatorPanel.add(operatorButton);
		}

		centrePanel.add(numPanel);
		centrePanel.add(lastPanel);
		mainPanel.add(centrePanel, BorderLayout.CENTER);
		mainPanel.add(operatorPanel, BorderLayout.EAST); // Add the operator panel to the east side

		frame.add(mainPanel);

		frame.setVisible(true);
		frame.setSize(500, 250);
		frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
	}
}
