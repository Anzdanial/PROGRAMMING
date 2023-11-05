package Contollers;

public class OrderDetail {
    public String bookTitle;
    public int quantity;
    public double bookPrice;

    public OrderDetail(String selectedBook, double quan, double price) {
        bookTitle = selectedBook;
        quantity = (int) quan;
        bookPrice = price;
    }


    public String getBookTitle() {
        return bookTitle;
    }

    public int getQuantity() {
        return quantity;
    }

    public double getBookPrice() {
        return bookPrice;
    }


}
