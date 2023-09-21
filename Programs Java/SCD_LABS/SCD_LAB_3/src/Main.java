import java.io.*;
import java.util.ArrayList;
import java.util.List;

class Person {
    private String name;
    private int age;
    private String location;
    private Integer pf_Marks;
    private Integer scd_Marks;

    public Person(String name, int age, String location, Integer pf_Marks, Integer scd_Marks) {
        this.name = name;
        this.age = age;
        this.location = location;
        this.pf_Marks = pf_Marks;
        this.scd_Marks = scd_Marks;
    }

    public String getName() {
        return name;
    }

    public int getAge() {
        return age;
    }

    public String getLocation() {
        return location;
    }

    public Integer getPfMarks() {
        return pf_Marks;
    }

    public Integer getScdMarks() {
        return scd_Marks;
    }


}

public class Main {
    public static void main(String[] args) {
        List<Person> people = new ArrayList<>();

        try (BufferedReader br = new BufferedReader(new FileReader("list - Sheet1.csv"))) {
            String line = br.readLine();

            while ((line = br.readLine()) != null) {
                String[] fields = line.split(",");
                if (fields.length == 5) {
                    String name = fields[0].trim();
                    int age = Integer.parseInt(fields[1].trim());
                    String location = fields[2].trim();
                    Integer scdMarks = Integer.parseInt(fields[3].trim());
                    Integer pfMarks = Integer.parseInt(fields[4].trim());

                    Person person = new Person(name, age, location, pfMarks, scdMarks);
                    people.add(person);
                } else {
                    System.err.println("Skipping invalid line: " + line);
                }
            }
        } catch (IOException e) {
            throw new RuntimeException(e);
        }

        for (Person person : people) {
            System.out.println(person.getName()+ " " +person.getScdMarks() + " " + person.getPfMarks());
        }


        double totalPfMarks = 0.0;
        double totalScdMarks = 0.0;

        for (Person person : people) {
            totalPfMarks += person.getPfMarks();
            totalScdMarks += person.getScdMarks();
        }

        double averagePfMarks = totalPfMarks / people.size();
        double averageScdMarks = totalScdMarks / people.size();

        try (BufferedWriter bw = new BufferedWriter(new FileWriter("marks.txt"))) {
            for (Person person : people) {
                String line = person.getName() + "," + averagePfMarks + "," + averageScdMarks;
                bw.write(line);
                bw.newLine();
            }
        } catch (IOException e) {
            e.printStackTrace();
        }

        System.out.println("Written to Mark.txt");
    }
}

