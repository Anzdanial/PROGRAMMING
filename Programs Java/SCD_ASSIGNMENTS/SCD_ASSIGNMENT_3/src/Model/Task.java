package Model;

import java.util.ArrayList;

public class Task implements skillSet {
    private ArrayList<Skill> skillsRequired = new ArrayList<>();
    private String name;

    public Task(String name) {
        this.name = name;
    }

    public Task(String name, ArrayList<Skill> skills) {
        this.name = name;
        skillsRequired = skills;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public ArrayList<Skill> getSkillsRequired() {
        return skillsRequired;
    }

    public void setSkillsRequired(ArrayList<Skill> skillsRequired) {
        this.skillsRequired = skillsRequired;
    }

    @Override
    public ArrayList<Skill> getSkills() {
        return skillsRequired;
    }

    @Override
    public void addSkill(Skill skill){
        getSkills().add(skill);

    }
    @Override
    public boolean containsAllSkills(ArrayList<Skill> skillsOfTask) {
        return false;
    }

    @Override
    public boolean containsAllSkillsWithExperience(ArrayList<Skill> skillsOfTask) {
        return false;
    }

    @Override
    public void print() {
        System.out.println("Model.Task name: " + name + ". Skills Required: ");
        for (Skill skill : skillsRequired) {
            System.out.print("\t");
            skill.print();
        }
    }

    @Override
    public String toString() {
        return name;
    }
}