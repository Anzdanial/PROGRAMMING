import javax.swing.*;
import javax.swing.event.ListSelectionEvent;
import javax.swing.event.ListSelectionListener;
import java.awt.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
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

class Data {
    private String name;
    private ArrayList<SkillSet>skillset;
    Data(){
        skillset = new ArrayList<>();
    }
    public String getName(){return name;}
    public ArrayList<SkillSet> getSkillSet(int ID){return skillset;}
    public int getSkillSetSize(){return skillset.size();}

    public void setDataName(String n){
        name = n;
    }
    public void addDataSkillSet(String s, int Number){
        SkillSet newSkillSet = new SkillSet();
        newSkillSet.setSkill(s);
        newSkillSet.setYearsExperience(Number);
        skillset.add(newSkillSet);
    }

}

class Resource {
    private static int ID = 0;
    private ArrayList<Data> applicants;
    Resource(){
        applicants = new ArrayList<>();
    }
    public void fileReader() throws IOException {
        char[] array = new char[1024];
        int characterRead = 0;
        try {
            FileReader input = new FileReader("taskFile.txt");
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

        for (int i = 0; i < result.length; i++) {
            result[i] = result[i].trim();
        }


        for (int i = 0; i < result.length; i++) {
            String[] parts = result[i].split("\\|");
            if (parts.length == 2) {
                String name = parts[0].trim();
                setName(name);
                String []skillsSet = parts[1].trim().split(", ");

                for (int j = 0; j < skillsSet.length; j++) {
                    String[] skillParts = skillsSet[j].split(":");
                    if (skillParts.length == 2) {
                        String skill = skillParts[0].trim();
                        String year = skillParts[1].trim();
                        int number = 0;
                        try {
                            number = Integer.parseInt(year);
                        } catch (NumberFormatException ex) {
                            System.out.println("Before Failure, Current: " + j);
                            ex.printStackTrace();
                        }
                        setSkill(skill,number,i);
                    }
                }
            }
        }
    }
    public Data getData(int ID){
        return applicants.get(ID);
    }
    public int getResourceSize(){
        return applicants.size();
    }
    public void setName(String n){
        Data applicant = new Data();
        applicant.setDataName(n);
        applicants.add(applicant);
    }
    public void setSkill(String s, int number, int ID){
        Data applicant = applicants.get(ID);
        applicant.addDataSkillSet(s, number);
    }
}


class Task {
    private static int ID = 0;
    private ArrayList<Data> tasks;
    Task(){tasks = new ArrayList<>();}

