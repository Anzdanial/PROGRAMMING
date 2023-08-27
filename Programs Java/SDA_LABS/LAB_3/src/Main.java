import java.util.Vector;
import java.util.Scanner;

class Product{
    public String productName;
    public int quantity;
    public Product(){
        productName = "";
        quantity = 0;
    }
}

class User{
    private Cart cart;
    private String username; // Also known as UserID
    private String password;

    public User(){
        username = "Anzdanial"; //test initialization
        password = "fast@lhr";
        cart = new Cart();
    }
    public boolean login(String username, String Password){
        Scanner input = new Scanner (System.in);
        System.out.println("Enter the Username and Password: ");
        String UserID = input.nextLine();
        String PassKey = input.nextLine();
        if(UserID.equals(username) && Password.equals(PassKey)) {
            System.out.println("Successful Login");
            return true;
        }
        else {
            System.out.println("Unsuccessful Login");
            return false;
        }
    }

    public Cart getCart(){
        return cart;
    }

    public String getUsername(){
        return username;
    }

    public String getPassword(){
        return password;
    }
}

class Cart{
    private Vector <Product> products;
    private Checkout checkoutInstance;
    private boolean loginStatus;

    public Cart(){
        products = new Vector<>();
        checkoutInstance = new Checkout();
        loginStatus = false;
    }
    public void addProductCart (String productName, int quantity){
        if(checkLoginStatus()) {
            Product pTemp = new Product();
            pTemp.productName = productName;
            pTemp.quantity = quantity;
            products.add(pTemp);
            System.out.println("Product Addition Successful");
        }
        else
            System.out.println("Product Addition Unsuccessful");
    }
    public boolean removeProductCart (String productName, int quantity){
        if(checkLoginStatus()) {
            Product pTemp = new Product();
            pTemp.productName = productName;
            pTemp.quantity = quantity;
            return products.remove(pTemp);
        }
        return false;
    }

    public boolean checkLoginStatus(){
        if(loginStatus)
            return true;
        else
            return false;
    }

    public void cartCheckout (Vector <Product> checkoutProducts, boolean loginStatus){
        checkoutInstance.setLoginStatus(loginStatus);
        products = checkoutProducts;
        if(checkoutInstance.getLoginStatus()) {
            System.out.println("Enter the Shipping Information: ");
            Scanner input = new Scanner(System.in);
            String country = input.nextLine();
            String city = input.nextLine();
            String address1 = input.nextLine();
            String address2 = input.nextLine();
            checkoutInstance.enterShipInfo(country, city, address1, address2);
            System.out.println("Enter the Payment Method: ");
            checkoutInstance.selectPayMethod(PaymentMethod.COD);
            checkoutInstance.confirmOrder();
            checkoutInstance.processPayment();
        }
    }

    public void setLoginStatus(boolean status){
        loginStatus = status;
    }
}

class Checkout{
    private Vector <Product> products;
    private PaymentMethod selectedPaymentMethod;
    private boolean loginConfirmation;
    private boolean orderConfirmation;
    private boolean paymentConfirmation;
    public Checkout(){
        loginConfirmation = false;
        orderConfirmation = false;
        paymentConfirmation = false;
    }
    public void enterShipInfo(String country, String city, String address1, String address2){
        System.out.println("Order to be shipped to " + country + ", " + city + ", " + address1 + ", " + address2);
    }
    public void selectPayMethod(PaymentMethod selection){
        selectedPaymentMethod = selection;
    }

    public void processPayment(){
        System.out.println("Displays the Payment Details.");
        paymentConfirmation = true;
    }
    public void confirmOrder(){
        System.out.println("The products are: ");
        for (int i = 0; i < products.size(); i++)
        {
            System.out.print(products.get(i) + " ");
        }
        System.out.println("Order Confirmed & Placed");
        orderConfirmation = true;
    }
    public void setLoginStatus(boolean status){
        if(status)
            loginConfirmation = status;
    }
    public boolean getLoginStatus(){
        return loginConfirmation;
    }
}

enum PaymentMethod{
    COD,
    CardPayment,
    Paypal
}

public class Main{
    public static void main(String[] args){
        User account1 = new User();
        boolean loginStatus = account1.login(account1.getUsername(), account1.getPassword());
        account1.getCart().setLoginStatus(loginStatus);
        Scanner input = new Scanner (System.in);

        System.out.println("Enter the Product Name and Quantity to Add to Cart: ");
        String addProductName = input.nextLine();
        int addQuantity = input.nextInt();
        account1.getCart().addProductCart(addProductName, addQuantity);

        System.out.println("Enter the Product Name and Quantity to Remove from Cart: ");
        String removeProductName = input.nextLine();
        int removeQuantity = input.nextInt();
        if(account1.getCart().removeProductCart(removeProductName, removeQuantity))
            System.out.println("Product Removal Successful");
        else
            System.out.println("Product Removal Unsuccessful");

        //account1.getCart().cartCheckout();
    }
}