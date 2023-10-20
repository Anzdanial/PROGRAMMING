package Controller;

import Model.Resource;
import Model.Skill;
import Model.skillSet;
import View.AllocatorView;

import javax.swing.*;
import javax.swing.event.ListSelectionEvent;
import javax.swing.event.ListSelectionListener;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.util.List;

public class ResourceController extends AllocatorController{

    public ResourceController(AllocatorView view, List<skillSet> skillSets) {
        super(view, skillSets);
        initialize();
    }

    @Override
    protected void addListeners() {

        //Add resource button
        view.getAddTalentButton().addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                String resourceName = view.getTalentField().getText();
                if(!resourceName.isEmpty()){
                    Resource addedResource = new Resource(resourceName);
                    skillSets.add(addedResource);
                    refreshView();
                }
            }
        });

        //Add Skill button
        view.getAddSkillButton().addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                skillSet selectedResource = view.getTalentList().getSelectedValue();
                if(selectedResource != null){
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
                        selectedResource.getSkills().add(newSkill);
                        refreshView();
                    }
                }
            }
        });

        //List skills of selected resource
        view.getTalentList().addListSelectionListener(new ListSelectionListener() {
            @Override
            public void valueChanged(ListSelectionEvent e) {
                if(!e.getValueIsAdjusting()){
                    skillSet selectedResource = view.getTalentList().getSelectedValue();
                    if(selectedResource!= null){
                        DefaultListModel<Object> skillsModel = new DefaultListModel<>();
                        for (Skill s : selectedResource.getSkills()) {
                            skillsModel.addElement(s.name + ": " + s.yearsOfExperience);
                        }
                        view.getSkillsList().setModel(skillsModel);
                    }
                }
            }
        });


    }


}
