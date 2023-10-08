import javax.swing.*;
import java.awt.*;
import java.util.ArrayList;

public class Main {
	static void buttonCreator(String name, ArrayList<JButton> buttons){
		JButton button = new JButton(name);
		buttons.add(button);
	}
	static void arithmeticButtonCreator(ArrayList<JButton> buttons){
		JButton plusButton = new JButton("+");
		JButton minusButton = new JButton("-");
		JButton multiplyButton = new JButton("*");
		JButton divideButton = new JButton("/");
		JButton equalsButton = new JButton("=");
		buttons.add(plusButton);
		buttons.add(minusButton);
		buttons.add(multiplyButton);
		buttons.add(divideButton);
		buttons.add(equalsButton);
	}
	public static void main(String[] args) {
		JFrame frame = new JFrame("Calculator");
		frame.setSize(500, 250);
		frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);

		JPanel panel1 = new JPanel();
		ArrayList<JButton> buttons = new ArrayList<>();
		for(int i = 0; i < 8; i++){
			buttonCreator(String.valueOf(i+1), buttons);
			panel1.add(buttons.get(i));
		}
		JButton zeroButton = new JButton("0");
		panel1.add(zeroButton);

		JPanel panel2 = new JPanel();
		for(int i = 10; i < 15; i++){
			arithmeticButtonCreator(buttons);
			panel2.add(buttons.get(i));
		}
		panel1.setLayout(new GridLayout(4,4));
		//panel2.setLayout(new GridLayout(5,1));

		frame.add(panel1);
		//frame.add(panel2);
		frame.setVisible(true);
	}
}