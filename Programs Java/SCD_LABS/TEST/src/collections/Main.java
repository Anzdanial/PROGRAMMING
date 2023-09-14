
/*
class Box{
    private Object val;
    public void set(Object o){
        val = o;
    }
    public Object get(){
        return val;
    }
}
public class Main {
    public static void main(String[] args) {
        Box b = new Box();
        b.set(1);
        Integer v = (Integer) b.get();
        }
    }
}
 */

package collections;

import java.io.BufferedReader;
import java.io.File;
import java.io.FileReader;
import java.io.FileWriter;
import java.util.ArrayList;
import java.util.Iterator;
import java.util.List;

public class Collections{

}


public class Main{
    public static void main(String[] args){
        MyList <Integer> l = new MyList<>();
        l.add(1).add(3).add(5);

        l.print();

        MyList<Integer>.Iterator iter = l.Iterator();
    }
}



class MyList<T>{
    private Node head;
    private Node tail;

    private class Node{
        public T val;
        public Node next;
    }

    public MyList(){
        head = null;
        tail = null;
    }

    public MyList<T> add (T v){
        Node n = new Node (v);
        if(head == null){
            head = n;
            tail = n;
        }
        else{
            tail.next = n;
            tail = n;
        }
        return this;
    }

    public class Iterator{
        private Node c;
        public Iterator() {
            c = head;
        }

        public boolean hasNext(){
            if(c == null){
                return false;
            }
            return true;
        }
        public T next(){
            T v = c.val;
            c = c.next;
            return v;
        }
    }

}