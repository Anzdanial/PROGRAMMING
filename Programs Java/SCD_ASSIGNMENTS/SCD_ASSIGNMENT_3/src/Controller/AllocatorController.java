package Controller;

import Model.Skill;
import Model.skillSet;
import View.AllocatorView;

import javax.swing.*;
import java.util.List;

public abstract class AllocatorController {

    protected AllocatorView view;
    protected List<skillSet> skillSets;

    public AllocatorController(AllocatorView view, List<skillSet> skillSets) {
        this.view = view;
        this.skillSets = skillSets;

    }

    protected abstract void addListeners();
    public void refreshView() {
        skillSet previouslySelectedSkillSet = view.getTalentList().getSelectedValue();

        //Refresh LHS
        DefaultListModel<skillSet> model = new DefaultListModel<>();
        for (skillSet t : skillSets) {
            model.addElement(t);
        }
        view.getTalentList().setModel(model);

        // Restore prev selection
        if (previouslySelectedSkillSet != null && model.contains(previouslySelectedSkillSet)) {
            view.getTalentList().setSelectedValue(previouslySelectedSkillSet, true);
        }
        //Refresh RHS
        skillSet selectedSkillSet = view.getTalentList().getSelectedValue();
        if (selectedSkillSet != null) {
            DefaultListModel<Object> skillsModel = new DefaultListModel<>();
            for (Skill s : selectedSkillSet.getSkills()) {
                skillsModel.addElement(s.name + ": " + s.yearsOfExperience);
            }
            view.getSkillsList().setModel(skillsModel);
        } else {
            System.out.println("No talent selected!");
            view.getSkillsList().setModel(new DefaultListModel<>()); // Clear the list if no talent is selected
        }
    }

    public void initialize(){
        refreshView();
        addListeners();
    }



}
