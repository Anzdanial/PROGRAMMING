package View;

import javax.swing.*;

public class TaskView extends AllocatorView{
    @Override
    protected void createUIComponents() {
        super.createUIComponents();
        addTalentButton.setText("Add Task");
        addSkillButton.setText("Add Skill");
    }

    @Override
    public JPanel getMainPanel() {
        JPanel panel = super.getMainPanel();
        panel.remove(skillsExperienceField);
        String[] options = {"b", "i", "e"};
        JComboBox dropdown = new JComboBox<String>(options);
        skillsExperienceField = dropdown;
        panel.add(skillsExperienceField);
        return panel;
    }
}
