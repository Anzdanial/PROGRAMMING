import javax.swing.*;
import javax.swing.border.Border;
import java.awt.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;

public class Main {
	public static void main(String[] args) {
		JFrame frame = new JFrame("Anas");
		frame.setLayout(new BorderLayout());

		JRadioButton maleRadio = new JRadioButton("Male");
		JComboBox<String> box = new JComboBox<>();
		box.addItem("Hello");
		String anas = box.getItemAt(box.getSelectedIndex());
		System.out.println(anas);
		JRadioButton femaleRadio = new JRadioButton("Female");
		JPanel panel = new JPanel();
		panel.setLayout(new FlowLayout());
		panel.add(maleRadio);
		panel.add(femaleRadio);
		panel.add(box);
		panel.setComponentOrientation(ComponentOrientation.LEFT_TO_RIGHT);


		frame.add(panel, BorderLayout.NORTH);


		frame.setVisible(true);
		frame.setSize(200, 100);
	}
}