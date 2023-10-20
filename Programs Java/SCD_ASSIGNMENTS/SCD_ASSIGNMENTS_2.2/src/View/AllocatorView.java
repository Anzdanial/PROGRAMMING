package View;

import Model.skillSet;
import javax.swing.*;
import java.awt.*;

// Holds common variables of all view classes
public abstract class AllocatorView {

    // Fields
    protected JTextField talentField;
    protected JTextField skillsNameField;
    protected JComponent skillsExperienceField;
    protected JButton addTalentButton;
    protected JButton addSkillButton;
    protected JList<Object> skillsList;
    protected JList<skillSet> talentList;

    public AllocatorView() {
        createUIComponents();
    }

    // Getters and Setters
    public JList<Object> getSkillsList() {
        return skillsList;
    }

    public JTextField getTalentField() {
        return talentField;
    }

    public void setTalentField(JTextField talentField) {
        this.talentField = talentField;
    }

    public JTextField getSkillsNameField() {
        return skillsNameField;
    }

    public void setSkillsNameField(JTextField skillsNameField) {
        this.skillsNameField = skillsNameField;
    }

    public JComponent getSkillsExperienceFieldField() {
        return skillsExperienceField;
    }

    public void setSkillsExperienceField(JTextField skillsExperienceField) {
        this.skillsExperienceField = skillsExperienceField;
    }

    public JButton getAddTalentButton() {
        return addTalentButton;
    }

    public void setAddTalentButton(JButton addTalentButton) {
        this.addTalentButton = addTalentButton;
    }

    public JButton getAddSkillButton() {
        return addSkillButton;
    }

    public void setAddSkillButton(JButton addSkillButton) {
        this.addSkillButton = addSkillButton;
    }

    public JList<skillSet> getTalentList() {
        return talentList;
    }

    public void setTalentList(JList<skillSet> talentList) {
        this.talentList = talentList;
    }

    // Initialize UI Components
    protected void createUIComponents() {
        talentField = new JTextField(20);
        skillsNameField = new JTextField(17);
        skillsExperienceField = new JTextField(3);
        addSkillButton = new JButton();
        addTalentButton = new JButton();
        talentList = new JList<>();
        skillsList = new JList<>();
        talentField.setPreferredSize(new Dimension(200, 30));
        skillsNameField.setPreferredSize(new Dimension(180, 30));
        skillsExperienceField.setPreferredSize(new Dimension(40, 30));
        talentList.setPreferredSize(new Dimension(300, 150));
        skillsList.setPreferredSize(new Dimension(300, 150));
    }

    public JPanel getMainPanel() {
        JPanel panel = new JPanel();
        panel.add(talentField);
        panel.add(addTalentButton);
        panel.add(skillsNameField);
        panel.add(skillsExperienceField);
        panel.add(addSkillButton);
        panel.add(new JScrollPane(talentList));
        panel.add(new JScrollPane(skillsList));
        return panel;
    }
}
