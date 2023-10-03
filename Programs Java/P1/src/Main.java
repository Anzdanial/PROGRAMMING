class C {
    public int x;
}
class D {
    public static void f (C c, int y) {
        System.out.println(c.x);
        c.x = y;
        y++;
        System.out.println(c.x);
        c = new C();
        c.x = y+2;
        System.out.println(c.x);
    }
    public static void main (String[]  args) {
        int z = 4;
        C c = new C();
        c.x = 3;
        System.out.println(c.x);
        f(c, z);
        System.out.println(c.x);
        System.out.println(z);
    }
}
