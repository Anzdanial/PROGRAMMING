import javax.swing.*;
import java.awt.*;
import javax.swing.table.DefaultTableModel;

class JTableExample {
	public static void main(String[] args) {
		SwingUtilities.invokeLater(() -> createAndShowGUI());
	}

	private static void createAndShowGUI() {
		JFrame frame = new JFrame("JTable Example");
		frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);

		// Create a JTable
		JTable table = new JTable();

		// Define the number of rows and columns
		int numRows = 5;
		int numColumns = 3;

		// Initialize the table with the specified number of rows and columns
		table = new JTable(numRows,numColumns);

		// Populate the table with dummy data
		for (int row = 0; row < numRows; row++) {
				table.setValueAt("BAS", row, 1);

		}

		// Add the table to a JScrollPane
		JScrollPane scrollPane = new JScrollPane(table);

		// Add the JScrollPane to the frame
		frame.getContentPane().add(scrollPane, BorderLayout.CENTER);

		// Set frame size and make it visible
		frame.setPreferredSize(new Dimension(400, 300));
		frame.pack();
		frame.setVisible(true);
	}
}
