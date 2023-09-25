import java.io.File;
import java.io.FileReader;
import java.io.IOException;
import java.util.ArrayList;

class SkillSet{
    private String skill;
    private int yearsExperience;
    public String getSkillName(){ return skill;}
    public int getYearsOfExperience(){ return yearsExperience;}
    public void setSkill(String s){
        skill = s;
    }
    public void setYearsExperience(int year){
        yearsExperience = year;
    }
}

class Person{
    private String name;
    private ArrayList<SkillSet>skillset;
    Person(){
        skillset = new ArrayList<>();
    }
    public String getName(){
        return name;
    }
    public SkillSet getSkillSet(int ID){
        SkillSet set = skillset.get(ID);
        return set;
    }
    public int getSkillSetSize(){
        return skillset.size();
    }

    public void setPersonName(String n){
        name = n;
    }
    public void addPersonSkillSet(String s, int Number){
        SkillSet newSkillSet = new SkillSet();
        newSkillSet.setSkill(s);
        newSkillSet.setYearsExperience(Number);
        skillset.add(newSkillSet);
    }

}

class TaskAllocation{
    private static int ID = 0;
    private ArrayList<Person> applicants;
    TaskAllocation(){
        applicants = new ArrayList<>();
    }
    void FileReader() throws IOException {
        char[] array = new char[1024];
        int characterRead = 0;
        try {
            FileReader input = new FileReader("file1.txt");
            characterRead = input.read(array);
            input.close();
        } catch (Exception e) {
            e.getStackTrace();
        }
        char[] arrayNew = new char[characterRead];
        for (int i = 0; i < characterRead; i++)
            arrayNew[i] = array[i];
        String inputString = new String(arrayNew);
        String[] result = inputString.split("\n");

        int[] stringLengths = new int[result.length];
        for (int i = 0; i < result.length; i++) {
            stringLengths[i] = result[i].length();
        }

        boolean flag = false;
        for(int k = 0; k < result.length; k++) {
            for (int j = 0; j < stringLengths[k] || flag ; j++) {
                int nameLength = 0;
                int i = 0;
                while (result[k].charAt(i) != '|') {
                    i++;
                    nameLength++;
                }

                //Block 1
                int startIndex = nameLength + 2;
                i = startIndex;
                while (result[k].charAt(i) != ':') {
                    i++;
                }
                int lastIndex = i;

                int startYear = lastIndex + 1;
                i = startYear;
                while (result[k].charAt(i) != ',') {
                    i++;
                }
                int lastYear = i;

                if(lastYear+1 > stringLengths[k]) {
                    flag = true;
                }

                //Block 2
                int startIndex1 = lastYear + 2;
                i = startIndex1;
                while (result[k].charAt(i) != ':') {
                    i++;
                }
                int lastIndex1 = i;

                int startYear1 = lastIndex1 + 1;
                i = startYear1;
                while (result[k].charAt(i) != ',') {
                    i++;
                }
                int lastYear1 = i;

                //Block3
                int startIndex2 = lastYear1 + 2;
                i = startIndex2;
                while (result[k].charAt(i) != ':') {
                    i++;
                }
                int lastIndex2 = i;

                int startYear2 = lastIndex2 + 1;
                i = startYear2;
                while (result[k].charAt(i) != ',') {
                    i++;
                }
                int lastYear2 = i;

                String year = result[k].substring(startYear, lastYear);
                int number = 0;
                try {
                    number = Integer.parseInt(year);
                } catch (NumberFormatException ex) {
                    System.out.println("Before Failure, Current: " + k);
                    ex.printStackTrace();
                }
                setName((result[k].substring(0, nameLength - 1)));
                setSkill(result[k].substring(startIndex, lastIndex), number, k);
                incrementID();
            }
        }
        /*
        int nameLength = 0;
        int i = 0;
        while (result[0].charAt(i) != '|') {
            i++;
            nameLength++;
        }
        int startIndex = nameLength + 2;
        i = startIndex;
        while (result[0].charAt(i) != ':') {
            i++;
        }
        int lastIndex = i;

        int startYear = lastIndex + 1;
        i = startYear;
        while (result[0].charAt(i) != ',') {
            i++;
        }
        int lastYear = i;

        String year = result[0].substring(startYear, lastYear);
        int number = 0;
        try {
            number = Integer.parseInt(year);
        } catch (NumberFormatException ex) {
            ex.printStackTrace();
        }

         */

    }
        public Person getData(int ID){
            return applicants.get(ID);
        }
        public void setName(String n){
            Person applicant = new Person();
            applicant.setPersonName(n);
            applicants.add(applicant);
        }
        public void setSkill(String s, int number, int ID){
            Person applicant = applicants.get(ID);
            applicant.addPersonSkillSet(s, number);
        }
         public void incrementID(){ID++;}
    }

public class Main {
    public static void main(String[] args) throws IOException {
        TaskAllocation t1 = new TaskAllocation();
        t1.FileReader();
        for(int j = 0; j < 5; j++) {
            Person temp = t1.getData(j);
            System.out.println(temp.getName());
            for (int i = 0; i < temp.getSkillSetSize(); i++) {
                System.out.print(temp.getSkillSet(i).getSkillName() + " , ");
                System.out.print(temp.getSkillSet(i).getYearsOfExperience() + " , ");
                System.out.println();
            }
            System.out.println();
        }
    }
}


