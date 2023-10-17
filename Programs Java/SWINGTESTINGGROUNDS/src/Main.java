import javax.swing.*;
import javax.swing.event.ListSelectionEvent;
import javax.swing.event.ListSelectionListener;
import java.awt.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;

class componentListener implements ListSelectionListener, ActionListener {
	private DefaultListModel<String> listModel;
	private JList<String> list;
	private JTextField field;

	componentListener(JList<String> list, DefaultListModel<String> listModel, JTextField field) {
		this.list = list;
		this.listModel = listModel;
		this.field = field;
	}

	public void actionPerformed(ActionEvent e) {
		listModel.addElement(field.getText());
	}

	public void valueChanged(ListSelectionEvent e) {
//		int index = 0, temp = 0;
//			index = e.getLastIndex();
//			String text = list.getSelectedValue();
//			listModel.set(index, text + " (Currently Selected)");
//			System.out.println(text);
//			System.out.println(e.getLastIndex());
//			System.out.println(e.getFirstIndex());

	}
}



public class Main{
	static public void main(String [] args){
		final String[] name = {""};
		JLabel label = new JLabel("Resource: ");
		JTextField field = new JTextField("", 10);
		JButton button = new JButton("Add");
		JFrame frame = new JFrame();
		Container contentPane = frame.getContentPane();
		SpringLayout layout = new SpringLayout();
		DefaultListModel<String> listModel = new DefaultListModel<>();
		JList<String> myList = new JList<>(listModel);
		JScrollPane scrollPane = new JScrollPane(myList);
		contentPane.setLayout(layout);
		contentPane.add(label);
		contentPane.add(field);
		contentPane.add(button);
		contentPane.add(scrollPane);

		layout.putConstraint(SpringLayout.WEST, label, 5, SpringLayout.WEST,contentPane);
		layout.putConstraint(SpringLayout.NORTH, label, 5, SpringLayout.NORTH,contentPane);

		layout.putConstraint(SpringLayout.WEST, field, 5, SpringLayout.EAST, label);
		layout.putConstraint(SpringLayout.NORTH, field, 5, SpringLayout.NORTH, contentPane);

		layout.putConstraint(SpringLayout.WEST, button, 5, SpringLayout.EAST, field);
		layout.putConstraint(SpringLayout.NORTH, button, 5, SpringLayout.NORTH, contentPane);

		layout.putConstraint(SpringLayout.WEST, scrollPane, 5, SpringLayout.WEST, contentPane);
		layout.putConstraint(SpringLayout.NORTH, scrollPane, 50, SpringLayout.NORTH, contentPane);

		componentListener listener = new componentListener(myList,listModel,field);

		button.addActionListener(listener);
		myList.addListSelectionListener(listener);
		frame.pack();
		frame.getRootPane().setDefaultButton(button);
		frame.setTitle("Add Form");
		frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		frame.setSize(400, 200);
		frame.setVisible(true);
	}
}