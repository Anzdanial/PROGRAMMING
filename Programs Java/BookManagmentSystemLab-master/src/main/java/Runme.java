import Views.ManagerHub;

public class Runme
{
    public static void main(String args[])
    {
        java.awt.EventQueue.invokeLater(new Runnable() {
            public void run() {
                new ManagerHub().setVisible(true);
            }
        });
    }

}
