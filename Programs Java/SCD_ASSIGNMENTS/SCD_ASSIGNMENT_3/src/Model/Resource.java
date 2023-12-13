package Model;

import java.util.ArrayList;

public class Resource implements skillSet {
    public String name;
    public ArrayList<Skill> skillsLearnt = new ArrayList<>();

    public Resource(String name) {
        this.name = name;
    }

    public Resource(String name, ArrayList<Skill> skillsLearnt) {
        this.name = name;
        this.skillsLearnt = skillsLearnt;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public void setSkillsLearnt(ArrayList<Skill> skillsLearnt) {
        this.skillsLearnt = skillsLearnt;
    }

    public ArrayList<Skill> getSkills() {
        return skillsLearnt;
    }

    @Override
    public void addSkill(Skill skill){
       getSkills().add(skill);

    }

    public boolean containsSkill(Skill checkingSkill) {
        for (Skill skill : skillsLearnt) {
            if (checkingSkill.name.equals(skill.name)) return true;
        }
        return false;
    }

    public boolean containsSkillWithExperience(Skill checkingSkill) {
        for (Skill skill : skillsLearnt) {
            if (checkingSkill.name.equals(skill.name) && (checkingSkill.yearsOfExperience <= skill.yearsOfExperience))
                return true;
        }
        return false;
    }

    public boolean containsAllSkills(ArrayList<Skill> skillsList) {
        for (Skill skill : skillsList) {
            if (!containsSkill(skill)) return false;
        }
        return true;
    }

    @Override
    public boolean containsAllSkillsWithExperience(ArrayList<Skill> skillsOfTask) {
        for (Skill skill : skillsOfTask) {
            if (!containsSkillWithExperience(skill)) return false;
        }
        return true;
    }

    @Override
    public void print() {
        System.out.println("Model.Resource name: " + name + ". Skills Present: ");
        for (Skill skill : skillsLearnt) {
            System.out.print("\t");
            skill.print();
        }
    }

    @Override
    public String toString() {
        return name;
    }
}