    public void fileReader(){
        char[] array = new char[1024];
        int characterRead = 0;
        try {
            FileReader input = new FileReader("resourceFile.txt");
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

        for (int i = 0; i < result.length; i++) {
            result[i] = result[i].trim();
        }

        for (int i = 0; i < result.length; i++) {
            String[] parts = result[i].split("\\|");
            if (parts.length == 2) {
                String name = parts[0].trim();
                setName(name);
                String []skillsSet = parts[1].trim().split(", ");
                for (int j = 0; j < skillsSet.length; j++) {
                    String[] skillParts = skillsSet[j].split(":");
                    if (skillParts.length == 2) {
                        String skill = skillParts[0].trim();
                        String year = skillParts[1].trim();
                        if(year.equals("b"))
                            setSkill(skill,1,i);
                        else if(year.equals("i"))
                            setSkill(skill,2,i);
                        else if(year.equals("e"))
                            setSkill(skill,3,i);
                    }
                }


            }
        }
    }
    public Data getData(int ID){
        return tasks.get(ID);
    }
    public int getTaskSize(){
        return tasks.size();
    }
    public void setName(String n){
        Data newTask = new Data();
        newTask.setDataName(n);
        tasks.add(newTask);
    }
    public void setSkill(String s, int number, int ID){
        Data newTask = tasks.get(ID);
        newTask.addDataSkillSet(s, number);
    }
}

interface MatchingStrategy{
    public void performMatch(Resource r, Task t);
}

class ExactMatch implements MatchingStrategy{
    private ArrayList<String>matches;
    ExactMatch(){ matches = new ArrayList<>(); }
    public void performMatch(Resource r, Task t){
        boolean flag = false, flag2 = false;
        for(int i = 0; i < r.getResourceSize(); i++){
            flag2 = false;
            for(int k = 0; k < t.getTaskSize(); k++ ){
                if(!flag2 && flag)
                    flag2 = true;
                for(int j = 0; j < r.getData(i).getSkillSetSize(); j++) {
                    flag = false;
                    for(int l = 0; l < t.getData(k).getSkillSetSize(); l++){
                        if(r.getData(i).getSkillSet(i).get(j).getSkillName().equals(t.getData(k).getSkillSet(k).get(l).getSkillName())) {
                            if(r.getData(i).getSkillSet(i).get(j).getYearsOfExperience() < t.getData(k).getSkillSet(k).get(l).getYearsOfExperience()) {
                                flag = false;
                                break;
                            }
                            flag = true;
                            continue;
                        }
                    }
                    if(!flag) {
                        flag = false;
                        break;
                    }
                }
                if(flag)
                    matches.add(r.getData(i).getName() + " " + t.getData(k).getName());
            }
        }
        if(matches.size() == 0)
            System.out.println("NONE FOUND!");

        for(int i = 0; i < matches.size(); i++){
            System.out.println(matches.get(i));
        }
    }
}

class SkillOnlyMatch implements MatchingStrategy{
    private ArrayList<String>matches;
    SkillOnlyMatch(){ matches = new ArrayList<>(); }
    public void performMatch(Resource r, Task t){
        boolean isMatch = false, isGood = false;
        for(int i = 0; i < r.getResourceSize(); i++){
            isGood = true;
            for(int k = 0; k < t.getTaskSize(); k++ ){
                isGood = true;
                for(int j = 0; j < t.getData(k).getSkillSetSize(); j++) {
                    isMatch = false;
                    for(int l = 0; l < r.getData(i).getSkillSetSize(); l++){
                        if(r.getData(i).getSkillSet(i).get(l).getSkillName().equals(t.getData(k).getSkillSet(k).get(j).getSkillName())) {
                            isMatch = true;
                            continue;
                        }
                    }
                    if(!isMatch) {
                        isGood = false;
                        continue;
                    }
                }
                if(isGood)
                    matches.add(r.getData(i).getName() + " " + t.getData(k).getName());
            }
        }
        for(int i = 0; i < matches.size(); i++)
            System.out.println(matches.get(i));
        if(matches.size() == 0) {
            System.out.print("Sadly, none");
        }
        System.out.println();
    }
}

class StrategyController{
    StrategyController(MatchingStrategy val) throws IOException {
        Resource r1 = new Resource();
        r1.fileReader();

        Task t1 = new Task();
        t1.fileReader();

        val.performMatch(r1,t1);
    }
}


//class ComponentListenerResource implements ListSelectionListener, ActionListener {
//    private DefaultListModel<String> listModelResource;
//    private DefaultListModel<String> listModelSkill;
//    private JList<String> listResource;
//    private JList<String> listSkill;
//    private JTextField fieldResource;
//    private JTextField fieldSkill;
//    private JTextField fieldExperience;
//
//    ComponentListenerResource(JList<String> listResource, JList<String> listSkill, DefaultListModel<String> listModelResource, DefaultListModel<String> listModelSkill, JTextField fieldResource, JTextField fieldSkill, JTextField fieldExperience) {
//        this.listResource = listResource;
//        this.listSkill = listSkill;
//        this.listModelResource = listModelResource;
//        this.listModelSkill = listModelSkill;
//        this.fieldResource = fieldResource;
//        this.fieldSkill = fieldSkill;
//       this.fieldExperience = fieldExperience;
//    }
//
//    public void actionPerformed(ActionEvent e) {
//        listModelResource.addElement(fieldResource);
//        if(selectedResource){
//            listModelSkill.addElement(fieldSkill.getText());
//        }
//    }
//
//    public void valueChanged(ListSelectionEvent e) {
//        if (!e.getValueIsAdjusting()) {
//            int index = e.getLastIndex();
//            String selectedValue = listResource.getSelectedValue();
//        }
//    }
//}

class AdditionForm {
    AdditionForm(){
        JFrame frame = new JFrame();
        Container contentPane = frame.getContentPane();
        SpringLayout layout = new SpringLayout();

        JLabel resourceLabel = new JLabel("Resource: ");
        JTextField resourceField = new JTextField("", 12);
        JButton resourceAddButton = new JButton("Add");

        JLabel skillLabel = new JLabel("Skill: ");
        JLabel skillExperienceLabel = new JLabel("Experience: ");
        JTextField skillField = new JTextField("", 8);
        JTextField skillExperienceField = new JTextField("", 6);
        JButton skillAddButton = new JButton("Add");

        DefaultListModel<String> listModelResource = new DefaultListModel<>();
        JList<String> myListResource = new JList<>(listModelResource);

        DefaultListModel<JList<String>> listModelResourceSkills = new DefaultListModel<>();
        JList<JList<String>> myListResourceSkills = new JList<>(listModelResourceSkills);

        JScrollPane scrollPaneResource = new JScrollPane(myListResource);
        JScrollPane scrollPaneSkill = new JScrollPane(myListResourceSkills);

        contentPane.setLayout(layout);

        contentPane.add(resourceLabel);
        contentPane.add(resourceField);
        contentPane.add(resourceAddButton);
        contentPane.add(scrollPaneResource);

        contentPane.add(skillLabel);
        contentPane.add(skillField);
        contentPane.add(skillExperienceLabel);
        contentPane.add(skillExperienceField);
        contentPane.add(skillAddButton);
        contentPane.add(scrollPaneSkill);

        layout.putConstraint(SpringLayout.WEST, resourceLabel, 20, SpringLayout.WEST,contentPane);
        layout.putConstraint(SpringLayout.NORTH, resourceLabel, 5, SpringLayout.NORTH,contentPane);

        layout.putConstraint(SpringLayout.WEST, resourceField, 5, SpringLayout.EAST, resourceLabel);
        layout.putConstraint(SpringLayout.NORTH, resourceField, 5, SpringLayout.NORTH, contentPane);

        layout.putConstraint(SpringLayout.WEST, resourceAddButton, 5, SpringLayout.EAST, resourceField);
        layout.putConstraint(SpringLayout.NORTH, resourceAddButton, 5, SpringLayout.NORTH, contentPane);

        layout.putConstraint(SpringLayout.WEST, scrollPaneResource, 25, SpringLayout.WEST, contentPane);
        layout.putConstraint(SpringLayout.NORTH, scrollPaneResource, 50, SpringLayout.NORTH, contentPane);

        layout.putConstraint(SpringLayout.WEST,skillLabel, 20, SpringLayout.EAST,resourceAddButton);
        layout.putConstraint(SpringLayout.NORTH, skillLabel, 5, SpringLayout.NORTH,contentPane);

        layout.putConstraint(SpringLayout.WEST, skillField, 5, SpringLayout.EAST, skillLabel);
        layout.putConstraint(SpringLayout.NORTH, skillField, 5, SpringLayout.NORTH, contentPane);

        layout.putConstraint(SpringLayout.WEST,skillExperienceLabel, 10, SpringLayout.EAST,skillField);
        layout.putConstraint(SpringLayout.NORTH, skillExperienceLabel, 5, SpringLayout.NORTH,contentPane);

        layout.putConstraint(SpringLayout.WEST, skillExperienceField, 5, SpringLayout.EAST, skillExperienceLabel);
        layout.putConstraint(SpringLayout.NORTH, skillExperienceField, 5, SpringLayout.NORTH, contentPane);

        layout.putConstraint(SpringLayout.WEST, skillAddButton, 5, SpringLayout.EAST, skillExperienceField);
        layout.putConstraint(SpringLayout.NORTH, skillAddButton, 5, SpringLayout.NORTH, contentPane);

        layout.putConstraint(SpringLayout.WEST, scrollPaneSkill, 20, SpringLayout.EAST, scrollPaneResource);
        layout.putConstraint(SpringLayout.NORTH, scrollPaneSkill, 50, SpringLayout.NORTH, contentPane);

//        ComponentListenerResource listenerResource = new ComponentListenerResource(myListResource, listModelResource, listModelSkill, resourceField);
//        ComponentListenerResource listenerSkill = new ComponentListenerResource(myListSkill, listModelSkill, listModelResource, skillField);

        ArrayList<Data>resourceDataList = new ArrayList<>();
        resourceAddButton.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                listModelResource.addElement(resourceField.getText());
                Data temp = new Data();
                temp.setDataName(resourceField.getText());
                resourceDataList.add(temp);
                resourceField.setText("");
            }
        });
//        myListResource.addListSelectionListener(new ListSelectionListener() {
//            @Override
//            public void valueChanged(ListSelectionEvent e) {
//                if(!e.getValueIsAdjusting()){
//                    int val = e.getLastIndex();
//                    System.out.println(listModelResource.get(val));
//                }
//            }
//        });
        skillAddButton.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                myListResource.addListSelectionListener(new ListSelectionListener() {
                    @Override
                    public void valueChanged(ListSelectionEvent e) {
                        int val = e.getLastIndex();
                        Data temp = resourceDataList.get(val);
                    }
                });
            }
        });

        frame.pack();
        frame.setTitle("Add Form");
        frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        frame.setSize(700, 300);
        frame.setVisible(true);
    }
}

