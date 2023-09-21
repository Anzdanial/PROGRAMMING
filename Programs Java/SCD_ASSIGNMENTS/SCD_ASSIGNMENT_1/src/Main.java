import java.io.File;
import java.io.FileReader;
import java.io.IOException;
import java.util.ArrayList;

class SkillSet{
    private String skillname;
    private int yearsexperience;

    public int getYearsOfExperience(){
        return yearsexperience;
    }

    public String getSkillName(){
        return skillname;
    }
}

class Person{
    private String name;
    private ArrayList<SkillSet>skillset;
}

class TaskAllocation{
    ArrayList<Person> applicants;
    void FileReader() throws IOException {
        char [] array = new char[1024];
        int characterRead = 0;
        try {
            FileReader input = new FileReader("file1.txt");
            characterRead = input.read(array);
            input.close();
        } catch(Exception e) {
        e.getStackTrace();
    }

        for(int i = 0; i < characterRead; i++){
            System.out.print(array[i]);
            System.out.println(i);
        }
        System.out.println();
    }

}


public class Main {
    public static void main(String[] args) throws IOException {
        TaskAllocation t1 = new TaskAllocation();
        t1.FileReader();

    }
}


