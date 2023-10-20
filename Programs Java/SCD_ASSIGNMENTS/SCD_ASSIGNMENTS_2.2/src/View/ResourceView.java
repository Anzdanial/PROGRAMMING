package View;

import javax.swing.*;

public class ResourceView extends AllocatorView{
    @Override
    protected void createUIComponents() {
        super.createUIComponents();
        addTalentButton.setText("Add Resource");
        addSkillButton.setText("Add Skill");
    }


}
