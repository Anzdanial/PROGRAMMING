import javax.swing.*;
import javax.swing.table.DefaultTableModel;
import java.awt.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;

public class Main {
	private static DefaultTableModel model;
	private static JLabel totalPriceLabel;

	public static void main(String[] args) {
		JFrame frame = new JFrame("Product Listing");
		JPanel panel = new JPanel(new BorderLayout());

		// Create the table
		model = new DefaultTableModel();
		JTable table = new JTable(model);
		model.addColumn("Name");
		model.addColumn("Price");
		model.addColumn("Quantity");

		// Create input fields
		JTextField nameField = new JTextField(10);
		JTextField priceField = new JTextField(10);
		JTextField quantityField = new JTextField(10);
		JButton addButton = new JButton("Add");
		JButton removeButton = new JButton("Remove");
		totalPriceLabel = new JLabel("Total Price: 0.0");

		// Add components to the panel
		panel.add(new JScrollPane(table), BorderLayout.CENTER);

		JPanel inputPanel = new JPanel();
		inputPanel.add(new JLabel("Name:"));
		inputPanel.add(nameField);
		inputPanel.add(new JLabel("Price:"));
		inputPanel.add(priceField);
		inputPanel.add(new JLabel("Quantity:"));
		inputPanel.add(quantityField);
		inputPanel.add(addButton);
		inputPanel.add(removeButton);
		panel.add(inputPanel, BorderLayout.NORTH);
		panel.add(totalPriceLabel, BorderLayout.SOUTH);

		// Add listeners
		addButton.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent e) {
				String name = nameField.getText();
				double price = Double.parseDouble(priceField.getText());
				int quantity = Integer.parseInt(quantityField.getText());
				boolean exists = false;

				for (int i = 0; i < model.getRowCount(); i++) {
					if (name.equals(model.getValueAt(i, 0)) && price == (double) model.getValueAt(i, 1)) {
						int updatedQuantity = (int) model.getValueAt(i, 2) + quantity;
						model.setValueAt(updatedQuantity, i, 2);
						exists = true;
						break;
					}
				}

				if (!exists) {
					Object[] row = {name, price, quantity};
					model.addRow(row);
				}

				updateTotalPrice();
			}
		});

		removeButton.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent e) {
				if (table.getSelectedRow() != -1) {
					model.removeRow(table.getSelectedRow());
					updateTotalPrice();
				}
			}
		});

		frame.add(panel);
		frame.pack();
		frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		frame.setVisible(true);
	}

	private static void updateTotalPrice() {
		double totalPrice = 0.0;
		for (int i = 0; i < model.getRowCount(); i++) {
			double price = (double) model.getValueAt(i, 1);
			int quantity = (int) model.getValueAt(i, 2);
			totalPrice += price * quantity;
		}
		totalPriceLabel.setText("Total Price: " + totalPrice);
	}
}