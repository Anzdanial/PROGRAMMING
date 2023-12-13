package Model;

import java.util.ArrayList;

public interface skillSet {
    void print();

    ArrayList<Skill> getSkills();

    boolean containsAllSkills(ArrayList<Skill> skillsOfTask);

    boolean containsAllSkillsWithExperience(ArrayList<Skill> skillsOfTask);

    String getName();
    void addSkill(Skill skill);
}