package Views;

/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/GUIForms/JFrame.java to edit this template
 */

import Contollers.OrderDetail;
import Contollers.adminController;

import javax.swing.*;
import java.util.ArrayList;

/**
 *
 * @author ShaafSalman
 */
public class placeOrder extends javax.swing.JFrame {

    adminController ad = new adminController();
    adminController controller = new adminController();
    ArrayList<OrderDetail> orderDetails = new ArrayList<>();
    double totalAmount = 0.0;

    public placeOrder() {
        initComponents();
        initializeComboBoxes();

    }

    private void initializeComboBoxes() {
        ad.setupCustomerComboBox(cmbCustomers);
        ad.setupBookComboBox(cmbBooks);
    }

    private void btnAddActionPerformed(java.awt.event.ActionEvent evt) {
        String selectedCustomer = (String) cmbCustomers.getSelectedItem();
        String selectedBook = (String) cmbBooks.getSelectedItem();
        int quantity = (int) spinnerQuantity.getValue(); // Get the quantity from the spinner

        double bookPrice = ad.getBookPrice(selectedBook);
        double subtotal = quantity * bookPrice;

        txtDescription.append("Selected Book: " + selectedBook + ", Quantity: " + quantity + ", Subtotal: " + subtotal + "\n");

        OrderDetail orderDetail = new OrderDetail(selectedBook, quantity, bookPrice);
        orderDetails.add(orderDetail);

        totalAmount += subtotal;
        txtTotal.setText("Total: " + totalAmount);
    }

    private void tbnSaveActionPerformed(java.awt.event.ActionEvent evt) {
        int confirmation = JOptionPane.showConfirmDialog(null, "Confirm order with total amount: " + totalAmount, "Confirmation", JOptionPane.YES_NO_OPTION);

        if (confirmation == JOptionPane.YES_OPTION) {
            // Get the selected customer
            String selectedCustomer = (String) cmbCustomers.getSelectedItem();

            boolean orderSaved = ad.saveOrderToDatabase(selectedCustomer, orderDetails, totalAmount);

            if (orderSaved) {
                JOptionPane.showMessageDialog(null, "Order saved successfully!", "Success", JOptionPane.INFORMATION_MESSAGE);
            } else {
                JOptionPane.showMessageDialog(null, "Failed to save the order.", "Error", JOptionPane.ERROR_MESSAGE);
            }

            orderDetails.clear();
            totalAmount = 0.0;
            txtDescription.setText("");
            txtTotal.setText("Total: " + totalAmount);
        }
    }

