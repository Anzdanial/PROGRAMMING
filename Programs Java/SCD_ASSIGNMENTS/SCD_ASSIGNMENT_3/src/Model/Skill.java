package Model;

import java.util.ArrayList;

public class Skill implements skillSet {
    public String name;
    public int yearsOfExperience;

    public Skill(String name, int years) {
        this.name = name;
        this.yearsOfExperience = years;
    }

    public Skill(String name, String years) {
        this.name = name;
        this.yearsOfExperience =
                years.equals("b") ? 1 :
                        years.equals("i") ? 2 :
                                years.equals("e") ? 3 : Integer.parseInt(years);
    }

    public String getName() {
        return name;
    }

    @Override
    public void addSkill(Skill skill) {
        return;
    }

    public void setName(String name) {
        this.name = name;
    }

    public int getYearsOfExperience() {
        return yearsOfExperience;
    }

    public void setYearsOfExperience(int yearsOfExperience) {
        this.yearsOfExperience = yearsOfExperience;
    }

    @Override
    public void print() {
        System.out.println("Model.Skill Name: " + name + ". Years: " + yearsOfExperience);
    }

    @Override
    public ArrayList<Skill> getSkills() {
        return null;
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
    public String toString() {
        return name;
    }
}
