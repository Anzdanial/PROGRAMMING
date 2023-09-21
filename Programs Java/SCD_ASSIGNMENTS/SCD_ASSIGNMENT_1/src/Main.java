import java.io.File;
import java.io.FileReader;
import java.io.IOException;
import java.util.ArrayList;

class SkillSet{
    private String skillname;
    private int yearsexperience;

    SkillSet(String data, int number){
        skillname = data;
        yearsexperience = number;
    }
    public String getSkillName(){ return skillname;}
    public int getYearsOfExperience(){ return yearsexperience;}
    public void setSkillSet(String data, int number){
        skillname = data;
        yearsexperience = number;
    }
}

class Person{
    private String name;
    private static int ID = 0;
    private ArrayList<SkillSet>skillset;
    Person(String n, String skill, int numberExperience){
        name = n;
        skillset = new ArrayList<SkillSet>();
        SkillSet newSkillset = new SkillSet(skill, numberExperience);
        skillset.add(newSkillset);
        ID++;
    }
    public String getName(){
        return name;
    }
    public String getSkill(int ID){
        SkillSet set = skillset.get(ID);
        return set.getSkillName();
    }
    public int getExperience(int ID){
        SkillSet set = skillset.get(ID);
        return set.getYearsOfExperience();
    }
    public void setSkillSet(String data, int number, int ID){
        SkillSet newSkillset = skillset.get(ID);
        newSkillset.setSkillSet(data,number);
        skillset.add(newSkillset);
    }
}

class TaskAllocation{
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
        char[] arraynew = new char[characterRead];
        for (int i = 0; i < characterRead; i++)
            arraynew[i] = array[i];
        String inputString = new String(arraynew);
        String[] result = inputString.split("\n");

        //for(String s: result)
        //  System.out.println(s);
        int[] stringLengths = new int[result.length];
        for (int i = 0; i < result.length; i++) {
            stringLengths[i] = result[i].length();
        }
        /*
        for(int k = 0; k < result.length; k++) {
            for (int j = 0; j < stringLengths[j]; j++) {
                int nameLength = 0;
                int i = 0;
                while (result[j].charAt(i) != '|') {
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

                setApplicants((result[0].substring(0, nameLength - 1)), result[0].substring(startIndex, lastIndex), number);
            }
        }
         */
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
        setApplicants((result[0].substring(0, nameLength - 1)), result[0].substring(startIndex, lastIndex), number);
        System.out.println(number+2);


    }
        public Person getPersonData(int ID){
            return applicants.get(ID);
        }
        public void setApplicants(String name, String Skill, int number){
            Person applicant = new Person(name, Skill, number);
            applicants.add(applicant);
        }

        public void setSkillSetByID(String skill, int number, int ID){
            Person newPerson = applicants.get(ID);
            newPerson.setSkillSet(skill,number,ID);
        }
    }




public class Main {
    public static void main(String[] args) throws IOException {
        TaskAllocation t1 = new TaskAllocation();
        t1.FileReader();
        //t1.getPersonData(0);
    }
}


