package Views;

import Contollers.adminController;

import javax.swing.*;
import java.awt.event.ActionEvent;
import java.sql.SQLException;

class manageBooks extends javax.swing.JFrame {

    private final adminController ad = new adminController();

    public manageBooks() {
        initComponents();
        ad.inititlazeBookstable(tblBooks);
    }

    private void initComponents() {
        pnlFront = new javax.swing.JPanel();
        lblTitle = new javax.swing.JLabel();
        lblLogin = new javax.swing.JLabel();
        txtTitle = new javax.swing.JTextField();
        lblAuthor = new javax.swing.JLabel();
        btnAddBook = new javax.swing.JButton();
        txtAuthor = new javax.swing.JTextField();
        lblPrice = new javax.swing.JLabel();
        txtPrice = new javax.swing.JTextField();
        lblStock = new javax.swing.JLabel();
        txtStock = new javax.swing.JTextField();
        jScrollPane1 = new javax.swing.JScrollPane();
        tblBooks = new javax.swing.JTable();

        setDefaultCloseOperation(WindowConstants.DISPOSE_ON_CLOSE);

        pnlFront.setBackground(new java.awt.Color(240, 240, 240));

        lblTitle.setFont(new java.awt.Font("Century Gothic", 0, 14));
        lblTitle.setText("Title");

        lblLogin.setFont(new java.awt.Font("Century Gothic", 1, 24));
        lblLogin.setText("Manage Books");

        lblAuthor.setFont(new java.awt.Font("Century Gothic", 0, 14));
        lblAuthor.setText("Author");

        btnAddBook.setBackground(new java.awt.Color(0, 153, 204));
        btnAddBook.setFont(new java.awt.Font("Century Gothic", 1, 18));
        btnAddBook.setForeground(new java.awt.Color(255, 255, 255));
        btnAddBook.setText("Add New Book");
        btnAddBook.addActionListener(this::btnAddBookActionPerformed);

        lblPrice.setFont(new java.awt.Font("Century Gothic", 0, 14));
        lblPrice.setText("Price");

        lblStock.setFont(new java.awt.Font("Century Gothic", 0, 14));
        lblStock.setText("Stock");

        tblBooks.setModel(new javax.swing.table.DefaultTableModel(
                new Object [][] {},
                new String [] {"ID", "Title", "Author", "Price", "Stock"}
        ));
        jScrollPane1.setViewportView(tblBooks);

        GroupLayout pnlFrontLayout = new GroupLayout(pnlFront);
        pnlFront.setLayout(pnlFrontLayout);
        pnlFrontLayout.setHorizontalGroup(
                pnlFrontLayout.createParallelGroup(GroupLayout.Alignment.LEADING)
                        .addGroup(pnlFrontLayout.createSequentialGroup()
                                .addContainerGap()
                                .addGroup(pnlFrontLayout.createParallelGroup(GroupLayout.Alignment.LEADING)
                                        .addComponent(lblLogin)
                                        .addGroup(pnlFrontLayout.createSequentialGroup()
                                                .addGroup(pnlFrontLayout.createParallelGroup(GroupLayout.Alignment.LEADING)
                                                        .addComponent(txtTitle, GroupLayout.PREFERRED_SIZE, 200, GroupLayout.PREFERRED_SIZE)
                                                        .addComponent(lblTitle)
                                                )
                                                .addPreferredGap(LayoutStyle.ComponentPlacement.RELATED)
                                                .addGroup(pnlFrontLayout.createParallelGroup(GroupLayout.Alignment.LEADING)
                                                        .addComponent(txtAuthor, GroupLayout.PREFERRED_SIZE, 200, GroupLayout.PREFERRED_SIZE)
                                                        .addComponent(lblAuthor)
                                                )
                                                .addPreferredGap(LayoutStyle.ComponentPlacement.RELATED)
                                                .addGroup(pnlFrontLayout.createParallelGroup(GroupLayout.Alignment.LEADING)
                                                        .addComponent(txtPrice, GroupLayout.PREFERRED_SIZE, 100, GroupLayout.PREFERRED_SIZE)
                                                        .addComponent(lblPrice)
                                                )
                                                .addPreferredGap(LayoutStyle.ComponentPlacement.RELATED)
                                                .addGroup(pnlFrontLayout.createParallelGroup(GroupLayout.Alignment.LEADING)
                                                        .addComponent(txtStock, GroupLayout.PREFERRED_SIZE, 100, GroupLayout.PREFERRED_SIZE)
                                                        .addComponent(lblStock)
                                                )
                                                .addPreferredGap(LayoutStyle.ComponentPlacement.RELATED)
                                                .addComponent(btnAddBook, GroupLayout.PREFERRED_SIZE, 150, GroupLayout.PREFERRED_SIZE)
                                        )
                                        .addComponent(jScrollPane1)
                                )
                                .addContainerGap()
                        )
        );
        pnlFrontLayout.setVerticalGroup(
                pnlFrontLayout.createParallelGroup(GroupLayout.Alignment.LEADING)
                        .addGroup(pnlFrontLayout.createSequentialGroup()
                                .addContainerGap()
                                .addComponent(lblLogin)
                                .addPreferredGap(LayoutStyle.ComponentPlacement.RELATED)
                                .addGroup(pnlFrontLayout.createParallelGroup(GroupLayout.Alignment.BASELINE)
                                        .addComponent(lblTitle)
                                        .addComponent(lblAuthor)
                                        .addComponent(lblPrice)
                                        .addComponent(lblStock)
                                )
                                .addPreferredGap(LayoutStyle.ComponentPlacement.RELATED)
                                .addGroup(pnlFrontLayout.createParallelGroup(GroupLayout.Alignment.BASELINE)
                                        .addComponent(txtTitle, GroupLayout.PREFERRED_SIZE, GroupLayout.DEFAULT_SIZE, GroupLayout.PREFERRED_SIZE)
                                        .addComponent(txtAuthor, GroupLayout.PREFERRED_SIZE, GroupLayout.DEFAULT_SIZE, GroupLayout.PREFERRED_SIZE)
                                        .addComponent(txtPrice, GroupLayout.PREFERRED_SIZE, GroupLayout.DEFAULT_SIZE, GroupLayout.PREFERRED_SIZE)
                                        .addComponent(txtStock, GroupLayout.PREFERRED_SIZE, GroupLayout.DEFAULT_SIZE, GroupLayout.PREFERRED_SIZE)
                                        .addComponent(btnAddBook)
                                )
                                .addPreferredGap(LayoutStyle.ComponentPlacement.RELATED)
                                .addComponent(jScrollPane1, GroupLayout.DEFAULT_SIZE, 197, Short.MAX_VALUE)
                        )
        );

        GroupLayout layout = new GroupLayout(getContentPane());
        getContentPane().setLayout(layout);
        layout.setHorizontalGroup(
                layout.createParallelGroup(GroupLayout.Alignment.LEADING)
                        .addComponent(pnlFront, GroupLayout.DEFAULT_SIZE, GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE)
        );
        layout.setVerticalGroup(
                layout.createParallelGroup(GroupLayout.Alignment.LEADING)
                        .addComponent(pnlFront, GroupLayout.DEFAULT_SIZE, GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE)
        );

        pack();
    }
    private void btnAddBookActionPerformed(ActionEvent evt) {
        try {
            String title = txtTitle.getText().trim();
            String author = txtAuthor.getText().trim();
            double price = Double.parseDouble(txtPrice.getText().trim());
            int stock = Integer.parseInt(txtStock.getText().trim());

            if (ad.addBook(title, author, price, stock)) {
                JOptionPane.showMessageDialog(this, "Book added successfully");
                ad.inititlazeBookstable(tblBooks);
            } else {
                JOptionPane.showMessageDialog(this, "Failed to add the book");
            }
        } catch (NumberFormatException e) {
            JOptionPane.showMessageDialog(this, "Invalid input. Please check the fields.");
        }
    }
    public static void main(String args[]) {
        /* Set the Nimbus look and feel */
        // Your existing look and feel setup code...

        /* Create and display the form */
        java.awt.EventQueue.invokeLater(() -> {
            new manageBooks().setVisible(true);
        });
    }

    // Variables declaration - do not modify
    private javax.swing.JButton btnAddBook;
    private javax.swing.JScrollPane jScrollPane1;
    private javax.swing.JLabel lblLogin;
    private javax.swing.JLabel lblAuthor;
    private javax.swing.JLabel lblPrice;
    private javax.swing.JLabel lblStock;
    private javax.swing.JLabel lblTitle;
    private javax.swing.JPanel pnlFront;
    private javax.swing.JTable tblBooks;
    private javax.swing.JTextField txtAuthor;
    private javax.swing.JTextField txtPrice;
    private javax.swing.JTextField txtStock;
    private javax.swing.JTextField txtTitle;
    // End of variables declaration
}
