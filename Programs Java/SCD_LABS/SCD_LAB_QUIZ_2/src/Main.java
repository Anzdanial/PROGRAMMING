import javax.swing.*;
import java.awt.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.sql.*;
import java.util.ArrayList;
import java.util.List;

class Employee {
	private int employeeId;
	private String employeeName;
	private String employeePosition;
	private double employeeSalary;

	public int getEmployeeId() {
		return employeeId;
	}

	public String getEmployeeName() {
		return employeeName;
	}

	public String getEmployeePosition() {
		return employeePosition;
	}

	public double getEmployeeSalary() {
		return employeeSalary;
	}

	public void setEmployeeId(int ID) {
		employeeId = ID;
	}

	public void setEmployeeName(String name) {
		employeeName = name;
	}

	public void setEmployeePosition(String position) {
		employeePosition = position;
	}

	public void setEmployeeSalary(double salary) {
		employeeSalary = salary;
	}
}

class EmployeeGUI extends JPanel {
	private JTextField nameField, positionField, salaryField;
	private JTextArea displayArea;
	private DefaultListModel<Employee> employeeListModel;
	private DatabaseManager databaseManager;

	public EmployeeGUI(DatabaseManager databaseManager) {
		setLayout(new BorderLayout());

		JPanel formPanel = new JPanel(new GridLayout(3, 2));
		formPanel.add(new JLabel("Name:"));
		nameField = new JTextField();
		formPanel.add(nameField);
		formPanel.add(new JLabel("Position:"));
		positionField = new JTextField();
		formPanel.add(positionField);
		formPanel.add(new JLabel("Salary:"));
		salaryField = new JTextField();
		formPanel.add(salaryField);

		JButton addButton = new JButton("Add Employee");
		addButton.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent e) {
				addEmployee();
			}
		});

		JButton viewButton = new JButton("View Employees");
		viewButton.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent e) {
				viewEmployees();
			}
		});

		employeeListModel = new DefaultListModel<>();
		JList<Employee> employeeList = new JList<>(employeeListModel);

		JButton editButton = new JButton("Edit Employee");
		editButton.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent e) {
				editEmployee(employeeList.getSelectedValue());
			}
		});

		JButton removeButton = new JButton("Remove Employee");
		removeButton.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent e) {
				removeEmployee(employeeList.getSelectedValue());
			}
		});

		add(formPanel, BorderLayout.NORTH);
		add(addButton, BorderLayout.WEST);
		add(viewButton, BorderLayout.CENTER);
		add(new JScrollPane(employeeList), BorderLayout.CENTER);
		add(editButton, BorderLayout.EAST);
		add(removeButton, BorderLayout.SOUTH);

		this.databaseManager = databaseManager;
	}

	private void editEmployee(Employee selectedValue) {
	}

	private void addEmployee() {
		String name = nameField.getText();
		String position = positionField.getText();
		double salary = Double.parseDouble(salaryField.getText());

		Employee employee = new Employee();
		employee.setEmployeeName(name);
		employee.setEmployeePosition(position);
		employee.setEmployeeSalary(salary);

		// Add to the database
		databaseManager.addEmployee(employee);

		// Update the list model
		employeeListModel.addElement(employee);

		nameField.setText("");
		positionField.setText("");
		salaryField.setText("");
	}

	private void viewEmployees() {
		List<Employee> employees = databaseManager.getEmployees();
		employeeListModel.clear();
		for (Employee employee : employees) {
			employeeListModel.addElement(employee);
		}
	}

	private void removeEmployee(Employee selectedEmployee) {
		if (selectedEmployee != null) {
			databaseManager.removeEmployee(selectedEmployee);
			employeeListModel.removeElement(selectedEmployee);
		}
	}
}

	class DatabaseManager {
		private Connection connection;

		public DatabaseManager(String url, String username, String password) {
			try {
				Class.forName("com.mysql.cj.jdbc.Driver");
				this.connection = DriverManager.getConnection(url, username, password);
			} catch (Exception e) {
				e.printStackTrace();
			}
		}

		public void addEmployee(Employee employee) {
			try {
				PreparedStatement preparedStatement = connection.prepareStatement(
						"INSERT INTO employees(employee_name, employee_position, employee_salary) VALUES (?, ?, ?)",
						Statement.RETURN_GENERATED_KEYS
				);

				preparedStatement.setString(1, employee.getEmployeeName());
				preparedStatement.setString(2, employee.getEmployeePosition());
				preparedStatement.setDouble(3, employee.getEmployeeSalary());

				preparedStatement.executeUpdate();

				ResultSet generatedKeys = preparedStatement.getGeneratedKeys();
				if (generatedKeys.next()) {
					employee.setEmployeeId(generatedKeys.getInt(1));
				}

				preparedStatement.close();
				generatedKeys.close();
			} catch (SQLException e) {
				e.printStackTrace();
			}
		}

		public List<Employee> getEmployees() {
			List<Employee> employees = new ArrayList<>();

			try {
				Statement statement = connection.createStatement();
				ResultSet resultSet = statement.executeQuery("SELECT * FROM employees");

				while (resultSet.next()) {
					Employee employee = new Employee();
					employee.setEmployeeId(resultSet.getInt("employee_id"));
					employee.setEmployeeName(resultSet.getString("employee_name"));
					employee.setEmployeePosition(resultSet.getString("employee_position"));
					employee.setEmployeeSalary(resultSet.getDouble("employee_salary"));

					employees.add(employee);
				}

				// Close resources
				statement.close();
				resultSet.close();
			} catch (SQLException e) {
				e.printStackTrace();
			}

			return employees;
		}

		public void updateEmployee(Employee employee) {
			try {
				PreparedStatement preparedStatement = connection.prepareStatement(
						"UPDATE employees SET employee_name=?, employee_position=?, employee_salary=? WHERE employee_id=?"
				);

				preparedStatement.setString(1, employee.getEmployeeName());
				preparedStatement.setString(2, employee.getEmployeePosition());
				preparedStatement.setDouble(3, employee.getEmployeeSalary());
				preparedStatement.setInt(4, employee.getEmployeeId());

				preparedStatement.executeUpdate();

				// Close resources
				preparedStatement.close();
			} catch (SQLException e) {
				e.printStackTrace();
			}
		}

		public void removeEmployee(Employee employee) {
			try {
				PreparedStatement preparedStatement = connection.prepareStatement(
						"DELETE FROM employees WHERE employee_id=?"
				);

				preparedStatement.setInt(1, employee.getEmployeeId());

				preparedStatement.executeUpdate();

				preparedStatement.close();
			} catch (SQLException e) {
				e.printStackTrace();
			}
		}

		public void closeConnection() {
			try {
				if (connection != null && !connection.isClosed()) {
					connection.close();
				}
			} catch (SQLException e) {
				e.printStackTrace();
			}
		}
	}

	class EmployeeManagementSystem extends JFrame {
		private DatabaseManager databaseManager;
		private EmployeeGUI employeeGUI;

		public EmployeeManagementSystem() {
			databaseManager = new DatabaseManager("jdbc:mysql://localhost:3306/", "anzdanial", "2001");
			employeeGUI = new EmployeeGUI(databaseManager);
			add(employeeGUI);
			setSize(1280, 700);
			setTitle("Employee Form");
			setVisible(true);
		}

		@Override
		public void setDefaultCloseOperation(int operation) {
			databaseManager.closeConnection();
			super.setDefaultCloseOperation(operation);
		}

		public static void main(String[] args) {
			SwingUtilities.invokeLater(new Runnable() {
				public void run() {
					new EmployeeManagementSystem();
				}
			});
		}
	}
