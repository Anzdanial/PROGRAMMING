package Controller.Strategies;

import Model.Skill;
import Model.skillSet;

import java.util.ArrayList;

public class ExactMatch implements MatchingStrategy {
    ArrayList<skillSet> tasksPresent;
    ArrayList<skillSet> resourcesPresent;

    public ExactMatch(ArrayList<skillSet> tasks, ArrayList<skillSet> resources){
        tasksPresent = tasks;
        resourcesPresent = resources;
    }


    @Override
    public void PrintMatches() {
        for(skillSet task : tasksPresent){
            ArrayList<Skill> skillsOfTask = task.getSkills();
            System.out.print("Task " + task.getName() +": ");
            for(skillSet resource : resourcesPresent) {
                if(resource.containsAllSkillsWithExperience(skillsOfTask)){
                    System.out.print( resource.getName() +", ");
                }
            }
            System.out.print("\n");

        }

    }

    @Override
    public ArrayList<Object> ListMatches() {
        ArrayList<Object> resultList = new ArrayList<>();
        for (skillSet task : tasksPresent) {
            ArrayList<Skill> skillsOfTask = task.getSkills();
            String matchedTalents = task.getName() +":  \t";
            for (skillSet resource : resourcesPresent) {
                if (resource.containsAllSkillsWithExperience(skillsOfTask)) {
                    matchedTalents += resource.getName() + ", ";
                }
            }
            resultList.add(matchedTalents);
        }
        return resultList;
    }
}