public class Main {
    public static void main(String[] args) throws IOException {
            AdditionForm anas = new AdditionForm();
//        System.out.println("Specify which Strategy you'd like to perform: ");
//        System.out.print("1) SkillOnlyMatch ");
//        System.out.println("2) ExactMatch ");
//        Scanner input = new Scanner(System.in);
//        int inputNum = input.nextInt();
//        if(inputNum == 1) {
//            SkillOnlyMatch search = new SkillOnlyMatch();
//            StrategyController s = new StrategyController(search);
//        }
//        else if(inputNum == 2){
//            ExactMatch search = new ExactMatch();
//            StrategyController s = new StrategyController(search);
//        }
        /*
        for(int j = 0; j < 5; j++) {
            Data temp = r1.getData(j);
            System.out.println(temp.getName());
            for (int i = 0; i < temp.getSkillSetSize(); i++) {
                System.out.print(temp.getSkillSet(i).getSkillName() + "  ");
                System.out.print(temp.getSkillSet(i).getYearsOfExperience() + "  ");
                System.out.println();
            }
            System.out.println();
        }


        for(int j = 0; j < 3; j++) {
            Data temp = t1.getData(j);
            System.out.println(temp.getName());
            for (int i = 0; i < temp.getSkillSetSize(); i++) {
                System.out.print(temp.getSkillSet(i).getSkillName() + "  ");
                System.out.print(temp.getSkillSet(i).getYearsOfExperience() + "  ");
                System.out.println();
            }
            System.out.println();
        }
         */
    }
}


