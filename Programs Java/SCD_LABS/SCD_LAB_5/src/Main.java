import javax.swing.*;
import java.awt.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;

class CalculatorGUI extends JFrame {
	private JTextField textField;
	private JButton[] digitButtons;
	private JButton[] operatorButtons;
	private JButton clearButton;
	private JButton equalsButton;

	private String currentInput;
	private double result;
	private char currentOperator;

	public CalculatorGUI() {
		setTitle("Calculator");
		setSize(300, 400);
		setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);

		currentInput = "";
		result = 0;
		currentOperator = ' ';

		setupUI();
	}

	private void setupUI() {
		textField = new JTextField();
		textField.setFont(new Font("Arial", Font.PLAIN, 24));
		textField.setEditable(false);
		add(textField, BorderLayout.NORTH);

		JPanel buttonPanel = new JPanel();
		buttonPanel.setLayout(new GridLayout(4, 4));

		digitButtons = new JButton[10];
		for (int i = 0; i < 10; i++) {
			digitButtons[i] = new JButton(String.valueOf(i));
			digitButtons[i].setFont(new Font("Arial", Font.PLAIN, 20));
			digitButtons[i].addActionListener(new DigitButtonListener(i));
			buttonPanel.add(digitButtons[i]);
		}

		operatorButtons = new JButton[4];
		String[] operators = {"+", "-", "*", "/"};
		for (int i = 0; i < 4; i++) {
			operatorButtons[i] = new JButton(operators[i]);
			operatorButtons[i].setFont(new Font("Arial", Font.PLAIN, 20));
			operatorButtons[i].addActionListener(new OperatorButtonListener(operators[i].charAt(0)));
			buttonPanel.add(operatorButtons[i]);
		}

		equalsButton = new JButton("=");
		equalsButton.setFont(new Font("Arial", Font.PLAIN, 20));
		equalsButton.addActionListener(new EqualsButtonListener());
		buttonPanel.add(equalsButton);

		clearButton = new JButton("C");
		clearButton.setFont(new Font("Arial", Font.PLAIN, 20));
		clearButton.addActionListener(new ClearButtonListener());
		buttonPanel.add(clearButton);

		add(buttonPanel, BorderLayout.CENTER);
	}

	class DigitButtonListener implements ActionListener {
		private int digit;

		public DigitButtonListener(int digit) {
			this.digit = digit;
		}

		public void actionPerformed(ActionEvent e) {
			currentInput += digit;
			textField.setText(currentInput);
		}
	}

	class OperatorButtonListener implements ActionListener {
		private char operator;

		public OperatorButtonListener(char operator) {
			this.operator = operator;
		}

		public void actionPerformed(ActionEvent e) {
			if (!currentInput.isEmpty()) {
				currentOperator = operator;
				result = Double.parseDouble(currentInput);
				currentInput = "";
			}
		}
	}

	class EqualsButtonListener implements ActionListener {
		@Override
		public void actionPerformed(ActionEvent e) {
			if (!currentInput.isEmpty() && currentOperator != ' ') {
				double operand = Double.parseDouble(currentInput);
				switch (currentOperator) {
					case '+':
						result += operand;
						break;
					case '-':
						result -= operand;
						break;
					case '*':
						result *= operand;
						break;
					case '/':
						if (operand != 0) {
							result /= operand;
						} else {
							currentInput = "Error";
							textField.setText(currentInput);
							return;
						}
						break;
				}
				currentInput = String.valueOf(result);
				textField.setText(currentInput);
				currentOperator = ' ';
			}
		}
	}

	class ClearButtonListener implements ActionListener {
		public void actionPerformed(ActionEvent e) {
			currentInput = "";
			result = 0;
			currentOperator = ' ';
			textField.setText(currentInput);
		}
	}

	public static void main(String[] args) {
		SwingUtilities.invokeLater(() -> {
			CalculatorGUI calculator = new CalculatorGUI();
			calculator.setVisible(true);
		});
	}
}