    @SuppressWarnings("unchecked")
    // <editor-fold defaultstate="collapsed" desc="Generated Code">                          
    private void initComponents() {

        jLabel1 = new javax.swing.JLabel();
        cmbCustomers = new javax.swing.JComboBox<>();
        cmbBooks = new javax.swing.JComboBox<>();
        btnAdd = new javax.swing.JButton();
        jScrollPane1 = new javax.swing.JScrollPane();
        txtDescription = new javax.swing.JTextArea();
        tbnSave = new javax.swing.JButton();
        txtTotal = new javax.swing.JLabel();
        spinnerQuantity = new javax.swing.JSpinner();

        setDefaultCloseOperation(WindowConstants.DISPOSE_ON_CLOSE);

        jLabel1.setFont(new java.awt.Font("Century Gothic", 1, 24)); // NOI18N
        jLabel1.setText("Place Order");

        cmbCustomers.setFont(new java.awt.Font("Century Gothic", 0, 14)); // NOI18N
        cmbCustomers.setModel(new javax.swing.DefaultComboBoxModel<>(new String[] { }));

        cmbBooks.setFont(new java.awt.Font("Century Gothic", 0, 14)); // NOI18N
        cmbBooks.setModel(new javax.swing.DefaultComboBoxModel<>(new String[] { }));

        btnAdd.setFont(new java.awt.Font("Century Gothic", 0, 14)); // NOI18N
        btnAdd.setText("Add");
        btnAdd.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                btnAddActionPerformed(evt);
            }
        });

        txtDescription.setColumns(20);
        txtDescription.setFont(new java.awt.Font("Century Gothic", 0, 12)); // NOI18N
        txtDescription.setRows(5);
        jScrollPane1.setViewportView(txtDescription);

        tbnSave.setFont(new java.awt.Font("Century Gothic", 0, 14)); // NOI18N
        tbnSave.setText("Place");
        tbnSave.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                tbnSaveActionPerformed(evt);
            }
        });

        txtTotal.setFont(new java.awt.Font("Century Gothic", 1, 18)); // NOI18N
        txtTotal.setText("Total :");

        spinnerQuantity.setFont(new java.awt.Font("Century Gothic", 1, 14)); // NOI18N

        javax.swing.GroupLayout layout = new javax.swing.GroupLayout(getContentPane());
        getContentPane().setLayout(layout);
        layout.setHorizontalGroup(
                layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                        .addGroup(layout.createSequentialGroup()
                                .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                                        .addGroup(layout.createSequentialGroup()
                                                .addGap(271, 271, 271)
                                                .addComponent(jLabel1))
                                        .addGroup(layout.createSequentialGroup()
                                                .addGap(97, 97, 97)
                                                .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING, false)
                                                        .addGroup(layout.createSequentialGroup()
                                                                .addComponent(cmbBooks, javax.swing.GroupLayout.PREFERRED_SIZE, 242, javax.swing.GroupLayout.PREFERRED_SIZE)
                                                                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.UNRELATED)
                                                                .addComponent(spinnerQuantity, javax.swing.GroupLayout.PREFERRED_SIZE, 106, javax.swing.GroupLayout.PREFERRED_SIZE)
                                                                .addGap(18, 18, 18)
                                                                .addComponent(btnAdd, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE))
                                                        .addComponent(cmbCustomers, 0, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE)
                                                        .addComponent(jScrollPane1)
                                                        .addGroup(layout.createSequentialGroup()
                                                                .addComponent(txtTotal, javax.swing.GroupLayout.PREFERRED_SIZE, 168, javax.swing.GroupLayout.PREFERRED_SIZE)
                                                                .addGap(18, 18, 18)
                                                                .addComponent(tbnSave, javax.swing.GroupLayout.DEFAULT_SIZE, 290, Short.MAX_VALUE)))))
                                .addContainerGap(117, Short.MAX_VALUE))
        );
        layout.setVerticalGroup(
                layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                        .addGroup(layout.createSequentialGroup()
                                .addGap(21, 21, 21)
                                .addComponent(jLabel1)
                                .addGap(44, 44, 44)
                                .addComponent(cmbCustomers, javax.swing.GroupLayout.PREFERRED_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE)
                                .addGap(38, 38, 38)
                                .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.BASELINE)
                                        .addComponent(cmbBooks, javax.swing.GroupLayout.PREFERRED_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE)
                                        .addComponent(btnAdd)
                                        .addComponent(spinnerQuantity, javax.swing.GroupLayout.PREFERRED_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE))
                                .addGap(30, 30, 30)
                                .addComponent(jScrollPane1, javax.swing.GroupLayout.PREFERRED_SIZE, 126, javax.swing.GroupLayout.PREFERRED_SIZE)
                                .addGap(32, 32, 32)
                                .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.BASELINE)
                                        .addComponent(tbnSave)
                                        .addComponent(txtTotal, javax.swing.GroupLayout.PREFERRED_SIZE, 26, javax.swing.GroupLayout.PREFERRED_SIZE))
                                .addContainerGap(54, Short.MAX_VALUE))
        );
        SpinnerModel spinnerModel = new SpinnerNumberModel(1, 1, Integer.MAX_VALUE, 1); // Starting value is 1

        // Create the spinner with the model
        spinnerQuantity = new javax.swing.JSpinner(spinnerModel);

        pack();
    }// </editor-fold>                        


    /**
     * @param args the command line arguments
     */
    public static void main(String args[]) {
        /* Set the Nimbus look and feel */
        //<editor-fold defaultstate="collapsed" desc=" Look and feel setting code (optional) ">
        /* If Nimbus (introduced in Java SE 6) is not available, stay with the default look and feel.
         * For details see http://download.oracle.com/javase/tutorial/uiswing/lookandfeel/plaf.html
         */
        try {
            for (javax.swing.UIManager.LookAndFeelInfo info : javax.swing.UIManager.getInstalledLookAndFeels()) {
                if ("Nimbus".equals(info.getName())) {
                    javax.swing.UIManager.setLookAndFeel(info.getClassName());
                    break;
                }
            }
        } catch (ClassNotFoundException ex) {
            java.util.logging.Logger.getLogger(placeOrder.class.getName()).log(java.util.logging.Level.SEVERE, null, ex);
        } catch (InstantiationException ex) {
            java.util.logging.Logger.getLogger(placeOrder.class.getName()).log(java.util.logging.Level.SEVERE, null, ex);
        } catch (IllegalAccessException ex) {
            java.util.logging.Logger.getLogger(placeOrder.class.getName()).log(java.util.logging.Level.SEVERE, null, ex);
        } catch (javax.swing.UnsupportedLookAndFeelException ex) {
            java.util.logging.Logger.getLogger(placeOrder.class.getName()).log(java.util.logging.Level.SEVERE, null, ex);
        }
        //</editor-fold>

        /* Create and display the form */
        java.awt.EventQueue.invokeLater(new Runnable() {
            public void run() {
                new placeOrder().setVisible(true);
            }
        });
    }

    // Variables declaration - do not modify                     
    private javax.swing.JButton btnAdd;
    private javax.swing.JComboBox<String> cmbBooks;
    private javax.swing.JComboBox<String> cmbCustomers;
    private javax.swing.JLabel jLabel1;
    private javax.swing.JLabel txtTotal;
    private javax.swing.JScrollPane jScrollPane1;
    private javax.swing.JTextArea txtDescription;
    private javax.swing.JButton tbnSave;
    private javax.swing.JTextField txtQuantity;
    private javax.swing.JSpinner spinnerQuantity;

    // End of variables declaration                   
}
