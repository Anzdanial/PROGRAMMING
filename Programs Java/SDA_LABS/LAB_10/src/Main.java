import java.util.Scanner;

interface Shape{
}
class Circle implements Shape{}
class Rectangle implements Shape{}
class Square implements Shape{}

interface Color{
}

class Red implements Color{}
class Green implements Color{}
class Blue implements Color{}

abstract class AbstractFactory{
    abstract public Shape createShape(String type);
    abstract public Color createColor(String type);
}

class ShapeFactory extends AbstractFactory{
    public Shape createShape(String type){
        if(type.equalsIgnoreCase("rectangle"))
            return new Rectangle();
        else if(type.equalsIgnoreCase("circle"))
            return new Circle();
        else if(type.equalsIgnoreCase("square"))
            return new Square();
        else
            return null;
    }
    public Color createColor(String type){
        return null;
    }
}
class ColorFactory extends AbstractFactory{
    public Shape createShape(String type){
        return null;
    }

    public Color createColor(String type){
        if(type.equalsIgnoreCase("red"))
            return new Red();
        else if(type.equalsIgnoreCase("green"))
            return new Green();
        else if(type.equalsIgnoreCase("blue"))
            return new Blue();
        else
            return null;
    }
}

class FactoryGenerator{
    private static AbstractFactory factory;
    public FactoryGenerator(){}
    public FactoryGenerator(AbstractFactory fac){
        factory = fac;
    }
    public static AbstractFactory generateFactory(String shape_color){
        if(shape_color.equalsIgnoreCase("Shape"))
            return new ShapeFactory();
        else if (shape_color.equalsIgnoreCase("Color"))
            return new ColorFactory();
        return null;
    }
}

public class Main {
    public static void Main(String args[]){
        Scanner userInput = new Scanner(System.in);
        FactoryGenerator Gen1 = new FactoryGenerator();
        Gen1.generateFactory("Shape");
    }
}