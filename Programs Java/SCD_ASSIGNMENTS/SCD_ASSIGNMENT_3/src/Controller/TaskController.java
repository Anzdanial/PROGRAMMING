package Controller;

import Model.Task;
import Model.Skill;
import Model.skillSet;
import View.AllocatorView;

import javax.swing.*;
import javax.swing.event.ListSelectionEvent;
import javax.swing.event.ListSelectionListener;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.util.List;

public class TaskController extends AllocatorController{

    public TaskController(AllocatorView view, List<skillSet> skillSets) {
        super(view, skillSets);
        initialize();
    }

    @Override
    protected void addListeners() {

        //Add skill button
        view.getAddTalentButton().addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                String taskName = view.getTalentField().getText();
                if(!taskName.isEmpty()){
                    Task addedTask = new Task(taskName);
                    skillSets.add(addedTask);
                    refreshView();
                }
            }
        });

        //Add Skill button
        view.getAddSkillButton().addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                skillSet selectedTask = view.getTalentList().getSelectedValue();
                if(selectedTask != null){
                    String skillName = view.getSkillsNameField().getText();
                    String skillXp;
                    JComponent skillXpField = view.getSkillsExperienceFieldField();
                    if(skillXpField instanceof JTextField)
                        skillXp = ((JTextField)skillXpField).getText();
                    else if(skillXpField instanceof JComboBox)
                        skillXp = (String)((JComboBox)skillXpField).getSelectedItem();
                    else
                        skillXp = "0";
                    if(!skillName.isEmpty()){
                        Skill newSkill = new Skill(skillName, skillXp);
                        selectedTask.getSkills().add(newSkill);
                        refreshView();
                    }
                }
            }
        });

        //List skills of selected task
        view.getTalentList().addListSelectionListener(new ListSelectionListener() {
            @Override
            public void valueChanged(ListSelectionEvent e) {
                if(!e.getValueIsAdjusting()){
                    skillSet selectedTask = view.getTalentList().getSelectedValue();
                    if(selectedTask!= null){
                        DefaultListModel<Object> skillsModel = new DefaultListModel<>();
                        for (Skill s : selectedTask.getSkills()) {
                            skillsModel.addElement(s.name + ": " + s.yearsOfExperience);
                        }
                        view.getSkillsList().setModel(skillsModel);
                    }
                }
            }
        });


    }


}
